using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIMVC.Models
{
	public class PrognozNBModel
	{
		public DateTime DateStart { get; protected set; }
		public DateTime DateEnd { get; protected set; }
		public PrognozNBModel() {
			DateStart = DateTime.Parse(DateTime.Now.AddDays(0).ToShortDateString());
			DateEnd = DateTime.Parse(DateTime.Now.AddDays(1).ToShortDateString());
		}
	}
}
