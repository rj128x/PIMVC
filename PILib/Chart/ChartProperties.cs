using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace PILib.Chart
{
	public enum XAxisTypeEnum{auto,datetime}	
	public class ChartProperties
	{
		public List<ChartSerieProperties> Series { get; set; }
		public XAxisTypeEnum XAxisType{get;set;}
		public List<ChartAxisProperties> Axes { get; set; }

		public ChartProperties() {
			

		}
		public static ChartProperties fromXML(string fileName) {
			try {
				XmlSerializer mySerializer = new XmlSerializer(typeof(ChartProperties));
				// To read the file, create a FileStream.
				FileStream myFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read); ;
				// Call the Deserialize method and cast to the object type.
				ChartProperties data = (ChartProperties)mySerializer.Deserialize(myFileStream);
				myFileStream.Close();

				if (data.Axes.Count == 0) {
					data.Axes = new List<ChartAxisProperties>();

					ChartAxisProperties main=new ChartAxisProperties();
					main.Auto = true;
					main.Index = 0;

					ChartAxisProperties sec=new ChartAxisProperties();
					sec.Auto = true;
					sec.Index = 1;

					data.Axes.Add(main);
					data.Axes.Add(sec);
				}
				return data;
			} catch (Exception e) {
				Logger.error(String.Format("Чтение файла {0}, Исключение: {1}", fileName, e.Message));
				return null;
			}
		}

		public void toXML(string fileName) {
			XmlSerializer mySerializer = new XmlSerializer(typeof(ChartProperties));
			// To write to a file, create a StreamWriter object.
			StreamWriter myWriter = new StreamWriter(fileName);
			mySerializer.Serialize(myWriter, this);
			myWriter.Close();
		}

		public static void createNullXML(string fileName) {
			ChartProperties prop=new ChartProperties();
			prop.Series = new List<ChartSerieProperties>();
			prop.Series.Add(new ChartSerieProperties());
			prop.toXML(fileName);
		}
	}
}
