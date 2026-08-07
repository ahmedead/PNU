<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucNewsLetter.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucNewsLetter" %>



<style>
button.btn-close {
    min-width: unset;
}
</style>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>

<asp:Panel ID="pnlData" runat="server">

    <section class="my-5 py-5">
        <div class="container">
            

            <!-- -->
            <div class="position-relative">

                <div dir="rtl" class="swiper swiper-container-14 col-lg-10 swiper-initialized swiper-horizontal swiper-pointer-events swiper-rtl swiper-backface-hidden">
                    <div class="swiper-wrapper py-4 align-items-center" id="swiper-wrapper-1b64988c6cb141ab" aria-live="off" style="height: 120px; transform: translate3d(0px, 0px, 0px); transition-duration: 0ms;">

                        <asp:Repeater ID="rep" runat="server">
                            <ItemTemplate>

                                <div class="swiper-slide d-flex justify-content-center align-items-start swiper-slide-active" role="group" aria-label="1 / 6" style="height: fit-content; width: 228px; margin-left: 60px;">
                                    <div class="card border rounded-4 h-100 w-100">
                                        <div class="card-body p-2 px-4">
                                            <div class="d-flex justify-content-between align-items-center">
                                                <div class="my-3 fs-4 fw-bold text-start d-flex flex-column justify-content-start">
                                                    <strong><%# Eval("Title") %></strong>
                                                    <span class="badge rounded-pill bg-turquoise-100 bg-opacity-75 text-turquoise-900 fs-6 w-auto d-none"
                                                        style="width: fit-content !important;">

                                                        <%# Eval("Extension") %> <%# Eval("FileSize") %>
                                                    </span>
                                                </div>
                                                <a role="button" href="<%# Eval("URL") %>" class="btn px-4 btn-outline-primary mb-2" target="_blank">
                                                    <div class="d-flex justify-content-center p">
                                                        <svg width="20" height="20">
                                                            <!-- You can add SVG content here -->
                                                        </svg>
                                                        <strong class="ms-2">
                                                            <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:PnuInternetResources, res_View%>"></asp:Literal>
                                                            
                                                        </strong>
                                                    </div>
                                                </a>

                                                <a role="button" href="<%# Eval("URL") %>" download="<%# Eval("Title") %>" class="btn px-4 btn-outline-primary mb-2 ">
                                                    <div class="d-flex justify-content-center p">
                                                        <svg width=" 20" height="20">
                                                        </svg>
                                                        <strong class="ms-2">
                                                            <asp:Literal ID="lit_download" runat="server" Text="<%$Resources:PnuInternetResources, res_Download%>"></asp:Literal>
                                                        </strong>
                                                    </div>
                                                </a>
                                            </div>


                                        </div>
                                    </div>
                                </div>

                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="mt-5">
                        <div class="swiper-pagination swiper-pagination-bullets swiper-pagination-horizontal">
                            <span class="swiper-pagination-bullet swiper-pagination-bullet-active" aria-current="true"></span>
                            <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                            <span class="swiper-pagination-bullet"></span><span class="swiper-pagination-bullet"></span>
                        </div>
                    </div>

                </div>
            </div>


        </div>

    </section>


</asp:Panel>







<%
    // Check the current site language
    var currentLanguage = SPContext.Current.Web.Language;
    string iframeUrl = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://heyzine.com/flip-book/094629f5bd.html"
        : "https://heyzine.com/flip-book/86a913e3b5.html";
		
	string iframeUrl2 = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://heyzine.com/flip-book/d3e3c3c333.html"
        : "https://heyzine.com/flip-book/88dedc4dda.html";
		
		
	string iframeUrl4 = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://heyzine.com/flip-book/8f067feb90.html"
        : "https://heyzine.com/flip-book/14922ae666.html";
		
		
		
	string Title1 = (currentLanguage == 1025) // Arabic Language LCID
        ? "الإصدار الأول"
        : "First Issue";
	string Title2 = (currentLanguage == 1025) // Arabic Language LCID
        ? "الإصدار الثاني"
        : "Second Issue";
	
	string OpenTitle = (currentLanguage == 1025) // Arabic Language LCID
        ? "عرض النشرة"
        : "Open Newsletter";
		
	string MainTitle = (currentLanguage == 1025) // Arabic Language LCID
        ? "النشرات الدورية"
        : "Newsletters";
	string Title3 = (currentLanguage == 1025) // Arabic Language LCID
        ? "الإصدار الثالث"
        : "Third Issue";
	string Title4 = (currentLanguage == 1025) // Arabic Language LCID
        ? "الإصدار الرابع"
        : "Fourth Issue";
