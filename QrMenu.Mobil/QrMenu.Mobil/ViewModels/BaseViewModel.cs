using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace QrMenu.Mobil.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation _navigation;
        //showing loading indicator while getting the data
        private bool _isLoading { get; set; }
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                if (value != _isLoading)
                {
                    _isLoading = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
