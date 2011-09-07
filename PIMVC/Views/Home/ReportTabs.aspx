<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
 Inherits="System.Web.Mvc.ViewPage<PIMVC.Models.PIReport.ReportModel>"  ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Model.report.Name %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">	
	<div class="tabs">
		<ul>		
			<%if ((ViewData["request"] as ReportRequest).RenderTable) {%>	
				<li><a href="#tabData">Данные</a></li>
			<%} %>

			<%if ((ViewData["request"] as ReportRequest).RenderCharts) {%>	
				<%foreach(KeyValuePair<string,ReportChartModel> de in Model.Charts){ %>
					<li><a href="#tabChart_<%=de.Value.ChartIndex%>"><%=de.Value.Name%></a></li>
				<%} %>
			 <%} %>
			 <%int index=0; %>
			 <%foreach(KeyValuePair<string,string> de in Model.AddTabs){index++;%>
					<li><a href="#tabAdd_<%=index%>"><%=de.Key%></a></li>
				<%} %>
			<li><a href="#tabQuery">Запрос</a></li>
		</ul>
		<%if ((ViewData["request"] as ReportRequest).RenderTable) {%>	
			<div id="tabData">
				<% if (!Model.Request.IsVert){ 
					Html.RenderPartial("ReportData", Model); 
					}else{
					Html.RenderPartial("ReportDataVertNice", Model); 
					}
				%>
			</div>
		<%} %>

		<%if ((ViewData["request"] as ReportRequest).RenderCharts) {%>	
			<%foreach(KeyValuePair<string,ReportChartModel> de in Model.Charts){ %>
					<div id="tabChart_<%=de.Value.ChartIndex%>">
						<%Html.RenderPartial("ChartView", de.Value);%>
					</div>
			<%} %>
		<%} %>

		<%index=0; %>
		<%foreach(KeyValuePair<string,string> de in Model.AddTabs){index++;%>
			<div id="tabAdd_<%=index%>">
			<%Html.RenderPartial(de.Value, Model); %>
			</div>
		<%} %>
		
		<div id="tabQuery">
			<%string queryTemplate=Model.Request.IsCustom?"CustomReportForm":"ReportForm"; %>
			<%Html.RenderPartial(queryTemplate, ViewData["request"]); %>
		</div>
	</div>	
</asp:Content>
