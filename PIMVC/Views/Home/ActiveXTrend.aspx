<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Анализ трендов
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			pi.createObjectDisplay('<%=AppHelper.getPDIPath(Request,"Анализ трендов.pdi") %>');
		});
	</script>
	<div class="ui-tabs ui-widget ui-widget-content ui-corner-all fullScreen">
		<ul class="ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all">			
			<li class="ui-state-default ui-corner-top">
			<a class='displayOpen' url='<%=AppHelper.getPDIPath(Request,"Анализ трендов.pdi") %>'
				href="#">Анализ трендов</a></li>			
		</ul>
		<div align="center">		
			<div class='resizable resizeFull' style="width:100%; height:600px; border-color:Blue; border-width:1px; border-style:solid;">
				<div id='embedObject' style="width:100%;height:100%;"></div>
			</div>
		</div>
	</div>
	
</asp:Content>
