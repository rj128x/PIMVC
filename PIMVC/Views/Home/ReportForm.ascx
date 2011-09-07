<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PILib.PIReport.ReportRequest>" %>

<script language="javascript">
	$(document).ready(function () {
		form = $(".reportForm");
		$("#mode", form).change(function () {
			mode = $("#mode", form).val();
			switch (mode) {
				case "day":
					$(".dateStartTR").hide();
					$(".dateEndTR").hide();
					$(".YearMonthTR").hide();
					$(".DateTR").show();
					$("#intervalLen").val("1h");
					break;
				case "dayToday":
					$(".dateStartTR").hide();
					$(".dateEndTR").hide();
					$(".YearMonthTR").hide();
					$(".DateTR").hide();
					$("#intervalLen").val("1h");
					break;
				case "dayYesterday":
					$(".dateStartTR").hide();
					$(".dateEndTR").hide();
					$(".YearMonthTR").hide();
					$(".DateTR").hide();
					$("#intervalLen").val("1h");
					break;
				case "month":
					$(".dateStartTR").hide();
					$(".dateEndTR").hide();
					$(".YearMonthTR").show();
					$(".DateTR").hide();
					$(".YearSpan").show();
					$(".MonthSpan").show();
					$("#intervalLen").val("1d");
					break;
				case "year":
					$(".dateStartTR").hide();
					$(".dateEndTR").hide();
					$(".YearMonthTR").show();
					$(".DateTR").hide();
					$(".YearSpan").show();
					$(".MonthSpan").hide();
					$("#intervalLen").val("1months");
					break;
				case "custom":
					$(".dateStartTR").show();
					$(".dateEndTR").show();
					$(".YearMonthTR").hide();
					$(".DateTR").hide();
					$("#intervalLen").val("1d");
					break;
			}
		}).change();
	});
</script>

