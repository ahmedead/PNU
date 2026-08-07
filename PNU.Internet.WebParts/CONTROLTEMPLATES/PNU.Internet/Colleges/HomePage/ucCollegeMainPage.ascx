<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/ucCollegePrograms.ascx" TagPrefix="uc1" TagName="ucCollegePrograms" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/ucCollegeServices.ascx" TagPrefix="uc1" TagName="ucCollegeServices" %>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCollegeMainPage.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.HomePage.ucCollegeMainPage" %>


<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

        <asp:Repeater ID="rptMainData" runat="server">
            <ItemTemplate>

				
				<div class="col-12 col-md-6 col-lg-8">
                                            <h2 id="reasons-section-title" class="mb-4">
											<asp:Literal runat="server" Text="<%$ Resources: PNUres, CollegeOverviewPrefix %>" />
                        <%# SPFactory.GetFacultyTitleLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
												</h2>
                                            <p class="mb-4 text-justify">
											<%# SPFactory.GetLocalizedTitle(Eval("Description"), Eval("Description_EN")) %>
                                            </p>
                                        </div>
										
										

            </ItemTemplate>
        </asp:Repeater>


        






