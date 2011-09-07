using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Xml.Serialization;
using PILib.Chart;

namespace PILib.PIReport
{
	[XmlRoot(ElementName="report")]
	public class Report
	{
		protected class ReplaceString{
			public string key;
			public string value;
			public ReplaceString(string key,string value){
				this.key=key;
				this.value=value;
			}
			public string getReplaced(string input){
				return input.Replace(key,value);
			}
		}
		public static string defPath = "";
		public static string defPathTempCharts="";
		public static string ns = "{reportSchema.xsd}";
		public string Name { get; protected set; }		
		protected List<ReplaceString> replaceStrings;
		
		[XmlElement(ElementName="dateStart")]
		public DateTime DateStart { get; set; }
		
		[XmlElement(ElementName = "dateEnd")]
		public DateTime DateEnd { get; set; }
		
		public string IntervalLen { get; set; }
		public List<TagInfo> tagInfoArray;
		public List<TagInfo> tagInfoFullArray;
		public SortedList<string, TagInfo> tagInfoCache;
		public SortedList<string, TagInfo> tagInfoTagCache;
		public SortedList<string, TagInfo> tagInfoCalcCache;
		public List<TagInfo> tagInfoCalcOrderCache;
		public SortedList<string, SortedList<DateTime, Tag>> tagsByID;
		public SortedList<string, Tag> resultTags;

		public List<DateTime> diffDates;
		public SortedList<int, string> TagsIDS = new SortedList<int, string>();
		public int countTags = -1;

		public string debugId=Guid.NewGuid().ToString();

		public Report(string fileName,string xmlString=null,bool useDefPath=true) {
			XDocument doc;
			if (xmlString != null) {
				doc=XDocument.Parse(xmlString);
			} else {
				if (useDefPath) {
					doc = XDocument.Load(String.Format("{0}/Data/{1}", defPath, fileName));
				} else {
					doc = XDocument.Load(String.Format(fileName));
				}
			}

			this.tagInfoArray = new List<TagInfo>();
			XElement items = doc.Root.Element(ns+"items");			
			XElement replaceStrings=doc.Root.Element(ns+"expReplace");

			this.replaceStrings = new List<ReplaceString>();
			if (replaceStrings != null) {
				foreach (XElement replace in replaceStrings.Elements(ns + "replace")) {
					this.replaceStrings.Add(new ReplaceString(replace.Attribute("key").Value,replace.Attribute("value").Value));
				}
			}

			Name = doc.Root.Element(ns + "nameReport") == null ? "----" : doc.Root.Element(ns + "nameReport").Value.ToString();

			foreach (XElement item in items.Elements(ns + "item")) {
				TagInfo tagInfo = TagInfo.readTagInfo(item, this);
				tagInfoArray.Add(tagInfo);
			}
			initCache();			
		}

		protected void initCache() {
			this.tagInfoFullArray = new List<TagInfo>();
			this.tagInfoCache = new SortedList<string, TagInfo>();
			this.tagInfoTagCache = new SortedList<string, TagInfo>();
			this.tagInfoCalcCache = new SortedList<string, TagInfo>();
			this.tagInfoCalcOrderCache = new List<TagInfo>();
			foreach (TagInfo tagInfo in tagInfoArray) {
				tagInfo.getAllTags(tagInfoCache);
				tagInfo.getAllTagsToArray(tagInfoFullArray);
				tagInfo.getAllTags(tagInfoTagCache, TagModeEnum.tag);
				tagInfo.getAllTags(tagInfoTagCache, TagModeEnum.expr);
				tagInfo.getAllTags(tagInfoCalcCache, TagModeEnum.calc);
				tagInfo.getAllTags(tagInfoCalcCache, TagModeEnum.noVal);
			}
			init();
			processCalcTags();
		}

		public virtual void init() {

		}

		protected void processCalcTags() {
			foreach (KeyValuePair<string, TagInfo> de in tagInfoCalcCache) {
				if (!tagInfoCalcOrderCache.Contains(de.Value)){
					tagInfoCalcOrderCache.Add(de.Value);					
				}
			}
		}

		protected void addCalcFunction(string tagName, TagInfoCalc.GetTagValDelegate function) {
			TagInfo tagInfo;
			this.tagInfoCalcCache.TryGetValue(tagName, out tagInfo);
			(tagInfo as TagInfoCalc).calcFunction = function;
			tagInfoCalcOrderCache.Add(tagInfo);
		}

