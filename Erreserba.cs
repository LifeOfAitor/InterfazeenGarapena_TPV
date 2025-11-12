using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronkaTPVsistema
{
    internal class Erreserba
    {
        public int mahaizenbakia { get; set; }
        public DateTime erreserbadata { get; set; }
        public string noiz { get; set; }

        public Erreserba(int mahaizenbakia, DateTime erreserbadata, string noiz)
        {
            this.mahaizenbakia = mahaizenbakia;
            this.erreserbadata = erreserbadata;
            this.noiz = noiz.Trim();
        }
    }
}
