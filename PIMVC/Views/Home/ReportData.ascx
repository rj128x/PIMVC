<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PIMVC.Models.PIReport.ReportModel>" %>
<div  class='resizable resizeFull' style="width:100%; height:400px; border-color:Blue; border-width:1px; border-style:solid;;overflow:auto;">
<h2>
	<%=String.Format("{0} c {1} по {2}",Model.report.Name,Model.report.DateStart.ToShortDateString(),Model.report.DateEnd.ToShortDateString())%></h2>
	<table class='treeTable tooltip'>
		<thead>
			<tr>
				<th>
					ТЭГ
				</th>
				<th>
					Результат
				</th>
				<%foreach (DateTime date in Model.report.diffDates) { %>
				<th>
					<%=date.ToString() %>
				</th>
				<%} %>
			</tr>
		</thead>
		<tbody>
			<%foreach (TagInfo tagInfo in Model.report.tagInfoArray) { %>
			<%Html.RenderPartial("ReportString", Model.report.getReportString(tagInfo.ID)); %>
			<%} %>
		</tbody>
	</table>
</div>