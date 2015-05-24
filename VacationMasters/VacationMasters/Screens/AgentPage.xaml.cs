using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private byte[] _image;
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
                pckBeginDate, pckEndDate, _image);


            _packageManager.AddPackage(package);

        }

        private async void LoadImage(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                // Ensure the stream is disposed once the image is loaded
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Set the image source to the selected bitmap
                   /* BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.DecodePixelHeight = 250;
                    bitmapImage.DecodePixelWidth = 250;

                    await bitmapImage.SetSourceAsync(fileStream);
                    _image = bitmapImage;*/

                    var reader = new Windows.Storage.Streams.DataReader(fileStream.GetInputStreamAt(0));
                    await reader.LoadAsync((uint)fileStream.Size);

                    byte[] pixels = new byte[fileStream.Size];

                    reader.ReadBytes(pixels);

                    _image = pixels;
                }
            }
        }
    }
}
