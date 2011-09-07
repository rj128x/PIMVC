using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.Chart;

namespace PILib.PrognozNB
{
	public enum PrognozDataType { PBRGekon, PBRUser, PFakt}
	public enum PrognozRashodCalcType { avg, min,max,fakt }
	public class PrognozNBAnswer
	{
		public ChartAnswer Chart { get; set; }
		public DateTime Date { get; set; }
		public SortedList<DateTime, double> PBR30 { get; set; }
	}
}
