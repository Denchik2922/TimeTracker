using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeTracker.ExtraClass;

namespace TimeTracker.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerPage : ContentPage
	{
		/// <summary>
		/// Название файла для всех рабочих дней.
		/// </summary>
		private readonly string NAME_FILE_ALL_WORK_DAYS = "all_work_days";

		/// <summary>
		/// Название файла для рабочего дня.
		/// </summary>
		private readonly string NAME_FILE_CURRENT_WORK_DAY = "work_day";

		/// <summary>
		/// Статус файла.
		/// </summary>
		private bool IsTimerRun { get; set; } = false;

		/// <summary>
		/// Стоимость часа.
		/// </summary>
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

		/// <summary>
		/// Время таймера.
		/// </summary>
		private TimeSpan _timeToday;
		public TimeSpan TimeToday
		{
			get => _timeToday;
			set
			{
				if (value != _timeToday)
				{
					_timeToday = value;
					OnPropertyChanged(nameof(TimeToday));
				}
			}
		}

		/// <summary>
		/// Время таймера.
		/// </summary>
		private TimeSpan _relaxToday;
		public TimeSpan RelaxToday
		{
			get => _relaxToday;
			set
			{
				if (value != _relaxToday)
				{
					_relaxToday = value;
					OnPropertyChanged(nameof(RelaxToday));
				}
			}
		}

		/// <summary>
		/// Заработано за смену.
		/// </summary>
		private decimal _earnToday;
		public decimal EarnToday
		{
			get => _earnToday;
			set
			{
				if (value != _earnToday)
				{
					_earnToday = value;
					OnPropertyChanged(nameof(EarnToday));
				}
			}
		}

		/// <summary>
		/// Рабочии дни.
		/// </summary>
		public List<WorkDay> WorkDays { get; set; }

		/// <summary>
		/// Рабочий день.
		/// </summary>
		public WorkDay CurrentWorkDay { get; set; }

		/// <summary>
		/// Контроль данных.
		/// </summary>
		public SerializeData Saver { get; }

		public TimerPage()
		{

			InitializeComponent();
			BindingContext = this;
			Saver = new SerializeData();
			CurrentWorkDay = Saver.Load<WorkDay>(NAME_FILE_CURRENT_WORK_DAY);
			WorkDays = Saver.Load<List<WorkDay>>(NAME_FILE_ALL_WORK_DAYS);
			Slid.Value = (double)CurrentWorkDay.HourCost;
			RestartWork();

		}

		/// <summary>
		/// Перезапуск таймера.
		/// </summary>
		private void RestartWork()
		{
			if(CurrentWorkDay.StatusDay == Status.Stop)
			{
				pause_block.IsVisible = false;
				btn_start.IsVisible = true;
			}
			else if(CurrentWorkDay.StatusDay == Status.Run)
			{
				RunTimer();
				hour_cost_block.IsVisible = false;
				RelaxToday = CurrentWorkDay.TotalRelax;
				btn_pause.IsVisible = true;
				btn_stop.IsVisible = true;
			}
			else
			{
				RunTimer();
				CurrentWorkDay.End = DateTime.Now;
				CurrentWorkDay.EndRelax = DateTime.Now;
				TimeToday = CurrentWorkDay.Total;
				EarnToday = CurrentWorkDay.Earning;
				hour_cost_block.IsVisible = false;
				pause_block.IsVisible = true;
				btn_end_pause.IsVisible = true;
			}
		}

		/// <summary>
		/// Начать рабочий день.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StartDay(object sender, EventArgs e)
		{
			CurrentWorkDay = new WorkDay(DateTime.Now, HourCost);
			
			CurrentWorkDay.StatusDay = Status.Run;
			hour_cost_block.IsVisible = false;
			Saver.Save(CurrentWorkDay, NAME_FILE_CURRENT_WORK_DAY);
			RunTimer();
	
			

			btn_start.IsVisible = false;
			btn_pause.IsVisible = true;
			btn_stop.IsVisible = true;
		}

		/// <summary>
		/// Поставить паузу.
		/// </summary>
		private void PauseDay(object sender, EventArgs e)
		{
			CurrentWorkDay.StartRelax = DateTime.Now;
			CurrentWorkDay.StatusDay = Status.Pause;
			Saver.Save(CurrentWorkDay, NAME_FILE_CURRENT_WORK_DAY);

			pause_block.IsVisible = true;
			btn_end_pause.IsVisible = true;
			btn_pause.IsVisible = false;
			btn_stop.IsVisible = false;
		}

		/// <summary>
		/// Поставить паузу.
		/// </summary>
		private void EndPause(object sender, EventArgs e)
		{
			CurrentWorkDay.EndRelax = DateTime.Now;
			CurrentWorkDay.StatusDay = Status.Run;
			Saver.Save(CurrentWorkDay, NAME_FILE_CURRENT_WORK_DAY);

			btn_end_pause.IsVisible = false;
			btn_pause.IsVisible = true;
			btn_stop.IsVisible = true;
		}

		/// <summary>
		/// Завершить рабочий день.
		/// </summary>
		private void StopDay(object sender, EventArgs e)
		{
			CurrentWorkDay.End = DateTime.Now;
			CurrentWorkDay.StatusDay = Status.Stop;
			hour_cost_block.IsVisible = true;

			WorkDays.Add(CurrentWorkDay);

			Saver.Save(WorkDays, NAME_FILE_ALL_WORK_DAYS);
			Saver.Delete(NAME_FILE_CURRENT_WORK_DAY);

			btn_start.IsVisible = true;
			btn_pause.IsVisible = false;
			btn_stop.IsVisible = false;

		}

		/// <summary>
		/// Запустить таймер.
		/// </summary>
		private void RunTimer()
		{
			if (IsTimerRun == false)
			{
				IsTimerRun = true;
				Device.StartTimer(TimeSpan.FromSeconds(1),() => 
				{
					if(CurrentWorkDay.StatusDay == Status.Run)
					{
						CurrentWorkDay.End = DateTime.Now;
						TimeToday = CurrentWorkDay.Total;
						EarnToday = CurrentWorkDay.Earning;
					}
					else if(CurrentWorkDay.StatusDay == Status.Pause)
					{
						CurrentWorkDay.EndRelax = DateTime.Now;
						RelaxToday = CurrentWorkDay.TotalRelax;
					}
					else if(CurrentWorkDay.StatusDay == Status.Stop)
					{
						IsTimerRun = false;
					}

					return IsTimerRun;
				});
			}
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

		protected override void OnAppearing()
		{
			base.OnAppearing();
			WorkDays = Saver.Load<List<WorkDay>>(NAME_FILE_ALL_WORK_DAYS);
		}
	}
}