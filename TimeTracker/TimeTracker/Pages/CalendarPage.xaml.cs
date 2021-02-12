using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			
		}

		private void Delete_Work_Day(object sender, EventArgs e)
		{
			
		}

		private void Select_Work_Day(object sender, ItemTappedEventArgs e)
		{
			WorkDay workDay = e.Item as WorkDay;
			var work = worksDays.SingleOrDefault(w => w.id == workDay.id);
			work.IsSelected = !work.IsSelected;

			int count = worksDays.Count(w => w.IsSelected);

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

			MonthWorkDays = worksDays.Select(w => w).ToList();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			
		}

		

		private async void EditDay(object sender, EventArgs e)
		{	
			var pathSender = sender as Path;

			if (pathSender.GestureRecognizers.Count > 0)
			{
				var gesture = (TapGestureRecognizer)pathSender.GestureRecognizers[0];
				int dayId = (int)gesture.CommandParameter;
				WorkDay day = worksDays.SingleOrDefault(w => w.id == dayId);
				await Navigation.PushAsync(new WorkDayPage(day));
			}
		}
	}
}