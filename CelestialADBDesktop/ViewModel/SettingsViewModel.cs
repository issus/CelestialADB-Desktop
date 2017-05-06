using GalaSoft.MvvmLight;
using Harris.CelestialADB.Desktop.Helpers;
using Harris.CelestialADB.Desktop.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            IntroBoxVisible = Properties.Settings.Default.ShowIntroBox;

            HideIntroBoxCommand = new DelegateCommand(() => { IntroBoxVisible = false; Properties.Settings.Default.ShowIntroBox = false; Properties.Settings.Default.Save(); });
            BrowseForDbLibCommand = new DelegateCommand(BrowseForDbLib);
        }

        void BrowseForDbLib()
        {
            var dblib = AltiumFile.BrowseForDbLib();

            if (dblib.Success)
            {
                PathToDbLib = dblib.Path;
                Properties.Settings.Default.AltiumDbPath = dblib.Path;
                Properties.Settings.Default.Save();
            }
        }

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

        private bool introBoxVisible;
        public bool IntroBoxVisible
        {
            get { return introBoxVisible; }
            set
            {
                introBoxVisible = value;
                RaisePropertyChanged("IntroBoxVisible");
            }
        }
        
        public ICommand HideIntroBoxCommand { get; private set; }
        public ICommand BrowseForDbLibCommand { get; private set; }
    }
}
