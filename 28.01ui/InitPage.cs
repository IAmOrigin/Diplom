using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls;

namespace _28._01ui
{
	public static class InitPage
	{
		public static void InitSlideUp(this Grid grid)
		{
			// Создаем анимацию для свойства Y TranslateTransform
			DoubleAnimation slideUpAnimation = new DoubleAnimation
			{
				From = 20, // Начальное значение (смещение вниз)
				To = 0,     // Конечное значение (вернуться на место)
				Duration = TimeSpan.FromSeconds(0.45), // Длительность анимации
				EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } // Плавное замедление
			};
			// Запускаем анимацию
			grid.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slideUpAnimation);
		}

		public static void ContentSlideUp(this Grid grid)
		{
			// Создаем анимацию для свойства Y TranslateTransform
			DoubleAnimation slideUpAnimation = new DoubleAnimation
			{
				From = 20, // Начальное значение (смещение вниз)
				To = 0,     // Конечное значение (вернуться на место)
				Duration = TimeSpan.FromSeconds(0.77), // Длительность анимации
				EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut } // Плавное замедление
			};
			// Запускаем анимацию
			grid.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slideUpAnimation);
		}
	}
}
