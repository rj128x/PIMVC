<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PIMVC.Models.PIReport.ReportModel>" %>
<div  class='resizable resizeFull %>' style="width:100%; height:400px; border-color:Blue; border-width:1px; border-style:solid;overflow:auto;">
<h2>
	<%=String.Format("{0} c {1} по {2}",Model.report.Name,Model.report.DateStart.ToShortDateString(),Model.report.DateEnd.ToShortDateString())%></h2>
	<table class='treeTable tooltip'>
		<thead>
			<%
				int maxLevel=Model.getMaxLevel();		
			%>
			<%for (int level=0;level<=maxLevel;level++){%>
			<tr>
				<%if (level==0){ %>
					<th rowspan='<%=maxLevel+1%>'>
						ТЭГ
					</th>
				<%}%>								
					<%foreach (TagInfo tagInfo in Model.report.tagInfoFullArray) {%>
						<%if (level==tagInfo.Level+1){ %>
							<th rowspan='<%=maxLevel-level+1%>'><%=tagInfo.Title%></th>
						<%} %>
						<%if (level==tagInfo.Level){ %>
						<th colspan='<%=Model.getColSpan(tagInfo)%>'><%=tagInfo.Title%></th>
						<%} %>
					<%} %>					
			</tr>
			<%}%>
			<tr>
				<th>
					Результат
				</th>				
				<%foreach (TagInfo tagInfo in Model.report.tagInfoFullArray) {%>
					<th style='text-align:right;'><%=tagInfo.TagMode == TagModeEnum.noVal ? "-" : Model.report.resultTags[tagInfo.ID].ComputedVal.ToString(tagInfo.StringFormat)%></th>
				<%} %>				
			</tr>
		</thead>
		<tbody>
			<%foreach (DateTime date in Model.report.diffDates) { %>
				<tr>
					<th>
						<%=date.ToString() %>
					</th>
					<%foreach (TagInfo tagInfo in Model.report.tagInfoFullArray) {%>
						<%
							Tag tag=null;
							try {
								tag = Model.report.tagsByID[tagInfo.ID][date];
							} catch { }
							string val=(tag == null)||(tagInfo.TagMode==TagModeEnum.noVal) ? "-" : tag.ComputedVal.ToString(tag.TagInfo.StringFormat);
							bool broken=tag == null || tag.Broken;
						 %>
						<td class='nowrap <%=broken?"broken":"" %>' style='text-align:right;'><%=val%></td>
					<%} %>
				</tr>				
				<%} %>			
		</tbody>
	</table>	
</div>
