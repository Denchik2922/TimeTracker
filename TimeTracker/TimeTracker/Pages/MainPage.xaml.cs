using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.Pages;
using Xamarin.Forms;

namespace TimeTracker
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();
			Children.Add(new TimerPage() { IconImageSource = "Home.png", Title = "Таймер" });
			Children.Add(new NavigationPage(new CalendarPage()) { IconImageSource = "Celendar.png", Title = "Календарь" });
		}
	}
}
