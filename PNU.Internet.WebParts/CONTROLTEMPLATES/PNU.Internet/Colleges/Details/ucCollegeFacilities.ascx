<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeFacilities.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details.ucCollegeFacilities" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section id="faculty-facilities" class="pnu-section-anchor mb-5">
    <h2><%= SPContext.Current.ListItem["Title"] %></h2>
    <p><%= Convert.ToString(SPContext.Current.ListItem["Comments"]) %></p>
    <div class="accordion accordion-flush mt-3" id="faculty-facilitiesAccordion">
        <asp:Repeater ID="rptFacilities" runat="server">
            <ItemTemplate>
                <div class="accordion-item">
                    <p class="accordion-header"
                       id='faculty-facilitiesAccordionHeading<%# Container.ItemIndex + 1 %>'>
                        <button
                            class='accordion-button <%# Container.ItemIndex == 0 ? "" : "collapsed" %>'
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target='#faculty-facilitiesAccordionCollapse<%# Container.ItemIndex + 1 %>'
                            aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                            aria-controls='faculty-facilitiesAccordionCollapse<%# Container.ItemIndex + 1 %>'>
                            <%# Eval("Title") %>
                        </button>
                    </p>
                    <div id='faculty-facilitiesAccordionCollapse<%# Container.ItemIndex + 1 %>'
                         class='accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>'
                         aria-labelledby='faculty-facilitiesAccordionHeading<%# Container.ItemIndex + 1 %>'
                         data-bs-parent="#faculty-facilitiesAccordion">
                        <div class="accordion-body">
                            <%# CleanRichText(Convert.ToString(Eval("Content"))) %>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>