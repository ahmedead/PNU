<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeStudents.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details.ucCollegeStudents" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

 <section id="faculty-students" class="pnu-section-anchor mb-5">
    <h2><%= SPContext.Current.ListItem["Title"] %></h2>
    <p><%= Convert.ToString(SPContext.Current.ListItem["Comments"]) %></p>
    <div class="accordion accordion-flush mt-3" id="faculty-studentsAccordion">
        <asp:Repeater ID="rptStudents" runat="server" OnItemDataBound="rptStudents_ItemDataBound">
            <ItemTemplate>
                <div class="accordion-item">
                    <p class="accordion-header"
                       id='faculty-studentsAccordionHeading<%# Container.ItemIndex + 1 %>'>
                        <button
                            class='accordion-button <%# Container.ItemIndex == 0 ? "" : "collapsed" %>'
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target='#faculty-studentsAccordionCollapse<%# Container.ItemIndex + 1 %>'
                            aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                            aria-controls='faculty-studentsAccordionCollapse<%# Container.ItemIndex + 1 %>'>
                            <%# Eval("Title") %>
                        </button>
                    </p>
                    <div id='faculty-studentsAccordionCollapse<%# Container.ItemIndex + 1 %>'
                         class='accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>'
                         aria-labelledby='faculty-studentsAccordionHeading<%# Container.ItemIndex + 1 %>'
                         data-bs-parent="#faculty-studentsAccordion">
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
                            <p id="LocationDiv"
                               runat="server"
                               visible='<%# !string.IsNullOrWhiteSpace(Convert.ToString(Eval("Location"))) %>'>
                                <strong>
                                    <%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                                        ? "الموقع:"
                                        : "Location:" %>
                                </strong>
                                <%# Eval("Location") %>
                            </p>
                            <div id="CoordinatorsDiv" runat="server">
                                <h3 class="h5 mb-3">
                                    <%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)
                                        ? "منسقو التدريب في الأقسام"
                                        : "Training Coordinators by Department" %>
                                </h3>
                                <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                                    <asp:Repeater ID="rptCoordinators" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-striped mb-0">
                                                <thead>
                                                    <tr>
														<th scope="col">
															<%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)? "البرنامج": "Program" %>
														</th>
														<th scope="col">
															<%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)? "المنسق": "Coordinator" %>
														</th>
														<th scope="col">
															<%= Request.Url.AbsolutePath.StartsWith("/ar/", StringComparison.OrdinalIgnoreCase)? "البريد الإلكتروني": "Email" %>
														</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Eval("ProgramName") %></td>
                                                <td><%# Eval("CoordinatorName") %></td>
                                                <td>
                                                    <a href='mailto:<%# Eval("Email") %>'>
                                                        <%# Eval("Email") %>
                                                    </a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                                </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <div class="row g-3 mt-3">
                                <asp:Repeater ID="rptImages" runat="server">
                                    <ItemTemplate>
                                        <div class="col-12 col-md-6">
                                            <img class="img-fluid rounded-3" src="<%# Eval("ImageUrl") %>" alt="Content Image" loading="lazy" decoding="async" />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>	
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
