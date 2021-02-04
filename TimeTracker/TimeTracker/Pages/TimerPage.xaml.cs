using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerPage : ContentPage
	{
		private int _hourCost;
		public int HourCost
		{
			get => _hourCost;
			set
			{
				if(value != _hourCost)
				{
					_hourCost = value;
					OnPropertyChanged(nameof(HourCost));
				}
				
			}
		}


		public TimerPage()
		{
			InitializeComponent();
			BindingContext = this;
			Slid.Value = 55;
		}

		private void StartDay(object sender, EventArgs e)
		{
			BtnAnimation(bnt_start, "start");
		}

		private void PauseDay(object sender, EventArgs e)
		{
			BtnAnimation(bnt_pause, "pause");
		}

		private void StopDay(object sender, EventArgs e)
		{
			BtnAnimation(bnt_stop, "stop");
		}

		/// <summary>
		/// Анимация для кнопки назад.
		/// </summary>
		private async void BtnAnimation(ImageButton btn,string nameButton)
		{
			await Task.Delay(100);
			btn.Source = $"tap_button_{nameButton}.png";
			
			await Task.Delay(150);

			
			btn.Source = $"button_{nameButton}.png";
		}

		private void ChangeHourCost(object sender, ValueChangedEventArgs e)
		{
			HourCost = (int)e.NewValue;
		}

		private void IncreaseValue(object sender, EventArgs e)
		{
			Slid.Value++;
		}

		private void DecreaseValue(object sender, EventArgs e)
		{
			Slid.Value--;
		}
	}
}