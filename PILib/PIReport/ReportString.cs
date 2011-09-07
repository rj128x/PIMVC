using System;
using System.Collections.Generic;
using System.Linq;

namespace PILib.PIReport
{
	public class ReportString
	{
		public TagInfo tagInfo;
		public SortedList<DateTime, Tag> tags;
		public Report report;

		public ReportString(TagInfo tagInfo, SortedList<DateTime, Tag> tags, Report report) {
			this.tagInfo = tagInfo;
			this.tags = tags;
			this.report = report;
		}

		public string ParentIDClass {
			get {
				TagInfo parent = tagInfo.Parent;
				return parent == null ? "" : String.Format("child-of-{0}", parent.ID);
			}
		}

		public string HasChildsClass {
			get {
				return tagInfo.HasChildrenTags?"hasChilds":"";
			}
		}

		public string ID {
			get {
				return tagInfo.ID.Replace('.', '_').Replace(':','_');
			}
		}

		public string OpenedClass {
			get {
				return tagInfo.Opened ? "opened" : "";
			}
		}
	}
}