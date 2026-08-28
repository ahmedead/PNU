<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUniversityPresidentOffice.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity.UniversityPresidentOffice.ucUniversityPresidentOffice, $SharePoint.Project.AssemblyFullName$" %>

<%@ Import Namespace="Portal.Main.Helper" %>


    <style>
        @media (min-width: 992px) {
            .president-message-composition {
                position: relative;
            }

            .president-message-card {
                width: 79.166667%;
                margin-inline-start: 0;
                margin-inline-end: auto;
            }

            .president-message-copy {
                padding-inline-end: calc(26.315789% - 1.5rem) !important;
            }

            .president-message-portrait {
                position: absolute;
                z-index: 1;
                inset-inline-end: 0;
                top: 50%;
                width: 41.666667%;
                transform: translateY(-50%);
            }
        }

        @media (min-width: 1200px) {
            .president-message-copy {
                padding-inline-end: calc(26.315789% - 3rem) !important;
            }
        }
    </style>
	
    <main id="main-content" class="dga-main-body" tabindex="-1">
        <div class="container py-5">
            <article class="pnu-detail-article">
                <section id="president-message" aria-labelledby="president-message-title" data-aos="fade-up">
                    <div class="president-message-composition d-flex flex-column d-lg-block">
                        <div class="card president-message-card order-2 order-lg-0 border-0 bg-primary-25 p-4">
                            <div class="president-message-copy d-flex h-100 flex-column gap-3 p-4 p-xl-5">
                                <span class="icon-container bg-white">
                                    <i class="hgi hgi-stroke hgi-quote-down fs-4" aria-hidden="true"></i>
                                </span>
                                 <h2 id="president-message-title" class="mb-2">
                                <asp:Literal ID="litMessageTitle" runat="server" />
                            </h2>
                                <asp:Repeater ID="rptMessageParagraphs" runat="server">
                                <ItemTemplate>
                                    <p class="mb-0 text-justify"><%# Container.DataItem %></p>
                                </ItemTemplate>
                            </asp:Repeater>
                                <div class="president-message-signature mt-2">
                                <p class="fw-semibold mb-0">
                                    <asp:Literal ID="litSignatureName" runat="server" /></p>
                                <p class="card-text mb-0">
                                    <asp:Literal ID="litSignatureTitle" runat="server" /></p>
                            </div>
                            </div>
                        </div>

                                
                        <figure class="president-message-portrait order-1 order-lg-0 mb-4 mb-lg-0 mt-0 text-center text-lg-end">
                        
                            <img src="https://pnu.edu.sa/ar/AboutUniversity/PublishingImages/UPOImages/pnu-president-photo.webp" 
						        alt="الدكتورة فوزية بنت سليمان العمرو، رئيسة جامعة الأميرة نورة بنت عبدالرحمن المُكلَّفة" 
						        width="1066" height="1600" class="img-fluid w-75 rounded-3 shadow-sm" loading="eager"
                                fetchpriority="high" decoding="async">
       
                        </figure>


                    </div>
                </section>
<%-- ======================== Additional Sections (if any) ======================== --%>
            <asp:Repeater ID="rptAdditionalSections" runat="server">
                <ItemTemplate>
                    <section id='<%# "president-section-" + Eval("Id") %>' aria-labelledby='<%# "president-section-title-" + Eval("Id") %>' data-aos="fade-up" class="mt-5">
                        <h2 id='<%# "president-section-title-" + Eval("Id") %>' class="mb-3"><%# Eval("Title") %></h2>
                        <asp:Repeater ID="rptExtraParagraphs" runat="server" DataSource='<%# Eval("Paragraphs") %>'>
                            <ItemTemplate>
                                <p class="mb-2 text-justify"><%# Container.DataItem %></p>
                            </ItemTemplate>
                        </asp:Repeater>
                    </section>
                </ItemTemplate>
            </asp:Repeater>



            <%-- ======================== Contact Section ======================== --%>
            <section id="president-contact" aria-labelledby="president-contact-title" data-aos="fade-up">
                <h2 id="president-contact-title" class="mt-5 mb-3">
                    <asp:Literal ID="litContactHeading" runat="server" />
                </h2>
                <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                    <table class="table table-striped align-middle mb-0">
                        <thead>
                            <tr>
                                <th scope="col">
                                    <asp:Literal ID="litChannelHeader" runat="server" /></th>
                                <th scope="col">
                                    <asp:Literal ID="litContactDataHeader" runat="server" /></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptContacts" runat="server" OnItemDataBound="rptContacts_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Title") %></td>
                                        
										<td <%# (string)Eval("ContactType") == "phone" ? (PortalHelper.IsArabic ? "dir=\"rtl\"" : "dir=\"ltr\"") : "" %>>
                                            <asp:HyperLink ID="lnkContact" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </section>

                
            </article>
        </div>
    </main>