<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Excel.Master" Inherits="System.Web.Mvc.ViewPage<PIMVC.Models.PIReport.ReportModel>" ContentType="application/vnd.ms-excel; charset=utf-8"%>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">	
<style>	
table 
{
  border: solid 1px #e8eef4;
  border-collapse: collapse;
}
td 
{
  padding: 1px;   
  border: solid 1px #e8eef4;
}
th
{
  padding:1px;
  text-align: left;
  background-color: #e8eef4; 
  border: solid 1px #e8eef4;   
  font-weight: bold;
}
</style>
<% if (!Model.Request.IsVert){ 
	Html.RenderPartial("ReportData", Model); 
	}else{
	Html.RenderPartial("ReportDataVertNice", Model); 
	}
%>
</asp:Content>