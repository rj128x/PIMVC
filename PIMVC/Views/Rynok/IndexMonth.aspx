<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PIMVC.Models.RynokModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	СИ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">	
	<script language="javascript" type="text/javascript" src="<%=Url.Content("~/Scripts/MyScripts/rynok.js")%>"></script>
	<script language="javascript" type="text/javascript">
		$(document).ready(function () {
			rynok = new Rynok({
				url: '<%=Url.Content("~/ClientBin/SilverChart.xap")%>',
				urlProp: '<%=Server.MapPath("~/Data/Charts/").Replace("\\","\\\\")%>RynokBrief.xml'
			});
			rynok.init();
		});
	</script>
	<style >
	.treeTable td
	{
		white-space:nowrap;
		text-align:right;
	}
	td.proc
	{
		color:Green;
		font-weight:bold;
	}
	td.fakt
	{
		color: Blue;
	}
	td.plan
	{
		color:Green;
	}
	a.showChart
	{
		background-color:#DFEFFC;
		color:#2E6E9E;
		border:1px solid #C5DBEC;
		font-weight:bold;
		cursor:pointer;
		width:150px;
		height:20px;
	}
</style>
<div class="tabs">
	<ul>
		<%if (Model.Reports != null) {%>	
			<li> <a href="#tabData">Данные</a></li>
			<li> <a href="#tabK">Коэффициенты</a></li>
		<%} %>
		<li> <a href="#tabQuery">Запрос</a></li>			
	</ul>		


