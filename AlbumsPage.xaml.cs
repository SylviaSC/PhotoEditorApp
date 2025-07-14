using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace PhotoEditorApp
{
    public partial class AlbumsPage : ContentPage
    {
        private ObservableCollection<Album> albums;
        private int columns = 2;

        public AlbumsPage()
        {
            InitializeComponent();
            albums = new ObservableCollection<Album>();
            albumsView.ItemsSource = albums;
            LoadAlbums();

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            albumsView.GestureRecognizers.Add(pinchGesture);
        }

        private void LoadAlbums()
        {
            // �������� ��������
            // � ������ ������� �� ����� ������������ ��������� ������
            albums.Add(new Album { Name = "Album 1", ImagePath = "dotnet_bot.png" });
            albums.Add(new Album { Name = "Album 2", ImagePath = "dotnet_bot.png" });
            albums.Add(new Album { Name = "Album 3", ImagePath = "dotnet_bot.png" });
            // �������� �������� ������ ��������
        }

        private void OnAlbumSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Album selectedAlbum)
            {
                // ������� �������� � ��������� ��������
                Navigation.PushAsync(new EditorPage(selectedAlbum.ImagePath));
                (sender as CollectionView).SelectedItem = null; // ����� ���������
            }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                // ��������� ��������� ������ ������
            }
            else if (e.Status == GestureStatus.Running)
            {
                // �������� ������ ������ � ����������� �� ��������
                if (e.Scale > 1.2)
                {
                    columns = Math.Max(1, columns - 1);
                }
                else if (e.Scale < 0.8)
                {
                    columns = Math.Min(4, columns + 1);
                }
                albumsView.ItemsLayout = new GridItemsLayout(columns, ItemsLayoutOrientation.Vertical);
            }
            else if (e.Status == GestureStatus.Completed)
            {
                // ��������� ��������� ������� ������
            }
        }
    }

    public class Album
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}