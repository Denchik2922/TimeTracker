using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeTracker.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarPage : ContentPage
	{


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

		public List<WorkDay> worksDays;

		public CalendarPage()
		{
			InitializeComponent();
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
			BackgroundColor = Color.FromHex("#E5E5E5");

			worksDays = new List<WorkDay>
			{	
				new WorkDay(DateTime.Now, 55),
				new WorkDay(DateTime.Now, 55),
				new WorkDay(DateTime.Now, 554),
				new WorkDay(DateTime.Now, 55),
				new WorkDay(DateTime.Now, 55),
				new WorkDay(DateTime.Now, 55),
				new WorkDay(DateTime.Now, 55),
			};

			MonthWorkDays = worksDays;


		}

		private void Add_Work_Day(object sender, EventArgs e)
		{
			BtnAnimation("add");
		}

		private void Delete_Work_Day(object sender, EventArgs e)
		{
			BtnAnimation("delete");
		}

		/// <summary>
		/// Анимация для кнопки назад.
		/// </summary>
		private async void BtnAnimation(string name)
		{
			await Task.Delay(100);
			btn_add.Source = $"tap_{name}_work_day.png";

			await Task.Delay(150);


			btn_add.Source = $"{name}_work_day.png";
		}

		private void Select_Work_Day(object sender, ItemTappedEventArgs e)
		{
			WorkDay workDay = e.Item as WorkDay;
			var work = worksDays.SingleOrDefault(w => w.id == workDay.id);
			work.IsSelected = !work.IsSelected;

			int count = worksDays.Count(w => w.IsSelected);

			if(count <= 0)
			{
				btn_add.Source = "add_work_day.png";

				
				btn_add.Clicked -= Delete_Work_Day;
				btn_add.Clicked += Add_Work_Day;
				
			}
			else
			{
				btn_add.Source = "delete_work_day.png";
				

				btn_add.Clicked -= Delete_Work_Day;
				btn_add.Clicked += Delete_Work_Day;
				btn_add.Clicked -= Add_Work_Day;
			}

			MonthWorkDays = worksDays.Select(w => w).ToList();

			

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			
		}

		

		private void EditDay(object sender, EventArgs e)
		{
			string dayId = (sender as ImageButton).CommandParameter.ToString();
		}
	}
}