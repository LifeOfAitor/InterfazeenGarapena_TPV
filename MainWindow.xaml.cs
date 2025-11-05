using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace erronkaTPVsistema
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //hutsik badago, errore mezua erakutsi
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                txt_erroreak.Text = "Erabiltzaile edo pasahitza hutsik daude.";
                return;
            }
            //datubasera konektatuko gara, kasu honetan datubasearen izena "jatetxea" da
            await erabiltzaileenKlasea.ConnectDatabaseAsync("jatetxea");
            bool erabiltzaileZuzena = await erabiltzaileenKlasea.checkErabiltzaileak(txtUsuario.Text, txtPassword.Password);

            //erabiltzailea existitzen bada, erabiltzaile motaren arabera leihoa irekiko dugu
            if (erabiltzaileZuzena)
            {
                bool adminDa = await erabiltzaileenKlasea.checkAdmin(txtUsuario.Text);

                //admin lehioa ireki
                if (adminDa)
                {
                    //MessageBox.Show("Administrazio erabiltzailea"+ adminDa);
                    var window = new AdminWindow();
                    window.Show();
                }
                // erabiltzaile arrunta lehioa ireki
                else
                {
                    //MessageBox.Show("Erabiltzaile arrunta" +adminDa);
                    var window = new MainAppWindow(txtUsuario.Text);
                    window.Show();
                }

                // login lehioa itxi, ez dugu behar
                this.Close();
            }
            else
            {
                txt_erroreak.Text = "Erabiltzaile edo pasahitza okerrak";
                txtUsuario.Clear();
                txtPassword.Clear();
            }
        }

        //enter sakatuz login egin ahal izateko
        //XAML fitxategian jarri PreviewKeyDown="Window_PreviewKeyDown"
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(this, new RoutedEventArgs());
            }
        }

    }
}
