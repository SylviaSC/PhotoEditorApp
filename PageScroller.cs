using Microsoft.Maui.Controls;
using System;

namespace PhotoEditorApp
{
    public class PageScroller
    {
        private DateTime _startTime;
        private double _startX, _startY;
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startTime = DateTime.Now;
                    _startX = e.TotalX;
                    _startY = e.TotalY;
                    break;

                case GestureStatus.Running:
                    // Пользователь двигает палец по экрану
                    break;

                case GestureStatus.Completed:
                    // Пользователь отпустил палец
                    var endTime = DateTime.Now;
                    var totalTime = (endTime - _startTime).TotalSeconds;

                    var totalX = e.TotalX - _startX;
                    var totalY = e.TotalY - _startY;

                    var velocityX = totalX / totalTime;
                    var velocityY = totalY / totalTime;

                    IdentifySwipe(totalX, totalY, velocityX, velocityY);
                    break;
            }
        }

        private void IdentifySwipe(double totalX, double totalY, double velocityX, double velocityY)
        {
            if (Math.Abs(totalX) > Math.Abs(totalY))
            {
                // Свайп горизонтальный
                if (totalX > 0)
                {
                    Console.WriteLine("Свайп вправо");
                }
                else
                {
                    Console.WriteLine("Свайп влево");
                }
            }
            else
            {
                // Свайп вертикальный
                if (totalY > 0)
                {
                    Console.WriteLine("Свайп вниз");
                    IdentifySpeed(velocityY);
                }
                else
                {
                    Console.WriteLine("Свайп вверх");
                    IdentifySpeed(velocityY);
                }
            }
        }

        private void IdentifySpeed(double velocity)
        {
            if (velocity > 1000)
            {
                Console.WriteLine("Очень быстрый свайп");
            }
            else if (velocity > 500)
            {
                Console.WriteLine("Быстрый свайп");
            }
            else if (velocity > 200)
            {
                Console.WriteLine("Средний свайп");
            }
            else
            {
                Console.WriteLine("Медленный свайп");
            }
        }
    }



}