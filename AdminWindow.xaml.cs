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

namespace erronkaTPVsistema
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        List<string> erabiltzaileak = null;
        List<Produktua> biltegia = null;
        List<string> produktuak = null;

        public AdminWindow()
        {
            InitializeComponent();
            erabiltzaileak = erabiltzaileenKlasea.kargatuErabiltzaileak();
            biltegia = erabiltzaileenKlasea.kargatuBiltegia();

            BiltegiaGrid.Visibility = Visibility.Hidden;
            ErabiltzaileakGrid.Visibility = Visibility.Visible;

            erakutsiErabiltzaileak();
            ListBoxErabiltzaileak.Visibility = Visibility.Visible;
        }

        //erabiltzaileen izenekin zerrenda erakutsi
        private void BegiratuErabiltzaileak_Click(object sender, RoutedEventArgs e)
        {
            BiltegiaGrid.Visibility = Visibility.Hidden;
            ErabiltzaileakGrid.Visibility = Visibility.Visible;

            ListBoxErabiltzaileak.Items.Clear();
            erakutsiErabiltzaileak();
            ListBoxErabiltzaileak.Visibility = Visibility.Visible;

            BtnAldatuErabiltzailea.Visibility = Visibility.Hidden;
            BtnEzabatuErabiltzailea.Visibility = Visibility.Hidden;
        }

        private void SortuErabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            Window sortuErabiltzaileaWindow = new sortu_editatu("sortu", erabiltzaileak, null);
            sortuErabiltzaileaWindow.ShowDialog();

            // zerrenda eguneratu ixtean
            ListBoxErabiltzaileak.Items.Clear();
            erakutsiErabiltzaileak();
        }

        private void BegiratuBiltegia_Click(object sender, RoutedEventArgs e)
        {
            ErabiltzaileakGrid.Visibility = Visibility.Hidden;
            BiltegiaGrid.Visibility = Visibility.Visible;

            erakutsiBiltegia();

            BtnAldatuStocka.Visibility = Visibility.Hidden;
            BtnEzabatuProduktua.Visibility = Visibility.Hidden;
        }

        private void AldatuErabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            string erabiltzailea = ListBoxErabiltzaileak.SelectedItem.ToString().Trim();
            Window sortuErabiltzaileaWindow = new sortu_editatu("editatu", erabiltzaileak, erabiltzailea);
            sortuErabiltzaileaWindow.ShowDialog();

            //zerrenda eguneratu ixtean
            ListBoxErabiltzaileak.Items.Clear();
            erakutsiErabiltzaileak();
        }

        private void EzabatuErabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            // Usar el nuevo nombre del botón
            string erabiltzailea = ListBoxErabiltzaileak.SelectedItem.ToString().Trim();

            if (MessageBox.Show($"{erabiltzailea} erabiltzailea ezabatuko duzu benetan?", "Ezabatu bai / ez", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                erabiltzaileenKlasea.ezabatuErabiltzailea(erabiltzailea);
                ListBoxErabiltzaileak.Items.Clear();
                erakutsiErabiltzaileak();
            }
        }

        private void AldatuStocka_Click(object sender, RoutedEventArgs e)
        {
            // Hautatutako aukera egiaztatu ea Produktua klasekoa den
            if (ListBoxBiltegia.SelectedItem is Produktua hautatutakoa)
            {
                if (string.IsNullOrWhiteSpace(txt_stockBerria.Text))
                {
                    MessageBox.Show("Mesedez, sartu zenbaki bat stock berria ezartzeko.", "Sarrera falta");
                    return;
                }

                int stockBerria = 0;

                //zenbakia Integer moduan parseatu
                if (Int32.TryParse(txt_stockBerria.Text, out stockBerria))
                {
                    //ezin da negatiboa izan
                    if (stockBerria >= 0)
                    {
                        //stocka aldatu datu basean
                        erabiltzaileenKlasea.aldatuStock(hautatutakoa.Izena.Trim(), stockBerria);

                        txt_stockBerria.Clear();

                        // Lista eguneratu
                        ListBoxBiltegia.Items.Clear();
                        erakutsiBiltegia();
                    }
                    else
                    {
                        MessageBox.Show("Stock kopurua ezin da negatiboa izan. Mesedez, sartu 0 edo zenbaki positibo bat.", "Sarrera baliogabea");
                    }
                }
                else
                {
                    MessageBox.Show("Mesedez, sartu zenbaki oso bat stock-ean.", "Errorea");
                }
            }
        }

        private void EzabatuProduktua_Click(object sender, RoutedEventArgs e)
        {
            //hautatutako aukera egiaztatu ea Produktua klasekoa den
            if (ListBoxBiltegia.SelectedItem is Produktua hautatutakoa)
            {
                string izena = hautatutakoa.Izena;

                if (MessageBox.Show($"{izena} produktua ezabatuko duzu benetan?", "Ezabatu bai / ez", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    erabiltzaileenKlasea.ezabatuProduktua(izena);

                    //lista eguneratu
                    ListBoxBiltegia.Items.Clear();
                    erakutsiBiltegia();
                }
            }
        }


        private void ListBoxErabiltzaileak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxErabiltzaileak.SelectedItem != null)
            {
                BtnAldatuErabiltzailea.Visibility = Visibility.Visible;
                BtnEzabatuErabiltzailea.Visibility = Visibility.Visible;
            }
            else
            {
                BtnAldatuErabiltzailea.Visibility = Visibility.Hidden;
                BtnEzabatuErabiltzailea.Visibility = Visibility.Hidden;
            }
        }

        private void ListBoxBiltegia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxBiltegia.SelectedItem != null)
            {
                if (ListBoxBiltegia.SelectedItem is Produktua hautatutakoa)
                {
                    BtnAldatuStocka.Visibility = Visibility.Visible;
                    BtnEzabatuProduktua.Visibility = Visibility.Visible;

                    txt_stock.Visibility = Visibility.Visible;
                    //hautatutako produktuaren stocka textboxean ikusi
                    txt_stock.Text = hautatutakoa.Stock.ToString();
                }
            }
            else
            {
                //izkutatu
                BtnAldatuStocka.Visibility = Visibility.Hidden;
                BtnEzabatuProduktua.Visibility = Visibility.Hidden;
                txt_stock.Visibility = Visibility.Hidden;
            }
        }

        // erabiltzaileak zerrendan kargatu
        private void erakutsiErabiltzaileak()
        {
            
            erabiltzaileak = erabiltzaileenKlasea.kargatuErabiltzaileak();

            ListBoxErabiltzaileak.Items.Clear();
            foreach (string erabiltzailea in erabiltzaileak)
            {
                ListBoxErabiltzaileak.Items.Add(erabiltzailea);
            }
        }

        // biltegiko produktuak zerrendan kargatu
        private void erakutsiBiltegia()
        {
            biltegia = erabiltzaileenKlasea.kargatuBiltegia();
            ListBoxBiltegia.Items.Clear();

            foreach (Produktua i in biltegia)
            {
                ListBoxBiltegia.Items.Add(i);
            }
        }
    }
}