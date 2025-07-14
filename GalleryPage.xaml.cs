using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace PhotoEditorApp
{
    public partial class GalleryPage : ContentPage
    {
        private ObservableCollection<string> imagePaths;

        public GalleryPage()
        {
            InitializeComponent();
            imagePaths = new ObservableCollection<string>();
            galleryView.ItemsSource = imagePaths;
            LoadGallery();
        }

        private void LoadGallery()
        {
            // Загрузка изображений из галереи телефона
            // В данном примере мы будем использовать фиктивные пути к изображениям
            imagePaths.Add("dotnet_bot.png");
            imagePaths.Add("dotnet_bot.png");
            imagePaths.Add("dotnet_bot.png");
            // Добавьте реальные пути к изображениям из галереи телефона
        }

        private void OnImageSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is string selectedImage)
            {
                // Открыть редактор с выбранным изображением
                Navigation.PushAsync(new EditorPage(selectedImage));
                (sender as CollectionView).SelectedItem = null; // Снять выделение
            }
        }
    }
}