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
using Harris.CelestialADB.Desktop.WebService;
using System.Windows.Input;
using Harris.CelestialADB.Desktop.WPF;
using System.Threading.Tasks;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDisposable
    {
        private string version;
        private int windowWidth;
        private int windowHeight;

        Timer firewallCheck;

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

            if (LoginRegisterViewModel.UserIsLoggedIn) // autologin from settings, doesnt fire property changed after attached
            {
                UserIsLoggedIn = true;

                UserHasLoggedIn();
            }

            FirewallRuleOk = false;
            firewallCheck = new Timer(CheckFirewallStatus);
            firewallCheck.Change(new TimeSpan(1, 0, 0), new TimeSpan(1, 0, 0));

            FirewallErrorButtonCommand = new DelegateCommand(async () => await UpdateFirewall());

            SettingsViewModel = new SettingsViewModel()
            {
                MainViewModel = this
            };
            AzureViewModel = new AzureViewModel();
            LocalDbViewModel = new LocalDbViewModel();
        }

        public async void CheckFirewallStatus(Object stateInfo)
        {
            await CheckFirewallStatusAsync(stateInfo);
        }

        public async Task CheckFirewallStatusAsync(Object stateInfo)
        {
            if (!UserIsLoggedIn)
                return;

            FirewallRuleOk = (await AltiumDbApi.CheckFirewallRule()).Success;
        }

        public async Task UpdateFirewall()
        {
            UpdatingFirewallRule = true;

            await AltiumDbApi.UpdateFirewallRule();
            await CheckFirewallStatusAsync(null);

            UpdatingFirewallRule = false;
        }

        private void LoginRegisterViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserIsLoggedIn")
            {
                UserIsLoggedIn = LoginRegisterViewModel.UserIsLoggedIn;

                if (UserIsLoggedIn)
                    UserHasLoggedIn();
            }
        }

        void UserHasLoggedIn()
        {
            CheckFirewallStatus(null);

            UsersName = AltiumDbApi.GetUsersName();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool native)
        {
            firewallCheck.Dispose();
        }

        public LoginRegisterViewModel LoginRegisterViewModel { get; private set; }
        public SettingsViewModel SettingsViewModel { get; private set; }
        public AzureViewModel AzureViewModel { get; private set; }
        public LocalDbViewModel LocalDbViewModel { get; private set; }

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

        private bool firewallRuleOk;
        public bool FirewallRuleOk
        {
            get { return firewallRuleOk; }
            set
            {
                firewallRuleOk = value;
                RaisePropertyChanged("FirewallRuleOk");
            }
        }

        public ICommand FirewallErrorButtonCommand { get; private set; }

        private bool updatingFirewallRule;
        public bool UpdatingFirewallRule
        {
            get { return updatingFirewallRule; }
            set
            {
                updatingFirewallRule = value;
                RaisePropertyChanged("UpdatingFirewallRule");
                ShowBusy = value;
            }
        }

        private bool showBusy;
        public bool ShowBusy
        {
            get { return showBusy; }
            set
            {
                showBusy = value;
                RaisePropertyChanged("ShowBusy");
            }
        }

        private string usersName;

        public string UsersName
        {
            get { return usersName; }
            set
            {
                usersName = value;
                RaisePropertyChanged("UsersName");
            }
        }

    }
}