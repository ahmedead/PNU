<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchAdmin.aspx.cs" Inherits="PNU.Internet.WebParts.Layouts.PNU.Internet.Search.SearchAdmin" 
    DynamicMasterPageFile="~masterurl/DGA_Internal.master"  Async="false"%>



<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container py-4">
        <h2>PNU Custom Search - admin</h2>
        <p>
            Configuration lives under
            <strong><asp:HyperLink runat="server" ID="hlConfigSite" /></strong>.
            The menu crawler resumes from where it stopped after a crash —
            already-processed sub-webs are skipped.
            Use <em>Reset</em> to force a full re-run.
        </p>

        <hr/>
        <h4>Per-task buttons</h4>
        <table class="table">
            <thead><tr>
                <th>Task</th><th>Description</th>
                <th>Run</th><th>Reset log</th>
            </tr></thead>
            <tbody>
                <tr>
                    <td>1. Menus</td>
                    <td>Recursive walk + bilingual page indexing.
                        Crashes? Click again to resume.</td>
                    <td><asp:Button runat="server" ID="btnTaskMenus"
                        CssClass="btn btn-primary" Text="Run Menus"
                        OnClick="btnTaskMenus_Click" /></td>
                    <td><asp:Button runat="server" ID="btnResetMenus"
                        CssClass="btn btn-sm btn-outline-warning" Text="Reset"
                        OnClick="btnResetMenus_Click" /></td>
                </tr>
                <tr>
                    <td>2. News</td>
                    <td>RequestsList, MainCategory = الأخبار الرئيسية</td>
                    <td><asp:Button runat="server" ID="btnTaskNews"
                        CssClass="btn btn-primary" Text="Run News"
                        OnClick="btnTaskNews_Click" /></td>
                    <td><asp:Button runat="server" ID="btnResetNews"
                        CssClass="btn btn-sm btn-outline-warning" Text="Reset"
                        OnClick="btnResetNews_Click" /></td>
                </tr>
                <tr>
                    <td>3. Digital Media</td>
                    <td>Same RequestsList, MainCategory ≠ الأخبار الرئيسية</td>
                    <td><asp:Button runat="server" ID="btnTaskDigital"
                        CssClass="btn btn-primary" Text="Run Digital Media"
                        OnClick="btnTaskDigital_Click" /></td>
                    <td><asp:Button runat="server" ID="btnResetDigital"
                        CssClass="btn btn-sm btn-outline-warning" Text="Reset"
                        OnClick="btnResetDigital_Click" /></td>
                </tr>
                <tr>
                    <td>4. Events</td>
                    <td>AdvertisementsRequests under
                        <code>/ar/MediaCenter/MediaCenterAdmin</code></td>
                    <td><asp:Button runat="server" ID="btnTaskEvents"
                        CssClass="btn btn-primary" Text="Run Events"
                        OnClick="btnTaskEvents_Click" /></td>
                    <td><asp:Button runat="server" ID="btnResetEvents"
                        CssClass="btn btn-sm btn-outline-warning" Text="Reset"
                        OnClick="btnResetEvents_Click" /></td>
                </tr>
                <tr>
                    <td>5. E-Services</td>
                    <td>EservicesList under root</td>
                    <td><asp:Button runat="server" ID="btnTaskEServices"
                        CssClass="btn btn-primary" Text="Run E-Services"
                        OnClick="btnTaskEServices_Click" /></td>
                    <td><asp:Button runat="server" ID="btnResetEServices"
                        CssClass="btn btn-sm btn-outline-warning" Text="Reset"
                        OnClick="btnResetEServices_Click" /></td>
                </tr>
                <tr>
                    <td>6. Faculties</td>
                    <td>AllFaculties list under <code>/Admin</code></td>
                    <td><asp:Button runat="server" ID="btnTaskFaculties"
                        CssClass="btn btn-primary" Text="Run Faculties"
                        OnClick="btnTaskFaculties_Click" /></td>
                    <td><asp:Button runat="server" ID="btnResetFaculties"
                        CssClass="btn btn-sm btn-outline-warning" Text="Reset"
                        OnClick="btnResetFaculties_Click" /></td>
                </tr>
                <tr>
                    <td>7. Manual Pages</td>
                    <td>URLs from <em>ManualPagesToIndex</em> list</td>
                    <td><asp:Button runat="server" ID="btnTaskManual"
                        CssClass="btn btn-primary" Text="Run Manual Pages"
                        OnClick="btnTaskManual_Click" /></td>
                    <td>—</td>
                </tr>
                <tr style="background:#fff8e1;">
                    <td>8. Retry Failed</td>
                    <td>Re-tries every URL in the <em>IndexErrors</em> list
                        where you ticked <strong>Retry = Yes</strong>.
                        Successful ones are auto-marked <em>Resolved</em>.</td>
                    <td><asp:Button runat="server" ID="btnTaskRetry"
                        CssClass="btn btn-warning" Text="Run Retry Failed"
                        OnClick="btnTaskRetry_Click" /></td>
                    <td>—</td>
                </tr>
                <tr>
                    <td>9. Index Website Pages</td>
                    <td>Crawl websites from root or targeted site URL, extract PageTitle, PageURL, PageLayout, UserControls &amp; UserControlProperties into <code>dbo.WebsitePages</code> with live status grid &amp; Excel export</td>
                    <td><a href="PageIndexAdmin.aspx" class="btn btn-success">Open Website Pages Indexer</a></td>
                    <td>—</td>
                </tr>

            </tbody>
        </table>

        <hr/>
        <h4>Configuration</h4>
        <asp:Button runat="server" ID="btnReloadConfig"
                    CssClass="btn btn-secondary"
                    Text="Reload configuration"
                    OnClick="btnReloadConfig_Click" />
        <asp:Button runat="server" ID="btnReprovision"
                    CssClass="btn btn-outline-secondary"
                    Text="Re-provision config lists"
                    OnClick="btnReprovision_Click" />
        <asp:Button runat="server" ID="btnClearAllLogs"
                    CssClass="btn btn-outline-danger"
                    Text="Clear ALL task logs"
                    OnClick="btnClearAllLogs_Click"
                    OnClientClick="return confirm('This forces every task to re-run from scratch. Continue?');" />

        <h4 class="mt-4">Status</h4>
        <asp:Literal runat="server" ID="litStatus" />

        <h4 class="mt-4">Result</h4>
        <asp:Literal runat="server" ID="litResult" />
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    PNU Custom Search admin
</asp:Content>
