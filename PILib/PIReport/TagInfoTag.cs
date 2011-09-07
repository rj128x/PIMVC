using System;
using System.Xml.Linq;

namespace PILib.PIReport
{
	public class TagInfoTag:TagInfoPI
	{
		public PISDK.PIPoint Point { get; protected set; }
		public string TagName { get; protected set; }

		public TagInfoTag(XElement element, TagModeEnum mode, Report report)
			: base(element,mode,report) {
				XAttribute tagNameAttr = element.Attribute("tagName");

			this.TagName = tagNameAttr != null ? report.processReplaceString(tagNameAttr.Value) : "NO_TAG_NAME";

			if ((this.AutoID) && (!report.TagsIDS.Values.Contains(this.TagName))) {
				this.ID = TagName;
			}

			try {
				Point = server.PIPoints[TagName];
				if (this.AutoTitle) {
					this.Title = Point.Name;
				}
			} catch (Exception e){
				Logger.error(String.Format("Не найден тэг {0}, Исключение: {1}",TagName,e.Message));
				Point = null;
				//Point=server.PIPoints["sinusoid"];
			}
		}




	}
}