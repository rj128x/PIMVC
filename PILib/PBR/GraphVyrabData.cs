using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PILib.PBR
{
	public class GraphVyrabData
	{
		public string Name { get; set; }
		public double Fakt { get; set; }
		public double Plan { get; set; }
		public double Diff { get; set; }
		public double DiffProc { get; set; }
		public double Recomended { get; set; }

		public GraphVyrabData(string name, double plan, double fakt,DateTime dt) {
			Fakt = fakt;
			Plan = plan;
			Name = name;
			Diff = Fakt - Plan;
			if (Plan == 0)
				DiffProc = Fakt == 0 ? 0 : 100;
			else
				DiffProc = Diff / Plan * 100;

			int min = dt.Minute;
			int min2=60-min;
			Recomended = min2 > 0 ?Plan- Diff / min2 : Plan;
		}

		public GraphVyrabData() {
			
		}
	}
}
