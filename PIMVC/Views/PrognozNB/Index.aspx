<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PIMVC.Models.PrognozNBModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Прогноз НБ
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div class='resizable resizeFull' style="width:100%; height:600px; border-color:Blue; border-width:1px; border-style:solid;">
		<object data="data:application/x-silverlight," type="application/x-silverlight-2"
			width="100%" height="100%">
			<param name="source" value="<%=Url.Content("~/ClientBin/SilverChart.xap")%>" />
			<param name="background" value="White" />
			<param name="windowless" value="true" />
			<param name="autoUpgrade" value="false" />
			<param name="initParams" value="mode=prognozNB" />
			<a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;">
				<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Получить сильверлайт"
					style="border-style: none" /></a>
		</object>
	</div>

</asp:Content>
