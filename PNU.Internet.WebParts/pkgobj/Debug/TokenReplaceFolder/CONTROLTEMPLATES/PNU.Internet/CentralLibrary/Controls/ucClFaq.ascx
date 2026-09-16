<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClFaq.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClFaq" %>

<section class="py-5" aria-labelledby="central-library-faq-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <h2 class="h3 mb-4" id="central-library-faq-title"><%= HeadingText %></h2>
        <% } %>
        <div class="accordion accordion-flush" id="centralLibraryFaqAccordion">
            <asp:Repeater ID="rptFaq" runat="server">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h3 class="accordion-header" id='<%# "centralLibraryFaq" + Container.ItemIndex + "Heading" %>'>
                            <button class='<%# Container.ItemIndex == 0 ? "accordion-button" : "accordion-button collapsed" %>' type="button" data-bs-toggle="collapse"
                                data-bs-target='<%# "#centralLibraryFaq" + Container.ItemIndex + "Collapse" %>'
                                aria-expanded='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                                aria-controls='<%# "centralLibraryFaq" + Container.ItemIndex + "Collapse" %>'>
                                <%# Eval("Title") %>
                            </button>
                        </h3>
                        <div class='<%# Container.ItemIndex == 0 ? "accordion-collapse collapse show" : "accordion-collapse collapse" %>'
                            id='<%# "centralLibraryFaq" + Container.ItemIndex + "Collapse" %>'
                            aria-labelledby='<%# "centralLibraryFaq" + Container.ItemIndex + "Heading" %>'
                            data-bs-parent="#centralLibraryFaqAccordion">
                            <div class="accordion-body"><%# Eval("Description") %></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
