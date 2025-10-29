using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace erronkaTPVsistema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Password.ToString()))
            {
                txt_erroreak.Text = "Erabiltzaile edo pasahitza okerrak";
                return;
            }
            if (erabiltzaileenKlasea.checkErabiltzaileak(txtUsuario.Text, txtPassword.Password))
            {
                if (txtUsuario.Text == bbdd.erabiltzaileak.admin)
                {
                    aplikazioa.erabiltzailea = "admin";
                    MessageBox.Show("Administrazio erabiltzailea");
                    Window window = new MainAppWindow(true, txtUsuario.Text);
                }

            }
            else 
            {
                txt_erroreak.Text = "Erabiltzaile edo pasahitza okerrak";
                txtUsuario.Clear();
                txtPassword.Clear();
            }
                
        }
    }
}