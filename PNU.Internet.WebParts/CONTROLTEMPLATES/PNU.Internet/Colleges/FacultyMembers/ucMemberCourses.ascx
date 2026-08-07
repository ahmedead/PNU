<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMemberCourses.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.FacultyMembers.ucMemberCourses" %>

<asp:Repeater ID="masterRepeater" runat="server">
    <ItemTemplate>
        <h3 class="h5 fw-bold mb-3"><%# Eval("LevelDesc") %></h3>

        <asp:Repeater ID="rptDays" runat="server" DataSource='<%# Eval("Days") %>'>
            <ItemTemplate>
                <div class="row g-4 mb-4">
                    <asp:Repeater ID="rptCourses" runat="server" DataSource='<%# Eval("Courses") %>'>
                        <ItemTemplate>
                            <div class="col-12 col-md-6 col-lg-4">
                                <article class="card nav-card h-100">
                                    <div class="card-body">
                                        <div class="icon-container">
                                            <i class="hgi hgi-stroke hgi-book-02 fs-3" aria-hidden="true"></i>
                                        </div>
                                        <div>
                                            <h3 class="card-title"><%# Eval("CourseTitle") %></h3>
                                            <p class="card-text mb-2">
                                                <%# Eval("CourseCode") %> - <%# Eval("SubjectSectionNo") %>
                                            </p>
                                            <p class="card-text mb-0 small text-body-secondary">
                                                <i class="hgi hgi-stroke hgi-calendar-03 me-1" aria-hidden="true"></i>
                                                <%# Eval("DayName") %>
                                                <i class="hgi hgi-stroke hgi-time-02 ms-2 me-1" aria-hidden="true"></i>
                                                <%# Eval("StartTime") %> - <%# Eval("EndTime") %>
                                            </p>
                                        </div>
                                    </div>
                                </article>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </ItemTemplate>
</asp:Repeater>
