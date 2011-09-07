<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PILib.PIReport.ReportRequest>" %>
	<script type="text/javascript">
		$(document).ready(function () {
			$("#btnSubmit").hide();
			$("#btnCustomSubmit").show().click(function () {
				control = document.getElementById("customReport");
				control.Content.CustomReportSettings.saveAndSubmit();
			});
		});
		function submitCustomReportForm() {
			$("#btnSubmit").click();
		}
	</script>
	<table>
		<tr>
			<th>Параметры отчета</th>
			<th>Настройка отчета</th>
		</tr>
		<td valign='top'><%Html.RenderPartial("ReportForm",Model); %></td>
		<td>
			<div class='resizable1' style="width:700px; height:400px; border-color:Blue; border-width:1px; border-style:solid;">
				<object data="data:application/x-silverlight," type="application/x-silverlight-2" id='customReport'
					width="100%" height="100%">
					<param name="source" value="<%=Url.Content("~/ClientBin/SilverChart.xap")%>" />
					<param name="background" value="White" />
					<param name="windowless" value="true" />
					<param name="autoUpgrade" value="false" />
					<param name="initParams" value="mode=customReport, fileName=<%=Model.Query %>" />
					<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">
						<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Получить сильверлайт"
							style="border-style: none" /></a>
				</object>
			</div>
		</td>
	</table>  