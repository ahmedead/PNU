<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Statiscs/ucPNUInNumbers.ascx" TagPrefix="uc1" TagName="ucPNUInNumbers" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/Forms/ucOpenDataRequest.ascx" TagPrefix="uc1" TagName="ucOpenDataRequest" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/AboutUniversity/ucAnnualReports.ascx" TagPrefix="uc1" TagName="ucAnnualReports" %>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPNUInNumbersTabs.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Statiscs.ucPNUInNumbersTabs" %>





<asp:Panel ID="pnlAll" runat="server">
    <section class="my-5 py-5">
        <div class="container">

            <div class="d-flex justify-content-center align-items-center flex-wrap flex-lg-nowrap mb-5">

                <div class="col-12 col-lg-auto tabbable">
                    <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="news1-tab" data-bs-toggle="tab"
                                data-bs-target="#news1-tab-pane" type="button" role="tab"
                                aria-controls="news1-tab-pane" aria-selected="false" tabindex="0">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, PNUinNumbers %>" />
                            </button>
                        </li>

                        <li class="nav-item" role="presentation" style="display:none">
                            <button class="nav-link" id="college22tab" data-bs-toggle="tab"
                                data-bs-target="#news2-tab-pane" type="button" role="tab" runat="server"
                                aria-controls="news2-tab-pane" aria-selected="false" tabindex="1">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, OpenDataTitle %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="news3-tab" data-bs-toggle="tab"
                                data-bs-target="#news3-tab-pane" type="button" role="tab"
                                aria-controls="news3-tab-pane" aria-selected="false" tabindex="2">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, Survey %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="news4-tab" data-bs-toggle="tab"
                                data-bs-target="#news4-tab-pane" type="button" role="tab"
                                aria-controls="news4-tab-pane" aria-selected="false" tabindex="2">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, AnnualReports %>" />
                            </button>
                        </li>

                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="news5-tab" data-bs-toggle="tab"
                                data-bs-target="#news5-tab-pane" type="button" role="tab"
                                aria-controls="news4-tab-pane" aria-selected="false" tabindex="3">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, GraduationEmployements %>" />
                            </button>
                        </li>
                    </ul>
                </div>
            </div>

            <div class="tab-content">

                <div class="tab-pane show active" id="news1-tab-pane" role="tabpanel" aria-labelledby="news1-tab" tabindex="0">
                    <div class="the-message p-2">
                        <div class="mt-3 px-md-5 px-0">



                            <uc1:ucPNUInNumbers runat="server" id="ucPNUInNumbers" />


                        </div>
                    </div>

                </div>


                <div class="tab-pane hide" id="news2-tab-pane" role="tabpanel" aria-labelledby="news2-tab" tabindex="1">
                    <uc1:ucOpenDataRequest runat="server" id="ucOpenDataRequest" />
                </div>

                <div class="tab-pane hide" id="news3-tab-pane" role="tabpanel" aria-labelledby="news3-tab" tabindex="2">
                    <div class="the-message p-2">
                        <div class="mt-3 px-md-5 px-0">


                            <iframe width="940px" height="900px"
                                src="https://forms.office.com/pages/responsepage.aspx?id=7a1C5YGsR0yJ3RlSO6gu5Wb0eYsRmTlLvQqKd3TJUT1URThYSDhTN1RUSDlWSTQ2SlNNODY0TEIxQS4u&amp;embed=true"
                                frameborder="0" allowfullscreen="" style="border: none; max-width: 100%; max-height: 100vh;"></iframe>



                        </div>
                    </div>


                </div>
                <div class="tab-pane hide" id="news4-tab-pane" role="tabpanel" aria-labelledby="news4-tab" tabindex="2">
                    <div class="the-message p-2">
                        <div class="mt-3 px-md-5 px-0">


                            <uc1:ucAnnualReports runat="server" id="ucAnnualReports" />


                        </div>
                    </div>


                </div>

                <div class="tab-pane hide" id="news5-tab-pane" role="tabpanel" aria-labelledby="news5-tab" tabindex="3">
                    <div class="the-message p-2">
                        <div class="mt-3 px-md-5 px-0">

                            <uc1:ucAnnualReports runat="server" id="ucAnnualReports1" ListName="GraduationEmployementsReports"/>
                            

                        </div>
                    </div>


                </div>

            </div>

        </div>
    </section>
</asp:Panel>


