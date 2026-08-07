<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeStudentServices.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details.ucCollegeStudentServices" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section id="faculty-student-services" class="pnu-section-anchor mb-5">
    <h2><%= SPContext.Current.ListItem["Title"] %></h2>
    <p><%= Convert.ToString(SPContext.Current.ListItem["Comments"]) %></p>
    <div class="accordion accordion-flush mt-3" id="faculty-student-servicesAccordion">
        <asp:Repeater ID="rptStudentServices"
            runat="server"
            OnItemDataBound="rptStudentServices_ItemDataBound">
            <ItemTemplate>
                <div class="accordion-item">
                    <p class="accordion-header"
                       id='faculty-student-servicesAccordionHeading<%# Container.ItemIndex + 1 %>'>
                        <button
                            class='accordion-button <%# Container.ItemIndex == 0 ? "" : "collapsed" %>'
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target='#faculty-student-servicesAccordionCollapse<%# Container.ItemIndex + 1 %>'
                            aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                            aria-controls='faculty-student-servicesAccordionCollapse<%# Container.ItemIndex + 1 %>'>
                            <%# Eval("Title") %>
                        </button>
                    </p>
                    <div id='faculty-student-servicesAccordionCollapse<%# Container.ItemIndex + 1 %>'
                         class='accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>'
                         aria-labelledby='faculty-student-servicesAccordionHeading<%# Container.ItemIndex + 1 %>'
                         data-bs-parent="#faculty-student-servicesAccordion">
                        <div class="accordion-body">
                            <asp:Repeater ID="rptSections" runat="server">
                                <ItemTemplate>
                                    <h3 runat="server"
                                        class="h5 mb-3"
                                        visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("Title"))) %>'>
                                        <%# Eval("Title") %>
                                    </h3>
                                    <%# CleanRichText(Convert.ToString(Eval("Content"))) %>
                                </ItemTemplate>
                            </asp:Repeater>
							<asp:HyperLink ID="lnkService"
								runat="server"
								CssClass="btn btn-secondary mt-3"
								NavigateUrl='<%# Eval("ServiceLink") %>'
								Text='<%# Eval("ServiceText") %>'
								Target="_blank"
								rel="noopener noreferrer" />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>