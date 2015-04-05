using System;
using VacationMasters.Wrappers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VacationMasters
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DbWrapper DbWrapper { get; set; }

         public MainPage()
        {
            this.DbWrapper = new DbWrapper();
            var types = DbWrapper.GetTypes();
            this.InitializeComponent();
            foreach (String t in types)
                type_combo.Items.Add(t);
           /*     type_combo.Items.Add("elem1");
                type_combo.Items.Add("elem2");*/
            
            this.DataContext = this;
           /* this.DbWrapper = new DbWrapper();
            var packageList = this.DbWrapper.GetAllPackages();*/
            this.InitializeComponent();
        }

        public Visibility CollapsedVisibility
        {
            get { return Visibility.Collapsed; }
        }

        public Visibility VisibleVisibility
        {
            get { return Visibility.Visible; }
        }

        private void Search(object sender, RoutedEventArgs e)
        {

        }

        private void Home(object sender, RoutedEventArgs e)
        {

        }

        private void Packages(object sender, RoutedEventArgs e)
        {

        }

        private void UserPanel(object sender, RoutedEventArgs e)
        {

        }

        private void Contact(object sender, RoutedEventArgs e)
        {

        }

        private void GoToAdminControl(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AdminControl", true);
        }

    }


}
