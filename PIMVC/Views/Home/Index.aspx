<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Домашняя страница
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<style>
		body{
			overflow:hidden !important;
			padding:0px !important;
			margin:0px !important;
		}
	</style>
	<div class='allMainPage' align='center'>
	<img class='bg' src='/Content/images/main/night5.jpg' vspace="0" hspace="0" width="100%"/>
	<img style="position:absolute;left:30px;top:50px;width:200px;" src="/Content/images/logo/votges.gif" />
	<div style="position:absolute;bottom:0px;right:0px" align="center">
		<div width="300px">
		<img style="height:50px;" src="/Content/images/logo/inda.gif" />
		<img style="height:50px;" src="/Content/images/logo/osi.gif" />
		</div>
	</div>
	
	<br>
</div>	

</asp:Content>
