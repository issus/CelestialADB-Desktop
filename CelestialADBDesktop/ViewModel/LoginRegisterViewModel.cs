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

namespace Harris.CelestialADB.Desktop.ViewModel
{
    public class LoginRegisterViewModel : ViewModelBase
    {
        public LoginRegisterViewModel()
        {
            username = "";
            password = "";
            passwordConfirm = "";
            emailAddress = "";
            verificationCode = "";
            registerUser = true;
            showVerificationCodeEntry = false;
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
            ShowValidationBox = false;
            ActivationCode = "";

            //TODO: Web Service Call
            componentCount = 28635;
            footprintCount = 601;

            VerifyCodeCommand = new DelegateCommand(VerifyCode);
            RegisterLoginCommand = new DelegateCommand(RegisterLogin, RegisterLoginButtonEnabled);

            SwitchBetweenRegisterLoginCommand = new DelegateCommand(() => RegisterUser = !RegisterUser);

            ShowVerificationCommand = new DelegateCommand(() => ShowValidationBox = !ShowValidationBox);

            ViewGithubCommand = new DelegateCommand(() => System.Diagnostics.Process.Start("https://github.com/issus/altium-library"));

            AltiumBrowseCommand = new DelegateCommand(BrowseForAltiumDirectory);
        }

        void RegisterLogin()
        {
            ErrorMessage = "";

            if (String.IsNullOrEmpty(Username) ||
                String.IsNullOrEmpty(Password))
            {
                ErrorMessage = "You need to enter a username and password.";
                return;
            }

            // todo: Switch email validation to proper regex
            if (RegisterUser && (String.IsNullOrEmpty(EmailAddress) || !EmailAddress.Contains('@') || !EmailAddress.Contains('.')))
            {
                ErrorMessage = "You need to enter an email address.";
                return;
            }
        }

        void VerifyCode()
        {
            ErrorMessage = "";
        }

        void BrowseForAltiumDirectory()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.EnsureFileExists = true;
            dialog.Title = "Locate DbLib File";
            dialog.Multiselect = false;
            dialog.Filters.Add(new CommonFileDialogFilter("Altium Database File", "*.DbLib"));
            ShowAltiumPathError = false;

            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            string path = dialog.FileName;

            Regex dblibRex = new Regex(@"^ConnectionString=FILE NAME=(?<File>.+)\s*$", RegexOptions.Multiline);
            string dblib = File.ReadAllText(path);
            if (!dblibRex.IsMatch(dblib))
            {
                ShowAltiumPathError = true;
                AltiumPathError = "DbLib has not been configured correctly. Please login with username and password.";
                return;
            }

            var udlPath = dblibRex.Match(dblib).Groups["File"].Value.Trim();
            if (!File.Exists(udlPath))
            {
                ShowAltiumPathError = true;
                AltiumPathError = "Connection file is missing. Please login with username and password.";
                return;
            }

            Regex tokenRex = new Regex(@"^(?=[^;])(?:(?<Token>.+?)=(?<Value>.+?);)+", RegexOptions.Multiline);
            string udl = File.ReadAllText(udlPath);
            if (!tokenRex.IsMatch(udl))
            {
                ShowAltiumPathError = true;
                AltiumPathError = "UDL file has not been configured correctly. Please login with username and password.";
                return;
            }

            Username = "";
            Password = "";

            var match = tokenRex.Match(udl);
            for (int i = 0; i < match.Groups["Token"].Captures.Count; i++)
            {
                if (match.Groups["Token"].Captures[i].Value == "User ID")
                    Username = match.Groups["Value"].Captures[i].Value;
                else if (match.Groups["Token"].Captures[i].Value == "Password")
                    Password = match.Groups["Value"].Captures[i].Value;
            }

            AltiumPath = path;
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
                username = value;

                RaisePropertyChanged("Username");
                RaisePropertyChanged("RegisterLoginCommand");
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
                RaisePropertyChanged("RegisterLoginCommand");
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
                RaisePropertyChanged("RegisterLoginCommand");
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
                RaisePropertyChanged("RegisterLoginCommand");
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

        private bool showVerificationCodeEntry;
        public bool ShowVerificationCodeEntry
        {
            get { return showVerificationCodeEntry; }
            set
            {
                showVerificationCodeEntry = value;

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

        private bool showValidationBox;
        public bool ShowValidationBox
        {
            get { return showValidationBox; }
            set
            {
                showValidationBox = value;
                RaisePropertyChanged("ShowValidationBox");
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
            }
        }



        public DelegateCommand VerifyCodeCommand { get; }
        public DelegateCommand RegisterLoginCommand { get; }
        public DelegateCommand ViewGithubCommand { get; }
        public DelegateCommand ShowVerificationCommand { get; }
        public DelegateCommand SwitchBetweenRegisterLoginCommand { get; }
        public DelegateCommand AltiumBrowseCommand { get; }

        bool RegisterLoginButtonEnabled()
        {
            if (RegisterUser)
                return !String.IsNullOrEmpty(Username) &&
                    !String.IsNullOrEmpty(Password) &&
                    !String.IsNullOrEmpty(PasswordConfirm) &&
                    (Password == PasswordConfirm) &&
                    !String.IsNullOrEmpty(EmailAddress) &&
                    (EmailAddress.Contains('@') && EmailAddress.Contains('.'));
            else
                return !String.IsNullOrEmpty(Username) &&
                    !String.IsNullOrEmpty(Password);
        }
    }
}
