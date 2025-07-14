using Microsoft.Maui.Controls;
using SkiaSharp;
using System.IO;

namespace PhotoEditorApp
{
    public partial class EditorPage : ContentPage
    {
        private SKBitmap originalBitmap;
        private SKBitmap filteredBitmap;
        private SKBitmap editedBitmap;

        public EditorPage(string imagePath)
        {
            InitializeComponent();
            LoadImage(imagePath);
        }

        private void LoadImage(string imagePath)
        {
            try
            {
                using (var stream = File.OpenRead(imagePath))
                {
                    originalBitmap = SKBitmap.Decode(stream);
                    editedBitmap = originalBitmap.Copy();
                    filteredBitmap = originalBitmap.Copy();
                    UpdateImageSource();
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                DisplayAlert("Error", $"Failed to load image: {ex.Message}", "OK");
            }
        }

        private void UpdateImageSource()
        {
            photoImage.Source = ImageSource.FromStream(() => new MemoryStream(editedBitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray()));
        }

        private void OnBrightnessChanged(object sender, ValueChangedEventArgs e)
        {
            ApplyBrightnessContrast((float)e.NewValue, (float)contrastSlider.Value);
        }

        private void OnContrastChanged(object sender, ValueChangedEventArgs e)
        {
            ApplyBrightnessContrast((float)brightnessSlider.Value, (float)e.NewValue);
        }

        private void ApplyBrightnessContrast(float brightness, float contrast)
        {
            if (filteredBitmap == null) return;

            editedBitmap = new SKBitmap(filteredBitmap.Width, filteredBitmap.Height);

            for (int y = 0; y < filteredBitmap.Height; y++)
            {
                for (int x = 0; x < filteredBitmap.Width; x++)
                {
                    var color = filteredBitmap.GetPixel(x, y);

                    var r = (byte)Clamp((color.Red - 128) * contrast / 128 + 128 + brightness, 0, 255);
                    var g = (byte)Clamp((color.Green - 128) * contrast / 128 + 128 + brightness, 0, 255);
                    var b = (byte)Clamp((color.Blue - 128) * contrast / 128 + 128 + brightness, 0, 255);

                    editedBitmap.SetPixel(x, y, new SKColor(r, g, b));
                }
            }

            UpdateImageSource();
        }

        private float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        private void OnApplyGrayscaleFilterClicked(object sender, EventArgs e)
        {
            ApplyGrayscaleFilter();
        }

        private void ApplyGrayscaleFilter()
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    var color = originalBitmap.GetPixel(x, y);
                    var gray = (byte)((color.Red + color.Green + color.Blue) / 3);
                    filteredBitmap.SetPixel(x, y, new SKColor(gray, gray, gray));
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private void OnApplySepiaFilterClicked(object sender, EventArgs e)
        {
            ApplySepiaFilter();
        }

        private void ApplySepiaFilter()
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    var color = originalBitmap.GetPixel(x, y);

                    var r = (byte)Clamp((color.Red * 0.393f) + (color.Green * 0.769f) + (color.Blue * 0.189f), 0, 255);
                    var g = (byte)Clamp((color.Red * 0.349f) + (color.Green * 0.686f) + (color.Blue * 0.168f), 0, 255);
                    var b = (byte)Clamp((color.Red * 0.272f) + (color.Green * 0.534f) + (color.Blue * 0.131f), 0, 255);

                    filteredBitmap.SetPixel(x, y, new SKColor(r, g, b));
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private void OnInvertColorsClicked(object sender, EventArgs e)
        {
            ApplyInvertColors();
        }

        private void ApplyInvertColors()
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    var color = originalBitmap.GetPixel(x, y);

                    var r = (byte)(255 - color.Red);
                    var g = (byte)(255 - color.Green);
                    var b = (byte)(255 - color.Blue);

                    filteredBitmap.SetPixel(x, y, new SKColor(r, g, b));
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private void OnApplyBlurClicked(object sender, EventArgs e)
        {
            ApplyBlur();
        }

        private void ApplyBlur()
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 1; y < originalBitmap.Height - 1; y++)
            {
                for (int x = 1; x < originalBitmap.Width - 1; x++)
                {
                    int r = 0, g = 0, b = 0;

                    for (int ky = -1; ky <= 1; ky++)
                    {
                        for (int kx = -1; kx <= 1; kx++)
                        {
                            var color = originalBitmap.GetPixel(x + kx, y + ky);
                            r += color.Red;
                            g += color.Green;
                            b += color.Blue;
                        }
                    }

                    r /= 9;
                    g /= 9;
                    b /= 9;

                    filteredBitmap.SetPixel(x, y, new SKColor((byte)r, (byte)g, (byte)b));
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            // Логика сохранения настроек (например, в файл или базу данных)
            // В данном примере мы просто выводим сообщение
            DisplayAlert("Settings Saved", "Your settings have been saved successfully.", "OK");
        }

        private async void OnLoadPhotoClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Please select an image"
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        originalBitmap = SKBitmap.Decode(stream);
                        editedBitmap = originalBitmap.Copy();
                        filteredBitmap = originalBitmap.Copy();
                        ResetAllSettings();
                        UpdateImageSource();
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                DisplayAlert("Error", $"Failed to load image: {ex.Message}", "OK");
            }
        }
        private void ResetAllSettings()
        {
            brightnessSlider.Value = 50;
            contrastSlider.Value = 50;
            saturationSlider.Value = 1;
            hueSlider.Value = 0;
            gammaSlider.Value = 1;
        }

