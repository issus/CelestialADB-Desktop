using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private string pathToDbLib;

        public string PathToDbLib
        {
            get { return pathToDbLib; }
            set
            {
                pathToDbLib = value;

                RaisePropertyChanged("PathToDbLib");
            }
        }

    }
}
