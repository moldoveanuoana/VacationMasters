using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using VacationMasters.Essentials;
using VacationMasters.PackageManagement;
using VacationMasters.Wrappers;

namespace VacationMasters.Screens
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AgentPage : UserControl
    {

        private readonly IDbWrapper _dbWrapper;
        private readonly PackageManager _packageManager;
        public AgentPage()
        {
            this.InitializeComponent();
            try
            {
                _dbWrapper = new DbWrapper();
                _packageManager = new PackageManager(_dbWrapper);
                LoadCategories();
            }
            catch (Exception e)
            {
                
            }
        }

        private void LoadCategories()
        {
            Cateogry.ItemsSource = _dbWrapper.GetTypes();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            var pckName = PackageName.Text;
            var pckCategory = Cateogry.SelectedItem.ToString();
            var pckIncluded = Included.Text;
            var pckTransport = Transport.Text;
            var pckPrice = Convert.ToDouble(Price.Text);
            var pckBeginDate = BeginDate.Date.DateTime;
            var pckEndDate = EndDate.Date.DateTime;

            if (pckName == null || pckCategory == null || pckIncluded == null || pckTransport == null || pckPrice == null)
            {
                var messageDialog = new MessageDialog("Please complete all fields before saving!");
                messageDialog.ShowAsync();
            }

            if (pckBeginDate.Equals(pckEndDate))
            {
                var messageDialog = new MessageDialog("Begin Date must be different from End Date");
                messageDialog.ShowAsync();
            }

            var package = new Package(pckName, pckCategory, pckIncluded, pckTransport, pckPrice, 0, 0,
                pckBeginDate, pckEndDate, null);

            _packageManager.AddPackage(package);

        }
    }
}
