using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Models
{
	[Serializable]
	public class WorkDay
	{
		public int id { get; }

		public bool IsSelected { get; set; }

		/// <summary>
		/// Дата
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Начало работы
		/// </summary>
		public DateTime Start { get; set; }


		/// <summary>
		/// Конец работы
		/// </summary>
		public DateTime End { get; set; }

		/// <summary>
		/// Всего наработано
		/// </summary>
		public TimeSpan Total
		{
			get => (End - Start) - TotalRelax;

		}

		/// <summary>
		/// Начало отдыха
		/// </summary>
		public DateTime StartRelax { get; set; }

		/// <summary>
		/// Конец отдыха
		/// </summary>
		public DateTime EndRelax { get; set; }

		/// <summary>
		/// Всего отдиха
		/// </summary>
		public TimeSpan TotalRelax
		{
			get => EndRelax - StartRelax;

		}

		/// <summary>
		/// Стоимость часа
		/// </summary>
		public decimal MinuteCost { get => HourCost / 60; }

		/// <summary>
		/// Стоимость часа
		/// </summary>
		public decimal HourCost { get; set; }

		/// <summary>
		/// Всего заработано
		/// </summary>
		public decimal Earning { get { return MinuteCost * (int)Total.TotalMinutes; } }


		public WorkDay(DateTime start, decimal hourCost)
		{
			if (hourCost < 1)
			{
				throw new ArgumentException("Стоимость часа не может быть меньше одного", nameof(hourCost));
			}

			Random rnd = new Random();
			id = rnd.Next(1,400000);
			HourCost = hourCost;
			Date = DateTime.Today;
			Start = start;
			End = start.AddHours(2);

		}

		public override string ToString()
		{
			return $"Дата: {Date}; \n \n" +
				   $"Начало работы: {Start}; \n" +
				   $"Конец работы:{End}; \n" +
				   $"Общее время:{Total}; \n \n" +
				   $"Начало отдыха:{StartRelax}; \n" +
				   $"Конец отдыха:{EndRelax}; \n" +
				   $"Общее время отдыха:{TotalRelax}; \n \n" +
				   $"Всего заработано:{Earning:0.00}; \n";
		}


		public override bool Equals(object obj)
		{

			WorkDay workday = obj as WorkDay;
			if (workday == null)
			{
				return false;
			}
			else
			{
				return (Date == workday.Date) && (Start == workday.Start);
			}
		}
	}

}
