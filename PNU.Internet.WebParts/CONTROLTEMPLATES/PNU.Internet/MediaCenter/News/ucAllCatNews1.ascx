<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllCatNews11.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News.ucAllCatNews1" %>



<%@ Import Namespace="PNU.Internet.WebParts" %>

 <div class="row">
                                <asp:Repeater ID="rptNews" runat="server" >
                                    <ItemTemplate>
                                        <div class="col-md-6 col-lg-4 mb-4">
                                            <div class="card border rounded-4 h-100">
                                                <div class="card-body p-2">
                                                    <div class="position-absolute top-0 start-0 m-4">
                                                        <span class="badge rounded-pill bg-tertiary text-white fs-6"><%# Eval("MediaTypes") %>
                                                        </span>
                                                    </div>
                                                    <img src='<%# Eval("AttachmentURL") %>' class="object-fit-cover rounded-3 w-100" height="250"
                                                       >
                                                    <div class="my-2 fs-4 fw-bold">
                                                        <a href='<%#  Eval("DetailsURL")  %>' class="stretched-link text-decoration-none text-body strong">
                                                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %> 
                                                        </a>
                                                    </div>
                                                    <div class="d-flex mb-3">
                                                        <svg width="19" height="20">
                                                            <use xlink:href="#calendar" />
                                                        </svg>
                                                        <span class="ms-2 text-muted"><%# Eval("MediaDate") %></span>
                                                    </div>
                                                    
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
           


<div>
                    <p class="mb-0 ">
                        <a id="AllMCNews" runat="server" class="btn btn-link fw-bolder text-primary text-decoration-none">
                            <asp:Literal runat="server" Text="<%$ Resources: PNUres, res_AllNews %>" />

                        </a>
                    </p>
                </div>