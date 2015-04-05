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

        private void home_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void packages_Click(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(Packages), null);

        }

        private void user_panel_Click(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(UserPanel), null);

        }

        private void admin_control_Click(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(AdminControl), null);
            VisualStateManager.GoToState(this, "AdminControl", true);
        }

        private void contact_Click(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(Contact), null);
        }


        private void search_Click(object sender, RoutedEventArgs e)
        {


        }



    }
}
