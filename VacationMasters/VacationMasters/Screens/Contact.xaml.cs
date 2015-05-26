using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using VacationMasters.Mail;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VacationMasters.Screens
{
    public sealed partial class Contact : UserControl, INotifyPropertyChanged
    {
        public Contact()
        {
            this.InitializeComponent();
            NotifyPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void SendEmail(object sender, RoutedEventArgs e)
        {
            string to = String.Empty;
            var smtpServer = "smtp.gmail.com";
            var port = 465;
            var from = "vmastersb28@gmail.com";
            var pwd = "vmasters28b";

            to = "vmastersb28@gmail.com";

            var subject = Subject.Text;
            var body = Body.Text;

            var client = new SmtpClient(smtpServer, port,
                                                    from, pwd, true);

            var message = new SmtpMessage(from, to, null, subject, body);
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
    }
}
