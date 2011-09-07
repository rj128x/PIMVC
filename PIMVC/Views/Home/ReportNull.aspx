<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PILib.PIReport.ReportRequest>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Model.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>Отчет <%=Model.Name %></h2>
	<h3>Параметры отчета</h3>
    <%Html.RenderPartial("ReportForm",Model); %>
</asp:Content>

