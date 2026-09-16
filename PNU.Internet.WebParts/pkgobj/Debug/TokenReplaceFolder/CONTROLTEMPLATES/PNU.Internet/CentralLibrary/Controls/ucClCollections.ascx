<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucClCollections.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.CentralLibrary.Controls.ucClCollections" %>

<section class="py-5 numbers-section" data-aos="fade-up" aria-labelledby="central-library-collections-title">
    <div class="container">
        <% if (HeadingVisible) { %>
            <div class="mb-4"><h2 class="mb-0" id="central-library-collections-title"><%= HeadingText %></h2></div>
        <% } %>
        <div class="card overflow-hidden" data-library-collections-unit>
            <div class="row row-cols-1 row-cols-md-3 g-0">
                <asp:Repeater ID="rptCollections" runat="server">
                    <ItemTemplate>
                        <div class="col position-relative">
                            <div class="list-group-item border-0 rounded-0 p-4 h-100" data-library-collection-item>
                                <div class="d-flex gap-3 align-items-center">
                                    <span class="bg-primary-25 rounded-2 px-3 py-2 text-center flex-shrink-0">
                                        <i class='<%# "hgi hgi-stroke " + Eval("IconClass") + " fs-4 text-primary" %>' aria-hidden="true"></i>
                                    </span>
                                    <div>
                                        <strong class="text-dark d-block"><%# Eval("Title") %></strong>
                                        <small class="text-muted d-block"><%= LocationLabel %>: <span dir="ltr"><%# Eval("SubTitle") %></span></small>
                                        <small class="text-muted d-block mt-1"><%# Eval("Description") %></small>
                                    </div>
                                </div>
                            </div>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-md-none" aria-hidden="true"></span>
                            <span class="position-absolute start-0 end-0 bottom-0 border-bottom d-none d-md-block" aria-hidden="true"></span>
                            <span class="position-absolute top-0 bottom-0 start-0 border-start d-none d-md-block" aria-hidden="true"></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</section>
