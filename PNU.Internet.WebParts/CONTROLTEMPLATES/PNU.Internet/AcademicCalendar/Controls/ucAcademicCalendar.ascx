<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAcademicCalendar.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AcademicCalendar.Controls.ucAcademicCalendar, $SharePoint.Project.AssemblyFullName$" %>

<main id="main-content" class="dga-main-body" tabindex="-1">
    <section id="secAcademicCalendar" runat="server" class="py-5" data-aos="fade-up" aria-labelledby="current-calendar-title">
        <div class="container">
            <asp:PlaceHolder ID="phHeading" runat="server">
                <div class="mb-4">
                    <h2 id="current-calendar-title" class="mb-3"><asp:Literal ID="ltSectionTitle" runat="server" /></h2>
                    <p class="mb-0"><asp:Literal ID="ltIntro" runat="server" /></p>
                </div>
            </asp:PlaceHolder>

            <div class="accordion accordion-flush" id="currentAcademicCalendarAccordion">
                <asp:Repeater ID="rptGroups" runat="server">
                    <ItemTemplate>
                        <div class="accordion-item">
                            <p class="accordion-header" id='<%# Eval("HeadingId") %>'>
                                <button type="button" class="accordion-button collapsed" aria-expanded="false"
                                        data-bs-toggle="collapse"
                                        data-bs-target='<%# "#" + Eval("CollapseId") %>'
                                        aria-controls='<%# Eval("CollapseId") %>'><%# Eval("Title") %></button>
                            </p>
                            <div class="accordion-collapse collapse" id='<%# Eval("CollapseId") %>'
                                 aria-labelledby='<%# Eval("HeadingId") %>' data-bs-parent="#currentAcademicCalendarAccordion">
                                <div class="accordion-body">
                                    <div class="table-responsive border border-top-0 border-bottom-0 rounded-2">
                                        <table class="table table-striped align-middle mb-0">
                                            <thead>
                                                <tr>
                                                    <th scope="col">الإجراء</th>
                                                    <th scope="col">الأسبوع</th>
                                                    <th scope="col">اليوم/الفترة</th>
                                                    <th scope="col">التاريخ الهجري</th>
                                                    <th scope="col">التاريخ الميلادي</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:Repeater ID="rptRows" runat="server" DataSource='<%# Eval("Items") %>'>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td><%# Eval("Procedure") %></td>
                                                            <td><%# Eval("Week") %></td>
                                                            <td><%# Eval("DayPeriod") %></td>
                                                            <td><%# Eval("HijriDate") %></td>
                                                            <td><%# Eval("GregorianDate") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <asp:PlaceHolder ID="phMoe" runat="server">
                <div class="mt-4">
                    <asp:HyperLink ID="lnkMoe" runat="server" CssClass="btn btn-secondary external-link"
                                   Target="_blank" rel="external noopener noreferrer" />
                </div>
            </asp:PlaceHolder>
        </div>
    </section>
</main>
