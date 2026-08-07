<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Src="~/_controltemplates/15/PNU.Internet/AboutUniversity/ucAnnualReports.ascx" TagPrefix="uc1" TagName="ucAnnualReports" %>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPNUInNumbers.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Statiscs.ucPNUInNumbers" %>




<%@ Import Namespace="PNU.Internet.WebParts" %>


<section class="faculty-member-details">
    <div class="container py-5 my-5">
        <div class="row">

            <div class="col-xl-12 mt-xl-0 mt-4">
                <div class="card border-0  rounded-4 h-100 bg-semi-light">
                    <ul class="nav nav-tabs  nav-pills  nav-fill nav-justified  mb-0 bg-semi-light p-0 rounded-4 w-100 d-flex" id="myTab" role="tablist">
                        <li class="nav-item" role="presentation" id="tabPersonal">
                            <button class="nav-link h5 py-3 active rounded-4  rounded-bottom-0 rounded-end-0 " id="home-tab" data-bs-toggle="tab" data-bs-target="#z1-tab-pane"
                                type="button" role="tab" aria-controls="z1-tab-pane" aria-selected="true">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, PNUinNumbers %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college2-tab" data-bs-toggle="tab" data-bs-target="#z2-tab-pane"
                                type="button" role="tab" aria-controls="z2-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, employee-dashboard %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college3-tab" data-bs-toggle="tab" data-bs-target="#z3-tab-pane" style="min-width: 100%; width: max-content;"
                                type="button" role="tab" aria-controls="z3-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, students-dashboard %>" />
                            </button>
                        </li>


                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college4-tab" data-bs-toggle="tab" data-bs-target="#z4-tab-pane" style="min-width: 100%; width: max-content;"
                                type="button" role="tab" aria-controls="z4-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, Research-paper %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3  rounded-0" id="college5-tab" data-bs-toggle="tab" data-bs-target="#z5-tab-pane"
                                type="button" role="tab" aria-controls="z5-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, Alumni-dashboard %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-4  rounded-bottom-0 rounded-start-0 " id="college6-tab" data-bs-toggle="tab" data-bs-target="#z6-tab-pane"
                                type="button" role="tab" aria-controls="z6-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, Alumni-dashboard-G %>" />
                            </button>
                        </li>

                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college7-tab" data-bs-toggle="tab" data-bs-target="#z7-tab-pane" style="min-width: 100%; width: max-content;"
                                type="button" role="tab" aria-controls="z7-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, patents-dashboard %>" />
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college8-tab" data-bs-toggle="tab" data-bs-target="#z8-tab-pane" style="min-width: 100%; width: max-content;"
                                type="button" role="tab" aria-controls="z8-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, prize %>" />
                            </button>
                        </li>

                        <li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college9-tab" data-bs-toggle="tab" data-bs-target="#z9-tab-pane" style="min-width: 100%; width: max-content;"
                                type="button" role="tab" aria-controls="z9-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, Sports %>" />
                            </button>
                        </li>

                        <%--<li class="nav-item" role="presentation">
                            <button class="nav-link h5 py-3 rounded-0" id="college10-tab" data-bs-toggle="tab" data-bs-target="#z10-tab-pane" style="min-width: 100%; width: max-content;"
                                type="button" role="tab" aria-controls="z10-tab-pane" aria-selected="false">
                                <asp:Literal runat="server" Text="<%$ Resources: PNUres, GraduationEmployements %>" />
                            </button>
                        </li>--%>


                        <%
    // Check the current site language
    var currentLanguage = SPContext.Current.Web.Language;
    string PNUInNumbersLINK = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiMjdhMDFlMzEtNjY5YS00YzRjLWE3M2QtMDA4MDcyMjcxNGQxIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiM2U1ODhmMTktOWQ1Yy00NWM0LTk1ZGMtNmM4Yzc3ZmEzNGJlIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";
		
	string EmployeesStatistics  = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiN2VkNjNmYTMtZTU2OC00MTQwLTkxZmQtMTJkMmZiMjBjYzY2IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiNDlmZjllYjItZGYzNy00Y2I2LTg1OGMtOWNiMzBjMTM5NjJjIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";
		
		
	
	string StudentsDashboardLINK  = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiODIxNDU4YjEtNDg0Zi00NDk4LTkwZmQtNTQzMmJjOTQ0Y2JjIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiNjVmOTZlZmYtZTA4Yi00ZDYxLTlkMTQtZDNhMDY3ZDdiOGU5IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";
	
string ResearchPaperLINK  = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiNzNiNWUyNmYtYmFhMS00OGRjLWJmOWUtNzViODJlMjNjMWFkIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiOGEwODU1MTgtM2RiNy00NjIzLWE2ZGEtNGI0MjAzMmQwMTE1IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";
	
string AlumniDashboardLINK  = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiZjA1ZDExMzMtYzg0Yy00ODE0LWExMmEtZjMyMGYzMGI3M2UzIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiMmY3ODFlMjctNDdjNy00YmExLTkyNGEtODI5MjMwMWYwYTlmIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";
	
