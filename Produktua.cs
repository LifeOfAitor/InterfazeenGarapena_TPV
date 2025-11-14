using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronkaTPVsistema
{
    // Biltegiko produktu bakoitza kargatzen denean bere informazioa gordeku dugun klase edo obj
    internal class Produktua
    {
        public string Izena { get; set; }
        public int Stock { get; set; }

        public string Kategoria { get; set; }

        public decimal Prezioa { get; set; }

        public int HautatutakoKantitatea { get; set; }

        public Produktua(string izena, int stock, string kategoria, decimal prezioa)
        {
            Izena = izena;
            Stock = stock;
            Kategoria = kategoria;
            Prezioa = prezioa;
        }
        public Produktua(string izena, int hautatutakoKantitatea, decimal prezioa)
        {
            Izena = izena;
            HautatutakoKantitatea = hautatutakoKantitatea;
            Prezioa = prezioa;
        }
    }
}
