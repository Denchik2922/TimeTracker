using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.ExtraClass;
using TimeTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarPage : ContentPage
	{
		/// <summary>
		/// Название файла для всех рабочих дней.
		/// </summary>
		private readonly string NAME_FILE_ALL_WORK_DAYS = "all_work_days";

		/// <summary>
		/// Дата.
		/// </summary>
		private DateTime date;
		public DateTime Date
		{
			get => date;
			set
			{
				if (value != date)
				{
					date = value;
					OnPropertyChanged(nameof(Date));
					SortWorkDays();
				}
			}
		}

		/// <summary>
		/// Рабочии дни.
		/// </summary>
		private List<WorkDay> monthWorkDays;
		public List<WorkDay> MonthWorkDays
		{
			get => monthWorkDays;
			set
			{
				monthWorkDays = value;
				OnPropertyChanged(nameof(MonthWorkDays));
			}
		}

		/// <summary>
		/// Часов за месяц.
		/// </summary>
		private string monthTime;
		public string MonthTime
		{
			get => monthTime;
			set
			{
				if (value != monthTime)
				{
					monthTime = value;
					OnPropertyChanged(nameof(MonthTime));
				}
			}
		}

		/// <summary>
		/// Заработано за месяц.
		/// </summary>
		private decimal monthEarn;
		public decimal MonthEarn
		{
			get => monthEarn;
			set
			{
				if (value != monthEarn)
				{
					monthEarn = value;
					OnPropertyChanged(nameof(MonthEarn));
				}
			}
		}

		/// <summary>
		/// Рабочии дни.
		/// </summary>
		private List<WorkDay> WorkDays { get; set; }

		/// <summary>
		/// Контроль данных.
		/// </summary>
		public SerializeData Saver { get; }

		public CalendarPage()
		{
			InitializeComponent();
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex("#E5E5E5");
			Saver = new SerializeData();
			Date = DateTime.Today;
		}

		/// <summary>
		/// Добавить робочий день
		/// </summary>
		private async void Add_Work_Day(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new WorkDayPage());
		}

		/// <summary>
		/// Удалить робочий день
		/// </summary>
		private async void Delete_Work_Day(object sender, EventArgs e)
		{
			int count = WorkDays.Count(n => n.IsSelected);
			if (count == 0)
			{
				await DisplayAlert("Внимание", "Вы должны выбрать елементы которые хотите удалить", "Ok");
			}
			else
			{
				bool IsDelete = await DisplayAlert("Внимание", $"Вы уверены что хотите удалить {(count == 1 ? "эту смену" : "эти смены")} ", "Да", "Отмена");
				if (IsDelete)
				{
					var IsSelectedCount = WorkDays.RemoveAll(n => n.IsSelected);
					Saver.Save(WorkDays, NAME_FILE_ALL_WORK_DAYS);
					OnAppearing();
				}

			}
		}

		/// <summary>
		/// Редактировать день.
		/// </summary>
		private async void EditDay(object sender, EventArgs e)
		{
			var pathSender = sender as Path;

			if (pathSender.GestureRecognizers.Count > 0)
			{
				var gesture = (TapGestureRecognizer)pathSender.GestureRecognizers[0];
				int dayId = (int)gesture.CommandParameter;
				
				await Navigation.PushAsync(new WorkDayPage(dayId));
			}
		}

		/// <summary>
		/// Выделить дни
		/// </summary>
		private void Select_Work_Day(object sender, ItemTappedEventArgs e)
		{
			WorkDay workDay = e.Item as WorkDay;
			var work = WorkDays.SingleOrDefault(w => w.Id == workDay.Id);
			work.IsSelected = !work.IsSelected;

			int count = WorkDays.Count(w => w.IsSelected);

			if (count <= 0)
			{
				btn_add.IsVisible = true;
				btn_delete.IsVisible = false;
			}
			else
			{
				btn_add.IsVisible = false;
				btn_delete.IsVisible = true;
			}

			SortWorkDays();
		}

		/// <summary>
		/// Сортировка данных.
		/// </summary>
		public void SortWorkDays()
		{
			MonthWorkDays = WorkDays?.Where(w => (w.Date.Month == Date.Month) && (w.Date.Year == Date.Year)).ToList() ?? new List<WorkDay>();
			MonthEarn = MonthWorkDays.Sum(w => w.Earning);
			TimeSpan time = new TimeSpan(MonthWorkDays.Sum(w => w.Total.Ticks));
			MonthTime = string.Format("{0}:{1:mm}:{1:ss}", (int)time.TotalHours, time);


		}

		/// <summary>
		/// Переход на предыдущий месяц.
		/// </summary>
		private void PrevMonth(object sender, EventArgs e)
		{
			Date = Date.AddMonths(-1);
		}

		/// <summary>
		/// Переход на следущий месяц.
		/// </summary>
		private void NextMonth(object sender, EventArgs e)
		{
			Date = Date.AddMonths(1);
		}



		protected override void OnAppearing()
		{
			base.OnAppearing();
			WorkDays = Saver.Load<List<WorkDay>>(NAME_FILE_ALL_WORK_DAYS);
			btn_add.IsVisible = true;
			btn_delete.IsVisible = false;
			SortWorkDays();
		}

		
		
	}
}