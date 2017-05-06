using GalaSoft.MvvmLight;
using Harris.CelestialADB.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Text.RegularExpressions;
using Harris.CelestialADB.ApiData;
using RestSharp;
using Harris.CelestialADB.Desktop.WebService;
using System.Globalization;
using Harris.CelestialADB.Desktop.WPF;
using System.Windows.Input;

namespace Harris.CelestialADB.Desktop.ViewModel
{
    public class LoginRegisterViewModel : ViewModelBase
    {
        //todo: this view model is getting pretty big and ugly now. Split out to register, login, auth, resend.

        public LoginRegisterViewModel()
        {
            username = "";
            password = "";
            passwordConfirm = "";
            emailAddress = "";
            verificationCode = "";
            registerUser = true;
            showActivationCodeEntry = false;
            errorMessage = "";
            boxTitleText = "Registration is FREE";
            displayText = "Register";
            ShowAltiumPathError = false;
            AltiumPath = "";
            AltiumPathError = "";
            databaseUse = UserType.Professional;
            firstName = "";
            LastName = "";
            showCompanyField = true;
            Company = "";
            allowEmail = true;
            ShowActivationBox = false;
            ActivationCode = "";
            ShowActivationMessage = false;
            ShowBusy = false;
            ShowResendToken = false;
            UserIsLoggedIn = false; //= false;
            
            try
            {
                var stats = AltiumDbApi.GetDatabaseStats();
                componentCount = stats.ComponentCount;
                footprintCount = stats.FootprintCount;
            }
            catch // (Exception err)
            {
                // todo: network issue?
            }

            ActivateAccountCommand = new AwaitableDelegateCommand(ActivateAccount, ActivateAccountButtonEnabled);
            RegisterLoginCommand = new AwaitableDelegateCommand(RegisterLogin, RegisterLoginButtonEnabled);
            ResendTokenCommand = new AwaitableDelegateCommand(ResendToken, ResendButtonEnabled);

            SwitchBetweenRegisterLoginCommand = new DelegateCommand(() => { RegisterUser = !RegisterUser; ShowLoginError = false; });

            ShowResendTokenCommand = new DelegateCommand(() => { ShowResendToken = !ShowResendToken; ShowActivationError = false; ShowActivationMessage = false; ShowActivationBox = !ShowActivationBox; });

            ShowVerificationCommand = new DelegateCommand(() => { ShowActivationBox = !ShowActivationBox; ShowActivationError = false; });

            ViewGithubCommand = new DelegateCommand(() => System.Diagnostics.Process.Start("https://github.com/issus/altium-library"));

            AltiumBrowseCommand = new DelegateCommand(BrowseForAltiumDirectory);


            if (!String.IsNullOrEmpty(Properties.Settings.Default.Username))
            {
                Username = Properties.Settings.Default.Username;
                RegisterUser = false;
            }

            if (!String.IsNullOrEmpty(Properties.Settings.Default.AccessToken))
            {
                try
                {
                    UserIsLoggedIn = AltiumDbApi.CheckTokenValid();
                }
                catch { } // will throw an exception if the token is invalid
            }
        }

        async Task<bool> RegisterLogin()
        {
            ShowRegisterError = false;
            ShowLoginError = false;
            ErrorMessage = "";

            if (registerUser)
            {
                return await DoRegistration();
            }
            else
            {
                return await DoLogin();
            }
        }

        async Task<bool> DoRegistration()
        {
            ActivationMessage = "";
            ErrorMessage = "";
            ShowLoginError = false;

            if (!CheckEmailAddressValid(EmailAddress))
            {
                ShowLoginError = true;
                ErrorMessage = "You need to enter a valid email address.";
                return false;
            }

            if (Password.Length < 10)
            {
                ShowLoginError = true;
                ErrorMessage = "Password must be at least 10 characters.";
                return false;
            }

            if (Password.Contains("\""))
            {
                ShowLoginError = true;
                ErrorMessage = "Password cannot contain a \".";
                return false;
            }

            UserRegistrationRequest user = new UserRegistrationRequest();
            user.AllowEmail = AllowEmail;
            user.Username = Username;
            user.Password = Password;
            user.ConfirmPassword = PasswordConfirm;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Company = Company;
            user.Email = EmailAddress;
            user.Password = Password;
            user.UserType = databaseUse;

            ShowBusy = true;
            try
            {
                var resp = await AltiumDbApi.AccountRegister(user);
                ShowBusy = false;

                if (resp.Success)
                {
                    ActivationCode = "";
                    ActivationMessage = "Registration complete. Please activate with the code you were emailed - it may take a few minutes to arrive.";
                    ShowActivationMessage = true;
                    ShowActivationBox = true;
                }
                else
                {
                    ShowLoginError = true;
                    ErrorMessage = resp.Message;
                    return false;
                }
            }
            catch (Exception err)
            {
                ShowBusy = false;

                ShowLoginError = true;
                ErrorMessage = err.InnerException.Message;
                return false;
            }

            return true;
        }

