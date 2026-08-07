<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCenterMembers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucCenterMembers" %>




<%@ Import Namespace="PNU.Internet.WebParts" %>


<style>
.card.card-floated-title {
    min-height: 161px;
}

.card-title {
    position: absolute;
    top: 0;
    left: 50%;
    transform: translate(-50%, -50%);
    color: #fff;
    padding: 5px;
    border-radius: 5px;
    font-size: 18px;
    white-space: nowrap;
    width: 150px;
    text-align: center;

    &.--bg-primary {
      background-image: url(/images/multiply-vector/multiply-vector-primary.png);
      background-color: $turquoise-500;
    }

    &.--bg-secondary {
      background-image: url(/images/multiply-vector/multiply-vector-secondary.png);
      background-color: $resonant-blue-400;
    }

    &.--bg-tertiary {
      background-image: url(/images/multiply-vector/multiply-vector-tertiary.png);
      background-color: #5eb495;
    }

    &.--bg-quaternary {
      background-image: url(/images/multiply-vector/multiply-vector-quaternary.png);
      background-color: #8b85ca;
    }

</style>




<section class="faculty-members">
    <div class="the-message p-2">


    <div class="mt-3 px-md-5 px-0">
         <div class="container py-5 my-5">
     <div class="d-flex justify-content-center mb-5">
         <div>
             <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">
                 <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIMembers %>" />
                 <span class="px-2 position-absolute mt-1 h2 text-primary">&bull;</span>
             </h1>
         </div>
     </div>

     <div class="row mt-4 pt-4">


         <div class="repeater-container row mt-4 pt-4">
    <asp:Repeater ID="rptMainData" runat="server">
        <ItemTemplate>
            <div class="col-lg-3 col-md-6 mb-5">
                <div class="card card-floated-title">
                    <div class="card-title <%# DataBinder.Eval(Container.DataItem, "ClassName") %>">
                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("TitleEN")) %>
                    </div>
                    <div class="card-body p-4">
                        <strong><%# SPFactory.GetLocalizedTitle(Eval("NameAr"), Eval("NameEN")) %></strong>
                        <div><%# SPFactory.GetLocalizedTitle(Eval("Position"), Eval("PositionEN")) %></div>
                        <div><%# SPFactory.GetLocalizedTitle(Eval("Email"), Eval("Email")) %></div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>


     </div>
 </div>



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

.repeater-container > div:nth-child(5) {
    margin-top: 150px;
}
.repeater-container > div:nth-child(6) {
    margin-top: 150px;
}
.repeater-container > div:nth-child(7) {
    margin-top: 150px;
}
.repeater-container > div:nth-child(8) {
    margin-top: 150px;
}
</style>
