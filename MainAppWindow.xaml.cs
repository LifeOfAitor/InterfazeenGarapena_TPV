using erronkaTPVsistema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace erronkaTPVsistema
{
    /// <summary>
    /// Interaction logic for MainAppWindow.xaml
    /// </summary>
    public partial class MainAppWindow : Window
    {
        string erabiltzailea; //aplikazioaren erabiltzailea
        public DateTime hogeiFromNow { get; set; }  //hemendik 20 egunera data

        List<Erreserba> erreserbak = new List<Erreserba>(); // erreserbak kargatzeko listan

        List<Produktua> biltegia = null;  //biltegia kargatzeko listan
        //tiketan dauden produktuen lista, ObservableCollection da Datagrid-a eguneratzeko automatikoki, refresh() ez egiteko
        public ObservableCollection<Produktua> TiketarenProduktuak { get; set; } = new ObservableCollection<Produktua>(); 

        decimal kontuTotala = 0; // kontutotala kontsumitutako produktu kantitateen arabera
        public MainAppWindow(string erabiltzailea)
        {
            InitializeComponent();
            DataContext = this;
            this.Title = $"{erabiltzailea.ToUpper()} menua";
            this.erabiltzailea = erabiltzailea;
            txt_erabiltzailea.Text = erabiltzailea.ToUpper();

            //Erreserbak egiteko
            hogeiFromNow = DateTime.Today.AddDays(20); //erreserbak bakarrik egin daitezke gaurtik 20 egunera
            datePickerReserva.SelectedDate = DateTime.Today;

            //biltegia kargatuko dugu behar dugun momentuetan kontsultatzeko
            biltegia = erabiltzaileenKlasea.kargatuBiltegia();
        }

        // ateratzerakoan LOGIN lehiora bueltatzen da
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            this.Close();
        }

        /////////ERRESERBEN METODOAK///////////////
        private void Button_Reservas_Click(object sender, RoutedEventArgs e)
        {
            // ikusi interesatzen zaigun menua
            GestionView.Visibility = Visibility.Collapsed;
            ReservasView.Visibility = Visibility.Visible;
        }

        private void radio_bazkaria_Checked(object sender, RoutedEventArgs e)
        {
            gridaMahaiena.Children.Clear(); // mahaien grida garbitu
            erreserbak = erabiltzaileenKlasea.kargatuErreserbak("janaria", datePickerReserva.SelectedDate.Value);
            mahaiakEzarri(erreserbak);
        }

        private void radio_afaria_Checked(object sender, RoutedEventArgs e)
        {
            gridaMahaiena.Children.Clear(); // mahaien grida garbitu
            erreserbak = erabiltzaileenKlasea.kargatuErreserbak("afaria", datePickerReserva.SelectedDate.Value);
            mahaiakEzarri(erreserbak);
        }

        //kargatu behar ditu ezarritako egunerako dauden erreserbak radio botoiko aukeraren arabera
        private void mahaiakEzarri(List<Erreserba> erreserbak)
        {
            gridaMahaiena.Children.Clear(); // mahaien grida garbitu
            int guztiraMahaiak = 5; // lokaleko mahai kopurua

            for (int i = 0; i < guztiraMahaiak; i++)
            {
                bool erreserbatuta = erreserbak.Any(e => e.mahaizenbakia == i + 1);

                TPV_Mahaia mahaiaUC = new TPV_Mahaia();
                mahaiaUC.Mahaia.NumeroMesa = i + 1;
                mahaiaUC.Mahaia.Estado = erreserbatuta ? EstadoMesa.Ocupado : EstadoMesa.Libre;


                Grid.SetRow(mahaiaUC, i);
                Grid.SetColumn(mahaiaUC, 0);

                gridaMahaiena.Children.Add(mahaiaUC);
            }
        }

        // hautatutako data aldatzean garbituko dira elementuak
        private void datePickerReserva_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridaMahaiena != null)
            {
                gridaMahaiena.Children.Clear();
                radio_bazkaria.IsChecked = false;
                radio_afaria.IsChecked = false;
            }
        }

        //datubasean gordetzen du erreserba bat ezarritako mahai edo mahaiekin logeatuta dagoen erabiltzailearentzako
        private void btn_erreserbatu_Click(object sender, RoutedEventArgs e)
        {
            var aukeratuta = checkAukeratutakoMahaiak();
            if (aukeratuta.Length == 0)
            {
                MessageBox.Show("Ez dago mahairik aukeratuta");
            }
            else
            {
                if (radio_bazkaria.IsChecked == true) //bazkarirako + konfirmazio mezua
                {
                    erabiltzaileenKlasea.erreserbatuMahaiak(aukeratuta, erabiltzailea, datePickerReserva.SelectedDate.Value, "janaria");
                    MessageBox.Show(
                                    $"===== ERRESERBA =====\n" +
                                    $"Erabiltzailea: {erabiltzailea.ToUpper()}\n" +
                                    $"Mahaiak: {aukeratuta}\n" +
                                    $"Eguna: {datePickerReserva.SelectedDate:dd/MM/yyyy}\n" +
                                    $"Janordua: janaria\n" +
                                    $"=====================",
                                    "Erreserba sortuta"
                                     );

                }
                else // afarirako + konfirmazio mezua
                {
                    erabiltzaileenKlasea.erreserbatuMahaiak(aukeratuta, erabiltzailea, datePickerReserva.SelectedDate.Value.Date, "afaria");
                    MessageBox.Show(
                                    $"===== ERRESERBA =====\n" +
                                    $"Erabiltzailea: {erabiltzailea.ToUpper()}\n" +
                                    $"Mahaiak: {aukeratuta}\n" +
                                    $"Eguna: {datePickerReserva.SelectedDate:dd/MM/yyyy}\n" +
                                    $"Janordua: afaria\n" +
                                    $"=====================",
                                    "Erreserba sortuta"
                                     );
                }
                gridaMahaiena.Children.Clear();
                radio_bazkaria.IsChecked = false;
                radio_afaria.IsChecked = false;

            }
        }

        // erreserbatutako botoiari ematean zein Mahai aukeratuta dauden bueltatuko digu string batean
        private string checkAukeratutakoMahaiak()
        {
            var seleccionados = gridaMahaiena.Children
            .OfType<TPV_Mahaia>()
            .Where(s => s.Mahaia.Estado == EstadoMesa.Seleccionado)
            .Select(s => s.Mahaia.NumeroMesa.ToString());
            return string.Join(" ", seleccionados);
        }

        /////////KUDEAKETARAKO METODOAK///////////////
        private void Button_Gestion_Click(object sender, RoutedEventArgs e)
        {
            // ikusi interesatzen zaigun menua
            ReservasView.Visibility = Visibility.Collapsed;
            GestionView.Visibility = Visibility.Visible;
            mahaia_combo.Items.Clear();
            janordua_combo.Items.Clear ();
            //kargatu mahaiak comboboxerako
            int mahaiak = erabiltzaileenKlasea.kargatuMahaiak();
            //mahaiaren comboboxari ezarri mahaiak
            for (int i = 1; i <= mahaiak; i++)
            {
                mahaia_combo.Items.Add(i.ToString());
            }
            //janorduko Comboboxari ezarri datuak
            janordua_combo.Items.Add("janaria");
            janordua_combo.Items.Add("afaria");
            ezarriKategoriak(biltegia);

        }

        // kategoriak comboboxean datubasean dauden kategoria guztiak insertatzen ditu
        private void ezarriKategoriak(List<Produktua> biltegia)
        {
            kategoriak_combo.Items.Clear(); // garbitu lehenengo

            //kategoriak ezarri comboboxean
            List<string> kategoriakUnikoak = new List<string>();
            foreach (Produktua produktua in biltegia)
            {
                string kategoria = produktua.Kategoria;

                if (!kategoriakUnikoak.Contains(kategoria))
                {
                    kategoriakUnikoak.Add(kategoria);
                    kategoriak_combo.Items.Add(kategoria);
                }
            }
        }


        // kategorien comboboxeko kateoria bat aukeratzen dugunean, kategoria horretako produktuak kargatuko ditu produktuak_combo comboboxean
        private void kategoriak_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            produktuak_combo.Items.Clear();

            if (kategoriak_combo.SelectedItem != null)
            {
                ezarriProduktuak(biltegia, kategoriak_combo.SelectedItem.ToString());
            }
        }


        // ezarritako kategoriaren arabera produktuen comboboxean ezarriko ditu bere produktuak
        private void ezarriProduktuak(List<Produktua> biltegia, string kategoria)
        {
            foreach (Produktua produktua in biltegia)
            {
                if (produktua.Kategoria.ToString() == kategoria)
                {
                    string item = produktua.Izena.ToString();
                    produktuak_combo.Items.Add(item);
                }
                
            }
        }

        // bakarrik zenbaki osoak sartu ahal izateko kantitatean
        private void kantitatea_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        // datagridean sartuko du guk ezarritako produktua eta bere kantitatea
        private void erantsi_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(kantitatea_txt.Text) ||
                !int.TryParse(kantitatea_txt.Text, out int kantitatea) ||
                kantitatea <= 0)
            {
                MessageBox.Show("Sartu zenbaki oso positibo bat kantitate gisa.", "Errorea");
                kantitatea_txt.Clear();
                return;
            }
            eguneratuTiket(kantitatea); // hemen kudeatzen du datagrid-a
            kantitatea_txt.Clear();
        }


        // datagridan eransten du gure selekzioa
        private void eguneratuTiket(int kantitatea)
        {
            string izenaAukatua = produktuak_combo.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(izenaAukatua))
            {
                MessageBox.Show("Aukeratu produktu bat.", "Errorea");
                return;
            }

            Produktua aurkituta = biltegia.FirstOrDefault(prod => prod.Izena.Equals(izenaAukatua));

            if (aurkituta == null)
            {
                MessageBox.Show("Produktua ez da aurkitu biltegian.", "Errorea");
                return;
            }

            decimal prezioOsoa = aurkituta.Prezioa * kantitatea;

            // Produktua klasearen bigarren konstruktorea erabiltzen dugu kasu honetan
            var newItem = new Produktua(
                aurkituta.Izena,
                kantitatea,
                prezioOsoa
            );
            TiketarenProduktuak.Add(newItem);

            // eguneratu kontu totalaren kantitatea textboxean produktu bat sartzen den bakoitzean
            kontuTotala += prezioOsoa;
            kontu_totala_txt.Text = kontuTotala.ToString();
        }


        //ordainketa egiterakoan, tiketa gordeko da datu basean eta kontsumitutako produktuen stocka murriztuko da.
        private void btn_ordainketa_Click(object sender, RoutedEventArgs e)
        {
            string tiketa = SortuTiketa();
            MessageBox.Show(tiketa, "Tiketa");
            // garbitu elementu guztiak
            mahaia_combo.Items.Clear();
            janordua_combo.Items.Clear();
            kategoriak_combo.Items.Clear();
            kontu_totala_txt.Clear();
            TiketarenProduktuak.Clear();
            kontuTotala = 0;
        }

        // string bat sortzen da tiketeko elementuekin
        private string SortuTiketa()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("===== TIKETA =====");

            string mahaia = mahaia_combo.SelectedItem?.ToString() ?? "—";
            string janordua = janordua_combo.SelectedItem?.ToString() ?? "—";

            sb.AppendLine($" Mahaia: {mahaia}   Janordua: {janordua}");
            sb.AppendLine();

            foreach (var prod in TiketarenProduktuak)
            {
                sb.AppendLine($"{prod.Izena}  x{prod.HautatutakoKantitatea}  →  {prod.Prezioa:C2}");
            }

            sb.AppendLine();
            sb.AppendLine($"GUZTIRA: {kontuTotala:C2}");
            sb.AppendLine("==================");

            return sb.ToString();
        }


    }
}