		public void readData(DateTime dateStart, DateTime dateEnd, string IntervalLen, int maxPointsCount, bool recorded = false) {
			Debug.start("Report.readData",debugId);
			this.DateStart = dateStart;
			this.DateEnd = dateEnd;
			this.IntervalLen = IntervalLen;
			tagsByID = new SortedList<string, SortedList<DateTime, Tag>>();
			resultTags = new SortedList<string, Tag>();
			diffDates = new List<DateTime>();

			Debug.start("Report.readData->getValuesFromPI", debugId);
			if (!recorded)
				PIReaderData.getValues2(dateStart, dateEnd, IntervalLen,maxPointsCount, tagInfoTagCache, this);
			else
				PIReaderData.getRecordedValues(dateStart, dateEnd,maxPointsCount,tagInfoTagCache,this);
			Debug.end("Report.readData->getValuesFromPI", debugId);

			preCalc();

			Debug.start("Report.readData->resultTags", debugId);
			foreach (KeyValuePair<string, TagInfo> de in tagInfoTagCache) {
				Tag tag = de.Value.getResultTag(tagsByID[de.Key]);
				resultTags.Add(de.Key, tag);
			}
			Debug.end("Report.readData->resultTags", debugId);

			Debug.start("Report.readData->calcTags", debugId);
			foreach (TagInfo tagInfo in tagInfoCalcOrderCache) {
				foreach (DateTime date in diffDates) {
					Tag tag = new Tag(tagInfo, 0, date);
					addTag(tag);
				}
			}

			foreach (TagInfo tagInfo in tagInfoCalcOrderCache) {
				Tag tag = tagInfo.getResultTag(tagsByID[tagInfo.ID]);
				resultTags.Add(tagInfo.ID, tag);
			}

			diffDates.Sort();
			Debug.end("Report.readData->calcTags", debugId);

			Debug.end("Report.readData", debugId);
		}

		protected virtual void preCalc(){
		}

		public void addTag(Tag tag) {
			Debug.start("Report->addTag", debugId);
			string id = tag.TagInfo.ID;
			DateTime date = tag.Date;

			SortedList<DateTime, Tag> tagsList;
			bool exist = tagsByID.TryGetValue(id, out tagsList);
			if (!exist) {
				tagsList = new SortedList<DateTime, Tag>();
				tagsByID.Add(id, tagsList);
			}
			if (!tagsList.Keys.Contains(date))
				tagsList.Add(date, tag);

			if (!diffDates.Contains(date)) {
				diffDates.Add(date);
			}
			Debug.end("Report->addTag", debugId);
		}

		public ReportString getReportString(string tagInfoName) {
			TagInfo tagInfo = this.tagInfoCache[tagInfoName];
			SortedList<DateTime, Tag> tags = this.tagsByID[tagInfoName];
			ReportString reportString = new ReportString(tagInfo, tags, this);
			return reportString;
		}

		public Tag getLastTag(string tagInfoName,DateTime date) {
			TagInfo tagInfo = this.tagInfoCache[tagInfoName];
			SortedList<DateTime, Tag> tags = this.tagsByID[tagInfoName];
			//Logger.debug(date.ToString()+":"+tags.Keys.Last().ToString());
			return tags.Last(de=>de.Key<=date).Value;			
		}

		public string writeChartData() {
			string fileName=String.Format("{0}/{1}.xml",defPathTempCharts,Guid.NewGuid().ToString());
			Chart.ChartData data=new Chart.ChartData(this);
			data.toXML(fileName);
			return fileName;
		}

		public string writeChartData(ChartData data) {
			string fileName=String.Format("{0}/{1}.xml", defPathTempCharts, Guid.NewGuid().ToString());
			data.toXML(fileName);
			return fileName;
		}

		public void addChartData(ChartData data, string tagName, string nameSerie,bool integrated=false,double scaleY=1) {
			TagInfo tagInfo=tagInfoCache[tagName];
			if (!tagInfo.ToChart)
				return;
			ChartDataSerie serie=new ChartDataSerie();
			serie.Name = nameSerie;
			SortedList<DateTime, Tag> tags=tagsByID[tagName];
			double integratedVal=0;
			foreach (KeyValuePair<DateTime,Tag> de in tags){				
				double yVal=de.Value.ComputedVal*scaleY;
				//double yVal=(150000+Debug.random.Next(20000)) * scaleY;
				integratedVal += yVal;
				double y=integrated ? integratedVal : yVal;
				ChartDataPoint point=new ChartDataPoint(de.Key.ToString(), y);
				serie.Points.Add(point);
			}
				
			data.Series.Add(serie);
		}

		public void addChartDataResult(ChartData data,string nameSerie, string[] names, string[] titles=null) {
			Chart.ChartDataSerie serie=new Chart.ChartDataSerie();
			serie.Name = nameSerie;
			for (int i=0;i<names.Length;i++) {
				string name=names[i];
				Tag tag=resultTags[name];
				string title=titles == null ? tag.TagInfo.Title : titles[i];
				serie.Points.Add(new Chart.ChartDataPoint(title, tag.ComputedVal));
			}
			data.Series.Add(serie);
		}

		public string processReplaceString(string input) {
			bool log=true;
			while (log) {
				log = false;
				foreach (ReplaceString replace in replaceStrings) {
					log = log || input.Contains(replace.key);
					input = replace.getReplaced(input);
				}
			}
			return input;
		}

	}
}