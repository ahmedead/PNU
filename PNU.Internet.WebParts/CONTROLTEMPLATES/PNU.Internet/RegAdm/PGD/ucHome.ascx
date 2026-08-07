<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHome.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.RegAdm.PGD.ucHome" %>



<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="PNU.Internet.WebParts" %>


    <section class=" position-relative  my-5 py-5 ">
      <div class="container">
          <div class=" pt-5 mt-5">
                      <div class="d-flex justify-content-center">
                          <h1 class="title text-dark fw-bold px-2 border-start border-primary mb-5">برامج الدراسات العليا
                              <span class="px-2 position-absolute mt-1 h2 text-primary">•</span>
                          </h1>
                      </div>
                      <div class="text-center mb-5">
                          <p class="h5 text-muted card-text lh-base">
                              تُعدّ برامج الدراسات العليا في جامعة الأميرة نورة بنت عبد الرحمن ركيزة أساسية في مسيرة التميز الأكاديمي وتمكين الكفاءات الوطنية، إذ تمثل مرحلة متقدمة من التعليم العالي تُعنى بتعميق المعرفة التخصصية، وتوسيع آفاق البحث والتحليل، وإعداد كوادر مؤهلة تسهم بفاعلية في خدمة المجتمع ودعم مستهدفات التنمية الوطنية ورؤية المملكة 2030. وتنطلق هذه البرامج من معايير أكاديمية رصينة وبيئة تعليمية محفزة تعزز التكامل بين الجوانب النظرية والتطبيقية، بما يضمن جودة المخرجات ورفع كفاءة الأداء المهني والعلمي.
كما تتميز برامج الدراسات العليا في الجامعة بإقامة شراكات استراتيجية فاعلة على المستويين المحلي والدولي، تسهم في تعزيز جودة البرامج الأكاديمية، وتبادل الخبرات والمعارف، ودعم البحث والابتكار. وتوفر هذه الشراكات فرصًا نوعية للتعاون العلمي، والتدريب، والمشروعات البحثية المشتركة، بما يعزز من جاهزية الخريجين والخريجات.</p>


                      </div>

                      
                  </div>
        <div class="position-relative my-5">
          <div class="row gy-4 mb-5">

              <asp:Repeater ID="rptServices" runat="server">
                  <ItemTemplate>
                      <div class="col-lg-6">
                          <%--<a href="ProgramsDetails.aspx?CatID=<%# Eval("ID") %>">
                              <div class="card p-0 position-relative rounded-3">
                                  <img src='<%# Eval("PublishingRollupImage") %>' class="img-fluid  w-100" alt="...">
                                  <div
                                      class="position-absolute w-100 bottom-0 p-4 bg-primary bg-opacity-75 text-center rounded-3 rounded-top-0 rounded-bottom">
                                      <p class="fs-3  mb-0 lh-base text-white"><%# Eval("Title") %> </p>
                                      <p class="text-justify text-white"> <%# Eval("Desc") %></p>
                                  </div>
                              </div>
                          </a>--%>

                          <a href='<%# string.IsNullOrEmpty(Eval("URL") as string) 
             ? "ProgramsDetails.aspx?CatID=" + Eval("ID") 
             : Eval("URL") %>'>
    <div class="card p-0 position-relative rounded-3">
        <img src='<%# Eval("PublishingRollupImage") %>' class="img-fluid w-100" alt="...">
        <div class="position-absolute w-100 bottom-0 p-4 bg-primary bg-opacity-75 text-center rounded-3 rounded-top-0 rounded-bottom">
            <p class="fs-3 mb-0 lh-base text-white"><%# Eval("Title") %></p>
            <p class="text-justify text-white"><%# Eval("Desc") %></p>
        </div>
    </div>
</a>
                      </div>

                  </ItemTemplate>
              </asp:Repeater>
          </div>
        </div>
      </div>
    </section>

<h1 class="title text-dark fw-bold px-2 border-start border-primary mb-4 mt-md-0 mt-5 me-md-4 flex-shrink-0" style="font-family: &quot; ibm plex sans arabic&quot; text-align: justify; border-color: #1b8354 !important;">
    <span style="font-weight: bolder;">
        <span class="ms-rteFontSize-4" style="font-weight: bolder;">يمكنك التقديم عبر بوابة القبول الإلكترونية :</span>
        <span class="ms-rteFontSize-4" style="text-decoration: underline;">&ZeroWidthSpace;</span>
        <a href="https://banssb.pnu.edu.sa/PROD_ar/bwykolad.P_ShowQuesAns?applno=MTQ0MzEwMDAwMDA3Nzgx" style="text-decoration: underline;">
            <span class="ms-rteFontSize-4" style="text-decoration: underline;">اضغط هنا</span>
        </a>
        <span class="ms-rteFontSize-4" style="text-decoration: underline;">&ZeroWidthSpace;</span>

    </span>

</h1>
