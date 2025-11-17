using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace erronkaTPVsistema
{
    // usercontroll mahaia. Hemen kudeatzen dira bere propietateak. Egoera eta zenbakia.
    public class Mahaia : INotifyPropertyChanged
    {
        private EstadoMesa egoera;
        private int zenbakia;

        // konstruktorea
        public Mahaia(EstadoMesa estado, int numeroAsiento)
        {
            this.egoera = estado;
            this.zenbakia = numeroAsiento;
        }

        // mahaiaren egoera kontrolatzen da hemendik
        public EstadoMesa Estado
        {
            get { return egoera; }
            set
            {
                if (egoera != value)
                {
                    egoera = value;
                    OnPropertyChanged(); // UI jakinarazten du egoera aldatu dela
                }
            }
        }

        // mahaiaren zenbakia kudeatzen da hemendik
        public int NumeroMesa
        {
            get { return zenbakia; }
            set
            {
                if (zenbakia != value)
                {
                    zenbakia = value;
                    OnPropertyChanged();
                }
            }
        }

        // denbora errealean aldaketak kudeatzeko erabiltzen da hau, adibidez egoera eta kolorea aldatzeko
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string nombrePropiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
        }
    }
}
