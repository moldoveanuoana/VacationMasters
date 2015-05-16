using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using VacationMasters.Essentials;
using VacationMasters.Mail;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.Screens
{
    public sealed partial class AdminControl : UserControl, INotifyPropertyChanged
    {

        private readonly IUserManager _userManager;
        private bool _banned;
        private bool _isUserSearched;
        private bool _isOperationInProgress;
        private bool _isUserManagerActive;
        private bool _isNewsletterActive;

        public AdminControl()
        {
            this.DataContext = this;
            var dbWrapper = new DbWrapper();
            _userManager = new UserManager(dbWrapper);
            this.InitializeComponent();
        }

        public bool Banned
        {
            get { return _banned; }
            set
            {
                if (value != _banned)
                {
                    _banned = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsUserSearched
        {
            get { return _isUserSearched; }
            set
            {
                if (value != _isUserSearched)
                {
                    _isUserSearched = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsOperationInProgress
        {
            get { return _isOperationInProgress; }
            set
            {
                if (value != _isOperationInProgress)
                {
                    _isOperationInProgress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private async void SearchUser(object sender, RoutedEventArgs e)
        {
            var userName = UserSearchBox.Text;
            if (String.IsNullOrEmpty(userName))
                return;

            try
            {
                IsOperationInProgress = true;
                IsUserSearched = false;
                User user = await Task.Run<User>(() => _userManager.GetUser(userName));
                Banned = user.Banned;
                IsUserSearched = true;
                IsOperationInProgress = false;
            }
            catch (Exception ex)
            {

            }
        }

        private async void BanUser(object sender, RoutedEventArgs e)
        {
            var userName = UserSearchBox.Text;
            if (String.IsNullOrEmpty(userName))
                return;

            IsOperationInProgress = true;

            await Task.Run(() => _userManager.BanUser(userName));
            Banned = true;
            IsOperationInProgress = false;
        }

        private async void UnbanUser(object sender, RoutedEventArgs e)
        {
            var userName = UserSearchBox.Text;
            if (String.IsNullOrEmpty(userName))
                return;

            IsOperationInProgress = true;

            await Task.Run(() => _userManager.UnbanUser(userName));
            Banned = false;
            IsOperationInProgress = false;
        }

        public bool IsUserManagerActive
        {
            get { return _isUserManagerActive; }
            set
            {
                if (_isUserManagerActive != value)
                {
                    _isUserManagerActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsNewsletterActive
        {
            get { return _isNewsletterActive; }
            set
            {
                if (_isNewsletterActive != value)
                {
                    _isNewsletterActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private async void SendNewsletter(object sender, RoutedEventArgs e)
        {
            string to = String.Empty;
            var smtpServer = "smtp.gmail.com";
            var port = 465;
            var from = "vmastersb28@gmail.com";
            var pwd = "vmasters28b";
            var allEmails = _userManager.GetAllEmails();
            to = allEmails.Aggregate(to, (current, email) => current + (email + ";"));

            var subject = Subject.Text;
            var body = Body.Text;

            var client = new SmtpClient(smtpServer, port,
                                                    from, pwd, true);

            var message = new SmtpMessage(from,to, null,subject, body);
            try
            {
                await client.SendMail(message);
                Subject.Text = String.Empty;
                Body.Text = String.Empty;
            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog("Something went wrong! Please try again later."); 
                messageDialog.ShowAsync();
            }
        }


        private void UserManager(object sender, RoutedEventArgs e)
        {
            IsNewsletterActive = false;
            IsUserManagerActive = true;
        }

        private void Newsletter(object sender, RoutedEventArgs e)
        {
            IsUserManagerActive = false;
            IsNewsletterActive = true;
        }
    }
}