        async Task<bool> DoLogin()
        {
            ErrorMessage = "";
            ShowLoginError = false;

            if (String.IsNullOrEmpty(Username) ||
                String.IsNullOrEmpty(Password))
            {
                ErrorMessage = "You need to enter a username and password.";
                ShowLoginError = true;
                return false;
            }

            ShowBusy = true;
            try
            {
                var response = await AltiumDbApi.Login(Username, Password);
                ShowBusy = false;

                if (!string.IsNullOrEmpty(response.error))
                {
                    ErrorMessage = response.error_description;
                    ShowLoginError = true;
                    return false;
                }
            }
            catch (Exception err)
            {
                ShowBusy = false;
                ErrorMessage = err.InnerException.Message;
                ShowLoginError = true;
            }

            ShowBusy = true;
            try
            {
                var active = await AltiumDbApi.CheckAccountActivated();
                ShowBusy = false;

                if (!active.Success)
                {
                    ErrorMessage = active.Message;
                    ShowLoginError = true;
                }
            }
            catch (Exception err)
            {
                ShowBusy = false;
                ErrorMessage = err.InnerException.Message;
                ShowLoginError = true;
            }

            UserIsLoggedIn = true;

            return true;
        }

        async Task<bool> ActivateAccount()
        {
            ErrorMessage = "";
            ShowActivationError = false;

            try
            {
                ShowBusy = true;
                var response = await AltiumDbApi.ActivateAccount(new AccountActivation { Username = this.Username, Password = this.Password, Code = this.ActivationCode });
                ShowBusy = false;

                if (response.Success)
                {
                    ShowActivationBox = false;
                    RegisterUser = false;
                }
                else
                {
                    ErrorMessage = response.Message;
                    ShowActivationError = true;
                    return false;
                }
            }
            catch (Exception err)
            {
                ShowBusy = false;
                ErrorMessage = err.InnerException.Message;
                ShowActivationError = true;
                return false;
            }

            return await DoLogin();
        }

        async Task<bool> ResendToken()
        {
            ErrorMessage = "";
            ShowResendError = false;

            ShowBusy = true;
            try
            {
                var response = await AltiumDbApi.ResendActivationEmail(EmailAddress);
                ShowBusy = false;

                if (!response.Success)
                {
                    ErrorMessage = response.Message;
                    ShowResendError = true;
                }
                else
                {
                    ShowResendToken = false;

                    ActivationCode = "";
                    ActivationMessage = "Please activate with the code you were emailed - it may take a few minutes to arrive.";
                    ShowActivationMessage = true;
                    ShowActivationBox = true;
                }

                return response.Success;
            }
            catch (Exception err)
            {
                ShowBusy = false;
                ErrorMessage = err.InnerException.Message;
                ShowResendError = true;
            }

            return false;
        }

