using GalaSoft.MvvmLight;
using Harris.CelestialADB.Desktop.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Reflection;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string version;
        private int windowWidth;
        private int windowHeight;

        public MainViewModel()
        {
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            WindowHeight = 900;
            WindowWidth = 1200;

            UserIsLoggedIn = false;


            if (SystemParameters.PrimaryScreenHeight < 800)
            {
                WindowHeight = 700;
            }
            else if (SystemParameters.PrimaryScreenHeight <= 900)
            {
                WindowHeight = 800;
            }
            else
            {
                WindowHeight = 900;
            }

            if (SystemParameters.PrimaryScreenWidth < 1300)
            {
                WindowWidth = 1100;
            }
            else
            {
                WindowWidth = 1200;
            }

            Date = DateTime.Now;

            LoginRegisterViewModel = new LoginRegisterViewModel();
            LoginRegisterViewModel.PropertyChanged += LoginRegisterViewModel_PropertyChanged;
        }

        private void LoginRegisterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserIsLoggedIn")
            {
                UserIsLoggedIn = LoginRegisterViewModel.UserIsLoggedIn;
            }
        }

        public LoginRegisterViewModel LoginRegisterViewModel { get; set; }

        public int WindowHeight
        {
            get
            {
                return this.windowHeight;
            }
            set
            {
                this.windowHeight = value;
                RaisePropertyChanged("WindowHeight");
            }
        }

        public int WindowWidth
        {
            get
            {
                return this.windowWidth;
            }
            set
            {
                this.windowWidth = value;
                RaisePropertyChanged("WindowWidth");
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                if (version == value)
                    return;

                this.version = value;

                RaisePropertyChanged("Version");
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }


        private bool userIsLoggedIn;
        public bool UserIsLoggedIn
        {
            get { return userIsLoggedIn; }
            set
            {
                userIsLoggedIn = value;
                RaisePropertyChanged("UserIsLoggedIn");
            }
        }

    }
}