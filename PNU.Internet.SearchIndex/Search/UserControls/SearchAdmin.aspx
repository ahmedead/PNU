<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="SearchAdmin.aspx.cs"
    Inherits="PNU.Internet.Search.Layouts.PNU.Internet.Search.SearchAdmin"
    MasterPageFile="~/_layouts/15/simple.master" %>

<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div class="container py-4">
        <h2>PNU Custom Search - admin</h2>
        <p>
            Configuration lives under
            <strong><asp:HyperLink runat="server" ID="hlConfigSite" /></strong>.
            Each task below logs the webs it processed to
            <em>IndexedWebs</em> so re-running it is fast (already-done
            webs are skipped). Use <em>Reset Task Log</em> to force a
            full re-run.
        </p>

        <hr/>

        <h4>Per-task buttons</h4>

        <table class="table">
            <thead>
                <tr>
                    <th>Task</th>
                    <th>Description</th>
                    <th>Run</th>
                    <th>Reset log</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>1. Menus</td>
                    <td>Walks <code>TopMenuLevel1/2/3</code> in every subsite</td>
                    <td>
                        <asp:Button runat="server" ID="btnTaskMenus"
                            CssClass="btn btn-primary"
                            Text="Run Menus" OnClick="btnTaskMenus_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnResetMenus"
                            CssClass="btn btn-sm btn-outline-warning"
                            Text="Reset"
                            OnClick="btnResetMenus_Click" />
                    </td>
                </tr>
                <tr>
                    <td>2. News</td>
                    <td>RequestsList where MainCategory = الأخبار الرئيسية</td>
                    <td>
                        <asp:Button runat="server" ID="btnTaskNews"
                            CssClass="btn btn-primary"
                            Text="Run News" OnClick="btnTaskNews_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnResetNews"
                            CssClass="btn btn-sm btn-outline-warning"
                            Text="Reset"
                            OnClick="btnResetNews_Click" />
                    </td>
                </tr>
                <tr>
                    <td>3. Digital Media</td>
                    <td>Same RequestsList, MainCategory ≠ الأخبار الرئيسية</td>
                    <td>
                        <asp:Button runat="server" ID="btnTaskDigital"
                            CssClass="btn btn-primary"
                            Text="Run Digital Media" OnClick="btnTaskDigital_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnResetDigital"
                            CssClass="btn btn-sm btn-outline-warning"
                            Text="Reset"
                            OnClick="btnResetDigital_Click" />
                    </td>
                </tr>
                <tr>
                    <td>4. Events</td>
                    <td>AdvertisementsRequests under <code>/ar/MediaCenter/MediaCenterAdmin</code></td>
                    <td>
                        <asp:Button runat="server" ID="btnTaskEvents"
                            CssClass="btn btn-primary"
                            Text="Run Events" OnClick="btnTaskEvents_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnResetEvents"
                            CssClass="btn btn-sm btn-outline-warning"
                            Text="Reset"
                            OnClick="btnResetEvents_Click" />
                    </td>
                </tr>
                <tr>
                    <td>5. E-Services</td>
                    <td>EservicesList under root</td>
                    <td>
                        <asp:Button runat="server" ID="btnTaskEServices"
                            CssClass="btn btn-primary"
                            Text="Run E-Services" OnClick="btnTaskEServices_Click" />
                    </td>
                    <td>
                        <asp:Button runat="server" ID="btnResetEServices"
                            CssClass="btn btn-sm btn-outline-warning"
                            Text="Reset"
                            OnClick="btnResetEServices_Click" />
                    </td>
                </tr>
                <tr>
                    <td>6. Manual Pages</td>
                    <td>URLs from <em>ManualPagesToIndex</em> list</td>
                    <td>
                        <asp:Button runat="server" ID="btnTaskManual"
                            CssClass="btn btn-primary"
                            Text="Run Manual Pages" OnClick="btnTaskManual_Click" />
                    </td>
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
                    OnClientClick="return confirm('This forces every task to re-run from scratch on next click. Continue?');" />

        <h4 class="mt-4">Status</h4>
        <asp:Literal runat="server" ID="litStatus" />

        <h4 class="mt-4">Result</h4>
        <asp:Literal runat="server" ID="litResult" />
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    PNU Custom Search admin
</asp:Content>
