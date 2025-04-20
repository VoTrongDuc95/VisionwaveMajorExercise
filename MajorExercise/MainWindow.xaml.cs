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
using Ookii.Dialogs.Wpf;
using Newtonsoft.Json;  


namespace MajorExercise
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class ImageItem
    {
        public string FilePath { get; set; }
        public BitmapImage ImageSource { get; set; }
        public bool IsFavorite { get; set; }
    }
    public partial class MainWindow : Window
    {
        private readonly string favoritesFolderPath;
        private readonly string favoritesJsonPath;
        public MainWindow()
        {
            InitializeComponent();
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            favoritesFolderPath = Path.Combine(projectDirectory, "Favorites");
            favoritesJsonPath = Path.Combine(projectDirectory, "favorites.json");
            LoadFavoriteJson();

            if (!Directory.Exists(favoritesFolderPath))
            {
                Directory.CreateDirectory(favoritesFolderPath);
            }
        }
        // save the path of the image
        private List<string> imageFiles = new List<string>();
        // save the path of the favorite image
        public Dictionary<string, bool> FavoriteImage = new Dictionary<string, bool>();
        public string nameImage;
        private List<ImageItem> allImages = new List<ImageItem>();
        private List<ImageItem> favoriteImages = new List<ImageItem>();

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog() == true)
            {
                string folderPath = dialog.SelectedPath;

                CurrentStatus.Content = "Current Status: Loading...";
                await System.Threading.Tasks.Task.Delay(100); // Cho UI kịp render

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                if (cbSubFolder.IsChecked == true)
                {
                    imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                        .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                        .ToList();
                }
                else
                {
                    imageFiles = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                        .ToList();
                }

                allImages = imageFiles.Select(file =>
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(file);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return new ImageItem
                    {
                        FilePath = file,
                        ImageSource = bitmap,
                        IsFavorite = FavoriteImage.ContainsKey(Path.GetFileName(file)) && FavoriteImage[Path.GetFileName(file)]
                    };
                }).ToList();
                cbViewImage.IsChecked = false;

                ImageItemsControl.ItemsSource = allImages;

                ImageFound.Content = $"Image Found: {allImages.Count}";
                ShowFavoriteImage();

                CurrentStatus.Content = "Current Status: Ready";
            }

            //using (var folderDialog = new CommonOpenFileDialog { IsFolderPicker = true })
            //{
            //    if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            //    {
            //        CurrentStatus.Content = "Current Status: Loading...";
            //        await Task.Delay(100);
            //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            //         imageFiles = Directory.GetFiles(folderDialog.FileName, "*.*", SearchOption.TopDirectoryOnly)
            //            .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
            //            .ToList();
            //        var imageSources = new List<BitmapImage>();
                    
            //        foreach (var file in imageFiles)
            //        {
            //            var bitmap = new BitmapImage();
            //            bitmap.BeginInit();
            //            bitmap.UriSource = new System.Uri(file);
            //            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            //            bitmap.EndInit();
            //            bitmap.Freeze(); // để tránh lỗi khi binding từ UI Thread
            //            imageSources.Add(bitmap);
            //        }
            //        CurrentStatus.Content = "Current Status: Ready";

            //        // DEBUG:
            //        System.Diagnostics.Debug.WriteLine($"Tìm thấy {imageSources.Count} ảnh");

            //        ImageItemsControl.ItemsSource = imageSources;
            //        ImageFound.Content = $"Image Found: {imageSources.Count}";
            //        ShowFavorieImage();
            //    }
            //}
        }

       

        private void ThumbnailClick(object sender, MouseButtonEventArgs e)
        {
            var clickedImage = sender as Image;
            if (clickedImage != null)
            {
                // Lấy đường dẫn của ảnh đã chọn
                //string selectedImagePath = imageFiles[ImageItemsControl.Items.IndexOf(clickedImage.Source)];

                var item = (clickedImage.DataContext as ImageItem);
                if (item == null) return;

                string selectedImagePath = item.FilePath;

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
                string imageSize = $"{bitmap.PixelWidth}x{bitmap.PixelHeight}px";
                // Cập nhật thông tin vào Label
                lblImageInfo.Content = fileName;
                lblImageSize.Content = imageSize;

                // Update favorite image
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

        private async void UnfavoriteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CurrentStatus.Content = "Current Status: Copying to favorites...";
            // wait for 100ms to show the status
            await System.Threading.Tasks.Task.Delay(100);
            favoriteImage.Visibility = Visibility.Visible;
            unfavoriteImage.Visibility = Visibility.Hidden;
            FavoriteImage[nameImage] = true;
            string filePath = imageFiles.FirstOrDefault(x => Path.GetFileName(x) == nameImage);
            if (filePath != null)
            {
                var imageItem = new ImageItem { FilePath = filePath };
                AddToFavorites(imageItem);
            }
            if (cbViewImage.IsChecked == true)
            {
                LoadFavoriteImages();
                UpdateImageView();
            }
            CurrentStatus.Content = "Current Status: Ready";
            ShowFavoriteImage();
            SaveFavoriteJson();

        }

        private async void FavoriteImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CurrentStatus.Content = "Current Status: Removing to favorites...";
            // wait for 100ms to show the status
            await System.Threading.Tasks.Task.Delay(100);
            favoriteImage.Visibility = Visibility.Hidden;
            unfavoriteImage.Visibility = Visibility.Visible;
            FavoriteImage[nameImage] = false;
            string favPath = Path.Combine(favoritesFolderPath, nameImage);
            if (File.Exists(favPath))
            {
                try
                {
                    File.Delete(favPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Can not delete image from favorite folder: {ex.Message}");
                }
            }
            if (cbViewImage.IsChecked == true)
            {
                LoadFavoriteImages();
                UpdateImageView();
            }
            CurrentStatus.Content = "Current Status: Ready";
            ShowFavoriteImage();
            SaveFavoriteJson();

        }
        // Show the number of favorite images
        public void ShowFavoriteImage()
        {
            var favoriteImages = FavoriteImage.Where(x => x.Value == true).Select(x => x.Key).ToList();
            lbFavoriteImages.Content = "Favorite Images: " + favoriteImages.Count.ToString();
        }
        private void AddToFavorites(ImageItem item)
        {
            string fileName = Path.GetFileName(item.FilePath);
            string destPath = Path.Combine(favoritesFolderPath, fileName);

            if (!File.Exists(destPath))
            {
                File.Copy(item.FilePath, destPath);
            }

            item.IsFavorite = true;
        }
        private void UpdateImageView()
        {
            if (cbViewImage.IsChecked == true)
                ImageItemsControl.ItemsSource = favoriteImages;
            else
            {
                ImageItemsControl.ItemsSource = allImages;
                ImageFound.Content = $"Image Found: {allImages.Count}";
            }               
        }
        private void LoadFavoriteImages()
        {
            var extensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

            favoriteImages = Directory.GetFiles(favoritesFolderPath)
                .Where(f => extensions.Contains(Path.GetExtension(f).ToLower()))
                .Select(path =>
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(path);
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    img.Freeze();

                    return new ImageItem
                    {
                        FilePath = path,
                        ImageSource = img,
                        IsFavorite = true
                    };
                }).ToList();
        }

        private async void cbViewImage_Checked(object sender, RoutedEventArgs e)
        {
            CurrentStatus.Content = "Current Status: Loading from favorite images...";
            // wait for 100ms to show the status
            await System.Threading.Tasks.Task.Delay(100);        
            LoadFavoriteImages();
            UpdateImageView();
            ShowFavoriteImage();
            ImageFound.Content = $"Image Found: {favoriteImages.Count}";
            CurrentStatus.Content = "Current Status: Ready";
        }

        private void cbViewImage_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateImageView();
            if (allImages.Count == 0)
            {
                ImageFound.Content = $"Image Found: 0";
                lbFavoriteImages.Content = "Favorite Images: 0";

            }
        }
        //Load json
        private void LoadFavoriteJson()
        {
            if (File.Exists(favoritesJsonPath))
            {
                string json = File.ReadAllText(favoritesJsonPath);
                FavoriteImage = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json)
                                ?? new Dictionary<string, bool>();
            }
            else
            {
                FavoriteImage = new Dictionary<string, bool>();
            }
        }
        //Save json
        private void SaveFavoriteJson()
        {
            string json = JsonConvert.SerializeObject(FavoriteImage, Formatting.Indented);
            File.WriteAllText(favoritesJsonPath, json);
        }
    }
}
