using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using VacationMasters.Wrappers;
using VacationMasters.UserManagement;
using VacationMasters.Essentials;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VacationMasters.Screens
{
    public sealed partial class SearchResult : UserControl, INotifyPropertyChanged
    {
        private bool _isOperationInProgress;
        private string _title = String.Empty;
        private string _price = String.Empty;
        private byte[] _photo;

        private IUserManager _userManager;
        private ObservableCollection<Package> _list;
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
        


        public SearchResult()
        {
            this.DataContext = this;  

            this.InitializeComponent();

            DbWrapper = new DbWrapper();

            Initialize();
        }

           public async void Initialize()
        {
            IsOperationInProgress = true;
         
            DbWrapper = new DbWrapper();
            UserManager = new UserManager(DbWrapper);
            if(VacationMasters.MainPage.search_criterion == 1)
                List = new ObservableCollection<Package>(DbWrapper.GetPackagesByName(VacationMasters.MainPage.pk_name));
            if(VacationMasters.MainPage.search_criterion == 2)
                List = new ObservableCollection<Package>(DbWrapper.GetPackagesByPrice(VacationMasters.MainPage.pk_min_price,VacationMasters.MainPage.pk_max_price));
            if (VacationMasters.MainPage.search_criterion == 3)
                List = new ObservableCollection<Package>(DbWrapper.GetPackagesByDate(VacationMasters.MainPage.pk_begin_date, VacationMasters.MainPage.pk_end_date));
            if (VacationMasters.MainPage.search_criterion == 4)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByType(VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 5)
                 List = new ObservableCollection<Package>(DbWrapper.getPackagesByPriceDate(VacationMasters.MainPage.pk_min_price,
                                                                                               VacationMasters.MainPage.pk_max_price,
                                                                                               VacationMasters.MainPage.pk_begin_date,
                                                                                               VacationMasters.MainPage.pk_end_date));
            if (VacationMasters.MainPage.search_criterion == 6)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByPriceType(VacationMasters.MainPage.pk_min_price,
                                                                                          VacationMasters.MainPage.pk_max_price,
                                                                                          VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 7)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByDateType(VacationMasters.MainPage.pk_begin_date,
                                                                                         VacationMasters.MainPage.pk_end_date,
                                                                                         VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 8)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByNamePrice(VacationMasters.MainPage.pk_name,
                                                                                          VacationMasters.MainPage.pk_min_price,
                                                                                          VacationMasters.MainPage.pk_max_price));
            if (VacationMasters.MainPage.search_criterion == 9)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByNameDate(VacationMasters.MainPage.pk_name,
                                                                                         VacationMasters.MainPage.pk_begin_date,
                                                                                         VacationMasters.MainPage.pk_end_date));
            if (VacationMasters.MainPage.search_criterion == 10)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByNameType(VacationMasters.MainPage.pk_name, VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 11)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByNamePriceDate(VacationMasters.MainPage.pk_name,
                                                                                              VacationMasters.MainPage.pk_min_price,
                                                                                              VacationMasters.MainPage.pk_max_price,
                                                                                              VacationMasters.MainPage.pk_begin_date,
                                                                                              VacationMasters.MainPage.pk_end_date));
            if (VacationMasters.MainPage.search_criterion == 12)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByNamePriceType(VacationMasters.MainPage.pk_name,
                                                                                              VacationMasters.MainPage.pk_min_price,
                                                                                              VacationMasters.MainPage.pk_max_price,
                                                                                              VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 13)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByNameDateType(VacationMasters.MainPage.pk_name,
                                                                                             VacationMasters.MainPage.pk_begin_date,
                                                                                             VacationMasters.MainPage.pk_end_date,
                                                                                             VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 14)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByPriceDateType(VacationMasters.MainPage.pk_min_price,
                                                                                              VacationMasters.MainPage.pk_max_price,
                                                                                              VacationMasters.MainPage.pk_begin_date,
                                                                                              VacationMasters.MainPage.pk_end_date,
                                                                                              VacationMasters.MainPage.pk_type));
            if (VacationMasters.MainPage.search_criterion == 15)
                List = new ObservableCollection<Package>(DbWrapper.getPackagesByAll(VacationMasters.MainPage.pk_name,
                                                                                    VacationMasters.MainPage.pk_min_price,
                                                                                    VacationMasters.MainPage.pk_max_price,
                                                                                    VacationMasters.MainPage.pk_begin_date,
                                                                                    VacationMasters.MainPage.pk_end_date,
                                                                                    VacationMasters.MainPage.pk_type));
   
            IsOperationInProgress = false;
            
        }

           public ObservableCollection<Package> List
        {
            get { return _list; }
            set
            {
                _list = value;
                NotifyPropertyChanged();
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ItemGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var frame = (Frame)Window.Current.Content;
            var page = (MainPage)frame.Content;
            page.UpdateSelectedPackage((Package)e.ClickedItem);
            VisualStateManager.GoToState(page, "PackagePage", true);
        }
    }
    
}
