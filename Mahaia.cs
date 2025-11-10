using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace erronkaTPVsistema
{
    public class Mahaia : INotifyPropertyChanged
    {
        private EstadoMesa egoera;
        private int zenbakia;

        public Mahaia(EstadoMesa estado, int numeroAsiento)
        {
            this.egoera = estado;
            this.zenbakia = numeroAsiento;
        }

        public EstadoMesa Estado
        {
            get { return egoera; }
            set
            {
                if (egoera != value)
                {
                    egoera = value;
                    OnPropertyChanged(); // Notifica a la UI que el valor cambió
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string nombrePropiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombrePropiedad));
        }
    }
}
