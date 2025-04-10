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
        public Dictionary<string, bool> FavoriteImage = new Dictionary<string, bool>();
        public string nameImage;
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
                    ImageFound.Content = $"Image Found: {imageSources.Count}";
                    ShowFavorieImage();
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
                nameImage = fileName;
                // Kích thước ảnh thực tế
                string imageSize = $"{bitmap.PixelWidth} x {bitmap.PixelHeight} px";
                // Cập nhật thông tin vào Label
                lblImageInfo.Content = fileName;
                lblImageSize.Content = imageSize;
                if (FavoriteImage.ContainsKey(nameImage))
                {
                    if (FavoriteImage[nameImage] == true)
                    {
                        favoriteImage.Visibility = Visibility.Visible;
                        unfavoriteImage.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        favoriteImage.Visibility = Visibility.Hidden;
                        unfavoriteImage.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    //FavoriteImage[nameImage] = false;
                    favoriteImage.Visibility = Visibility.Hidden;
                    unfavoriteImage.Visibility = Visibility.Visible;
                }
            }

        }

        private void UnfavoriteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            favoriteImage.Visibility = Visibility.Visible;
            unfavoriteImage.Visibility = Visibility.Hidden;
            FavoriteImage[nameImage] = true;
            ShowFavorieImage();

        }

        private void FavoriteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            favoriteImage.Visibility = Visibility.Hidden;
            unfavoriteImage.Visibility = Visibility.Visible;
            FavoriteImage[nameImage] = false;
            ShowFavorieImage();

        }
        public void ShowFavorieImage()
        {
            var favoriteImages = FavoriteImage.Where(x => x.Value == true).Select(x => x.Key).ToList();
            lbFavoriteImages.Content = "Favorite Images: " + favoriteImages.Count.ToString();
            //if (favoriteImages.Count > 0)
            //{
            //    var message = "Danh sách ảnh yêu thích:\n" + string.Join("\n", favoriteImages);
            //    MessageBox.Show(message, "Ảnh yêu thích", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    MessageBox.Show("Không có ảnh nào được đánh dấu yêu thích.", "Ảnh yêu thích", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
        }
    }
}
