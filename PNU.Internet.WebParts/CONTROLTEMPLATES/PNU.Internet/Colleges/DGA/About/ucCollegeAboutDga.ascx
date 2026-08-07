<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeAboutDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.DGA.About.ucCollegeAboutDga" %>



<section id="secAbout" runat="server" class="page-padding">
    <div class="row g-5">

        <%-- Vision / Mission cards --%>
        <asp:Repeater ID="rptCards" runat="server">
            <ItemTemplate>
                <div class="col-12 col-md-6 col-sm-12">
                    <div class="card h-100">
                        <div class="card-body">
                            <div class="icon-container">
                                <i class="hgi hgi-stroke <%# Eval("IconClass") %> fs-3" aria-hidden="true"></i>
                            </div>
                            <div>
                                <h3 class="card-title h5"><%# Eval("DisplayTitle") %></h3>
                                <p class="card-text"><%# Eval("DisplayText") %></p>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <%-- Goals card (full width, bullet list) --%>
        <asp:PlaceHolder ID="phGoals" runat="server">
            <div class="col-12">
                <div class="card h-100">
                    <div class="card-body">
                        <div class="icon-container">
                            <i class="hgi hgi-stroke hgi-target-02 fs-3" aria-hidden="true"></i>
                        </div>
                        <div>
                            <h3 class="card-title h5"><asp:Literal ID="ltrGoalsTitle" runat="server" /></h3>
                            <ul class="card-text mb-0">
                                <asp:Repeater ID="rptGoals" runat="server">
                                    <ItemTemplate>
                                        <li><%# Container.DataItem %></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>

        <%-- Dean's welcome --%>
        <%--<asp:PlaceHolder ID="phDeanWord" runat="server">
            <section id="president-welcome" aria-labelledby="president-welcome-title">
                <div class="card mb-4 bg-primary-25 border-0">
                    <div class="card-body p-4 p-lg-5">
                        <div class="d-flex flex-column gap-3">
                            <span class="icon-container bg-white">
                                <i class="hgi hgi-stroke hgi-quote-down fs-4" aria-hidden="true"></i>
                            </span>
                            <h2 id="president-welcome-title" class="mb-0"><asp:Literal ID="ltrDeanWordTitle" runat="server" /></h2>
                            <p class="lead mb-0 text-justify"><asp:Literal ID="ltrDeanWordText" runat="server" /></p>
                        </div>
                        <div class="card-body">
                            <div>
                                <h3 class="card-title"><asp:Literal ID="ltrDeanName" runat="server" /></h3>
                                <p class="card-text mb-0"><asp:Literal ID="ltrDeanPosition" runat="server" /></p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </asp:PlaceHolder>--%>

    </div>
</section>
