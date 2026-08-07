<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSideMenu.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.SideMenu.ucSideMenu" %>


<asp:Panel ID="pnlSideMenu" runat="server" CssClass="entity-details-sidemenu bg-body" role="navigation">

    <%-- Mobile toggle: label shows the currently active item --%>
    <button type="button"
        class="entity-details-sidemenu-toggle btn btn-link w-100 align-items-center gap-3 px-3 text-decoration-none has-rotatable-icon collapsed border-bottom"
        data-bs-toggle="collapse" data-bs-target="#sideMenuDrawer"
        aria-expanded="false" aria-controls="sideMenuDrawer">
        <i class="hgi hgi-stroke hgi-menu-02 fs-4 text-primary" aria-hidden="true"></i>
        <span class="entity-details-sidemenu-current flex-grow-1 text-start fw-semibold">
            <asp:Literal ID="litCurrentTitle" runat="server" EnableViewState="false" /></span>
        <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
    </button>

    <div class="entity-details-sidemenu-drawer collapse d-lg-block" id="sideMenuDrawer">
        <nav class="entity-details-tabs nav flex-column align-items-stretch" id="sideMenuNav">

            <asp:Repeater ID="rptLevel1" runat="server" OnItemDataBound="rptLevel1_ItemDataBound">
                <ItemTemplate>

                    <%-- ============ Leaf item (no children) ============ --%>
                    <asp:PlaceHolder ID="phLeaf" runat="server" Visible="false">
                        <a class='<%# ((bool)Eval("IsActive")) ? "nav-link text-start text-nowrap active" : "nav-link text-start text-nowrap" %>'
                           href='<%# Eval("ResolvedUrl") %>'
                           aria-current='<%# ((bool)Eval("IsActive")) ? "page" : "" %>'>
                            <%# Server.HtmlEncode((string)Eval("DisplayTitle")) %>
                        </a>
                    </asp:PlaceHolder>

                    <%-- ============ Group item (has children) ============ --%>
                    <asp:PlaceHolder ID="phGroup" runat="server" Visible="false">
                        <div class="entity-details-sidemenu-group d-flex flex-column align-items-stretch">
                            <button type="button"
                                class='<%# ((bool)Eval("IsActive")) ? "nav-link text-start has-rotatable-icon active" : "nav-link text-start has-rotatable-icon collapsed" %>'
                                data-bs-toggle="collapse"
                                data-bs-target='<%# "#" + Eval("GroupId") %>'
                                aria-expanded='<%# ((bool)Eval("IsActive")) ? "true" : "false" %>'
                                aria-controls='<%# Eval("GroupId") %>'>
                                <span class="flex-grow-1"><%# Server.HtmlEncode((string)Eval("DisplayTitle")) %></span>
                                <span class="badge text-bg-light rounded-pill"><%# Eval("ChildCount") %></span>
                                <i class="hgi hgi-stroke hgi-arrow-down-01 rotatable-icon rotation-transition" aria-hidden="true"></i>
                            </button>

                            <div class='<%# ((bool)Eval("IsActive")) ? "toc-sub collapse show" : "toc-sub collapse" %>'
                                 id='<%# Eval("GroupId") %>'>
                                <asp:Repeater ID="rptLevel2" runat="server">
                                    <ItemTemplate>
                                        <a class='<%# ((bool)Eval("IsActive")) ? "toc-item d-block w-100 text-start text-decoration-none active" : "toc-item d-block w-100 text-start text-decoration-none" %>'
                                           href='<%# Eval("ResolvedUrl") %>'
                                           aria-current='<%# ((bool)Eval("IsActive")) ? "page" : "" %>'>
                                            <span><%# Server.HtmlEncode((string)Eval("DisplayTitle")) %></span>
                                        </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </asp:PlaceHolder>

                </ItemTemplate>
            </asp:Repeater>

        </nav>
    </div>
</asp:Panel>
