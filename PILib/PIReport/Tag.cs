using System;

namespace PILib.PIReport
{
	public class Tag
	{
		public bool Broken { get; protected set; }
		private DateTime date;

		public DateTime Date {
			get { return date; }
			set { date = value; }
		}

		private TagInfo tagInfo;
		public TagInfo TagInfo {
			get { return tagInfo; }
			set { tagInfo = value; }
		}

		protected double realVal;
		public virtual double RealVal {
			get { return realVal; }
			set {
				realVal = value;
				if (tagInfo != null) {
					Broken = (realVal < tagInfo.minVal) || (realVal > tagInfo.maxVal);
					realVal = realVal < tagInfo.minVal ? tagInfo.defVal : realVal;
					realVal = realVal > tagInfo.maxVal ? tagInfo.defVal : realVal;
				}

				computedVal = tagInfo == null ? realVal : tagInfo.getTagComputedVal(this);
			}
		}

		protected double computedVal = 0;
		public virtual double ComputedVal {
			get { return computedVal; }
		}

		public Tag(TagInfo tagInfo, double realVal, DateTime date) {
			this.Date = date;
			this.TagInfo = tagInfo;
			this.RealVal = realVal;
		}

		public virtual Tag getContextTag(string tagName) {
			Report report = this.TagInfo.OwnerReport;
			DateTime date = this.date;
			try {
				Tag tag = report.tagsByID[tagName][date];
				return tag;
			} catch {
				return new Tag(null, 0, date);
			}
		}

		public string getComputedValStr() {
			return ComputedVal.ToString(tagInfo.StringFormat);
		}
	}
}