        void BrowseForAltiumDirectory()
        {
            var dbLib = AltiumFile.BrowseForDbLib();

            if (!dbLib.Success)
            {
                ShowAltiumPathError = true;
                AltiumPathError = "DbLib could not be found/opened. Please login with username and password.";
                return;
            }
            
            if (String.IsNullOrEmpty(dbLib.ConnectionStringPath))
            {
                ShowAltiumPathError = true;
                AltiumPathError = "DbLib has not been configured correctly. Please login with username and password.";
                return;
            }

            var conn = AltiumFile.ReadUdlFile(dbLib.ConnectionStringPath);
            if (!String.IsNullOrEmpty(conn.Error))
            {
                ShowAltiumPathError = true;
                AltiumPathError = String.Format("{0} Please login with username and password.", conn.Error);
                return;
            }

            if (conn.Server != "csql.database.windows.net")
            {
                ShowAltiumPathError = true;
                AltiumPathError = "DbLib configured for a local server. Please login with username and password.";
                return;
            }

            Username = conn.Username;
            Password = conn.Password;

            AltiumPath = dbLib.Path;
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


        private string altiumPath;
        public string AltiumPath
        {
            get { return altiumPath; }
            set
            {
                altiumPath = value;
                RaisePropertyChanged("AltiumPath");

            }
        }

        private string altiumPathError;
        public string AltiumPathError
        {
            get { return altiumPathError; }
            set
            {
                altiumPathError = value;
                RaisePropertyChanged("AltiumPathError");
            }
        }

        private bool showAltiumPathError;
        public bool ShowAltiumPathError
        {
            get { return showAltiumPathError; }
            set
            {
                showAltiumPathError = value;
                RaisePropertyChanged("ShowAltiumPathError");
            }
        }

        private int componentCount;
        public int ComponentCount
        {
            get { return componentCount; }
            set
            {
                componentCount = value;
                RaisePropertyChanged("ComponentCount");
            }
        }

        private int footprintCount;
        public int FootprintCount
        {
            get { return footprintCount; }
            set
            {
                footprintCount = value;
                RaisePropertyChanged("FootprintCount");
            }
        }

        private string boxTitleText;
        public string BoxTitleText
        {
            get { return boxTitleText; }
            set
            {
                boxTitleText = value;

                RaisePropertyChanged("BoxTitleText");
            }
        }

        private string displayText;
        public string DisplayText
        {
            get { return displayText; }
            set
            {
                displayText = value;

                RaisePropertyChanged("DisplayText");
            }
        }

        private bool showLoginError;
        public bool ShowLoginError
        {
            get { return showLoginError; }
            set
            {
                showLoginError = value;
                RaisePropertyChanged("ShowLoginError");
            }
        }

        private bool showRegisterError;
        public bool ShowRegisterError
        {
            get { return showRegisterError; }
            set
            {
                showRegisterError = value;
                RaisePropertyChanged("ShowRegisterError");
            }
        }

        private bool showActivationError;
        public bool ShowActivationError
        {
            get { return showActivationError; }
            set
            {
                showActivationError = value;
                RaisePropertyChanged("ShowActivationError");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;

                RaisePropertyChanged("ErrorMessage");
            }
        }


        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value.Replace("@csql", "");

                RaisePropertyChanged("Username");
                RegisterLoginCommand.RaiseCanExecuteChanged();
                ActivateAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;

                RaisePropertyChanged("Password");
                RegisterLoginCommand.RaiseCanExecuteChanged();
                ActivateAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string passwordConfirm;
        public string PasswordConfirm
        {
            get { return passwordConfirm; }
            set
            {
                passwordConfirm = value;

                RaisePropertyChanged("PasswordConfirm");
                RegisterLoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string emailAddress;
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;

                RaisePropertyChanged("EmailAddress");
                RegisterLoginCommand.RaiseCanExecuteChanged();
                ResendTokenCommand.RaiseCanExecuteChanged();
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                RaisePropertyChanged("FirstName");
                RaisePropertyChanged("RegisterLoginCommand");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                RaisePropertyChanged("LastName");
                RaisePropertyChanged("RegisterLoginCommand");
            }
        }


        private bool registerUser;
        public bool RegisterUser
        {
            get { return registerUser; }
            set
            {
                registerUser = value;

                RaisePropertyChanged("RegisterUser");

                if (registerUser)
                {
                    BoxTitleText = "Registration is FREE";
                    DisplayText = "Register";
                }
                else
                {
                    BoxTitleText = "Sign In";
                    DisplayText = "Sign in";
                }

                RaisePropertyChanged("RegisterLoginCommand");
            }
        }

        private bool showActivationCodeEntry;
        public bool ShowActivationCodeEntry
        {
            get { return showActivationCodeEntry; }
            set
            {
                showActivationCodeEntry = value;

                RaisePropertyChanged("ShowVerificationCodeEntry");
            }
        }

        private string verificationCode;
        public string VerificationCode
        {
            get { return verificationCode; }
            set { verificationCode = value; }
        }

        private UserType databaseUse;
        public string DatabaseUse
        {
            get
            {
                return databaseUse.ToString();
            }
            set
            {
                Enum.TryParse(value, out databaseUse);
                RaisePropertyChanged("DatabaseUse");

                if (databaseUse == UserType.Hobbyist)
                    ShowCompanyField = false;
                else
                    ShowCompanyField = true;
            }
        }

        private bool showCompanyField;
        public bool ShowCompanyField
        {
            get { return showCompanyField; }
            set
            {
                showCompanyField = value;
                RaisePropertyChanged("ShowCompanyField");
            }
        }

        private string company;
        public string Company
        {
            get { return company; }
            set
            {
                company = value;
                RaisePropertyChanged("Company");
            }
        }

        private bool allowEmail;
        public bool AllowEmail
        {
            get { return allowEmail; }
            set
            {
                allowEmail = value;
                RaisePropertyChanged("AllowEmail");
            }
        }

        private bool showActivationBox;
        public bool ShowActivationBox
        {
            get { return showActivationBox; }
            set
            {
                showActivationBox = value;
                RaisePropertyChanged("ShowActivationBox");
                RaisePropertyChanged("ShowBlur");
            }
        }

        private string activationCode;
        public string ActivationCode
        {
            get { return activationCode; }
            set
            {
                activationCode = value;
                RaisePropertyChanged("ActivationCode");
                RaisePropertyChanged("ActivateAccountCommand");
            }
        }

        private bool showActivationMessage;
        public bool ShowActivationMessage
        {
            get { return showActivationMessage; }
            set
            {
                showActivationMessage = value;
                RaisePropertyChanged("ShowActivationMessage");
            }
        }

        private string activationMessage;
        public string ActivationMessage
        {
            get { return activationMessage; }
            set
            {
                activationMessage = value;
                RaisePropertyChanged("ActivationMessage");
            }
        }

        private bool showBusy;
        public bool ShowBusy
        {
            get { return showBusy; }
            set
            {
                showBusy = value;
                RaisePropertyChanged("ShowBlur");
                RaisePropertyChanged("ShowBusy");
            }
        }

        public bool ShowBlur
        {
            get { return ShowActivationBox || ShowBusy || ShowResendToken; }
        }

        private bool showResendToken;
        public bool ShowResendToken
        {
            get { return showResendToken; }
            set
            {
                showResendToken = value;
                RaisePropertyChanged("ShowResendToken");
                RaisePropertyChanged("ShowBlur");
            }
        }

        private bool showResendError;
        public bool ShowResendError
        {
            get { return showResendError; }
            set
            {
                showResendError = value;
                RaisePropertyChanged("ShowResendError");
            }
        }


        public IAsyncCommand ResendTokenCommand { get; }
        public IAsyncCommand ActivateAccountCommand { get; }
        public IAsyncCommand RegisterLoginCommand { get; }
        public ICommand ViewGithubCommand { get; }
        public ICommand ShowVerificationCommand { get; }
        public ICommand SwitchBetweenRegisterLoginCommand { get; }
        public ICommand AltiumBrowseCommand { get; }
        public ICommand ShowResendTokenCommand { get; }

        bool RegisterLoginButtonEnabled()
        {
            if (Username != Username.Trim())
            {
                Username = Username.Trim();
            }

            Regex usernameCheck = new Regex("^[a-z0-9]+$", RegexOptions.IgnoreCase);

            if (!usernameCheck.IsMatch(Username))
            {
                return false;
            }

            if (RegisterUser)
                return !String.IsNullOrEmpty(Username) &&
                    !String.IsNullOrEmpty(Password) &&
                    !String.IsNullOrEmpty(PasswordConfirm) &&
                    (Password == PasswordConfirm) &&
                    !String.IsNullOrEmpty(FirstName) &&
                    !String.IsNullOrEmpty(LastName) &&
                    !String.IsNullOrEmpty(EmailAddress) &&
                    CheckEmailAddressValid(EmailAddress);
            else
                return !String.IsNullOrEmpty(Username) &&
                    !String.IsNullOrEmpty(Password);
        }

        bool ActivateAccountButtonEnabled()
        {
            if (String.IsNullOrEmpty(Username) ||
                String.IsNullOrEmpty(Password) ||
                String.IsNullOrEmpty(ActivationCode))
                return false;

            return true;
        }

        bool ResendButtonEnabled()
        {
            return !string.IsNullOrEmpty(EmailAddress) && CheckEmailAddressValid(EmailAddress);
        }

        bool CheckEmailAddressValid(string email)
        {
            bool mappingInvalid = false;
            email = Regex.Replace(email, @"(@)(.+)$", match =>
            {
                String domainName = match.Groups[2].Value;
                try
                {
                    domainName = new IdnMapping().GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    mappingInvalid = true;
                }
                return match.Groups[1].Value + domainName;
            });
            if (mappingInvalid)
            {
                return false;
            }
            return Regex.IsMatch(email,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                    RegexOptions.IgnoreCase);
        }
    }
}
