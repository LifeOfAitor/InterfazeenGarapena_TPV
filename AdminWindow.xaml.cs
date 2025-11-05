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
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void sortu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            //sortu lehio berria erabiltzailea sortzeko
        }

        private void aldatu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            //adldatzeko izena edo pasahitza
        }

        private void ezabatu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void begiratu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            list_box_lista.Items.Clear();
            erakutsiErabiltzaileak();
            list_box_lista.Visibility = Visibility.Visible;
        }

        private void begiratu_biltegia_Click(object sender, RoutedEventArgs e)
        {

        }

        private void list_box_lista_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {
            if (list_box_lista.SelectedItem != null)
            {
                btn_aldatu.Visibility = Visibility.Visible;
                btn_ezabatu.Visibility = Visibility.Visible;
            }
            else
            {
                btn_aldatu.Visibility = Visibility.Hidden;
                btn_ezabatu.Visibility = Visibility.Hidden;
            }
        }

        private void erakutsiErabiltzaileak()
        {
            List<string> erabiltzaileak = erabiltzaileenKlasea.kargatuErabiltzaileak();
            foreach (string erabiltzailea in erabiltzaileak)
            {
                list_box_lista.Items.Add(erabiltzailea);
            }
        }

    }
}
