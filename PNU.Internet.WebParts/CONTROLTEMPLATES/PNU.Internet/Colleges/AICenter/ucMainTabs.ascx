<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAboutCenter.ascx" TagPrefix="uc1" TagName="ucAboutCenter" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucCenterMembers.ascx" TagPrefix="uc1" TagName="ucCenterMembers" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucResearchPaths.ascx" TagPrefix="uc1" TagName="ucResearchPaths" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucNewsLetterFromList.ascx" TagPrefix="uc1" TagName="ucNewsLetter" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAIContactUS.ascx" TagPrefix="uc1" TagName="ucAIContactUS" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAICalenderNew.ascx" TagPrefix="uc2" TagName="ucAICalender" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Colleges/AICenter/ucAICenterNews.ascx" TagPrefix="uc1" TagName="ucAICenterNews" %>




<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMainTabs.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucMainTabs" %>




<asp:Panel ID="pnlAll" runat="server">
    <section class="my-5 py-5">
        <div class="container">

            <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">

                <div class="col-12 col-lg-auto tabbable">
                    <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="news1-tab" data-bs-toggle="tab"
                                data-bs-target="#news1-tab-pane" type="button" role="tab"
                                aria-controls="news1-tab-pane" aria-selected="false" tabindex="0">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, AboutCenter %>" />
                            </button>
                        </li>

                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="college22tab" data-bs-toggle="tab"
                                data-bs-target="#news2-tab-pane" type="button" role="tab" runat="server"  onserverclick="college2tab_Click"
                                aria-controls="news2-tab-pane" aria-selected="false" tabindex="1">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, ResearchPaths %>" />
                            </button>
                        </li>
						<li class="nav-item" role="presentation">
                            <button class="nav-link" id="new7-tab" data-bs-toggle="tab"
                                data-bs-target="#news7-tab-pane" type="button" role="tab"
                                aria-controls="news7-tab-pane" aria-selected="true" tabindex="3">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIRecentNews %>" />
                            </button>
                        </li>

                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="news3-tab" data-bs-toggle="tab"
                                data-bs-target="#news3-tab-pane" type="button" role="tab"
                                aria-controls="news3-tab-pane" aria-selected="false" tabindex="2">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, NewsArticles %>" />
                            </button>
                        </li>


                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="new4-tab" data-bs-toggle="tab"
                                data-bs-target="#news4-tab-pane" type="button" role="tab"
                                aria-controls="news4-tab-pane" aria-selected="true" tabindex="3">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, AICalender %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="new5-tab" data-bs-toggle="tab"
                                data-bs-target="#news5-tab-pane" type="button" role="tab"
                                aria-controls="news5-tab-pane" aria-selected="true" tabindex="3">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIMembers %>" />
                            </button>
                        </li>
                        
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="new6-tab" data-bs-toggle="tab"
                                data-bs-target="#news6-tab-pane" type="button" role="tab"
                                aria-controls="news6-tab-pane" aria-selected="true" tabindex="3">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, AIContactUS %>" />
                            </button>
                        </li>

                    </ul>
                </div>
            </div>

            <div class="tab-content">

                <div class="tab-pane show hide" id="news1-tab-pane" role="tabpanel" aria-labelledby="news1-tab" tabindex="0">
                    <uc1:ucAboutCenter runat="server" id="ucAboutCenter" />
                </div>


                <div class="tab-pane hide" id="news2-tab-pane" role="tabpanel" aria-labelledby="news2-tab" tabindex="1">
                    <uc1:ucResearchPaths runat="server" id="ucResearchPaths" />
                </div>

                <div class="tab-pane active" id="news3-tab-pane" role="tabpanel" aria-labelledby="news3-tab" tabindex="2">
					<uc1:ucNewsLetter runat="server" id="ucNewsLetter" ListName="Newsletters"/>
						
                </div>

                <div class="tab-pane  hide" id="news4-tab-pane" role="tabpanel" aria-labelledby="news4-tab" tabindex="3">
                    <uc2:ucAICalender runat="server" id="ucAICalender" />
                   
                </div>

                <div class="tab-pane  hide" id="news5-tab-pane" role="tabpanel" aria-labelledby="news5-tab" tabindex="4">
                    <uc1:ucCenterMembers runat="server" id="ucCenterMembers" />
                </div>
                
                <div class="tab-pane  hide" id="news7-tab-pane" role="tabpanel" aria-labelledby="news7-tab" tabindex="5">
                    <uc1:ucAICenterNews runat="server" id="ucAICenterNews" />
                </div>

                <div class="tab-pane  hide" id="news6-tab-pane" role="tabpanel" aria-labelledby="news6-tab" tabindex="6">
                    <uc1:ucAIContactUS runat="server" id="ucAIContactUS" />
                </div>


            </div>

        </div>
    </section>
</asp:Panel>


