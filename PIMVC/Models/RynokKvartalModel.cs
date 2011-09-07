using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PILib.Reports;

namespace PIMVC.Models
{
	public class RynokKvartalModel
	{
		public int Kvartal { get; set; }
		public int Year { get; set; }
		public DateTime Date { get; set; }

		public SortedList<int, ReportRynokSN> Reports { get; protected set; }
		
		public RynokKvartalModel() {
			Kvartal = DateTime.Now.Month / 3 + 1;
			Year = DateTime.Now.Year;
		}

		public void processData() {
			DateTime dateStart=DateTime.Now;
			DateTime dateEnd=DateTime.Now;
			switch (Kvartal) {
				case 1:
					dateStart = new DateTime(Year, 1, 1);
					dateEnd = new DateTime(Year, 4, 1);
					break;
				case 2:
					dateStart = new DateTime(Year, 4, 1);
					dateEnd = new DateTime(Year, 7, 1);
					break;
				case 3:
					dateStart = new DateTime(Year, 7, 1);
					dateEnd = new DateTime(Year, 10, 1);
					break;
				case 4:
					dateStart = new DateTime(Year, 10, 1);
					dateEnd = new DateTime(Year + 1, 1, 1);
					break;
			}
			//dateEnd = dateEnd > DateTime.Now ? DateTime.Parse(DateTime.Now.ToShortDateString()) : dateEnd;

			Reports = new SortedList<int, ReportRynokSN>();
			for (int month=1; month <= 3; month++) {
				int Month=(Kvartal - 1) * 3 + month;
				DateTime ds=new DateTime(Year, Month, 1);
				DateTime de=new DateTime((Month + 1) <= 12 ? Year : Year + 1, (Month + 1) <= 12 ? Month + 1 : 1, 1);
				de = de > DateTime.Now ? DateTime.Parse(DateTime.Now.ToShortDateString()) : de;

				ReportRynokSN report = new ReportRynokSN("RynokSN.xml");
				report.readData(dateStart, dateEnd, "1h", 10000, false);
				Reports.Add(month, report);
			}
		}

		public string getMonthName(int month) {
			month = (Kvartal - 1) * 3 + month;
			DateTime date=new DateTime(Year, month, 1);
			return date.ToString("MMMM");
		}

		public double SUMSI {
			get {
				if (Reports != null)
					return Reports[1].SUMSI + Reports[2].SUMSI + Reports[3].SUMSI;
				else
					return 0;
			}
		}

		public double MINKSIGTP1 {
			get {
				if (Reports != null)
					return (new double[] { Reports[1].GTP1KSI, Reports[2].GTP1KSI, Reports[3].GTP1KSI }).Min();
				else
					return 0;
			}
		}

		public double MINKSIGTP2 {
			get {
				if (Reports != null)
					return (new double[] { Reports[1].GTP2KSI, Reports[2].GTP2KSI, Reports[3].GTP2KSI }).Min();
				else
					return 0;
			}
		}

		public double MINKSI {
			get {
				if (Reports != null)
					return (new double[] { Reports[1].GTP2KSI, Reports[2].GTP2KSI, Reports[3].GTP2KSI,Reports[1].GTP1KSI, Reports[2].GTP1KSI, Reports[3].GTP1KSI }).Min();
				else
					return 0;
			}
		}

		public double K {
			get {
				if ((MINKSI < 0.01) && (SUMSI / 3060 < 0.02))
					return 1;
				else
					return 0;
			}
		}

	}
}