<%int indexReport=0;%>
<%if (Model.Reports != null) {%>	
	<div id='tabData'>
	<div  class='resizeFull' style="width:100%; height:400px; border-color:Blue; border-width:1px; border-style:solid; overflow:auto">
	<table class="treeTable tooltip">
		<tr>
			<th rowspan='2'>Дата</th>
			<th rowspan='2'>График</th>
			<th colspan='12'>ГЭС</th>
			<th rowspan='2'>Дата</th>
			<th colspan='12'>ГТП1</th>
			<th rowspan='2'>Дата</th>
			<th colspan='12'>ГТП2</th>
		</tr>
		<tr>			
			<%for (int i=0; i < 3; i++) { %>
			<th>План</th>
			<th>Факт</th>

			<th>Откл</th>
			<th>Откл -</th>
			<th>Откл +</th>
			<th>Откл абс</th>
			<th>Откл абс%</th>

			<th>Откл СИ</th>
			<th>Откл СИ -</th>
			<th>Откл СИ +</th>
			<th>Откл СИ абс</th>
			<th>Откл СИ абс %</th>
			<%} %>
		</tr>

		 <tr id="Tr1" class='hasChilds'>
			<td class='header'>Итог</td>
			<td>&nbsp;</td>
			<td class='plan'><%=Model.Report.resultTags["gesPlan"].getComputedValStr()%></td>
			<td class='fakt'><%=Model.Report.resultTags["gesFakt"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gesDiff"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gesDiffMinus"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gesDiffPlus"].getComputedValStr()%></td>
			<td>|<%=Model.Report.resultTags["gesDiffAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=Model.Report.resultTags["gesDiffAbsProc"].getComputedValStr()%>%</td>
			<td><%=Model.Report.resultTags["gesDiffSI"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gesDiffSIMinus"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gesDiffSIPlus"].getComputedValStr()%></td>
			<td>|<%=Model.Report.resultTags["gesDiffSIAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=Model.Report.resultTags["gesDiffSIAbsProc"].getComputedValStr()%>%</td>

			<td class='header'>Итог</td>
			<td class='plan'><%=Model.Report.resultTags["gtp1Plan"].getComputedValStr()%></td>
			<td class='fakt'><%=Model.Report.resultTags["gtp1Fakt"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp1Diff"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp1DiffMinus"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp1DiffPlus"].getComputedValStr()%></td>
			<td>|<%=Model.Report.resultTags["gtp1DiffAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=Model.Report.resultTags["gtp1DiffAbsProc"].getComputedValStr()%>%</td>
			<td><%=Model.Report.resultTags["gtp1DiffSI"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp1DiffSIMinus"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp1DiffSIPlus"].getComputedValStr()%></td>
			<td>|<%=Model.Report.resultTags["gtp1DiffSIAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=Model.Report.resultTags["gtp1DiffSIAbsProc"].getComputedValStr()%>%</td>

			<td class='header'>Итог</td>
			<td class='plan'><%=Model.Report.resultTags["gtp2Plan"].getComputedValStr()%></td>
			<td class='fakt'><%=Model.Report.resultTags["gtp2Fakt"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp2Diff"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp2DiffMinus"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp2DiffPlus"].getComputedValStr()%></td>
			<td>|<%=Model.Report.resultTags["gtp2DiffAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=Model.Report.resultTags["gtp2DiffAbsProc"].getComputedValStr()%>%</td>
			<td><%=Model.Report.resultTags["gtp2DiffSI"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp2DiffSIMinus"].getComputedValStr()%></td>
			<td><%=Model.Report.resultTags["gtp2DiffSIPlus"].getComputedValStr()%></td>
			<td>|<%=Model.Report.resultTags["gtp2DiffSIAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=Model.Report.resultTags["gtp2DiffSIAbsProc"].getComputedValStr()%>%</td>
	  </tr>

	<%foreach (KeyValuePair<DateTime,PILib.Reports.ReportRynokSN> de in Model.Reports) { %>
		<%indexReport++; %>
	  <tr id="report_<%=indexReport%>" class='hasChilds'>
			<td class='treeHeader header'><%=de.Key.ToShortDateString() %></td>
			<td> <a class='showChart' dateStart='<%=de.Key.ToString()%>' dateEnd='<%=de.Key.AddDays(1).ToString()%>'>График</a></td>
			<td class='plan'><%=de.Value.resultTags["gesPlan"].getComputedValStr()%></td>
			<td class='fakt'><%=de.Value.resultTags["gesFakt"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gesDiff"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gesDiffMinus"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gesDiffPlus"].getComputedValStr()%></td>
			<td>|<%=de.Value.resultTags["gesDiffAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=de.Value.resultTags["gesDiffAbsProc"].getComputedValStr()%>%</td>
			<td><%=de.Value.resultTags["gesDiffSI"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gesDiffSIMinus"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gesDiffSIPlus"].getComputedValStr()%></td>
			<td>|<%=de.Value.resultTags["gesDiffSIAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=de.Value.resultTags["gesDiffSIAbsProc"].getComputedValStr()%>%</td>

			<td class='header'><%=de.Key.ToShortDateString() %></td>
			<td class='plan'><%=de.Value.resultTags["gtp1Plan"].getComputedValStr()%></td>
			<td class='fakt'><%=de.Value.resultTags["gtp1Fakt"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp1Diff"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp1DiffMinus"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp1DiffPlus"].getComputedValStr()%></td>
			<td>|<%=de.Value.resultTags["gtp1DiffAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=de.Value.resultTags["gtp1DiffAbsProc"].getComputedValStr()%>%</td>
			<td><%=de.Value.resultTags["gtp1DiffSI"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp1DiffSIMinus"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp1DiffSIPlus"].getComputedValStr()%></td>
			<td>|<%=de.Value.resultTags["gtp1DiffSIAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=de.Value.resultTags["gtp1DiffSIAbsProc"].getComputedValStr()%>%</td>

			<td class='header'><%=de.Key.ToShortDateString() %></td>
			<td class='plan'><%=de.Value.resultTags["gtp2Plan"].getComputedValStr()%></td>
			<td class='fakt'><%=de.Value.resultTags["gtp2Fakt"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp2Diff"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp2DiffMinus"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp2DiffPlus"].getComputedValStr()%></td>
			<td>|<%=de.Value.resultTags["gtp2DiffAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=de.Value.resultTags["gtp2DiffAbsProc"].getComputedValStr()%>%</td>
			<td><%=de.Value.resultTags["gtp2DiffSI"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp2DiffSIMinus"].getComputedValStr()%></td>
			<td><%=de.Value.resultTags["gtp2DiffSIPlus"].getComputedValStr()%></td>
			<td>|<%=de.Value.resultTags["gtp2DiffSIAbs"].getComputedValStr()%>|</td>
			<td class='proc'><%=de.Value.resultTags["gtp2DiffSIAbsProc"].getComputedValStr()%>%</td>
	  </tr>


	  <%foreach(DateTime dt in de.Value.diffDates){ %>
			<tr id="reportTR_<%=indexReport%>" class="child-of-report_<%=indexReport%>">
				<td  class='treeHeader header'><%=dt.ToShortTimeString() %></td>
				<td>&nbsp;</td>
				<td class='plan'><%=de.Value.tagsByID["gesPlan"][dt].getComputedValStr()%></td>
				<td class='fakt'><%=de.Value.tagsByID["gesFakt"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gesDiff"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gesDiffMinus"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gesDiffPlus"][dt].getComputedValStr()%></td>
				<td>|<%=de.Value.tagsByID["gesDiffAbs"][dt].getComputedValStr()%>|</td>
				<td class='proc'><%=de.Value.tagsByID["gesDiffAbsProc"][dt].getComputedValStr()%>%</td>
				<td><%=de.Value.tagsByID["gesDiffSI"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gesDiffSIMinus"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gesDiffSIPlus"][dt].getComputedValStr()%></td>
				<td>|<%=de.Value.tagsByID["gesDiffSIAbs"][dt].getComputedValStr()%>|</td>
				<td class='proc'><%=de.Value.tagsByID["gesDiffSIAbsProc"][dt].getComputedValStr()%>%</td>

				<td  class='header'><%=dt.ToShortTimeString() %></td>
				<td class='plan'><%=de.Value.tagsByID["gtp1Plan"][dt].getComputedValStr()%></td>
				<td class='fakt'><%=de.Value.tagsByID["gtp1Fakt"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp1Diff"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp1DiffMinus"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp1DiffPlus"][dt].getComputedValStr()%></td>
				<td>|<%=de.Value.tagsByID["gtp1DiffAbs"][dt].getComputedValStr()%>|</td>
				<td class='proc'><%=de.Value.tagsByID["gtp1DiffAbsProc"][dt].getComputedValStr()%>%</td>
				<td><%=de.Value.tagsByID["gtp1DiffSI"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp1DiffSIMinus"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp1DiffSIPlus"][dt].getComputedValStr()%></td>
				<td>|<%=de.Value.tagsByID["gtp1DiffSIAbs"][dt].getComputedValStr()%>|</td>
				<td class='proc'><%=de.Value.tagsByID["gtp1DiffSIAbsProc"][dt].getComputedValStr()%>%</td>

				<td  class='header'><%=dt.ToShortTimeString() %></td>
				<td class='plan'><%=de.Value.tagsByID["gtp2Plan"][dt].getComputedValStr()%></td>
				<td class='fakt'><%=de.Value.tagsByID["gtp2Fakt"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp2Diff"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp2DiffMinus"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp2DiffPlus"][dt].getComputedValStr()%></td>
				<td>|<%=de.Value.tagsByID["gtp2DiffAbs"][dt].getComputedValStr()%>|</td>
				<td class='proc'><%=de.Value.tagsByID["gtp2DiffAbsProc"][dt].getComputedValStr()%>%</td>
				<td><%=de.Value.tagsByID["gtp2DiffSI"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp2DiffSIMinus"][dt].getComputedValStr()%></td>
				<td><%=de.Value.tagsByID["gtp2DiffSIPlus"][dt].getComputedValStr()%></td>
				<td>|<%=de.Value.tagsByID["gtp2DiffSIAbs"][dt].getComputedValStr()%>|</td>
				<td class='proc'><%=de.Value.tagsByID["gtp2DiffSIAbsProc"][dt].getComputedValStr()%>%</td>
			</tr>
	  <%} %>
	<%}%>
	</table>
</div>
</div>
	<div id='dlgChart' style='display:none;width:800px;height:600px;'></div>

	<div id='tabK'>
	<table>
		<tr>
			<th>Параметр</th>
			<th>Значение</th>
		</tr>
		<tr>
			<th>k<sub>ИС-ГТП1</sub></th>
			<td><%=(Model.Report.resultTags["gtp1DiffSIAbsProc"].ComputedVal/100).ToString("##0.000000")%></td>
		</tr>
		<tr>
			<th>k<sub>ИС-ГТП2</sub></th>
			<td><%=(Model.Report.resultTags["gtp2DiffSIAbsProc"].ComputedVal/100).ToString("##0.000000")%></td>
		</tr>
		<tr>
			<th>k<sub>ИС-ГТП1</sub>*220+k<sub>ИС-ГТП2</sub>*800</th>
			<td><%=(Model.Report.resultTags["gtp1DiffSIAbsProc"].ComputedVal / 100 * 220 + Model.Report.resultTags["gtp2DiffSIAbsProc"].ComputedVal / 100 * 800).ToString("##0.000000")%></td>
		</tr>
		<tr>
			<th>(k<sub>ИС-ГТП1</sub>*220+k<sub>ИС-ГТП2</sub>*800)/1020</th>
			<td><%=((Model.Report.resultTags["gtp1DiffSIAbsProc"].ComputedVal / 100 * 220 + Model.Report.resultTags["gtp2DiffSIAbsProc"].ComputedVal / 100 * 800)/1020).ToString("##0.000000")%></td>
		</tr>
	</table>
	
</div>
<% } %>




<div id="tabQuery">
<% using (Html.BeginForm()){ %>
	<table>
	<tr>
		<th colspan='4'>Отчет СИ за месяц (<%=Html.ActionLink("Квартал","IndexKvartal","Rynok") %>)</th>
	</tr>
		<tr>		
		<th>Месяц</th>
		<td>
			<%=Html.DropDownList("month",new []{
				new SelectListItem{Text="Январь", Value="1"},
				new SelectListItem{Text="Февраль", Value="2"},
				new SelectListItem{Text="Март", Value="3"},
				new SelectListItem{Text="Апрель", Value="4"},
				new SelectListItem{Text="Май", Value="5"},
				new SelectListItem{Text="Июнь", Value="6"},
				new SelectListItem{Text="Июль", Value="7"},
				new SelectListItem{Text="Август", Value="8"},
				new SelectListItem{Text="Сентябрь", Value="9"},
				new SelectListItem{Text="Октябрь", Value="10"},
				new SelectListItem{Text="Ноябрь", Value="11"},
				new SelectListItem{Text="Декабрь", Value="12"}			 
			}) %>:		  
		</td>
		<th>Год</th>
		<td><%=Html.TextBox("year", Model.Year,  new { size = 5 })%>  </td>
		</tr>	 
		<tr>      
		<th colspan='2' style='text-align:right;'><input type="submit" value='Получить отчет' /></th>
		</tr>
	</table>  
<%} %>
</div>

</div>
</asp:Content>
