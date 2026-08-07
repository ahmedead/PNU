<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllItems.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common.ucAllItems" %>


<div class="row">
    <asp:Repeater ID="rep" runat="server">
        <ItemTemplate>
            <div class="col-md-6 col-xl-4 col-xxl-3">
                <div class="flip-card card item bg-transparent position-relative">
                    <a href="<%#DataBinder.Eval(Container.DataItem,"LinkUrl") %> " class="stretched-link "></a>
                    <div class="flip-card-inner">
                        <div class="flip-card-front">
                            <div class="card item bg-transparent rounded-4">
                                <div class="thumb position-relative overflow-hidden">
                                    <img src="<%#DataBinder.Eval(Container.DataItem,"DisplayImageURL") %> " class="d-block  w-100">
                                </div>
                                <h2 class="position-absolute fixed-bottom text-white p-3 px-4"><%#DataBinder.Eval(Container.DataItem,"Title") %> 
                                </h2>
                            </div>
                        </div>
                        <div class="flip-card-back py-4 px-4 border">
                            <h2 class="mb-3 text-black"><%#DataBinder.Eval(Container.DataItem,"Title") %> </h2>
                            <p class="text-justify"><%#DataBinder.Eval(Container.DataItem,"Description") %>
                            </p>
                            <div class="position-absolute fixed-bottom  text-end p-3 px-4">
                                <img height="52" src="/Style Library/NewStyle/UI5/images/pnu-logo-ar.svg" alt="pnu-logo" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>