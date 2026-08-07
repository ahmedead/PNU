<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAboutCenter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAboutCenter" %>




<style>
    .wrap-text {
        white-space: normal; /* Allows text to wrap to the next line */
        word-wrap: break-word; /* Allows long words to be broken and wrap onto the next line */
        overflow-wrap: break-word; /* Similar to word-wrap for better compatibility */
    }
</style>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<div class="the-message p-2">
    <div class="mt-3 px-md-5 px-0">
        <asp:Repeater ID="rptMainData" runat="server">
            <ItemTemplate>







                <div class="d-block">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                        <%# SPFactory.GetLocalizedTitle(Eval("AboutTitle"), Eval("AboutTitle")) %>
                    </h1>
                </div>
                <div class="d-block text-start">
                    <p class=" text-muted lh-base fs-5">
                        <%# SPFactory.GetLocalizedTitle(Eval("AboutText"), Eval("AboutText")) %>
                    </p>
                </div>

                <div class="d-block">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                        <%# SPFactory.GetLocalizedTitle(Eval("VisionTitle"), Eval("VisionTitle")) %>
                    </h1>
                </div>
                <div class="d-block text-start">
                    <p class=" text-muted lh-base fs-5">
                        <%# SPFactory.GetLocalizedTitle(Eval("VisionText"), Eval("VisionText")) %>
                    </p>
                </div>

                <div class="d-block">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                        <%# SPFactory.GetLocalizedTitle(Eval("MessageTitle"), Eval("MessageTitle")) %>
                    </h1>
                </div>
                <div class="d-block text-start">
                    <p class=" text-muted lh-base fs-5">
                        <%# SPFactory.GetLocalizedTitle(Eval("MessageText"), Eval("MessageText")) %>
                    </p>
                </div>








            </ItemTemplate>
        </asp:Repeater>

        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <div class="d-block">
                    <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                        <%# SPFactory.GetLocalizedTitle(Eval("StrategicGoalsTitle"), Eval("StrategicGoalsTitle")) %>
                    </h1>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <div class="d-block text-start">
            <p class=" text-muted lh-base fs-5">
                <table class="d-block">


                    <tbody>
                        <asp:Repeater ID="rptStrategicPlan" runat="server">
                            <ItemTemplate>
                                <tr>

                                    <td class="text-muted lh-base fs-5"><%# SPFactory.GetLocalizedTitle(Eval("DisplayNo"), Eval("DisplayNo_EN")) %> - </td>
                                    <td class="text-muted lh-base fs-5"><%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>

                </table>
            </p>
        </div>





    </div>
</div>











