using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.ExtraClass;

namespace TimeTracker.Models
{
	[Serializable]
	public class WorkDay
	{
		#region Свойства
		/// <summary>
		/// ID.
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Элемент выбран.
		/// </summary>
		public bool IsSelected { get; set; }


		/// <summary>
		/// Статус дня.
		/// </summary>
		public Status StatusDay { get; set; } = Status.Stop;


		/// <summary>
		/// Дата.
		/// </summary>
		public DateTime Date { get; set; }
		

		/// <summary>
		/// Начало работы.
		/// </summary>
		private DateTime _start;
		public DateTime Start
		{ 
			get => _start;
			set
			{
				if(End == default(DateTime))
				{
					_start = value;
				}
				else if(value < End)
				{
					_start = value;
				}
				else
				{
					throw new ArgumentException("Start time must be less than a End time", nameof(_start));
				}
			}
		}


		/// <summary>
		/// Конец работы.
		/// </summary>
		private DateTime _end;
		public DateTime End 
		{ 
			get => _end;
			set
			{
				if (Start == default(DateTime))
				{
					_end = value;
				}
				else if (value > Start)
				{
					_end = value;
				}
				else
				{
					throw new ArgumentException("End time must be less than a Start time", nameof(_end));
				}
			}
		}

		/// <summary>
		/// Всего наработано.
		/// </summary>
		public TimeSpan Total
		{
			get => (End - Start) - TotalRelax;

		}

		/// <summary>
		/// Начало отдыха.
		/// </summary>
		private DateTime _startRelax;
		public DateTime StartRelax 
		{
			get => _startRelax;
			set 
			{
				if (EndRelax == default(DateTime))
				{
					_startRelax = value;
				}
				else if (value < EndRelax)
				{
					_startRelax = value;
				}
				else
				{
					throw new ArgumentException("Start time must be less than a End time", nameof(_startRelax));
				}
			}
		}


		/// <summary>
		/// Конец отдыха.
		/// </summary>
		private DateTime _endRelax;
		public DateTime EndRelax
		{
			get => _endRelax;
			set
			{
				if (EndRelax == default(DateTime))
				{
					_endRelax = value;
				}
				else if (value > StartRelax)
				{
					_endRelax = value;
				}
				else
				{
					throw new ArgumentException("End time must be less than a Start time", nameof(_endRelax));
				}
			}
		}

		/// <summary>
		/// Всего отдиха.
		/// </summary>
		public TimeSpan TotalRelax
		{
			get => EndRelax - StartRelax;

		}

		/// <summary>
		/// Стоимость часа.
		/// </summary>
		public decimal MinuteCost { get => HourCost / 60; }

		/// <summary>
		/// Стоимость часа.
		/// </summary>
		public decimal _hourCost;
		public decimal HourCost
		{
			get => _hourCost;
			set
			{
				if (value < 1)
				{
					throw new ArgumentException("The cost of an hour cannot be less than one", nameof(_hourCost));
				}

				_hourCost = value;
			} 
		}

		/// <summary>
		/// Всего заработано.
		/// </summary>
		public decimal Earning { get { return MinuteCost * (int)Total.TotalMinutes; } }

		#endregion

		public WorkDay()
		{
			Id = (int)(DateTime.Now.Ticks);
			Date = DateTime.Today;
			HourCost = 55;
		}

		public WorkDay(DateTime start, decimal hourCost) : this()
		{
			HourCost = hourCost;
			Start = start;	
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
	}

	/// <summary>
	/// Дополнение для DateTime
	/// </summary>
	static class DateTimeHelper
	{

		public static bool EqualDay(this DateTime NewDate, DateTime Date)
		{
			if(NewDate.Day == Date.Day && NewDate.Month == Date.Month && NewDate.Year == Date.Year)
			{
				return true;
			}
			return false;
		}
	}

}
