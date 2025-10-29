using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronkaTPVsistema
{
    static class erabiltzaileenKlasea
    {
        public static bool checkErabiltzaileak(string erabiltzaile, string pasahitza)
        {
            if (erabiltzaile == bbdd.erabiltzaileak.admin && pasahitza == bbdd.pasahitzak.adminPasahitza)
            {
                return true;
            }
            else if (erabiltzaile == bbdd.erabiltzaileak.user && pasahitza == bbdd.pasahitzak.userPasahitza)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
