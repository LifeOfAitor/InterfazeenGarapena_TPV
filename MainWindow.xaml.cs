using System;
using System.Threading.Tasks;
using System.Windows;

namespace erronkaTPVsistema
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 🔹 Hacemos el método async para poder usar await
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Validamos campos vacíos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                txt_erroreak.Text = "Erabiltzaile edo pasahitza hutsik daude.";
                return;
            }

            // 🔹 Conectamos con la base de datos
            await erabiltzaileenKlasea.ConnectDatabaseAsync("jatetxea", "admin");

            // 🔹 Verificamos si el usuario existe
            bool erabiltzaileZuzena = await erabiltzaileenKlasea.checkErabiltzaileak(txtUsuario.Text, txtPassword.Password);

            if (erabiltzaileZuzena)
            {
                // 🔹 Verificamos si es admin
                bool adminDa = await erabiltzaileenKlasea.checkAdmin(txtUsuario.Text);

                if (adminDa)
                {
                    MessageBox.Show("Administrazio erabiltzailea");
                    var window = new MainAppWindow(true, txtUsuario.Text);
                    window.Show();
                }
                else
                {
                    MessageBox.Show("Erabiltzaile arrunta");
                    var window = new MainAppWindow(false, txtUsuario.Text);
                    window.Show();
                }

                // Cerramos la ventana de login
                this.Close();
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