<% using (Html.BeginForm()){ %>
  <table class='reportForm'>	
	<tr class='modeTR'>
		<th>Период:</th>
		<td>
		<%=Html.DropDownList("mode",new []{
			 new SelectListItem{Text="Сутки сегодня", Value=PILib.PIReport.ReportMode.dayToday.ToString()},
			 new SelectListItem{Text="Сутки вчера", Value=PILib.PIReport.ReportMode.dayYesterday.ToString()},
          new SelectListItem{Text="Сутки", Value=PILib.PIReport.ReportMode.day.ToString()},
			 new SelectListItem{Text="Месяц", Value=ReportMode.month.ToString()},
			 new SelectListItem{Text="Год", Value=ReportMode.year.ToString()},
			 new SelectListItem{Text="Произвольно", Value=ReportMode.custom.ToString()}			 
        }) %>
		</td>
	</tr>
    <tr class='dateStartTR'>
      <th>Старт</th>
      <td>
			<%= Html.TextBox("dateStart",Model.DateStart.ToString("dd.MM.yyyy"),new{@class="txtDate",size=20})%>
			<%=Html.DropDownList("dateStartHour",new []{
          new SelectListItem{Text="00", Value="0"},
			 new SelectListItem{Text="01", Value="1"},
			 new SelectListItem{Text="02", Value="2"},
			 new SelectListItem{Text="03", Value="3"},
			 new SelectListItem{Text="04", Value="4"},
			 new SelectListItem{Text="05", Value="5"},
			 new SelectListItem{Text="06", Value="6"},
			 new SelectListItem{Text="07", Value="7"},
			 new SelectListItem{Text="08", Value="8"},
			 new SelectListItem{Text="09", Value="9"},
			 new SelectListItem{Text="10", Value="10"},
			 new SelectListItem{Text="11", Value="11"},
			 new SelectListItem{Text="12", Value="12"},
			 new SelectListItem{Text="13", Value="13"},
			 new SelectListItem{Text="14", Value="14"},
			 new SelectListItem{Text="15", Value="15"},
			 new SelectListItem{Text="16", Value="16"},
			 new SelectListItem{Text="17", Value="17"},
			 new SelectListItem{Text="18", Value="18"},
			 new SelectListItem{Text="19", Value="19"},
			 new SelectListItem{Text="20", Value="20"},
			 new SelectListItem{Text="21", Value="21"},
			 new SelectListItem{Text="22", Value="22"},
			 new SelectListItem{Text="23", Value="23"},
        }) %>:
		  <%=Html.DropDownList("dateStartMin",new []{
          new SelectListItem{Text="00", Value="0"},
			 new SelectListItem{Text="30", Value="30"},
        }) %>
		</td>
    </tr>
    <tr class='dateEndTR'>
      <th>Стоп</th>
      <td>
			<%= Html.TextBox("dateEnd", Model.DateEnd.ToString("dd.MM.yyyy"), new { @class = "txtDate", size = 20 })%>
			<%=Html.DropDownList("dateEndHour",new []{
          new SelectListItem{Text="00", Value="0"},
			 new SelectListItem{Text="01", Value="1"},
			 new SelectListItem{Text="02", Value="2"},
			 new SelectListItem{Text="03", Value="3"},
			 new SelectListItem{Text="04", Value="4"},
			 new SelectListItem{Text="05", Value="5"},
			 new SelectListItem{Text="06", Value="6"},
			 new SelectListItem{Text="07", Value="7"},
			 new SelectListItem{Text="08", Value="8"},
			 new SelectListItem{Text="09", Value="9"},
			 new SelectListItem{Text="10", Value="10"},
			 new SelectListItem{Text="11", Value="11"},
			 new SelectListItem{Text="12", Value="12"},
			 new SelectListItem{Text="13", Value="13"},
			 new SelectListItem{Text="14", Value="14"},
			 new SelectListItem{Text="15", Value="15"},
			 new SelectListItem{Text="16", Value="16"},
			 new SelectListItem{Text="17", Value="17"},
			 new SelectListItem{Text="18", Value="18"},
			 new SelectListItem{Text="19", Value="19"},
			 new SelectListItem{Text="20", Value="20"},
			 new SelectListItem{Text="21", Value="21"},
			 new SelectListItem{Text="22", Value="22"},
			 new SelectListItem{Text="23", Value="23"},
        }) %>:
		  <%=Html.DropDownList("dateEndMin",new []{
          new SelectListItem{Text="00", Value="0"},
			 new SelectListItem{Text="30", Value="30"},
        }) %>
		</td>
    </tr>
	 <tr class='DateTR'>
		<th>Дата</th>
		<td><%= Html.TextBox("date", Model.Date.ToString("dd.MM.yyyy"), new { @class = "txtDate", size = 20 })%></td>
	 </tr>
	 <tr class='YearMonthTR'>
		<th>Год/Месяц</th>
		<td>
				<span class='YearSpan'><%= Html.TextBox("year", Model.Year, new {size = 5 })%></span>
				<span class='MonthSpan'>
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
					new SelectListItem{Text="Декабрь", Value="12"},
					
				}) %>
				</span>
		</td>
	 </tr>
    <tr>
      <th>Интервал</th>
      <!--<td><%= Html.TextBox("intervalLen", Model.IntervalLen, new {size = 5 })%></td>-->
      <td><%=Html.DropDownList("intervalLen",new []{
			 new SelectListItem{Text="1 минута", Value="1m"},
          new SelectListItem{Text="10 минут", Value="10m"},
          new SelectListItem{Text="30 минут", Value="30m"},
          new SelectListItem{Text="1 час", Value="1h"},
			 new SelectListItem{Text="12 часов", Value="12h"},
          new SelectListItem{Text="1 сутки", Value="1d"},
			 new SelectListItem{Text="10 суток", Value="10d"},
          new SelectListItem{Text="1 неделя", Value="1w"},
          new SelectListItem{Text="1 месяц", Value="1months"},
          new SelectListItem{Text="1 квартал", Value="3months"}          
        }) %>
		  Не более	<%=Html.TextBox("maxPointsCount", Model.MaxPointsCount,  new { size = 5 })%> точек
		  </td>
    </tr>
	 <tr>
		<th>Рендер</th>
		<td><%=Html.CheckBox("renderTable",Model.RenderTable)%>Таблица		 
			 (<%=Html.CheckBox("isVert",Model.IsVert)%>Верт)
			 <%=Html.CheckBox("renderCharts",Model.RenderCharts)%>Графики </td>
	 </tr>
	 <tr>
		<th>Прочее</th>
		<td>
			<%=Html.CheckBox("recordedData",Model.RecordedData)%>Зап.Данные <br />			
		</td>
	 </tr>
    <tr> 
		<%=Html.Hidden("query",Model.Query) %>     
		<th colspan='2' style='text-align:right;'><%=Html.CheckBox("isExcel",Model.IsExcel)%>Excel<input type="submit" value='Получить отчет' id='btnSubmit'/>
		<input type='button' id='btnCustomSubmit' style='display:none' value='Получить отчет'</th>
    </tr>	 
  </table>
  
  
  
  
<%} %>
