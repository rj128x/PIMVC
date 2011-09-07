using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PILib;
using PILib.PIReport;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;


namespace PILib.Chart
{
	[XmlRoot(ElementName = "chart")]	
	public class ChartData
	{

		[XmlIgnore]
		protected List<ChartDataSerie> series=null;

		public List<ChartDataSerie> Series {
			get {				
				return series;
			}
			set {
				this.series = value;
			}
		}

		public ChartData(Report report) {
			series = new List<ChartDataSerie>();
			foreach (KeyValuePair<string, SortedList<DateTime, Tag>> de in report.tagsByID) {
				report.addChartData(this, de.Key, de.Key, false,1);
			}
		}

		public ChartData() {
			Series = new List<ChartDataSerie>();
		}

		public void toXML(string fileName) {
			XmlSerializer mySerializer = new XmlSerializer(typeof(ChartData));
			// To write to a file, create a StreamWriter object.
			StreamWriter myWriter = new StreamWriter(fileName);
			mySerializer.Serialize(myWriter, this);
			myWriter.Close();
		}

		public static ChartData fromXML(string fileName) {
			try {
				XmlSerializer mySerializer = new XmlSerializer(typeof(ChartData));
				// To read the file, create a FileStream.
				FileStream myFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				// Call the Deserialize method and cast to the object type.
				ChartData data = (ChartData)mySerializer.Deserialize(myFileStream);
				myFileStream.Close();
				return data;
			} catch (Exception e) {
				Logger.error(String.Format("Чтение файла {0}, Исключение: {1}", fileName, e.Message));
				return null;
			}
		}

		public ChartDataSerie this[string name]{
			get {
				foreach (ChartDataSerie serie in series) {
					if (serie.Name == name) return serie;
				}
				return null;
			}
		}
		
		public ChartDataSerie getSerie(string name) {			
			foreach (ChartDataSerie serie in series) {
				if (serie.Name == name) return serie;
			}
			return null;
		}
	}
}
