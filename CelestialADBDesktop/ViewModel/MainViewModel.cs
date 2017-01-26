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

        private DateTime buildDate;

        private bool isUserLoggedIn;

        private int windowWidth;

        private int windowHeight;

        private bool showRegisterLogin;

        public MainViewModel()
        {
            version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            buildDate = DateTime.Now;

            WindowHeight = 900;
            WindowWidth = 1200;


            if (System.Windows.SystemParameters.PrimaryScreenHeight < 800)
            {
                WindowHeight = 700;
            }
            else if (System.Windows.SystemParameters.PrimaryScreenHeight <= 900)
            {
                WindowHeight = 800;
            }
            else
            {
                WindowHeight = 900;
            }

            if (System.Windows.SystemParameters.PrimaryScreenWidth < 1300)
            {
                WindowWidth = 1100;
            }
            else
            {
                WindowWidth = 1200;
            }

            isUserLoggedIn = false;
            showRegisterLogin = true;

            LoginRegisterViewModel = new LoginRegisterViewModel();
        }

        public LoginRegisterViewModel LoginRegisterViewModel { get; set; }

        public bool ShowRegisterLogin {
            get
            {
                return showRegisterLogin;
            }
            set
            {
                this.showRegisterLogin = value;

                RaisePropertyChanged("ShowRegisterLogin");
            }
        }

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

        public DateTime BuildDate
        {
            get
            {
                return this.buildDate;
            }
            set
            {
                if (buildDate == value)
                    return;

                this.buildDate = value;

                RaisePropertyChanged("BuildDate");
            }
        }
    }
}