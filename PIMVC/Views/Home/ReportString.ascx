<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PILib.PIReport.ReportString>" %>
<tr id='<%=Model.ID%>' class='<%=Model.ParentIDClass%> <%=Model.HasChildsClass%> <%=Model.OpenedClass %>'>
  <td  class='treeHeader header nowrap' style="text-align:left"> <%=Model.tagInfo.Title%></td>
  <th class='nowrap' style='text-align:right;'><%=Model.tagInfo.TagMode==TagModeEnum.noVal?"-":Model.report.resultTags[Model.tagInfo.ID].ComputedVal.ToString(Model.tagInfo.StringFormat) %></th>
    <%foreach (DateTime dt in Model.report.diffDates) {				
			Tag tag=Model.tags.Keys.Contains(dt)?Model.tags[dt]:null;
			bool broken=tag == null || tag.Broken;
			%>
		
      <td class='nowrap <%=broken?"broken":"" %>' style='text-align:right;'><%=(tag != null)&&(Model.tagInfo.TagMode!=TagModeEnum.noVal) ? tag.ComputedVal.ToString(Model.tagInfo.StringFormat) : "-"%></td>
    <%} %>  
</tr>
<%foreach (TagInfo tagInfo in Model.tagInfo.ChildrenTags) { %>
  <%Html.RenderPartial("ReportString", Model.report.getReportString(tagInfo.ID)); %>
<%} %>
