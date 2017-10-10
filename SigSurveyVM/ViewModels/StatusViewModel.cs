using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigSurveyVM.ViewModels
{
    class StatusViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _AD2CP_StatusText;
        private string _GPS_StatusText;

        public StatusViewModel() {
            AD2CP_StatusText = "AD2CP Status messages";
            GPS_StatusText = "GPS Status messages";
        }

        public string AD2CP_StatusText { get { return _AD2CP_StatusText; } set { _AD2CP_StatusText = value; RaisePropertyChanged("AD2CP_StatusText"); } }
        public string GPS_StatusText { get { return _GPS_StatusText; } set { _GPS_StatusText = value; RaisePropertyChanged("GPS_StatusText"); } }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
