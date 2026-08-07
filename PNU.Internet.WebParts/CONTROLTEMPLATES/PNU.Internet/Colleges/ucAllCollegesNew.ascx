<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAllCollegesNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.ucAllCollegesNew" %>


<%@ Import Namespace="PNU.Internet.WebParts" %>







<style>
    img.d-block.w-100 
    {
    border-radius: 10px;
	height: 426px !important;
    
    }
</style>












<section>
    <div class="container py-5 mt-5">
        <div class="d-flex justify-content-center tabbable">
            <ul class="nav nav-tabs nav-pills  mb-5 bg-semi-light p-1 rounded-2" id="myTab" role="tablist">
                <asp:Repeater ID="masterRepeater" runat="server">
                    <ItemTemplate>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id='<%# "college" + Eval("ID") + "-tab" %>' data-bs-toggle="tab" data-bs-target='<%# "#college" + Eval("ID") + "-tab-pane" %>' type="button" role="tab" aria-controls='<%# "college" + Eval("ID") + "-tab-pane" %>' aria-selected="false" tabindex="-1">
                                <%# SPFactory.GetLocalizedTitle(Eval("COLL_CLASS_AR"), Eval("COLL_CLASS_EN")) %>
                            </button>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="tab-content" id="myTabContent">
            <asp:Repeater ID="masterRepeater1" runat="server">
                <ItemTemplate>
                    <div class="tab-pane fade" id='<%# "college" + Eval("ID") + "-tab-pane" %>' role="tabpanel" aria-labelledby='<%# "college" + Eval("ID") + "-tab" %>' tabindex="0">
                        <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4">
                            <%# SPFactory.GetLocalizedTitle(Eval("COLL_CLASS_AR"), Eval("COLL_CLASS_EN")) %>
                            <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                        </h1>

                        <div class="row">
                            <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("Colleges") %>'>
                                <ItemTemplate>
                                    <div class="col-md-6 col-xl-4 col-xxl-3">
                                        <div class="flip-card card item bg-transparent position-relative">
                                            <a href="<%# String.Format("{1}Faculties/{0}/Pages/speech.aspx", Eval("Code"),SPFactory.GetSiteURL()) %>" class="stretched-link "></a>
                                            <div class="flip-card-inner">
                                                <div class="flip-card-front">
                                                    <div class="card item bg-transparent rounded-4">
                                                        <div class="thumb position-relative overflow-hidden">
                                                            <img src="<%#DataBinder.Eval(Container.DataItem,"DisplayImage") %>" class="d-block  w-100">
                                                        </div>
                                                        <h2 class="position-absolute fixed-bottom text-white p-3 px-4">
                                                            <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                        </h2>

                                                    </div>
                                                </div>
                                                <div class="flip-card-back py-4 px-4 border">
                                                    <h2 class="mb-3 text-black">
                                                        <%# SPFactory.GetLocalizedTitle(Eval("Title"), Eval("Title_EN")) %>
                                                    </h2>
                                                    <p class="text-justify"  id='<%# "paragraph_" + Container.ItemIndex %>'>
                                                        <%# SPFactory.GetLocalizedTitle(Eval("DescriptionDisplay"), Eval("DescriptionDisplay_EN")) %>
                                                    </p>
                                                    
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                    </div>










                </ItemTemplate>
            </asp:Repeater>
        </div>
    
    </div>
</section>




<script type="text/javascript">

    function activeTab(tabno) {
        $(document).ready(function () {
            $('#myTab button[data-bs-target="#college' + tabno + '-tab-pane"]').tab('show');
        });
    }

</script>

<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        document.addEventListener('DOMContentLoaded', (event) => {
            // Select all paragraphs
            const paragraphs = document.querySelectorAll('p[id^="paragraph_"]');

            paragraphs.forEach(paragraph => {
                // Get the text content
                const text = paragraph.textContent.trim();

                // Split the text into an array of words
                const words = text.split(' ');

                // If the text has more than 20 words, truncate it
                if (words.length > 20) {
                    // Slice the first 20 words
                    const first20Words = words.slice(0, 40);

                    // Join the words back into a string
                    const truncatedText = first20Words.join(' ') + '...';

                    // Replace the original paragraph text with the truncated text
                    paragraph.textContent = truncatedText;
                }
            });
        });
    </script>