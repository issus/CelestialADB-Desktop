using GalaSoft.MvvmLight;
using Harris.CelestialADB.Desktop.Database;
using Harris.CelestialADB.Desktop.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    public class LocalDbViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }

        public LocalDbViewModel()
        {
            TestConnectionCommand = new AwaitableDelegateCommand(TestConnection, () => !String.IsNullOrEmpty(DatabaseHost) && !String.IsNullOrEmpty(DatabaseUser) && !String.IsNullOrEmpty(DatabasePassword) && DatabasePort > 0);

            DatabaseHost = "localhost";
            DatabasePort = 3306;

            DatabaseUser = "root";
            DatabasePassword = "";
        }

        private async Task TestConnection()
        {
            ShowTestErrorMessage = false;
            ShowTestSuccessMessage = false;
            TestMessage = "";

            Properties.Settings.Default.MySqlConnectionString = String.Format("Server={0};Port={1};Uid={2};Pwd={3};",
                DatabaseHost, DatabasePort, DatabaseUser, DatabasePassword);
            Properties.Settings.Default.Save();


            try
            {
                using (MySQLReplicator db = new MySQLReplicator())
                {
                    if (await db.TestConnection())
                    {
                        ShowTestSuccessMessage = true;
                        TestMessage = "Connection successful!";
                    }
                    else
                    {
                        ShowTestErrorMessage = true;
                        TestMessage = "Could not connect to the database.";
                    }
                }

            }
            catch (Exception err)
            {
                ShowTestErrorMessage = true;
                TestMessage = String.Format("Could not connect to the database: {0}", err.Message);
            }
        }

        private string databasePassword;
        public string DatabasePassword
        {
            get { return databasePassword; }
            set
            {
                databasePassword = value;
                RaisePropertyChanged("DatabasePassword");
                TestConnectionCommand.RaiseCanExecuteChanged();
            }
        }


        private string databaseUser;
        public string DatabaseUser
        {
            get { return databaseUser; }
            set
            {
                databaseUser = value;
                RaisePropertyChanged("DatabaseUser");
                TestConnectionCommand.RaiseCanExecuteChanged();
            }
        }

        private int databasePort;
        public int DatabasePort
        {
            get { return databasePort; }
            set
            {
                databasePort = value;
                RaisePropertyChanged("DatabasePort");
                TestConnectionCommand.RaiseCanExecuteChanged();
            }
        }

        private string databaseHost;
        public string DatabaseHost
        {
            get { return databaseHost; }
            set
            {
                databaseHost = value;
                RaisePropertyChanged("DatabaseHost");
                TestConnectionCommand.RaiseCanExecuteChanged();
            }
        }

        private bool showTestErrorMessage;
        public bool ShowTestErrorMessage
        {
            get { return showTestErrorMessage; }
            set
            {
                showTestErrorMessage = value;
                RaisePropertyChanged("ShowTestErrorMessage");
            }
        }

        private bool showTestSuccessMessage;
        public bool ShowTestSuccessMessage
        {
            get { return showTestSuccessMessage; }
            set
            {
                showTestSuccessMessage = value;
                RaisePropertyChanged("ShowTestSuccessMessage");
            }
        }

        private string testMessage;

        public string TestMessage
        {
            get { return testMessage; }
            set
            {
                testMessage = value;
                RaisePropertyChanged("TestMessage");
            }
        }


        public IAsyncCommand TestConnectionCommand { get; private set; }
    }
}
