using System;
using System.Collections.Generic;
using System.Linq;

namespace PILib.PIReport
{
	public enum ReportMode { month, year, day, dayToday, dayYesterday, custom}
	public class ReportRequest
	{
		public DateTime DateStart{get;set;}
		public DateTime DateEnd{get;set;}		
		public int DateStartHour { get; set; }
		public int DateEndHour { get; set; }
		public int DateStartMin { get; set; }
		public int DateEndMin { get; set; }
		public string IntervalLen { get; set; }
		public bool RenderTable { get; set; }
		public bool RenderCharts { get; set; }
		public string Name { get; set; }
		public bool RecordedData { get; set; }
		public bool IsExcel { get; set; }
		public int MaxPointsCount{get;set;}
		public bool IsVert { get; set; }
		public bool IsCustom { get; set; }
		public ReportMode mode { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Kvartal { get; set; }
		public DateTime Date { get; set; }
		public String Query { get; set; }
		protected bool inited;

		public ReportRequest() {
			DateStart = DateTime.Now.AddHours(-24);
			DateEnd = DateTime.Now.AddHours(0);
			IntervalLen = "30m";
			RenderTable = true;
			RenderCharts = true;
			DateStartHour = 0;
			DateEndHour = 0;
			DateStartMin = 0;
			DateEndMin=0;
			Name = "Отчет";
			RecordedData = false;
			IsExcel = false;
			MaxPointsCount = 5000;
			IsVert = false;
			mode = ReportMode.day;
			Year = DateTime.Now.Year;
			Month = DateTime.Now.Month;
			Date = DateTime.Now.AddDays(-1);
			inited = false;
			init();
		}

		public void init() {
			switch (mode) {
				case ReportMode.custom:
					Year = DateStart.Year;
					Month = DateStart.Month;
					Date = DateStart.AddDays(0);
					break;
				case ReportMode.day:
					DateStart = Date.AddDays(0);
					DateEnd = Date.AddDays(1);
					Year = DateStart.Year;
					Month = DateStart.Month;
					break;
				case ReportMode.dayToday:
					Date = DateTime.Now.AddDays(0).Date;
					DateStart = DateTime.Now.AddDays(0).Date;
					DateEnd = Date.AddDays(1);
					Year = DateStart.Year;
					Month = DateStart.Month;
					break;
				case ReportMode.dayYesterday:
					Date = DateTime.Now.AddDays(-1).Date;
					DateStart = DateTime.Now.AddDays(-1).Date;
					DateEnd = Date.AddDays(1);
					Year = DateStart.Year;
					Month = DateStart.Month;
					break;
				case ReportMode.month:
					DateStart=new DateTime(Year, Month, 1);
					DateEnd = DateStart.AddMonths(1);
					break;
				case ReportMode.year:
					DateStart = new DateTime(Year, 1, 1);
					DateEnd = DateStart.AddYears(1);
					break;					
			}
			DateStart = DateStart > DateTime.Now ? DateTime.Now : DateStart;
			DateEnd = DateEnd > DateTime.Now ? DateTime.Now : DateEnd;
			inited = true;
		}

		public DateTime DateTimeStart {
			get {
				return DateStart.AddHours(DateStartHour).AddMinutes(DateStartMin);
			}
		}

		public DateTime DateTimeEnd {
			get {
				return DateEnd.AddHours(DateEndHour).AddMinutes(DateEndMin);
			}
		}
	}
}