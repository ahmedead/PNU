<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUniversityPresidentOffice.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice.ucUniversityPresidentOffice, $SharePoint.Project.AssemblyFullName$" %>

<article class="col-12 col-lg-8 pnu-detail-article" data-aos="fade-up">

    <%-- ======================== Content Sections ======================== --%>
    <asp:Repeater ID="rptSections" runat="server">
        <ItemTemplate>

            <section id='<%# "president-section-" + Eval("Id") %>' aria-labelledby='<%# "president-section-title-" + Eval("Id") %>'>

                <%-- Lead card (shown only for the first/welcome section) --%>
                <asp:PlaceHolder ID="phLeadCard" runat="server"
                    Visible='<%# Eval("HasLeadCard") %>'>
                    <div class="card mb-4 bg-primary-25 border-0">
                        <div class="card-body p-4 p-lg-5">
                            <div class="d-flex flex-column gap-3">
                                <span class="icon-container">
                                    <i class='<%# "hgi hgi-stroke " + Eval("IconClass") + " fs-4" %>' aria-hidden="true"></i>
                                </span>
                                <h2 id='<%# "president-section-title-" + Eval("Id") %>' class="mb-0"><%# Eval("Title") %></h2>
                                <p class="lead mb-0"><%# Eval("LeadText") %></p>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>

                <%-- Section heading (when no lead card) --%>
                <asp:PlaceHolder ID="phSectionHeading" runat="server"
                    Visible='<%# !(bool)Eval("HasLeadCard") %>'>
                    <h2 id='<%# "president-section-title-" + Eval("Id") %>' class="mt-5 mb-3"><%# Eval("Title") %></h2>
                </asp:PlaceHolder>

                <%-- Section body --%>
                <asp:PlaceHolder ID="phDescription" runat="server"
                    Visible='<%# Eval("HasDescription") %>'>
                    <div><%# Eval("Description") %></div>
                </asp:PlaceHolder>

                <%-- Optional image --%>
                <asp:PlaceHolder ID="phImage" runat="server"
                    Visible='<%# Eval("HasImage") %>'>
                    <div class="mt-4">
                        <img class="w-100 rounded-2" loading="lazy" decoding="async"
                             src='<%# Eval("ImageUrl") %>'
                             alt='<%# Eval("Title") %>' />
                    </div>
                </asp:PlaceHolder>

            </section>

        </ItemTemplate>
    </asp:Repeater>

    <%-- ======================== President Signature ======================== --%>
    <section id="president-signature" aria-labelledby="president-signature-title">
        <h2 id="president-signature-title" class="mt-5 mb-3">
            <asp:Literal ID="litSignatureHeading" runat="server" />
        </h2>
        <div class="row g-4">
            <asp:Repeater ID="rptSignature" runat="server">
                <ItemTemplate>
                    <div class="col-6">
                        <article class="card nav-card h-100">
                            <div class="card-body">
                                <asp:PlaceHolder ID="phSigImage" runat="server"
                                    Visible='<%# Eval("HasImage") %>'>
                                    <img width="400" height="250" class="w-100 rounded-2"
                                         alt='<%# Eval("Title") %>' loading="lazy" fetchpriority="low"
                                         src='<%# Eval("ImageUrl") %>'
                                         decoding="async">
                                </asp:PlaceHolder>
                                <div>
                                    <h3 class="card-title"><%# Eval("Title") %></h3>
                                    <p class="card-text mb-0"><%# Eval("Description") %></p>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>

    <%-- ======================== Contact Table ======================== --%>
    <section id="president-contact" aria-labelledby="president-contact-title">
        <h2 id="president-contact-title" class="mt-5 mb-3">
            <asp:Literal ID="litContactHeading" runat="server" />
        </h2>
        <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
            <table class="table table-striped align-middle mb-0">
                <thead>
                    <tr>
                        <th scope="col"><asp:Literal ID="litChannelHeader" runat="server" /></th>
                        <th scope="col"><asp:Literal ID="litContactDataHeader" runat="server" /></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptContacts" runat="server" OnItemDataBound="rptContacts_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Title") %></td>
                                <td>
                                    <asp:HyperLink ID="lnkContact" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </section>

</article>
