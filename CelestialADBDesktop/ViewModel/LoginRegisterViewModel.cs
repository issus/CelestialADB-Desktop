using GalaSoft.MvvmLight;
using Harris.CelestialADB.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            registerUser = false;
            showVerificationCodeEntry = false;
            errorMessage = "";
            displayText = "Login";

            VerifyCodeCommand = new DelegateCommand(VerifyCode);
            RegisterLoginCommand = new DelegateCommand(RegisterLogin, RegisterLoginButtonEnabled);
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

        private bool registerUser;
        public bool RegisterUser
        {
            get { return registerUser; }
            set
            {
                registerUser = value;

                RaisePropertyChanged("RegisterUser");

                if (registerUser)
                    DisplayText = "Register";
                else
                    DisplayText = "Login";

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


        public DelegateCommand VerifyCodeCommand { get; }
        public DelegateCommand RegisterLoginCommand { get; }


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
