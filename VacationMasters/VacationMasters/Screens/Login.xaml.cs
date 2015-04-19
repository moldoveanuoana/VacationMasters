using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VacationMasters.Essentials;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;
using System.Windows.Controls.Primitives;

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
            if (userName != null && _userManager.CheckIfUserExists(userName) == true)
            {
                var currUser = _userManager.GetUser(userName);
                if (currUser.Banned)
                {
                    Popup codePopup = new Popup();
                    TextBlock popupText = new TextBlock();
                    popupText.Text = "The account has been blocked.";
                   // popupText.Background = Brushes.LightBlue;
                   // popupText.Foreground = System.Drawing.Design.Brushes.Blue;
                    codePopup.Child = popupText;
                    return;
                }

                if (_userManager.CheckCredentials(userName, this.pwdBox.Password) == false)
                    return;

                UserManager.UserSignIn(_userManager.GetUser(userName));
                MainPage redirectToMainPage = new MainPage();
                
                    //TODO session for the user. And instead of returns => Validation controls.
            }

        }
    }
}
