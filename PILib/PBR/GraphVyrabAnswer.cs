using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib.Chart;

namespace PILib.PBR
{
	public class GraphVyrabAnswer
	{
		public List<GraphVyrabData> CurrentVals;
		public List<GraphVyrabData> HourVals;
		public DateTime date { get; set; }

		public ChartAnswer chartAnswer { get; set; }

		public GraphVyrabAnswer() {
			CurrentVals = new List<GraphVyrabData>();
			HourVals = new List<GraphVyrabData>();
		}
	}
}
