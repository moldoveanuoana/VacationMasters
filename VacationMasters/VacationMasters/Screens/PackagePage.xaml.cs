using System;
using System.IO;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using VacationMasters.Essentials;
using VacationMasters.PackageManagement;
using VacationMasters.Wrappers;

// The User Control item template is documented at http://go.micosoft.com/fwlink/?LinkId=234236

namespace VacationMasters.Screens
{
    public sealed partial class PackagePage : UserControl
    {
        private readonly PackageManager _packageManager;
        private int _orderId;

        public PackagePage()
        {
            InitializeComponent();
            var dbWrapper = new DbWrapper();
            _packageManager = new PackageManager(dbWrapper);
        }

        private void ReserveOrCancel_OnClick(object sender, RoutedEventArgs e)
        {
            var package = MainPage.SelectedPackage;
            if (MainPage.CurrentUser == null)
            {
                var messageDialog = new MessageDialog("Please login in order to reserve");
                messageDialog.ShowAsync();
                return;
            }
            if (_orderId == 0)
            {
                _packageManager.ReservePackage(MainPage.CurrentUser.UserName, DateTime.Now, package.Price, package.ID);
                this.UpdatePackage();
                var messageDialog = new MessageDialog("The reservation has been made");
                messageDialog.ShowAsync();
            }
            else
            {
                _packageManager.CancelReservation(package.ID, _orderId);
                this.UpdatePackage();
                var messageDialog = new MessageDialog("The order has been canceled");
                messageDialog.ShowAsync();
            }
        }

        public void UpdatePackage()
        {
            var package = MainPage.SelectedPackage;
            var tab = "  ";
            imagePackage.Source = package.Photo;
            NameTextBlock.Text = tab + package.Name;
            TypeTextBlock.Text = tab + package.Type;
            IncludedTextBlock.Text = tab + package.Included;
            PriceTextBlock.Text = tab + package.Price;
            DateTextBlock.Text = tab + package.BeginDate + " - " + package.EndDate;
            Rating.Value = package.Rating;
            if (MainPage.CurrentUser != null)
            {
                _orderId = _packageManager.CheckIfUserHasOrderedThePackage(package.ID, MainPage.CurrentUser.UserName);
                var hasVoted = _packageManager.CheckIfUserDidVote(package.ID, MainPage.CurrentUser.UserName);
                if (_orderId != 0) ReserveOrCancel.Content = "Cancel"; 
                else ReserveOrCancel.Content = "Reserve";
                if (_orderId != 0 && hasVoted == false) 
                {Rating.IsEnabled = true; Rating.ApplyTemplate();}
                else Rating.IsEnabled = false;
            }
        }

        private void Rating_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (MainPage.CurrentUser != null)
                _packageManager.UpdateRating(MainPage.SelectedPackage.ID, Rating.Value, _orderId);
        }
    }
}