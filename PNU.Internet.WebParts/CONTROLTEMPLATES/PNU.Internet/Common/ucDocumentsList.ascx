<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucDocumentsList.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Common.ucDocumentsList" %>


<asp:Repeater ID="rptFolders" runat="server" OnItemDataBound="rptFolders_ItemDataBound">
    <ItemTemplate>
        <section class="py-5 mb-5">
            <div class="container">

                <!-- Folder title (e.g., الأدلة) -->
                <div class="d-flex align-items-start mt-4">
                    <h1
                        class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-4 me-md-4 flex-shrink-0 h3">
                        <%# Eval("CategoryName") %>
                    </h1>
                </div>

                <!-- Files in this folder -->
                <div class="row g-3">
                    <asp:Repeater ID="rptFiles" runat="server">
                        <ItemTemplate>
                            <div class="col-xl-4 col-lg-5">
                                <div class="card mb-4 p-3 news-item">
                                    <div class="row g-0">
                                        <div class="col-md-12">
                                            <!-- Static image or dynamic if you want -->
                                            <img src="<%# Eval("ImageUrl") %>" class="img-fluid rounded" alt="...">
                                            <div class="card-body p-2">

                                                <!-- File size -->
                                                <p class="rounded-pill bg-primary bg-opacity-75 text-white mt-2 w-auto px-2 py-1"
                                                   style="width: fit-content !important;">
                                                    <%# Eval("SizeMB") %>
                                                </p>

                                                <!-- File name -->
                                                <h5 class="card-title mb-4 fw-bold">
                                                    <%# Eval("FileName") %>
                                                </h5>

                                                <!-- Download link -->
                                                <a role="button"
                                                   href="<%# Eval("DownloadUrl") %>"
                                                   download="<%# Eval("FileName") %>"
                                                   class="btn w-100 px-4 btn-outline-primary">
                                                    <div class="d-flex justify-content-center">
                                                        <svg width="20" height="20">
                                                            <use xlink:href="#download"></use>
                                                        </svg>
                                                        <strong class="ms-2">تحميل</strong>
                                                    </div>
                                                </a>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </div>
        </section>
    </ItemTemplate>
</asp:Repeater>
