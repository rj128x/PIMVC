<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
 Inherits="System.Web.Mvc.ViewPage<PILib.PIReport.ReportRequest>" ValidateRequest="false"%>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Model.Name %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.RenderPartial("CustomReportForm", Model);%>

	
</asp:Content>


     



