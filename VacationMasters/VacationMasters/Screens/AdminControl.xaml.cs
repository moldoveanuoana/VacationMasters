using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VacationMasters.Essentials;
using VacationMasters.Mail;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.Screens
{
    public sealed partial class AdminControl : UserControl, INotifyPropertyChanged
    {

        private readonly IUserManager _userManager;
        private readonly IDbWrapper _dbWrapper;
        private bool _banned;
        private bool _isUserSearched;
        private bool _isOperationInProgress;
        private bool _isUserManagerActive;
        private bool _isNewsletterActive;
        private ObservableCollection<Package> _list;
        private bool _isPackageActive;
        private bool _packageDisplay;

        public AdminControl()
        {
            this.DataContext = this;
            _dbWrapper = new DbWrapper();
            _userManager = new UserManager(_dbWrapper);
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

        public bool IsPackageActive
        {
            get { return _isPackageActive; }
            set
            {
                if (_isPackageActive != value)
                {
                    _isPackageActive = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool PackageDisplay
        {
            get { return _packageDisplay; }
            set
            {
                if (_packageDisplay != value)
                {
                    _packageDisplay = value;
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
            IsPackageActive = false;
        }

        private void Newsletter(object sender, RoutedEventArgs e)
        {
            IsUserManagerActive = false;
            IsNewsletterActive = true;
            IsPackageActive = false;
        }

        private void SearchPackage(object sender, RoutedEventArgs e)
        {
            var package = PackageSearchBox.Text;

            if (package == null)
                return;

            List = new ObservableCollection<Package>(_dbWrapper.GetPackagesByName(package));

            PackageDisplay = true;
        }

        public ObservableCollection<Package> List
        {
            get { return _list; }
            set
            {
                _list = value;
                NotifyPropertyChanged();
            }
        }

        private void PManager(object sender, RoutedEventArgs e)
        {
            IsPackageActive = true;
            IsNewsletterActive = false;
            IsUserManagerActive = false;
        }
    }
}
