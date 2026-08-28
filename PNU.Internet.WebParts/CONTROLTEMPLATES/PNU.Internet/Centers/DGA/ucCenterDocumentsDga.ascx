<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterDocumentsDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA.ucCenterDocumentsDga" %>

<section id="center-documents" class="pnu-section-anchor mb-5">
    <h2 class="mb-4">
        <asp:Literal ID="ltrHeading" runat="server" />
    </h2>

    <div class="accordion accordion-flush" id="center-documentsAccordion">
        <asp:Repeater ID="rptGroups" runat="server" OnItemDataBound="rptGroups_ItemDataBound">
            <ItemTemplate>
                <div class="accordion-item">
                    <h3 class="accordion-header" id='<%# Eval("HeadingId") %>'>
                        <button class="accordion-button collapsed" type="button"
                                data-bs-toggle="collapse"
                                data-bs-target='<%# "#" + Eval("CollapseId") %>'
                                aria-expanded="false"
                                aria-controls='<%# Eval("CollapseId") %>'>
                            <%# Eval("GroupTitle") %>
                        </button>
                    </h3>

                    <div class="accordion-collapse collapse" id='<%# Eval("CollapseId") %>'
                         aria-labelledby='<%# Eval("HeadingId") %>'
                         data-bs-parent="#center-documentsAccordion">
                        <div class="accordion-body">
                            <div class="row g-4">
                                <asp:Repeater ID="rptItems" runat="server">
                                    <ItemTemplate>
                                        <div class="col-12 col-md-6">
                                            <div class="card nav-card h-100">
                                                <div class="d-flex card-body flex-column gap-4">
                                                    <div class="icon-container">
                                                        <i class="hgi hgi-stroke hgi-file-02 fs-3" aria-hidden="true"></i>
                                                    </div>
                                                    <div>
                                                        <h4 class="card-title h6"><%# Eval("Name") %></h4>
                                                    </div>
                                                    <div class="d-flex justify-content-end mt-auto">
                                                        <a class="btn btn-secondary stretched-link"
                                                           href='<%# Eval("Url") %>' target="_blank"
                                                           rel="noopener noreferrer"
                                                           aria-label='<%# Eval("Name") %>'>
                                                            <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
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
