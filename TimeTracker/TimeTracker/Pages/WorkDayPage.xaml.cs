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
	public partial class WorkDayPage : ContentPage
	{
		WorkDay WorkDay { get; }

		public WorkDayPage(WorkDay workDay )
		{
			InitializeComponent();
			WorkDay = workDay;
			NavigationPage.SetHasNavigationBar(this, false);

		}

		/// <summary>
		/// Возврат на предыдущую страницу.
		/// </summary>
		private async void BackButton(object sender, EventArgs e)
		{
			
			await Navigation.PopAsync();
		}
	}
}