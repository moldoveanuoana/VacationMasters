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

         public MainPage()
        {
             this.DbWrapper = new DbWrapper();
             this.InitializeComponent();
             this.DataContext = this;
             this.InitializeComponent();
             begin_date.Date = DateTimeOffset.MinValue;
             end_date.Date = DateTimeOffset.MinValue;
             Task.Run(() => Initialize());
        }

         private async void Initialize()
         {
             var types = DbWrapper.GetTypes();
             await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
             {
                 foreach (String t in types)
                     type_combo.Items.Add(t);
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
            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue && 
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.GetPackagesByName(name.Text);
                foreach(Package pack in packages)
                {
                    //set gridview components
                }
            }

            if (String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue &&
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.GetPackagesByPrice(Convert.ToInt32(min_price.Text), Convert.ToInt32(max_price.Text));
                foreach (Package pack in packages)
                {
                    //set gridview components
                    
                }


            }

            if(String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.GetPackagesByDate(begin_date.Date.DateTime,end_date.Date.DateTime);
                foreach (Package pack in packages)
                {
                    //set gridview components
                    
                }
            }
            
            if(String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue && 
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByType(type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.getPackagesByPriceDate(Convert.ToInt32(min_price.Text), Convert.ToInt32(max_price.Text), 
                               begin_date.Date.DateTime, end_date.Date.DateTime);
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByPriceType(Convert.ToInt32(min_price.Text), Convert.ToInt32(max_price.Text),
                                                                type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByDateType(begin_date.Date.DateTime, end_date.Date.DateTime, type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue &&
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.getPackagesByNamePrice(name.Text, Convert.ToInt32(min_price.Text), Convert.ToInt32(min_price.Text));
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.getPackagesByNameDate(name.Text, begin_date.Date.DateTime, end_date.Date.DateTime);
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByNameType(name.Text, type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem == null)
            {
                var packages = DbWrapper.getPackagesByNamePriceDate(name.Text, Convert.ToInt32(min_price), Convert.ToInt32(max_price),
                                                                    begin_date.Date.DateTime, end_date.Date.DateTime);
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date == DateTimeOffset.MinValue && end_date.Date == DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByNamePriceType(name.Text, Convert.ToInt32(min_price), Convert.ToInt32(max_price),
                                                                    type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && String.IsNullOrEmpty(min_price.Text) && String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByNameDateType(name.Text, begin_date.Date.DateTime, end_date.Date.DateTime,
                                                                  type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByPriceDateType(Convert.ToInt32(min_price), Convert.ToInt32(max_price),
                                                                    begin_date.Date.DateTime, end_date.Date.DateTime,
                                                                    type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(min_price.Text) && !String.IsNullOrEmpty(max_price.Text)
                && begin_date.Date != DateTimeOffset.MinValue && end_date.Date != DateTimeOffset.MinValue &&
                type_combo.SelectedItem != null)
            {
                var packages = DbWrapper.getPackagesByAll(name.Text, Convert.ToInt32(min_price), Convert.ToInt32(max_price),
                                                          begin_date.Date.DateTime, end_date.Date.DateTime,
                                                          type_combo.SelectedValue.ToString());
                foreach (Package pack in packages)
                {
                    //set gridview components
                }
            }

        }

        private void Home(object sender, RoutedEventArgs e)
        {

        }

        private void Packages(object sender, RoutedEventArgs e)
        {
            // this.Frame.Navigate(typeof(AdminControl), null);
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

     
       
    }


}
