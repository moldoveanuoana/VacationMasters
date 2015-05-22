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
using VacationMasters.Wrappers;
using VacationMasters.UserManagement;
using System.Threading.Tasks;
using VacationMasters.Essentials;
using VacationMasters.PackageManagement;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace VacationMasters.Screens
{
    public sealed partial class PackagesDisplay : UserControl
    {
        private IUserManager _userManager;
        public UserManager UserManager { get; set; }
        public DbWrapper DbWrapper { get; set; }
     
        public PackagesDisplay()
        {
            
            this.InitializeComponent();
            Task.Run(() => Initialize());
        }

        private  void Initialize()
        {
             DbWrapper = new DbWrapper();
             UserManager = new UserManager(DbWrapper);
             var list = DbWrapper.getRandomPackages();
            
             
             
             
        }


     


       




    }
}
