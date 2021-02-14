using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.ExtraClass;
using TimeTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Pages
{

	/// <summary>
	/// Дополнение для DateTime
	/// </summary>
	static class DateTimeHelper
	{

		public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes)
		{
			return new DateTime(
				dateTime.Year,
				dateTime.Month,
				dateTime.Day,
				hours,
				minutes,
				dateTime.Second);
		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkDayPage : ContentPage
	{
		/// <summary>
		/// Название файла для всех рабочих дней.
		/// </summary>
		private readonly string NAME_FILE_ALL_WORK_DAYS = "all_work_days";

		/// <summary>
		/// Контроль данных.
		/// </summary>
		public SerializeData Saver { get; }

		private DateTime _date;
		public DateTime Date
		{
			get => _date;
			set
			{
				
				_date = value;
				date.Text = $"{ WorkDay.Date:d MMMM}";
				OnPropertyChanged(nameof(Date));
			}
		}

		private TimeSpan _startWork;
		public TimeSpan StartWork 
		{
			get => _startWork;
			set
			{
				
				try
				{
						
					WorkDay.Start = WorkDay.Start.ChangeTime((int)value.Hours, (int)value.Minutes);
					_startWork = value;

				}
				catch (ArgumentException)
				{

					ErrorMes("Начало смены должно быть раньше ее окончания");
				}
				finally
				{
					OnPropertyChanged(nameof(StartWork));
					OnPropertyChanged(nameof(TotalWork));
					OnPropertyChanged(nameof(Earning));
				}
				
				
			} 
		}

		private TimeSpan _endWork;
		public TimeSpan EndWork
		{
			get => _endWork;
			set
			{
				try
				{
					WorkDay.End = WorkDay.End.ChangeTime((int)value.Hours, (int)value.Minutes);
					_endWork = value;

				}
				catch (ArgumentException ex)
				{
					ErrorMes("Конец смены должен быть позже ее начала");
				}
				finally
				{
					OnPropertyChanged(nameof(EndWork));
					OnPropertyChanged(nameof(TotalWork));
					OnPropertyChanged(nameof(Earning));
				}
			}
		}

		private TimeSpan _startRelax;
		public TimeSpan StartRelax
		{
			get => _startRelax;
			set
			{
				try
				{
					WorkDay.StartRelax = WorkDay.StartRelax.ChangeTime((int)value.Hours, (int)value.Minutes);
					_startRelax = value;

				}
				catch (ArgumentException ex)
				{
					ErrorMes("Начало перерыва должно быть раньше его окончания");
				}
				finally
				{
					OnPropertyChanged(nameof(StartRelax));
					OnPropertyChanged(nameof(TotalWork));
					OnPropertyChanged(nameof(Earning));
				}
			}
		}

		private TimeSpan _endRelax;
		public TimeSpan EndRelax
		{
			get => _endRelax;
			set
			{
				try
				{
					WorkDay.EndRelax = WorkDay.EndRelax.ChangeTime((int)value.Hours, (int)value.Minutes);
					_endRelax = value;

				}
				catch (ArgumentException ex)
				{
					ErrorMes("Конец перерыва должен быть позже его начала");
				}
				finally
				{
					OnPropertyChanged(nameof(EndRelax));
					OnPropertyChanged(nameof(TotalWork));
					OnPropertyChanged(nameof(Earning));
				}
			}
		}

		/// <summary>
		/// Стоимость часа.
		/// </summary>
		private int _hourCost;
		public int HourCost
		{
			get => _hourCost;
			set
			{
				if (value != _hourCost)
				{
					_hourCost = value;
					WorkDay.HourCost = value;
					OnPropertyChanged(nameof(HourCost));
					OnPropertyChanged(nameof(Earning));
				}

			}
		}

		/// <summary>
		/// Общее время.
		/// </summary>
		public TimeSpan TotalWork
		{
			get => WorkDay.Total;
		}

		/// <summary>
		/// Заработано.
		/// </summary>
		public decimal Earning
		{
			get => WorkDay.Earning;
		}


		List<WorkDay> WorkDays { get; set; }

		WorkDay WorkDay { get; }

		public WorkDayPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			
			BindingContext = this;

			Saver = new SerializeData();
			WorkDays = Saver.Load<List<WorkDay>>(NAME_FILE_ALL_WORK_DAYS);
			WorkDay = new WorkDay();

			InitializeComponent();
			SetParameters();


		}

		public WorkDayPage(int dayId) : this()
		{
			WorkDay = WorkDays.SingleOrDefault(w => w.Id == dayId);

			SetParameters();
		}


		private void SetParameters()
		{
			
			Date = WorkDay.Date;
			StartWork = WorkDay.Start.TimeOfDay;
			EndWork = WorkDay.End.TimeOfDay;
			StartRelax = WorkDay.StartRelax.TimeOfDay;
			EndRelax = WorkDay.EndRelax.TimeOfDay;
			Slid.Value = (double)WorkDay.HourCost;

		}

		/// <summary>
		/// Вывод сообщения об ошибке
		/// </summary>
		/// <param name="mes">Сообщения</param>
		private async void ErrorMes(string mes)
		{
			await DisplayAlert("Не верный ввод!", mes, "ok");
		}

		/// <summary>
		/// Изменить стоимость часа.
		/// </summary>
		private void ChangeHourCost(object sender, ValueChangedEventArgs e)
		{
			HourCost = (int)e.NewValue;
		}

		/// <summary>
		/// Прибавить к стоимости значение.
		/// </summary>
		private void IncreaseValue(object sender, EventArgs e)
		{
			Slid.Value++;
		}

		/// <summary>
		/// Убавить от стоимости значение.
		/// </summary>
		private void DecreaseValue(object sender, EventArgs e)
		{
			Slid.Value--;
		}

		/// <summary>
		/// Возврат на предыдущую страницу.
		/// </summary>
		private async void BackButton(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		/// <summary>
		/// Сохранение и возврат на предыдущую страницу.
		/// </summary>
		private async void SaveDay(object sender, EventArgs e)
		{
			WorkDay.Date = Date;

			if(!WorkDays.Exists(w => w.Id == WorkDay.Id))
			{
				WorkDays.Add(WorkDay);
			}
			Saver.Save(WorkDays, NAME_FILE_ALL_WORK_DAYS);

			await Navigation.PopAsync();
		}
	}
}