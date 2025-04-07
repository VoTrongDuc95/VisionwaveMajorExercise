using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;


namespace MajorExercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // Biến lưu danh sách ảnh
        private List<string> imageFiles = new List<string>();

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            using (var folderDialog = new CommonOpenFileDialog { IsFolderPicker = true })
            {
                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

                     imageFiles = Directory.GetFiles(folderDialog.FileName, "*.*", SearchOption.AllDirectories)
                        .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                        .ToList();

                    var imageSources = new List<BitmapImage>();

                    foreach (var file in imageFiles)
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new System.Uri(file);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze(); // để tránh lỗi khi binding từ UI Thread

                        imageSources.Add(bitmap);
                    }

                    // DEBUG:
                    System.Diagnostics.Debug.WriteLine($"Tìm thấy {imageSources.Count} ảnh");

                    ImageItemsControl.ItemsSource = imageSources;
                }
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ThumbnailClick(object sender, MouseButtonEventArgs e)
        {
            var clickedImage = sender as Image;
            if (clickedImage != null)
            {
                // Lấy đường dẫn của ảnh đã chọn
                string selectedImagePath = imageFiles[ImageItemsControl.Items.IndexOf(clickedImage.Source)];

                // Tạo BitmapImage cho ảnh full-size
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new System.Uri(selectedImagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                // Hiển thị ảnh full-size
                ImageView.Source = bitmap;

                // Lấy thông tin ảnh: Tên và kích thước
                string fileName = Path.GetFileName(selectedImagePath);
                // Kích thước ảnh thực tế
                string imageSize = $"{bitmap.PixelWidth} x {bitmap.PixelHeight} px";
                // Cập nhật thông tin vào Label
                lblImageInfo.Content = fileName;
                lblImageSize.Content = imageSize;
            }

        }
    }
}
