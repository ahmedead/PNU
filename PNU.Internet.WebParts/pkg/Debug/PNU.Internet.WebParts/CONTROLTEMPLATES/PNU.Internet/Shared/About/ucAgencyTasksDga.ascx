<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAgencyTasksDga.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Shared.About.ucAgencyTasksDga" %>

<section id="secTasks" runat="server" class="pb-5" data-aos="fade-up" aria-labelledby="agency-main-tasks-title">
    <div class="container">
        <div>
            <h2 id="agency-main-tasks-title" class="mb-4">
                <asp:Literal ID="litTasksHeading" runat="server" /></h2>
        </div>
        <div class="accordion accordion-flush" id="agency-main-tasks-accordion">
            <asp:Repeater ID="rptTaskGroups" runat="server" OnItemDataBound="rptTaskGroups_ItemDataBound">
                <ItemTemplate>
                    <div class="accordion-item">
                        <h4 class="accordion-header" id='<%# "agency-main-tasks-heading-" + Eval("Index") %>'>
                            <button type="button" data-bs-toggle="collapse"
                                data-bs-target='<%# "#agency-main-tasks-collapse-" + Eval("Index") %>'
                                aria-controls='<%# "agency-main-tasks-collapse-" + Eval("Index") %>'
                                aria-expanded='<%# Eval("IsFirst") %>'
                                class='<%# (bool)Eval("IsFirst") ? "accordion-button" : "accordion-button collapsed" %>'>
                                <%# Eval("Title") %>
                            </button>
                        </h4>
                        <div id='<%# "agency-main-tasks-collapse-" + Eval("Index") %>'
                            aria-labelledby='<%# "agency-main-tasks-heading-" + Eval("Index") %>'
                            data-bs-parent="#agency-main-tasks-accordion"
                            class='<%# (bool)Eval("IsFirst") ? "accordion-collapse collapse show" : "accordion-collapse collapse" %>'>
                            <div class="accordion-body">
                                <ul class="mb-0">
                                    <asp:Repeater ID="rptTaskPoints" runat="server">
                                        <ItemTemplate>
                                            <li><%# Eval("Text") %></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