        private void OnSaturationChanged(object sender, ValueChangedEventArgs e)
        {
            ApplySaturation((float)e.NewValue);
        }

        private void ApplySaturation(float saturation)
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    var color = originalBitmap.GetPixel(x, y);

                    var hsv = RgbToHsv(color.Red, color.Green, color.Blue);
                    hsv.Item2 = Clamp(hsv.Item2 * saturation, 0, 1);
                    var newColor = HsvToRgb(hsv.Item1, hsv.Item2, hsv.Item3);

                    filteredBitmap.SetPixel(x, y, newColor);
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private void OnHueChanged(object sender, ValueChangedEventArgs e)
        {
            ApplyHue((float)e.NewValue);
        }

        private void ApplyHue(float hue)
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    var color = originalBitmap.GetPixel(x, y);

                    var hsv = RgbToHsv(color.Red, color.Green, color.Blue);
                    hsv.Item1 = (hsv.Item1 + hue) % 360;
                    var newColor = HsvToRgb(hsv.Item1, hsv.Item2, hsv.Item3);

                    filteredBitmap.SetPixel(x, y, newColor);
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private void OnGammaChanged(object sender, ValueChangedEventArgs e)
        {
            ApplyGamma((float)e.NewValue);
        }

        private void ApplyGamma(float gamma)
        {
            if (originalBitmap == null) return;

            filteredBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);

            for (int y = 0; y < originalBitmap.Height; y++)
            {
                for (int x = 0; x < originalBitmap.Width; x++)
                {
                    var color = originalBitmap.GetPixel(x, y);

                    var r = (byte)(Math.Pow(color.Red / 255.0, gamma) * 255);
                    var g = (byte)(Math.Pow(color.Green / 255.0, gamma) * 255);
                    var b = (byte)(Math.Pow(color.Blue / 255.0, gamma) * 255);

                    filteredBitmap.SetPixel(x, y, new SKColor(r, g, b));
                }
            }

            editedBitmap = filteredBitmap.Copy();
            UpdateImageSource();
        }

        private (float, float, float) RgbToHsv(byte r, byte g, byte b)
        {
            float rd = r / 255.0f;
            float gd = g / 255.0f;
            float bd = b / 255.0f;

            float max = Math.Max(rd, Math.Max(gd, bd));
            float min = Math.Min(rd, Math.Min(gd, bd));
            float delta = max - min;

            float h = 0;
            if (delta != 0)
            {
                if (max == rd)
                {
                    h = (gd - bd) / delta % 6;
                }
                else if (max == gd)
                {
                    h = (bd - rd) / delta + 2;
                }
                else
                {
                    h = (rd - gd) / delta + 4;
                }
                h *= 60;
                if (h < 0) h += 360;
            }

            float s = max == 0 ? 0 : delta / max;
            float v = max;

            return (h, s, v);
        }

        private SKColor HsvToRgb(float h, float s, float v)
        {
            int hi = (int)(h / 60) % 6;
            float f = h / 60 - (int)(h / 60);
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);

            float r, g, b;
            switch (hi)
            {
                case 0:
                    r = v; g = t; b = p;
                    break;
                case 1:
                    r = q; g = v; b = p;
                    break;
                case 2:
                    r = p; g = v; b = t;
                    break;
                case 3:
                    r = p; g = q; b = v;
                    break;
                case 4:
                    r = t; g = p; b = v;
                    break;
                case 5:
                    r = v; g = p; b = q;
                    break;
                default:
                    r = 0; g = 0; b = 0;
                    break;
            }

            return new SKColor((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

    }
}