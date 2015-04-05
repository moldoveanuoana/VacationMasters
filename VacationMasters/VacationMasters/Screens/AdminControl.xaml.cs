using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using VacationMasters.Essentials;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.Screens
{
    public sealed partial class AdminControl : UserControl, INotifyPropertyChanged
    {
        private IUserManager _userManager;
        private bool _banned;
        private bool _isUserSearched;
        private bool _isOperationInProgress;

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
            IsOperationInProgress = true;
            var userName = UserSearchBox.Text;
            await Task.Run(() => _userManager.BanUser(userName));
            Banned = true;
            IsOperationInProgress = false;
        }

        private async void UnbanUser(object sender, RoutedEventArgs e)
        {
            IsOperationInProgress = true;
            var userName = UserSearchBox.Text;
            await Task.Run(() => _userManager.UnbanUser(userName));
            Banned = false;
            IsOperationInProgress = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
