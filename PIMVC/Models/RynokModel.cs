using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PILib.Reports;

namespace PIMVC.Models
{
	public class RynokModel
	{
		public int Month{get;set;}
		public int Year { get; set; }
		public DateTime Date { get; set; }

		public SortedList<DateTime, ReportRynokSN> Reports { get; protected set; }
		public ReportRynokSN Report { get; protected set; }

		public RynokModel() {
			Month = DateTime.Now.Month;
			Year = DateTime.Now.Year;
		}

		public RynokModel(int Month, int Year) {
			this.Month = Month;
			this.Year = Year;
		}

		public void processDataMonth(bool readDays=true) {
			DateTime dateStart=new DateTime(Year, Month, 1);
			DateTime dateEnd=new DateTime((Month + 1)<=12?Year:Year+1, (Month + 1)<=12?Month+1:1, 1);
			dateEnd = dateEnd > DateTime.Now ? DateTime.Parse(DateTime.Now.ToShortDateString()) : dateEnd;

			Report = new ReportRynokSN("RynokSN.xml");
			Report.readData(dateStart, dateEnd, "1h", 10000, false);

			if (readDays) {
				DateTime date=new DateTime(Year, Month, 1);
				Reports = new SortedList<DateTime, ReportRynokSN>();
				while (date < dateEnd) {
					ReportRynokSN report=new ReportRynokSN("RynokSN.xml");
					report.readData(date, date.AddDays(1), "1h", 100, false);
					Reports.Add(date, report);
					date = date.AddDays(1);
				}
			}
		}
	}
}