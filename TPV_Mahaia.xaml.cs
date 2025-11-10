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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace erronkaTPVsistema
{
    /// <summary>
    /// Interaction logic for TPV_Mahaia.xaml
    /// </summary>
    public partial class TPV_Mahaia : UserControl
    {
        public Mahaia Mahaia{ get; set; }
        public event EventHandler EstadoCambiado;

        private void SillaButton_Click(object sender, RoutedEventArgs e)
        {
            if (Mahaia.Estado == EstadoMesa.Libre)
                Mahaia.Estado = EstadoMesa.Seleccionado;
            else if (Mahaia.Estado == EstadoMesa.Seleccionado)
                Mahaia.Estado = EstadoMesa.Libre;

            EstadoCambiado?.Invoke(this, EventArgs.Empty); // aldaketa jakinarazten du
        }


        public TPV_Mahaia()
        {
            InitializeComponent();
            Mahaia = new Mahaia(EstadoMesa.Libre, 1);
            this.DataContext = Mahaia;
        }
    }
}