using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronkaTPVsistema
{
    // objetua erreserba baterako
    internal class Erreserba
    {
        public int mahaizenbakia { get; set; } // erreserbarako mahai zenbakia
        public DateTime erreserbadata { get; set; }  // erreserbaren data
        public string noiz { get; set; } // noiz, janaria edo afaria

        public Erreserba(int mahaizenbakia, DateTime erreserbadata, string noiz)
        {
            this.mahaizenbakia = mahaizenbakia;
            this.erreserbadata = erreserbadata;
            this.noiz = noiz.Trim();
        }
    }
}
