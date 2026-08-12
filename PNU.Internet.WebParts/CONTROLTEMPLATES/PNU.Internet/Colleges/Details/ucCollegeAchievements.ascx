<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeAchievements.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details.ucCollegeAchievements" %>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section id="faculty-achievements" class="pnu-section-anchor mb-5">
	<h2><%= SPContext.Current.ListItem["Title"] %></h2>
	<p><%= Convert.ToString(SPContext.Current.ListItem["Comments"]) %></p>
	<div class="accordion accordion-flush mt-3" id="faculty-achievementsAccordion">
		<asp:Repeater ID="rptAchievements" runat="server" OnItemDataBound="rptAchievements_ItemDataBound">
			<ItemTemplate>
				<div class="accordion-item">
					<h3 class="accordion-header"
						id='faculty-achievementsAccordionHeading<%# Container.ItemIndex + 1 %>'>
						<button
							class='accordion-button <%# Container.ItemIndex == 0 ? "" : "collapsed" %>'
							type="button"
							data-bs-toggle="collapse"
							data-bs-target='#faculty-achievementsAccordionCollapse<%# Container.ItemIndex + 1 %>'
							aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
							aria-controls='faculty-achievementsAccordionCollapse<%# Container.ItemIndex + 1 %>'>
							<%# Eval("Title") %>
						</button>
					</h3>
					<div id='faculty-achievementsAccordionCollapse<%# Container.ItemIndex + 1 %>'
						class='accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>'
						aria-labelledby='faculty-achievementsAccordionHeading<%# Container.ItemIndex + 1 %>'
						data-bs-parent="#faculty-achievementsAccordion">
						<div class="accordion-body">
							<%# CleanRichText(Convert.ToString(Eval("Content"))) %>
							<p runat="server"
							   visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("Date"))) %>'>
								<strong><%= Request.Url.AbsolutePath.IndexOf("/ar/", StringComparison.OrdinalIgnoreCase) >= 0 ? "التاريخ:" : "Date:" %></strong>
								<%# Eval("Date") %>
							</p>
							<asp:Repeater ID="rptImages" runat="server">
								<ItemTemplate>
									<img src="<%# Container.DataItem %>" alt="Content Image" />
								</ItemTemplate>
							</asp:Repeater>
						</div>
					</div>
				</div>
			</ItemTemplate>
		</asp:Repeater>
	</div>
</section>



