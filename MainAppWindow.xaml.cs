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
        const int MAHAIKOPURUA = 5;
        public DateTime hogeiFromNow { get; set; }
        public MainAppWindow(string erabiltzailea)
        {
            InitializeComponent();
            this.Title = $"{erabiltzailea} menua";

            //Erreserbak egiteko
            hogeiFromNow = DateTime.Today.AddDays(20); //erreserbak bakarrik egin daitezke gaurtik 20 egunera
            DataContext = this;

            datePickerReserva.SelectedDate = DateTime.Today;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            mahaiakEzarri("bazkaria");
        }

        private void radio_afaria_Checked(object sender, RoutedEventArgs e)
        {
            mahaiakEzarri("afaria");
        }

        //kargatu behar ditu ezarritako egunerako dauden erreserbak radio botoiko aukeraren arabera
        private void mahaiakEzarri(string noiz)
        {
            gridaMahaiena.Children.Clear(); // garbitu grida

        }

    }
}
