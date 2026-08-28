<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterMembers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucCenterMembers" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<div class="d-flex flex-column gap-4">
    <h2 class="mb-4 fw-semibold">
        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIMembers %>" />
    </h2>

    <div class="row g-4">
        <asp:Repeater ID="rptMainData" runat="server">
            <ItemTemplate>
                <div class="col-12 col-md-6 col-xl-4">
                    <article class="card h-100 border rounded-3 overflow-hidden shadow-sm">
                        <div class="card-body p-4 d-flex flex-column gap-3">
                            <div class="d-flex align-items-center justify-content-between">
                                <div class="icon-container">
                                    <i class="hgi hgi-stroke hgi-user-03 fs-3 text-primary" aria-hidden="true"></i>
                                </div>
                                <span class="badge text-bg-light border small">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEn")) %>
                                </span>
                            </div>

                            <div>
                                <h3 class="card-title h6 fw-bold mb-1">
                                    <%# SPFactory.GetLocalizedTitle(Eval("NameAr"), Eval("NameEn")) %>
                                </h3>
                                <p class="card-text text-muted small mb-0">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Position"), Eval("PositionEn")) %>
                                </p>
                            </div>

                            <div class="mt-auto pt-3 border-top d-flex align-items-center gap-2 small">
                                <i class="hgi hgi-stroke hgi-mail-01 text-primary flex-shrink-0" aria-hidden="true"></i>
                                <a href='<%# "mailto:" + Eval("Email") %>' class="text-decoration-none text-body text-truncate" dir="ltr">
                                    <%# SPFactory.GetLocalizedTitle(Eval("Email"), Eval("Email")) %>
                                </a>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
