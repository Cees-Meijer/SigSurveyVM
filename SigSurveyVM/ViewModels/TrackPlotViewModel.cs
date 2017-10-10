using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using System.ComponentModel;

namespace SurveyVM
{
    class TrackPlotViewModel  : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private double yaxis_min;
        private double yaxis_max;
        private double xaxis_min;
        private double xaxis_max;

        public TrackPlotViewModel()
        {
            this.Title = "Track Plot";
            this.Points = new List<DataPoint> {  };
            this.YAxisMinimum = 1000;
            this.YAxisMaximum = 100000;
            this.XAxisMinimum = 1000;
            this.XAxisMaximum = 100000;

        }
        public string Title { get; private set; }
                
        public double YAxisMinimum { get { return yaxis_min; } set { yaxis_min = value; RaisePropertyChanged("YAxisMinimum"); } }
        public double YAxisMaximum { get { return yaxis_max; } set { yaxis_max = value; RaisePropertyChanged("YAxisMaximum"); } }
        public double XAxisMinimum { get { return xaxis_min; } set { xaxis_min = value; RaisePropertyChanged("XAxisMinimum"); } }
        public double XAxisMaximum { get { return xaxis_max; } set { xaxis_max = value; RaisePropertyChanged("XAxisMaximum"); } }
        
        public IList<DataPoint> Points { get; private set; }
        
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
