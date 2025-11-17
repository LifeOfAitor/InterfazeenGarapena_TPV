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
    /// Interaction logic for sortu_editatu.xaml
    /// </summary>
    
    // erabiltzaileak sortzeko edo editatzeko erabiliko dugu klase hau. Window bat da. Ezartzen diogun moduaren arabera egingo du gauza bat edo bestea.
    public partial class sortu_editatu : Window
    {
        string modua = null;
        List<string> erabiltzaileak = null;
        public sortu_editatu(string modua, List<string> erabiltzaileak, string erabiltzailea)
        {
            InitializeComponent();
            this.modua = modua;
            this.erabiltzaileak = erabiltzaileak;
            if (modua == "editatu")
            {
                txtbox_izena.Text = erabiltzailea;
                txtbox_izena.IsReadOnly = true; // izena ez da aldatzen hasieran
            }
        }

        // erabiltzaile izena existitzen den konprobatuko dugu hemendik, ez errepikatzeko
        private bool konprobatuErabiltzailea()
        {
            foreach (string izena in erabiltzaileak)
            {
                if (izena == txtbox_izena.Text)
                {
                    MessageBox.Show("Erabiltzaile hori existitzen da, mesedez aukeratu beste izen bat");
                    txtbox_izena.Clear();
                    return false;
                }
            }
            return true;
        }

        // ezarri dugun erabiltzailea datu basean gordeko dugu hemendik
        private void btn_gorde_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_izena.Text == "" || txtbox_pasahitza.Text == "")
            {
                MessageBox.Show("Mesedez, bete eremu guztiak");
                return;
            }
            switch (modua)
            {
                case "sortu":
                    if (konprobatuErabiltzailea())
                    {
                        erabiltzaileenKlasea.sortuErabiltzailea(txtbox_izena.Text, txtbox_pasahitza.Text);
                        this.Close();
                    }
                    break;
                case "editatu":
                    erabiltzaileenKlasea.aldatuErabiltzailea(txtbox_izena.Text, txtbox_pasahitza.Text);
                    this.Close();
                    break;
                default:
                    break;
            }

        }

        // klasea funtzionatzeko bakarrik, kasurik ez egin (errorea ematen du bestela)
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
