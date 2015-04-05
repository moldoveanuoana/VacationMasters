using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VacationMasters.Essentials;
using VacationMasters.Wrappers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        private void AdminControl(object sender, RoutedEventArgs e)
        {

        }

    }


}
