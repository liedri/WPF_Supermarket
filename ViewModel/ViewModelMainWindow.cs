using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace Supermarket.ViewModel
{

    


    class ViewModelMainWindow: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UserControl _UserControl;
        public UserControl UserControl
        {
            get { return _UserControl; }
            set
            {
                _UserControl = value;
                PropertyChanged(this, new PropertyChangedEventArgs(null));
            }
        }

        public ViewModelMainWindow()
        {
        }
    }
}
