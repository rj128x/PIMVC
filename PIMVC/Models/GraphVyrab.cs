using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMVC.Models
{
	public class GraphVyrab
	{
		public DateTime DateStart { get; protected set; }
		public DateTime DateEnd { get; protected set; }
		public GraphVyrab() {
			DateStart = DateTime.Parse(DateTime.Now.AddDays(0).ToShortDateString());
			DateEnd = DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString());
		}
	}
}