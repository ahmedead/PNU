<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTwContactCards.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Tawasul.Controls.ucTwContactCards, $SharePoint.Project.AssemblyFullName$" %>

<section id="secContactCards" runat="server" class="py-5" aria-labelledby="tw-contact-title">
    <div class="container">
        <div class="row g-4 mb-5">

            <%-- ------------------- التواصل مع الجامعة ------------------- --%>
            <div class="col-12 col-lg-6">
                <article class="card h-100">
                    <div class="card-body p-4 p-lg-5 d-flex flex-column gap-4">
                        <div>
                            <h2 id="tw-contact-title" class="mb-2"><asp:Literal ID="ltUniTitle" runat="server" /></h2>
                            <p class="mb-0 text-body-secondary"><asp:Literal ID="ltUniSubtitle" runat="server" /></p>
                        </div>

                        <div class="d-flex flex-column gap-3">
                            <asp:Repeater ID="rptUniChannels" runat="server">
                                <ItemTemplate>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class='hgi hgi-stroke <%# Eval("IconClass") %>' aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <h3 class="fw-bold mb-1 h6"><%# Eval("Title") %></h3>
                                            <%# Eval("ChannelBodyHtml") %>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Literal ID="ltUniAddress" runat="server" />
                        </div>

                        <asp:PlaceHolder ID="phPlatforms" runat="server">
                            <div class="mt-auto">
                                <h3 class="fw-bold mb-2 h6"><asp:Literal ID="ltPlatforms" runat="server" /></h3>
                                <div class="d-flex flex-wrap gap-2">
                                    <asp:Repeater ID="rptSocial" runat="server">
                                        <ItemTemplate>
                                            <a class="btn btn-outline-primary btn-sm" href='<%# Eval("LinkUrl") %>'
                                               target="_blank" rel="noopener noreferrer" aria-label='<%# Eval("Title") %>'>
                                                <i class='hgi hgi-stroke <%# Eval("IconClass") %> fs-5' aria-hidden="true"></i>
                                            </a>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </article>
            </div>

            <%-- ------------------------ تواصل نورة ---------------------- --%>
            <div class="col-12 col-lg-6">
                <article class="card h-100">
                    <div class="card-body p-4 p-lg-5 d-flex flex-column gap-4">
                        <div>
                            <h2 class="mb-2"><asp:Literal ID="ltNourahTitle" runat="server" /></h2>
                            <p class="mb-0 text-body-secondary"><asp:Literal ID="ltNourahSubtitle" runat="server" /></p>
                        </div>

                        <div class="d-flex flex-column gap-3">
                            <asp:Repeater ID="rptNourahChannels" runat="server">
                                <ItemTemplate>
                                    <div class="d-flex gap-2 align-items-start">
                                        <span class="flex-shrink-0 d-inline-flex fs-3 lh-1 text-primary">
                                            <i class='hgi hgi-stroke <%# Eval("IconClass") %>' aria-hidden="true"></i>
                                        </span>
                                        <div>
                                            <h3 class="fw-bold mb-1 h6"><%# Eval("Title") %></h3>
                                            <%# Eval("ChannelBodyHtml") %>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </article>
            </div>

        </div>
    </div>
</section>
