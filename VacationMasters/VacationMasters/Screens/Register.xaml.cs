using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VacationMasters.Essentials;
using VacationMasters.UserManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.Screens
{
    public sealed partial class Register : UserControl
    {
        public UserManager UserManager { get; set; }
        public DbWrapper DbWrapper { get; set; }
        public Register()
        {
            DbWrapper = new DbWrapper();
            UserManager = new UserManager(this.DbWrapper);
            var preferences = DbWrapper.GetAllPreferences();
            InitializeComponent();
            comboBoxCountry.ItemsSource = preferences.Where(c => c.Category == "Country").Select(d => d.Name).ToArray();
            comboBoxType.ItemsSource = preferences.Where(c => c.Category == "Type").Select(d => d.Name).ToArray();
        }
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyRegisterFields() == false) return;
            var preferencesIds = new List<int>();
            var preferences = this.DbWrapper.GetAllPreferences();
            if (this.comboBoxCountry.SelectedValue != null)
            {
                var countryPref = preferences.First(c => c.Name == this.comboBoxCountry.SelectedValue.ToString());
                preferencesIds.Add(countryPref.ID);
            }
            if (this.comboBoxType.SelectedValue != null)
            {
                var typePref = preferences.First(c => c.Name == this.comboBoxType.SelectedValue.ToString());
                preferencesIds.Add(typePref.ID);
            }
            var user = new User(this.txtBoxUsrName.Text, this.txtBoxFrsName.Text, this.txtBoxLstName.Text, this.txtBoxEmail.Text, this.txtBoxPhone.Text, false, "User", null);
            this.UserManager.AddUser(user, this.pwdBox.Password, preferencesIds);
        }

        private bool VerifyRegisterFields()
        {
            var completedFields = true;
            if (ChangeRequiredTextBlock(this.usrRequired, this.txtBoxUsrName) == false) completedFields = false;
            if (ChangeRequiredTextBlock(this.frsRequired, this.txtBoxFrsName) == false) completedFields = false;
            if (ChangeRequiredTextBlock(this.lstRequired, this.txtBoxLstName) == false) completedFields = false;
            if (ChangeRequiredTextBlock(this.emailRequired, this.txtBoxEmail) == false) completedFields = false;
            if (ChangeRequiredTextBlock(this.phoneRequired, this.txtBoxPhone) == false) completedFields = false;
            if (ChangeRequiredTextBlock(this.pwdRequired, this.pwdBox) == false) completedFields = false;
            if (ChangeRequiredTextBlock(this.confRequired, this.confirmPwdBox) == false) completedFields = false;
            if (UserManager.CheckIfUserExists(this.txtBoxUsrName.Text) == true)
            {
                this.usrRequired.FontSize = 11;
                this.usrRequired.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                this.usrRequired.Text = "The username is already taken";
                completedFields = false;
            }
            if (this.txtBoxEmail.Text != null && IsValidEmail(this.txtBoxEmail.Text) == false)
            {
                this.emailRequired.FontSize = 11;
                this.emailRequired.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                this.emailRequired.Text = "The e-mail is not valid";
                completedFields = false;
            }
            if (this.txtBoxEmail.Text != null && IsValidEmail(this.txtBoxEmail.Text) == true && this.UserManager.CheckIfEmailExists(this.txtBoxEmail.Text) == true)
            {
                this.emailRequired.FontSize = 11;
                this.emailRequired.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                this.emailRequired.Text = "The e-mail is already used";
                completedFields = false;
            }
            if (this.txtBoxPhone.Text != null && IsValidPhoneNumber(this.txtBoxPhone.Text) == false)
            {
                this.phoneRequired.FontSize = 11;
                this.phoneRequired.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                this.phoneRequired.Text = "The phone is not valid";
                completedFields = false;
            }
            if (pwdBox.Password != confirmPwdBox.Password)
            {
                this.errorTxt.Text = "Password and Confirm Password do not match";
                completedFields = false;
            }
            return completedFields;
        }

        private bool ChangeRequiredTextBlock(TextBlock textblock, TextBox textbox)
        {
            if (string.IsNullOrEmpty(textbox.Text))
            {
                textblock.FontSize = 11;
                textblock.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                textblock.Text = "This field is required!";
                return false;
            }
            return true;
        }
        private bool ChangeRequiredTextBlock(TextBlock textblock, PasswordBox passwordbox)
        {
            if (string.IsNullOrEmpty(passwordbox.Password))
            {
                textblock.FontSize = 11;
                textblock.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                textblock.Text = "This field is required!";
                return false;
            }
            return true;
        }
        public bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
        public bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(phoneNumber);
        }

    }
}
