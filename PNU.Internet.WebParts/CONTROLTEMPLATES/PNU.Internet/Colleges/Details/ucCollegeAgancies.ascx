<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeAgancies.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.Details.ucCollegeAgancies" %>

<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<section id="faculty-agencies" class="pnu-section-anchor mb-5">
<h2><%= SPContext.Current.ListItem["Title"] %></h2>
<p><%= Convert.ToString(SPContext.Current.ListItem["Comments"]) %></p>
<div class="accordion accordion-flush mt-3" id="faculty-agenciesAccordion">
    <asp:Repeater ID="rptAgencies" runat="server" OnItemDataBound="rptAgencies_ItemDataBound">
        <ItemTemplate>
            <div class="accordion-item">
                <p class="accordion-header"
                   id='faculty-agenciesAccordionHeading<%# Container.ItemIndex + 1 %>'>
                    <button
                        class='accordion-button <%# Container.ItemIndex == 0 ? "" : "collapsed" %>'
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target='#faculty-agenciesAccordionCollapse<%# Container.ItemIndex + 1 %>'
                        aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                        aria-controls='faculty-agenciesAccordionCollapse<%# Container.ItemIndex + 1 %>'>
                        <%# Eval("Title") %>
                    </button>
                </p>
                <div id='faculty-agenciesAccordionCollapse<%# Container.ItemIndex + 1 %>'
                     class='accordion-collapse collapse <%# Container.ItemIndex == 0 ? "show" : "" %>'
                     aria-labelledby='faculty-agenciesAccordionHeading<%# Container.ItemIndex + 1 %>'
                     data-bs-parent="#faculty-agenciesAccordion">
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
						<h3 class="h5 mb-3 mt-4" id="DeptTitle" runat="server">
							<strong><%= Request.Url.AbsolutePath.IndexOf("/ar/", StringComparison.OrdinalIgnoreCase) >= 0 ? "الإدارات والوحدات التابعة للوكالة:" : "Departments and Units Reporting to the Vice Deanship:" %></strong>
						</h3>
						<div class="row g-4" id="DeptRow" runat="server">
							<asp:Repeater ID="rptDepartments" runat="server">
								<ItemTemplate>
									<div class="col-12 col-md-6">
										<div class="card nav-card h-100">
											<div class="d-flex card-body flex-column gap-4">
												<div class="icon-container">
													<i class="hgi hgi-stroke hgi-hierarchy-square-02 fs-3"></i>
												</div>
												<div>
													<h4 class="card-title">
														<%# Eval("Text") %>
													</h4>
												</div>
												<div class="d-flex justify-content-end mt-auto">
													<a class="btn btn-secondary stretched-link"
													   href="<%# Eval("URL") %>"
													   target="_blank"
													   rel="noopener noreferrer">
													   <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4"></i>
													</a>
												</div>
											</div>
										</div>
									</div>
								</ItemTemplate>
							</asp:Repeater>
						</div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
</section>