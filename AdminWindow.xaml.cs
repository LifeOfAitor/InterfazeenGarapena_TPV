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
        public AdminWindow()
        {
            InitializeComponent();
            erabiltzaileak = erabiltzaileenKlasea.kargatuErabiltzaileak();
        }

        private void sortu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            Window sortuErabiltzaileaWindow = new sortu_editatu("sortu", erabiltzaileak, null);
            sortuErabiltzaileaWindow.ShowDialog();
            list_box_lista.Items.Clear();
            erakutsiErabiltzaileak();
            list_box_lista.Visibility = Visibility.Visible;
        }

        private void aldatu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            Window sortuErabiltzaileaWindow = new sortu_editatu("editatu", erabiltzaileak, list_box_lista.SelectedItem.ToString().Trim());
            sortuErabiltzaileaWindow.ShowDialog();
        }

        private void ezabatu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            erabiltzaileenKlasea.ezabatuErabiltzailea(list_box_lista.SelectedItem.ToString().Trim());
            list_box_lista.Items.Clear();
            erakutsiErabiltzaileak();
        }

        private void begiratu_erabiltzailea_Click(object sender, RoutedEventArgs e)
        {
            erabiltzaileak_grid.Visibility = Visibility.Visible;
            biltegia_grid.Visibility = Visibility.Hidden;
            list_box_lista.Items.Clear();
            erakutsiErabiltzaileak();
            list_box_lista.Visibility = Visibility.Visible;
        }

        private void begiratu_biltegia_Click(object sender, RoutedEventArgs e)
        {
            biltegia_grid.Visibility = Visibility.Visible;
            erabiltzaileak_grid.Visibility = Visibility.Hidden;
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
            erabiltzaileak = erabiltzaileenKlasea.kargatuErabiltzaileak();
            foreach (string erabiltzailea in erabiltzaileak)
            {
                list_box_lista.Items.Add(erabiltzailea);
            }
        }

    }
}
