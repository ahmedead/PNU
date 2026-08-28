<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterDigitalChannelsDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Centers.DGA.ucCenterDigitalChannelsDga" %>

<section id="center-digital-channels" class="pnu-section-anchor mb-5">
    <h2 class="mb-4">
        <asp:Literal ID="litChannelsTitle" runat="server" />
    </h2>
    <div class="row g-4">
        <asp:Repeater ID="rptChannels" runat="server">
            <ItemTemplate>
                <div class="col-12 col-md-6">
                    <article class="card nav-card h-100">
                        <div class="d-flex card-body flex-column gap-4">
                            <div class="icon-container">
                                <i class='<%# "hgi hgi-stroke " + Eval("IconClass") + " fs-3" %>' aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="card-title"><%# Eval("Title") %></h3>
                                <p class="card-text"><%# Eval("Description") %></p>
                            </div>
                            <div class="d-flex justify-content-end mt-auto">
                                <a class="btn btn-secondary stretched-link" href='<%# Eval("Url") %>' target="_blank"
                                   rel="external noopener noreferrer" aria-label='<%# "الانتقال إلى " + Eval("Title") %>'>
                                    <i class="hgi hgi-stroke hgi-arrow-left-02 fs-4" aria-hidden="true"></i>
                                </a>
                            </div>
                        </div>
                    </article>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</section>
