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
            // �������� ����������� �� ������� ��������
            // � ������ ������� �� ����� ������������ ��������� ���� � ������������
            imagePaths.Add("dotnet_bot.png");
            imagePaths.Add("dotnet_bot.png");
            imagePaths.Add("dotnet_bot.png");
            // �������� �������� ���� � ������������ �� ������� ��������
        }

        private void OnImageSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is string selectedImage)
            {
                // ������� �������� � ��������� ������������
                Navigation.PushAsync(new EditorPage(selectedImage));
                (sender as CollectionView).SelectedItem = null; // ����� ���������
            }
        }
    }
}