<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PIMVC.Models.RynokKvartalModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	СИ по кварталам
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class='tabs'>
		<ul>
			<%if (Model.Reports!=null){ %>
				<li><a href="#tabData">Отчет</a></li>
			<%} %>
			<li><a href="#tabQuery">Запрос</a></li>
			
		</ul>

		<%if (Model.Reports != null) { %>
			<div id='tabData'>
				<table>
					<tr>
						<th>Параметр</th>
						<th>&nbsp;</th>	
						<th><%=Model.getMonthName(1) %></th>					
						<th><%=Model.getMonthName(2) %></th>
						<th><%=Model.getMonthName(3) %></th>
					</tr>
					<tr>
						<th>k<sub>ИС ГТП1</sub></th>
						<th>Min: <%=Model.MINKSIGTP1.ToString("##0.000 000")%></th>
						<td><%=Model.Reports[1].GTP1KSI.ToString("##0.000 000") %></td>
						<td><%=Model.Reports[2].GTP1KSI.ToString("##0.000 000") %></td>
						<td><%=Model.Reports[3].GTP1KSI.ToString("##0.000 000") %></td>
					</tr>
					<tr>
						<th>k<sub>ИС ГТП2</sub></th>
						<th>Min: <%=Model.MINKSIGTP2.ToString("##0.000 000")%></th>
						<td><%=Model.Reports[1].GTP2KSI.ToString("##0.000 000") %></td>
						<td><%=Model.Reports[2].GTP2KSI.ToString("##0.000 000") %></td>
						<td><%=Model.Reports[3].GTP2KSI.ToString("##0.000 000") %></td>
					</tr>
					<tr>
						<th>k<sub>ИС ГТП2</sub>*220+k<sub>ИС ГТП2</sub>*800</th>
						<th><%=Model.SUMSI.ToString("##0.000 000") %></th>
						<td><%=Model.Reports[1].SUMSI.ToString("##0.000 000") %></td>
						<td><%=Model.Reports[2].SUMSI.ToString("##0.000 000") %></td>
						<td><%=Model.Reports[3].SUMSI.ToString("##0.000 000") %></td>
					</tr>
					<tr>
						<th>Min<sub>k<sub>СИ</sub></sub></th>
						<th colspan='4'><%=Model.MINKSI.ToString("##0.000 000")%></th>
					</tr>
					<tr>
						<th>(k<sub>ИС ГТП2</sub>*220+k<sub>ИС ГТП2</sub>*800)/3*1020</th>
						<th colspan='4'><%=(Model.SUMSI/3060).ToString("##0.000 000") %></th>						
					</tr>
					<tr>
						<th>K</th>
						<th colspan='4'><%=(Model.K).ToString() %></th>						
					</tr>
				</table>
			</div>
		<%} %>
		<div id='tabQuery'>
			<% using (Html.BeginForm()){ %>
				<table>
				<tr>
					<th colspan='4'>Отчет СИ за Квартал</th>
				</tr>
					<tr>		
					<th>Месяц</th>
					<td>
						<%=Html.DropDownList("kvartal",new []{
							new SelectListItem{Text="1 Квартал", Value="1"},
							new SelectListItem{Text="2 Квартал", Value="2"},
							new SelectListItem{Text="3 Квартал", Value="3"},
							new SelectListItem{Text="4 Квартал", Value="4"}										 
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
