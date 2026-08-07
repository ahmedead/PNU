<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUniversityCounsil.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.ucUniversityCounsil" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>


<style>
.card.card-floated-title {
    min-height: 134px;
}
</style>



<section class="faculty-members">
    <div class="container py-5 my-5">
        <div class="d-flex justify-content-center mb-5">
            <div>
                <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                    <asp:Literal runat="server" Text="<%$ Resources: PNUres, UniversityCounsil %>" />
                    <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
                </h1>
            </div>
        </div>

        <div class="row mt-4 pt-4">


            <asp:Repeater ID="rptMainData" runat="server">
                <ItemTemplate>

                    <div class="col-lg-3 col-md-6 mb-5">
                        <div class="card card-floated-title">
                            <div class="card-title <%#DataBinder.Eval(Container.DataItem,"ClassName") %>">
                                <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEN")) %>
                            </div>
                            <div class="card-body p-4">
                                <strong> <%# SPFactory.GetLocalizedTitle(Eval("NameAr"), Eval("NameEN")) %></strong>
                                <div> <%# SPFactory.GetLocalizedTitle(Eval("PositionAr"), Eval("PositionEN")) %></div>

                            </div>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>
</section>
<style>
img.Counsil-img {
    height: 350px;
    padding: 5;
    width:400px
}

.col-sm-6.col-lg-4.col-xl-3.text-center.mb-4.pb-4 {
    padding: 10px;
}
</style>