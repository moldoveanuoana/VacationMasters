using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VacationMasters.Essentials;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;
using System;

namespace VacationMasters.Screens
{
     public sealed partial class Login : UserControl
    {
        private IUserManager _userManager;
        public Login()
        {
            var dbWrapper = new DbWrapper();
            _userManager = new UserManager(dbWrapper);
            InitializeComponent();
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string userName = this.txtBoxUsrName.Text;
            string password = this.pwdBox.Password;
        
            if(_userManager.Login(userName, password))
                VisualStateManager.GoToState(this, "LoginControl", true);  
        }
    }
}
