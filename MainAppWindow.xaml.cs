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
    /// Interaction logic for MainAppWindow.xaml
    /// </summary>
    public partial class MainAppWindow : Window
    {
        public MainAppWindow(bool user, string erabiltzailea)
        {
            InitializeComponent();
            if (user)
            {
                // admin menua
                this.Title = "administratzaile menua";
            }
            else
            {
                // erabiltzaile menua
                this.Title = $"{erabiltzailea} menua";
            }
        }
    }
}
