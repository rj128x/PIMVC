using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PILib.PIReport
{
	public enum TagModeEnum { tag, calc, noVal, expr };
	public enum TagResultModeEnum { sum, avg, min, max ,oper};
	public class TagInfo
	{
		public TagModeEnum TagMode { get; protected set; }
		public TagResultModeEnum TagResultMode { get; protected set; }

		public double minVal { get; protected set; }
		public double maxVal { get; protected set; }
		public double defVal { get; protected set; }
		public string Title { get; protected set; }
		public string StringFormat { get; protected set; }
		public bool Opened { get; protected set; }		
		public int AddMinutes { get; protected set; }
		public bool ToChart { get; protected set; }

		public string TitleLevel {
			get {
				string str = new String('-', Level);
				str += Title;
				return str;
			}
		}

		private string id;

		public string ID {
			get { return id; }
			protected set {
				id = value;

				if (OwnerReport.TagsIDS.Keys.Contains(IndexTag))
					OwnerReport.TagsIDS[IndexTag] = id;
				else
					OwnerReport.TagsIDS.Add(IndexTag, id);
			}
		}

		protected bool AutoID { get; set; }
		protected bool AutoTitle { get; set; }
		public int IndexTag { get; protected set; }

		public List<TagInfo> ChildrenTags { get; protected set; }
		public bool HasChildrenTags {
			get {
				return ChildrenTags.Count > 0;
			}
		}

		public Report OwnerReport { get; protected set; }
		public TagInfo Parent { get; protected set; }
		public int Level { get; protected set; }
				

		public TagInfo(XElement element, TagModeEnum mode, Report report) {
			OwnerReport = report;
			this.TagMode = mode;
			OwnerReport.countTags++;
			this.IndexTag = OwnerReport.countTags;

			XAttribute titleAttr = element.Attribute("title");
			XAttribute idAttr = element.Attribute("id");
			XAttribute minValAttr = element.Attribute("minVal");
			XAttribute maxValAttr = element.Attribute( "maxVal");
			XAttribute defValAttr = element.Attribute("defVal");
			XAttribute stringFormatAttr = element.Attribute("stringFormat");
			XAttribute openedAttr=element.Attribute("opened");
			XAttribute toChartAttr=element.Attribute("toChart");
			XAttribute AddMinutesAttr=element.Attribute("addMinutes");

			XAttribute resultModeAttr = element.Attribute("resultMode");
			String resultModeStr = resultModeAttr != null ? resultModeAttr.Value : "sum";
			TagResultMode = (TagResultModeEnum)Enum.Parse(typeof(TagResultModeEnum), resultModeStr, true);

			this.Title = titleAttr != null ? titleAttr.Value : "NO TITLE";
			this.AutoTitle = titleAttr == null;
			this.ID = idAttr != null ? idAttr.Value : String.Format("TAG_{0}", IndexTag);
			this.AutoID = idAttr == null;

			this.minVal = minValAttr != null ? Double.Parse(minValAttr.Value) : -2000000000;
			this.maxVal = maxValAttr != null ? Double.Parse(maxValAttr.Value) : 2000000000;
			this.AddMinutes = AddMinutesAttr != null ? Int32.Parse(AddMinutesAttr.Value) : 0;
			this.defVal = defValAttr != null ? Double.Parse(defValAttr.Value) : 0;
			this.StringFormat = stringFormatAttr != null ? stringFormatAttr.Value : "### ### ### ##0";
			this.Opened = openedAttr!=null ? Boolean.Parse(openedAttr.Value) : false;
			this.ToChart = toChartAttr != null ? Boolean.Parse(toChartAttr.Value) : false;

			this.ChildrenTags = new List<TagInfo>();
		}	
		

		protected virtual void initInputVals(XElement element) {
		}

		public static TagInfo readTagInfo(XElement element, Report report, int level = 0) {
			XAttribute tagModeAttr = element.Attribute("mode");

			string tagModeStr = tagModeAttr != null ? tagModeAttr.Value : "noVal";
			TagModeEnum tagMode = (TagModeEnum)Enum.Parse(typeof(TagModeEnum), tagModeStr, true);

			TagInfo tagInfo;
			switch (tagMode) {
				case TagModeEnum.tag:
					tagInfo = new TagInfoTag(element, tagMode, report);
					break;
				case TagModeEnum.expr:
					tagInfo = new TagInfoExpr(element, tagMode, report);
					break;
				case TagModeEnum.calc:
				case TagModeEnum.noVal:
					tagInfo = new TagInfoCalc(element, tagMode, report);
					break;
				default:
					tagInfo = new TagInfo(element, tagMode, report);
					break;
			}
			tagInfo.Level = level;

			XElement children = element.Element(Report.ns + "items");
			if (children != null) {
				foreach (XElement child in children.Elements(Report.ns + "item")) {
					TagInfo newTagInfo = readTagInfo(child, report, level + 1);
					newTagInfo.Parent = tagInfo;
					tagInfo.ChildrenTags.Add(newTagInfo);
				}
			}
			return tagInfo;
		}

		public virtual double getTagComputedVal(Tag tag) {
			return 0;
		}

		public string getFullName() {
			return this.Parent == null ? this.Title : this.Parent.getFullName()+" - "+this.Title;
		}

		public void getAllTagsToArray(List<TagInfo> result) {
			result.Add(this);
			foreach (TagInfo child in ChildrenTags) {
				child.getAllTagsToArray(result);
			}
		}

		public void getAllTags(SortedList<string, TagInfo> result) {
			result.Add(this.id, this);
			foreach (TagInfo child in ChildrenTags) {
				child.getAllTags(result);
			}
		}

		public void getAllTags(SortedList<string, TagInfo> result, TagModeEnum mode) {
			foreach (TagInfo child in ChildrenTags) {
				child.getAllTags(result, mode);
			}
			string code = this.id;

			if (this.TagMode == mode)
				result.Add(code, this);
		}

		public Tag getResultTag(SortedList<DateTime, Tag> tags) {			
			double min = Double.MaxValue;
			double max = Double.MinValue;
			double sum = 0;
			int count = 0;
			foreach (KeyValuePair<DateTime, Tag> de in tags) {
				double val = de.Value.ComputedVal;
				min = min > val ? val : min;
				max = max < val ? val : max;
				sum += val;
				count++;
			}
			double res = 0;
			switch (TagResultMode) {
				case TagResultModeEnum.avg:
					res = sum / count;
					break;
				case TagResultModeEnum.sum:
					res = sum;
					break;
				case TagResultModeEnum.min:
					res = min;
					break;
				case TagResultModeEnum.max:
					res = max;
					break;
			}
			return new ResultTag(this, res);
		}		
	}
}