%>




<div class="the-message p-2">
    <div class="mt-3 px-md-5 px-0">
        <div class="d-block">
            <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 h3">
                <%= MainTitle %>
            </h1>
        </div>
    </div>

    <div class="row g-3">
	<!-- Fourth Flipbook -->
        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header text-center fw-bold"><%= Title4 %></div>
                <div class="card-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="<%= iframeUrl4 %>"
                        style="border: 1px solid lightgray; width: 100%; height: 400px;"></iframe>
                    <button type="button" class="btn btn-primary mt-3" data-bs-toggle="modal"
                        data-bs-target="#flipbookModal4">
                        <%= OpenTitle %>
                    </button>
                </div>
            </div>
        </div>
        <!-- Second Flipbook -->
        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header text-center fw-bold"><%= Title3 %></div>
                <div class="card-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="https://heyzine.com/flip-book/ec549f05db.html"
                        style="border: 1px solid lightgray; width: 100%; height: 400px;"></iframe>
                    <button type="button" class="btn btn-primary mt-3" data-bs-toggle="modal"
                        data-bs-target="#flipbookModal3">
                        <%= OpenTitle %>
                    </button>
                </div>
            </div>
        </div>
        <!-- First Flipbook -->
        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header text-center fw-bold"><%= Title2 %></div>
                <div class="card-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="<%= iframeUrl2 %>"
                        style="border: 1px solid lightgray; width: 100%; height: 400px;"></iframe>
                    <button type="button" class="btn btn-primary mt-3" data-bs-toggle="modal"
                        data-bs-target="#flipbookModal1">
                        <%= OpenTitle %>
                    </button>
                </div>
            </div>
        </div>

        <!-- Second Flipbook -->
        <div class="col-md-6">
            <div class="card h-100">
                <div class="card-header text-center fw-bold"><%= Title1 %></div>
                <div class="card-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="<%= iframeUrl %>"
                        style="border: 1px solid lightgray; width: 100%; height: 400px;"></iframe>
                    <button type="button" class="btn btn-primary mt-3" data-bs-toggle="modal"
                        data-bs-target="#flipbookModal2">
                        <%= OpenTitle %>
                    </button>
                </div>
            </div>
        </div>



    </div>

    <!-- Modal for First Flipbook -->
    <div class="modal fade" id="flipbookModal1" tabindex="-1" aria-labelledby="flipbookModal1Label" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><%= Title2 %></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="<%= iframeUrl2 %>"
                        style="border: 1px solid lightgray; width: 100%; height: 800px;"></iframe>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal for Second Flipbook -->
    <div class="modal fade" id="flipbookModal2" tabindex="-1" aria-labelledby="flipbookModal2Label" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><%= Title1 %></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="<%= iframeUrl %>"
                        style="border: 1px solid lightgray; width: 100%; height: 800px;"></iframe>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="flipbookModal3" tabindex="-1" aria-labelledby="flipbookModal3Label" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><%= Title3 %></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="https://heyzine.com/flip-book/ec549f05db.html"
                        style="border: 1px solid lightgray; width: 100%; height: 800px;"></iframe>
                </div>
            </div>
        </div>
    </div>
	
	<div class="modal fade" id="flipbookModal4" tabindex="-1" aria-labelledby="flipbookModal4Label" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><%= Title4 %></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-center">
                    <iframe allowfullscreen class="fp-iframe" src="<%= iframeUrl4 %>"
                        style="border: 1px solid lightgray; width: 100%; height: 800px;"></iframe>
                </div>
            </div>
        </div>
    </div>

</div>



                        
                        
                        
                        
                        