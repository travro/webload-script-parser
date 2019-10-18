using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WLScriptParser.Utilities
{
    public sealed class AppLogger: INotifyPropertyChanged
    {
        private static AppLogger _appLogger = null;
        private string _logMessage;

        public event PropertyChangedEventHandler PropertyChanged;
        
        private AppLogger() { }

        public static AppLogger Log
        {
            get
            {
                if (_appLogger == null)
                {
                    _appLogger = new AppLogger();
                }
                return _appLogger;
            }
        }

        public void LogMessage(string message)
        {
            _logMessage = message;
            OnPropertyChanged();
        }

        private void OnPropertyChanged()
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_logMessage));
            }
        }
    }
}