string PrizeLINK  = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiZDM2ZTAyNmEtZGZjNC00MWVmLWJkYmItZDE2ZWJjNjE2OTA2IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiN2FiNWE3NTQtYjgxZC00ZmEyLTllZDktNTU5OTY1YzVhMTAyIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";
	
	string PatentsDashboardLINK  = (currentLanguage == 1025) // Arabic Language LCID
        ? "https://app.powerbi.com/view?r=eyJrIjoiMWY4YWQxM2QtNDU4Ny00NDgxLWJhZjAtMDRlYjc5ZDk4OTZkIiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9"
        : "https://app.powerbi.com/view?r=eyJrIjoiZDM1N2VmMTYtOWY5MC00ZWE0LWExOTYtMTI1YTg1ZWJjODE1IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9";

	
	
%>


                    </ul>
                    <div class="card-body p-0">


                        <div class="p-5 tab-content" id="myTabContent">
                            <div class="tab-pane fade show active" id="z1-tab-pane" role="tabpanel" aria-labelledby="1-tab" tabindex="0">
                                <iframe width="1024" height="804" src="<%= PNUInNumbersLINK %>" frameborder="0" allowfullscreen="true"></iframe>
                                <div>
                                    ​​​<br />
                                </div>
                                <br />
                                ​<br />

                            </div>

                            <div class="tab-pane fade" id="z2-tab-pane" role="tabpanel" aria-labelledby="2-tab" tabindex="0">


                                <iframe width="1024" height="804" src="<%= EmployeesStatistics  %>" frameborder="0" allowfullscreen="true"></iframe>
                                ​​​​<br />
                                ​<br />

                            </div>

                            <div class="tab-pane fade" id="z3-tab-pane" role="tabpanel" aria-labelledby="3-tab" tabindex="0">

                                <iframe width="1024" height="804" src="<%= StudentsDashboardLINK %>"></iframe>
                                ​​<br />
                                ​<br />


                            </div>


                            <div class="tab-pane fade" id="z4-tab-pane" role="tabpanel" aria-labelledby="4-tab" tabindex="0">

                                <iframe width="1024" height="804" src="<%= ResearchPaperLINK %>"></iframe>
                                <h3>​​<br />
                                    ​<br />
                                </h3>

                            </div>

                            <div class="tab-pane fade" id="z5-tab-pane" role="tabpanel" aria-labelledby="5-tab" tabindex="0">

                                <iframe width="1024" height="804" src="<%= AlumniDashboardLINK %>"></iframe>
                                ​​​​<br />
                                ​<br />

                            </div>
                            <div class="tab-pane fade" id="z6-tab-pane" role="tabpanel" aria-labelledby="6-tab" tabindex="0">


                                <p>​​</p>
                                <iframe width="1024" height="804" src="
								https://app.powerbi.com/view?r=eyJrIjoiOWZjOTZkMGItNWIxZi00NDFkLWE4YTEtODM5NDc2OTY0NzY5IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9" frameborder="0" allowfullscreen="true" style="word-spacing: normal;"></iframe>
                                ​<br />
                                <p>
                                </p>

                            </div>


                            <div class="tab-pane fade" id="z7-tab-pane" role="tabpanel" aria-labelledby="7-tab" tabindex="0">

                                <iframe width="1024" height="804" src="<%= PatentsDashboardLINK %>"></iframe>
                                <h2>​<br />
                                    ​<br />
                                </h2>


                            </div>

                            <div class="tab-pane fade" id="z8-tab-pane" role="tabpanel" aria-labelledby="8-tab" tabindex="0">




                                <iframe width="1024" height="804" src="<%= PrizeLINK %>" frameborder="0" allowfullscreen="true"></iframe>
                                ​​<br />
                                ​<br />

                            </div>

                            <div class="tab-pane fade" id="z9-tab-pane" role="tabpanel" aria-labelledby="9-tab" tabindex="0">




                                <p>
                                    <iframe width="1024" height="804" src="https://app.powerbi.com/view?r=eyJrIjoiZDQ1ZmM5ZjYtZDM5Zi00ZmU4LTliYTUtZmVlYWMzNDBiOTg0IiwidCI6ImU1NDJhZGVkLWFjODEtNGM0Ny04OWRkLTE5NTIzYmE4MmVlNSIsImMiOjl9" frameborder="0" allowfullscreen="true" style="word-spacing: normal;"></iframe>
                                    <span style="word-spacing: normal;">​</span>​<br />
                                </p>

                            </div>
                            
                            <%--<div class="tab-pane fade" id="z10-tab-pane" role="tabpanel" aria-labelledby="10-tab" tabindex="0">

                                <uc1:ucAnnualReports runat="server" id="ucAnnualReports" ListName="GraduationEmployementsReports"/>


                                <p>
                                    
                                </p>

                            </div>--%>

                        </div>





                    </div>
                </div>

            </div>
        </div>

    </div>
</section>

