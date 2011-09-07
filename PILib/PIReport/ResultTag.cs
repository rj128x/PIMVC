using System;

namespace PILib.PIReport
{
    public class ResultTag:Tag
    {
        public ResultTag(TagInfo tagInfo, double realVal)
            : base(tagInfo, realVal,new DateTime(1000,1,1))
        {

        }

        public override double RealVal
        {
            get
            {
                return base.realVal;
            }
            set
            {
                base.realVal = value;
					 base.computedVal = TagInfo.TagResultMode==TagResultModeEnum.oper?TagInfo.getTagComputedVal(this):realVal;
            }
        }

		  public override Tag getContextTag(string tagName) {
			  Report report = this.TagInfo.OwnerReport;
			  try {
				  Tag tag = report.resultTags[tagName];
				  return tag;
			  } catch {
				  return new Tag(null, 0, this.Date);
			  }
		  }


    }
}