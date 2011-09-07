using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.PIReport;
using System.Runtime.Serialization;

namespace PILib.Chart
{
	public class ChartDataPoint
	{
		public double YVal{get; set;}
		public string XVal { get; set; }

		public ChartDataPoint() {
		}

		public ChartDataPoint(string XVal, double YVal) {
			
			this.XVal = XVal;
			this.YVal = YVal;
			//this.Index = Index;
		}
	}
}
