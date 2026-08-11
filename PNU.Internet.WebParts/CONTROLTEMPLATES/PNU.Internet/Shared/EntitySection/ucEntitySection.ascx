<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEntitySection.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.EntitySection.ucEntitySection" %>

<asp:Panel ID="pnlSection" runat="server">
    <section id="deanship-beneficiary-tracks" class="pnu-section-anchor mb-5">
        <h2 class="mb-3">
            <asp:Literal ID="litSectionTitle" runat="server" />
        </h2>
        <div class="accordion accordion-flush" id="deanship-beneficiary-tracks-accordion">
            <asp:Repeater ID="rptTracks" runat="server" OnItemDataBound="rptTracks_ItemDataBound">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h3 class="accordion-header" id='<%# "deanship-beneficiary-tracks-accordion-heading-" + Eval("Index") %>'>
                            <button class='<%# Convert.ToBoolean(Eval("IsExpanded")) ? "accordion-button" : "accordion-button collapsed" %>'
                                    type="button"
                                    data-bs-toggle="collapse"
                                    data-bs-target='<%# "#deanship-beneficiary-tracks-accordion-collapse-" + Eval("Index") %>'
                                    aria-expanded='<%# Convert.ToBoolean(Eval("IsExpanded")) ? "true" : "false" %>'
                                    aria-controls='<%# "deanship-beneficiary-tracks-accordion-collapse-" + Eval("Index") %>'>
                                <%# Eval("Title") %>
                            </button>
                        </h3>
                        <div class='<%# Convert.ToBoolean(Eval("IsExpanded")) ? "accordion-collapse collapse show" : "accordion-collapse collapse" %>'
                             id='<%# "deanship-beneficiary-tracks-accordion-collapse-" + Eval("Index") %>'
                             aria-labelledby='<%# "deanship-beneficiary-tracks-accordion-heading-" + Eval("Index") %>'
                             data-bs-parent="#deanship-beneficiary-tracks-accordion">
                            <div class="accordion-body">
                                <asp:PlaceHolder ID="phDescription" runat="server" Visible='<%# !string.IsNullOrEmpty(Eval("Description") as string) %>'>
                                    <p><%# Eval("Description") %></p>
                                </asp:PlaceHolder>

                                <asp:PlaceHolder ID="phBullets" runat="server">
                                    <asp:Literal ID="litListOpenTag" runat="server" />
                                    <asp:Repeater ID="rptBullets" runat="server">
                                        <ItemTemplate>
                                            <li><%# Eval("Text") %></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:Literal ID="litListCloseTag" runat="server" />
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Panel>
