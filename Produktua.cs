using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronkaTPVsistema
{
    // administrazio menutik erabiliko diren produktuentzako objetua
    internal class Produktua
    {
        public string Izena { get; set; }
        public int Stock { get; set; }

        public Produktua(string izena, int stock)
        {
            Izena = izena;
            Stock = stock;
        }
    }
}
