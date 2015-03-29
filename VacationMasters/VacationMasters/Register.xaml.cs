using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using VacationMasters.Wrappers;
using System.Text.RegularExpressions;
using VacationMasters.Essentials;
using VacationMasters.UserManagement;

namespace VacationMasters
{
    public sealed partial class Register : UserControl
    {
        public UserManager UserManager { get; set; }
        public DbWrapper DbWrapper { get; set; }
        public Register()
        {
            this.DbWrapper = new DbWrapper();
            this.UserManager = new UserManager(this.DbWrapper);
            var preferences = this.DbWrapper.GetAllPreferences();
            this.InitializeComponent();
            this.comboBoxCountry.ItemsSource = preferences.Where(c => c.Category == "Country").Select(d => d.Name).ToArray();
            this.comboBoxType.ItemsSource = preferences.Where(c => c.Category == "Type").Select(d => d.Name).ToArray();
        }
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (VerifyRegisterFields() == false) return;
            var preferencesIds = new List<int>();
            var preferences = this.DbWrapper.GetAllPreferences();
            if (this.comboBoxCountry.SelectedValue != null)
            {
                var countryPref = preferences.Where(c => c.Name == this.comboBoxCountry.SelectedValue.ToString()).First();
                preferencesIds.Add(countryPref.ID);
            }
            if (this.comboBoxType.SelectedValue != null)
            {
                var typePref = preferences.Where(c => c.Name == this.comboBoxType.SelectedValue.ToString()).First();
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
            if (this.txtBoxUsrName.Text != null && this.UserManager.CheckIfUserExists(this.txtBoxUsrName.Text) == true)
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
