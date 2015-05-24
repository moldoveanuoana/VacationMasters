using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using VacationMasters.Wrappers;
using VacationMasters.UserManagement;
using System.Threading.Tasks;
using VacationMasters.Essentials;
using VacationMasters.PackageManagement;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VacationMasters.Screens
{
    public sealed partial class PackagesDisplay : UserControl, INotifyPropertyChanged
    {
        private bool _isOperationInProgress;
        private string _title = String.Empty;
        private string _price = String.Empty;
        private byte[] _photo;

        private IUserManager _userManager;
        public UserManager UserManager { get; set; }
        public DbWrapper DbWrapper { get; set; }
        public bool IsOperationInProgress
        {
            get { return _isOperationInProgress; }
            set
            {
                if (value != _isOperationInProgress)
                {
                    _isOperationInProgress = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public byte[] Photo
        {
            get { return _photo; }
            set
            {
                    _photo = value;
                    NotifyPropertyChanged();
            }
        }

        public PackagesDisplay()
        {
            this.DataContext = this;  

            this.InitializeComponent();

            Package p = new Package("AAA", "AAA", "AAA", "AAA", 23, 2, 2, DateTime.Now, DateTime.Now, null);
            itemGridView.Items.Add(p);

            DbWrapper = new DbWrapper();

            Initialize();
        }

        private async void Initialize()
        {
            IsOperationInProgress = true;

            DbWrapper = new DbWrapper();
            UserManager = new UserManager(DbWrapper);
            var list = DbWrapper.getRandomPackages();

           //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => SetSource(list));
           // itemGridView.ItemsSource = List;
            Package p = new Package("AAA","AAA","AAA","AAA",23,2,2,DateTime.Now,DateTime.Now,null);
            itemGridView.Items.Add(p);
            IsOperationInProgress = false;
             
        }
   
        private void SetSource(List<Package> list)
        {
            itemGridView.ItemsSource = list;
        }

        public ObservableCollection<Package> List { get { return new ObservableCollection<Package>(DbWrapper.getRandomPackages()); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    
}
