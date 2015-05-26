using System;
using VacationMasters.Wrappers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VacationMasters.Essentials;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VacationMasters
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public DbWrapper DbWrapper { get; set; }
        public static User CurrentUser { get; set; }
        public static Package SelectedPackage { get; set; }
        public static int search_criterion;
        public static String pk_name;
        public static int pk_min_price;
        public static int pk_max_price;
        public static DateTime pk_begin_date;
        public static DateTime pk_end_date;
        public static String pk_type;
         public MainPage()
        {
             this.DbWrapper = new DbWrapper();
             this.InitializeComponent();
             this.DataContext = this;
             this.InitializeComponent();
             begin_date.Date = DateTimeOffset.Now.Date;
             end_date.Date = DateTimeOffset.Now.Date;
             Task.Run(() => Initialize());
        }

        private async void Initialize()
        {
            var types = DbWrapper.GetTypes();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                foreach (String t in types)
                    type_combo.Items.Add(t);
                Home(null, null);
            });
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
            if (!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {
                search_criterion = 1;
                pk_name = name.Text;

            }

            if (String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {

                search_criterion = 2;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
            }

            if(String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {
                search_criterion = 3;
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
            }
            
            if(String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) && 
                type_combo.SelectedItem != null)
            {
                search_criterion = 4;
                pk_type = type_combo.SelectedValue.ToString();
            }


            if(String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {
                search_criterion = 5;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
               
            }

            if(String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 6;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
                pk_type = type_combo.SelectedValue.ToString();
            }


            if(String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 7;
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
                pk_type = type_combo.SelectedValue.ToString();    
            }


            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {
                search_criterion = 8;
                pk_name = name.Text;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
            }

            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {
                search_criterion = 9;
                pk_name = name.Text;
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
            }

            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 10;
                pk_name = name.Text;
                pk_type = type_combo.SelectedValue.ToString();
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem == null)
            {
                search_criterion = 11;
                pk_name = name.Text;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;  
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) &&  DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 12;
                pk_name = name.Text;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
                pk_type = type_combo.SelectedValue.ToString();
            }

            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 13;
                pk_name = name.Text;
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
                pk_type = type_combo.SelectedValue.ToString();
            }

            if(String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 14;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
                pk_type = type_combo.SelectedValue.ToString();
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && ! DateTimeOffset.Equals(begin_date.Date.Date,DateTimeOffset.Now.Date) && ! DateTimeOffset.Equals(end_date.Date.Date,DateTimeOffset.Now.Date) &&
                type_combo.SelectedItem != null)
            {
                search_criterion = 15;
                pk_name = name.Text;
                pk_min_price = Convert.ToInt32(min_price.Text);
                pk_max_price = Convert.ToInt32(max_price.Text);
                pk_begin_date = begin_date.Date.Date;
                pk_end_date = end_date.Date.Date;
                pk_type = type_combo.SelectedValue.ToString();
            }
            VisualStateManager.GoToState(this, "SearchResult", true);
            this.SearchView.Initialize();

        }

        private void Home(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "PackagesV", true);
        }

        private void UserPanel(object sender, RoutedEventArgs e)
        {

        }


        private void GoToAdminControl(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AdminControl", true);
        }

        private void GoToRegisterControl(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "RegisterControl", true);
        }

        private void GoToContactControl(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "ContactControl", true);
        }

        private void GoToLoginControl(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "LoginControl", true);
        }

        private void GoToAgentPage(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "AgentPageControl", true);
        }
        public void UpdateSelectedPackage(Package package)
        {
            SelectedPackage = package;
            this.PackagesPageView.UpdatePackage();
        }
    }


}
