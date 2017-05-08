using GalaSoft.MvvmLight;
using Harris.CelestialADB.Desktop.Helpers;
using Harris.CelestialADB.Desktop.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }

        public SettingsViewModel()
        {
            IntroBoxVisible = Properties.Settings.Default.ShowIntroBox;

            HideIntroBoxCommand = new DelegateCommand(() => { IntroBoxVisible = false; Properties.Settings.Default.ShowIntroBox = false; Properties.Settings.Default.Save(); });
            BrowseForDbLibCommand = new DelegateCommand(BrowseForDbLib);
            UpdateUdlFileCommand = new DelegateCommand(UpdateUdlFile, () => !String.IsNullOrEmpty(PathToDbLib) && File.Exists(PathToDbLib));

            PathToDbLib = Properties.Settings.Default.AltiumDbPath;
            if (!String.IsNullOrEmpty(PathToDbLib))
            {
                PathToUdl = AltiumFile.ReadUdlLocation(PathToDbLib);
            }
        }

        void UpdateUdlFile()
        {
            ShowUdlSuccessMessage = false;
            ShowUdlErrorMessage = false;
            UdlMessage = "";

            if (String.IsNullOrEmpty(MainViewModel.LoginRegisterViewModel.Password))
            {
                MainViewModel.LoginRegisterViewModel.UserIsLoggedIn = false;
                return;
            }

            string newUdlPath = Path.ChangeExtension(PathToDbLib, "udl");
            var cred = new UdlCredentials
            {
                Username = MainViewModel.LoginRegisterViewModel.Username,
                Password = MainViewModel.LoginRegisterViewModel.Password,
                Server = "csql.database.windows.net"
            };

            if (AltiumFile.CreateMsSqlUdlFile(newUdlPath, cred))
            {
                if (AltiumFile.SetUdlLocationInDbLib(PathToDbLib, newUdlPath))
                {
                    PathToUdl = AltiumFile.ReadUdlLocation(PathToDbLib);
                    ShowUdlSuccessMessage = true;
                    UdlMessage = "Successfully created/updated the UDL file with your Azure credentials, and set the file path in the Altium DbLib.";
                }
                else
                {
                    ShowUdlErrorMessage = true;
                    UdlMessage = "Could not update Altium DbLib file with new UDL file location - check Altium is not open, and you have permission to edit the file!";
                }
            }
            else
            {
                ShowUdlErrorMessage = true;
                UdlMessage = "Could not create the UDL file on disk - check you have permission to create a file here!";
            }

        }

        void BrowseForDbLib()
        {
            var dblib = AltiumFile.BrowseForDbLib();

            if (dblib.Success)
            {
                PathToDbLib = dblib.Path;
                Properties.Settings.Default.AltiumDbPath = dblib.Path;
                Properties.Settings.Default.Save();

                PathToUdl = dblib.ConnectionStringPath;
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
                UpdateUdlFileCommand.RaiseCanExecuteChanged();
            }
        }

        private string pathToUdl;
        public string PathToUdl
        {
            get { return pathToUdl; }
            set
            {
                pathToUdl = value.Replace("\\\\", "\\");
                RaisePropertyChanged("PathToUdl");

                UdlExists = File.Exists(pathToUdl);
            }
        }

        private bool udlExists;
        public bool UdlExists
        {
            get { return udlExists; }
            set
            {
                udlExists = value;
                RaisePropertyChanged("UdlExists");
            }
        }

        private string udlMessage;
        public string UdlMessage
        {
            get { return udlMessage; }
            set
            {
                udlMessage = value;
                RaisePropertyChanged("UdlMessage");
            }
        }


        private bool showUdlSuccessMessage;
        public bool ShowUdlSuccessMessage
        {
            get { return showUdlSuccessMessage; }
            set
            {
                showUdlSuccessMessage = value;
                RaisePropertyChanged("ShowUdlSuccessMessage");
            }
        }

        private bool showUdlErrorMessage;
        public bool ShowUdlErrorMessage
        {
            get { return showUdlErrorMessage; }
            set
            {
                showUdlErrorMessage = value;
                RaisePropertyChanged("ShowUdlErrorMessage");
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
        public ICommand UpdateUdlFileCommand { get; private set; }
    }
}
