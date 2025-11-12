using erronkaTPVsistema;
using System;
using System.Collections.Generic;
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
        string erabiltzailea;
        const int MAHAIKOPURUA = 5;
        public DateTime hogeiFromNow { get; set; }

        List<Erreserba> erreserbak = new List<Erreserba>();
        public MainAppWindow(string erabiltzailea)
        {
            InitializeComponent();
            this.Title = $"{erabiltzailea} menua";
            this.erabiltzailea = erabiltzailea ;

            //Erreserbak egiteko
            hogeiFromNow = DateTime.Today.AddDays(20); //erreserbak bakarrik egin daitezke gaurtik 20 egunera
            DataContext = this;

            datePickerReserva.SelectedDate = DateTime.Today;
        }

        // ateratzerakoan LOGIN lehiora bueltatzen da
        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Button_Reservas_Click(object sender, RoutedEventArgs e)
        {
            // ikusi interesatzen zaigun menua
            GestionView.Visibility = Visibility.Collapsed;
            ReservasView.Visibility = Visibility.Visible;
        }

        private void Button_Gestion_Click(object sender, RoutedEventArgs e)
        {
            // ikusi interesatzen zaigun menua
            ReservasView.Visibility = Visibility.Collapsed;
            GestionView.Visibility = Visibility.Visible;
        }

        private void radio_bazkaria_Checked(object sender, RoutedEventArgs e)
        {
            gridaMahaiena.Children.Clear(); // limpiar grid
            erreserbak = erabiltzaileenKlasea.kargatuErreserbak("janaria", datePickerReserva.SelectedDate.Value);
            mahaiakEzarri(erreserbak);
        }

        private void radio_afaria_Checked(object sender, RoutedEventArgs e)
        {
            gridaMahaiena.Children.Clear(); // limpiar grid
            erreserbak = erabiltzaileenKlasea.kargatuErreserbak("afaria", datePickerReserva.SelectedDate.Value);
            mahaiakEzarri(erreserbak);
        }

        //kargatu behar ditu ezarritako egunerako dauden erreserbak radio botoiko aukeraren arabera
        private void mahaiakEzarri(List<Erreserba> erreserbak)
        {
            gridaMahaiena.Children.Clear(); // limpiar grid
            int guztiraMahaiak = 5;

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
                if (radio_bazkaria.IsChecked == true)
                {
                    erabiltzaileenKlasea.erreserbatuMahaiak(aukeratuta, erabiltzailea, datePickerReserva.SelectedDate.Value, "janaria");
                    MessageBox.Show($"{erabiltzailea}k sortu du erreserba. {aukeratuta} | {datePickerReserva.SelectedDate.Value.Date} | janarirako");
                }
                else
                {
                    erabiltzaileenKlasea.erreserbatuMahaiak(aukeratuta, erabiltzailea, datePickerReserva.SelectedDate.Value.Date, "afaria");
                    MessageBox.Show($"{erabiltzailea}k sortu du erreserba. {aukeratuta} | {datePickerReserva.SelectedDate.Value} | afarirako");
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
    }
}
