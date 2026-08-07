<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAICalender.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAICalender" %>



 <script src='https://cdn.jsdelivr.net/npm/fullcalendar/index.global.min.js'></script>

 <section class="full-calendar">
     <div class="container py-5 my-5">
         <div class="row justify-content-center">
             <div class="col-12 col-lg-auto tabbable">
                 <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab1" role="tablist">
                     <li class="nav-item" role="presentation">
                         <button class="nav-link active" id="Calender1-tab" data-bs-toggle="tab"
                             data-bs-target="#Calender1-tab-pane" type="button" role="tab"
                             aria-controls="Calender1-tab-pane" aria-selected="false" tabindex="0">
                             <span class="month-name">سبتمبر 2024</span>
                         </button>
                     </li>

                     <li class="nav-item" role="presentation">
                         <button class="nav-link" id="Calender2-tab" data-bs-toggle="tab"
                             data-bs-target="#Calender2-tab-pane" type="button" role="tab"
                             aria-controls="Calender2-tab-pane" aria-selected="false" tabindex="1">
                             <span class="month-name">أكتوبر 2024</span>
                         </button>
                     </li>
                     <li class="nav-item" role="presentation">
                         <button class="nav-link" id="Calender3-tab" data-bs-toggle="tab"
                             data-bs-target="#Calender3-tab-pane" type="button" role="tab"
                             aria-controls="Calender3-tab-pane" aria-selected="false" tabindex="2">
                             <span class="month-name">نوفمبر 2024</span>
                         </button>
                     </li>

                     <li class="nav-item" role="presentation">
                         <button class="nav-link" id="new4-tab" data-bs-toggle="tab"
                             data-bs-target="#Calender4-tab-pane" type="button" role="tab"
                             aria-controls="Calender4-tab-pane" aria-selected="true" tabindex="3">
                             <span class="month-name">ديسمبر 2024</span>
                         </button>
                     </li>
                 </ul>

                 <script>
                     // Function to update the month names based on the detected language
                     function updateMonthNames(language) {
                         const monthNames = {
                             ar: ['سبتمبر 2024', 'أكتوبر 2024', 'نوفمبر 2024', 'ديسمبر 2024'],
                             en: ['September 2024', 'October 2024', 'November 2024', 'December 2024']
                         };

                         const selectedMonths = monthNames[language] || monthNames['en'];
                         const monthElements = document.querySelectorAll('.month-name');

                         monthElements.forEach((element, index) => {
                             element.textContent = selectedMonths[index];
                         });
                     }

                     // Example: Detect language (you can customize this logic)
                     const userLanguage = navigator.language.startsWith('ar') ? 'ar' : 'en';

                     // Update month names based on detected language
                     updateMonthNames(userLanguage);
                 </script>

             </div>

             <div class="tab-content">

                 <div class="tab-pane show active" id="Calender1-tab-pane" role="tabpanel" aria-labelledby="Calender1-tab" tabindex="0">

                     <img src="https://pnu.edu.sa/Style%20Library/Images/AICalender/1.jpeg" />
                 </div>


                 <div class="tab-pane hide" id="Calender2-tab-pane" role="tabpanel" aria-labelledby="Calender2-tab" tabindex="1">
                     <img src="https://pnu.edu.sa/Style%20Library/Images/AICalender/2.jpeg" />
                 </div>

                 <div class="tab-pane hide" id="Calender3-tab-pane" role="tabpanel" aria-labelledby="Calender3-tab" tabindex="2">
                     <img src="https://pnu.edu.sa/Style%20Library/Images/AICalender/3.jpeg" />
                 </div>

                 <div class="tab-pane  hide" id="Calender4-tab-pane" role="tabpanel" aria-labelledby="Calender4-tab" tabindex="3">
                     <img src="https://pnu.edu.sa/Style%20Library/Images/AICalender/4.jpeg" />
                 </div>




             </div>


         </div>

     </div>
 </section>



<%--<script src='https://cdn.jsdelivr.net/npm/fullcalendar/index.global.min.js'></script>

 <div class="col-12 col-lg-auto tabbable">
                    <ul class="nav nav-tabs nav-pills bg-semi-light p-1 rounded-2" id="myTab1" role="tablist">
                        <li class="nav-item" role="presentation">
                            <button class="nav-link active" id="Calender1-tab" data-bs-toggle="tab"
                                data-bs-target="#Calender1-tab-pane" type="button" role="tab"
                                aria-controls="Calender1-tab-pane" aria-selected="false" tabindex="0">
                                 سبتمبر 2024
                            </button>
                        </li>

                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="Calender2-tab" data-bs-toggle="tab"
                                data-bs-target="#Calender2-tab-pane" type="button" role="tab" ]
                                aria-controls="Calender2-tab-pane" aria-selected="false" tabindex="1">
                                أكتوبر 2024
                            </button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="Calender3-tab" data-bs-toggle="tab"
                                data-bs-target="#Calender3-tab-pane" type="button" role="tab"
                                aria-controls="Calender3-tab-pane" aria-selected="false" tabindex="2">
                                نوفمبر 2024
                            </button>
                        </li>


                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="new4-tab" data-bs-toggle="tab"
                                data-bs-target="#Calender4-tab-pane" type="button" role="tab"
                                aria-controls="Calender4-tab-pane" aria-selected="true" tabindex="3">
                                ديسمبر 2024
                            </button>
                        </li>
                        

                    </ul>
                </div>

            <div class="tab-content">

                <div class="tab-pane show active" id="Calender1-tab-pane" role="tabpanel" aria-labelledby="Calender1-tab" tabindex="0">
                    
                    <div class="col-lg-8">
                        <div id="calendarZ"></div>
                    </div>
                    <div class="col-lg-4">
                        <div class="d-flex flex-column">
                            <div class="d-lg-block d-none">
                                <h4 class="fs-4">روزنامة مركز الذكاء الاصطناعي</h4>
                                <img src="https://placehold.co/300" alt="Placeholder Image" class="rounded-4"
                                    width="100%">
                            </div>
                            <div>
                                <div class="p-2 px-3 my-2 border rounded-4">
                                    <ul class="list-unstyled mb-0">
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #a8c3c3;"></span>
                                            <span>مسار التتبع والمتغيرس</span>
                                        </li>
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #3b4cc0;"></span>
                                            <span>مسار أمن المعلومات</span>
                                        </li>
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #9f74c1;"></span>
                                            <span>مسار الصحة والمعلوماتية الحيوية</span>
                                        </li>
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #56b0b6;"></span>
                                            <span>مسار نظرية التعلم الحوسبي</span>
                                        </li>
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #7a5a9c;"></span>
                                            <span>مسار إنترنت الأشياء</span>
                                        </li>
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #d15555;"></span>
                                            <span>مسار رؤية الحاسب</span>
                                        </li>
                                        <li class="d-flex align-items-center my-2">
                                            <span class="circle" style="background-color: #f2c04e;"></span>
                                            <span>مسار الذكاء الاصطناعي التوليدي ومعالجة اللغات الطبيعية</span>
                                        </li>
                                    </ul>
                                </div>

                            </div>
                        </div>



                    </div>
                

       
                </div>


                <div class="tab-pane hide" id="Calender2-tab-pane" role="tabpanel" aria-labelledby="Calender2-tab" tabindex="1">
                    
                </div>

                <div class="tab-pane hide" id="Calender3-tab-pane" role="tabpanel" aria-labelledby="Calender3-tab" tabindex="2">
                    
                </div>

                <div class="tab-pane  hide" id="Calender4-tab-pane" role="tabpanel" aria-labelledby="Calender4-tab" tabindex="3">
                    
                </div>

                


            </div>

 <style>
            .fc a {
                text-decoration: unset !important;
            }

            .fc .fc-daygrid-day-top {
                flex-direction: row;
            }
            .fc-h-event .fc-event-title{
                text-wrap: wrap;
            }

            .circle {
                width: 15px;
                height: 15px;
                border-radius: 50%;
                display: inline-block;
                margin-left: 10px;
            }

            #calendarZ .fc-scrollgrid {
                border-radius: 0.75rem;
                overflow: hidden;
            }



            #calendarZ .fc-scrollgrid-section.fc-scrollgrid-section-body td[role='presentation'] {
                border-radius: 0 0 0.75rem 0.75rem;

            }

 

            #calendarZ th[role='presentation']:first-of-type {
                border-radius: 0.75rem 0 0 0;
            }

            #calendarZ th[role='presentation']:last-of-type {
                border-radius: 0 0.75rem 0 0;
            }
        </style>
       

<script>

            document.addEventListener('DOMContentLoaded', function () {
                const calendarEl = document.getElementById('calendarZ')
                const calendar = new FullCalendar.Calendar(calendarEl, {
                    initialView: 'dayGridMonth', // Month view
                    headerToolbar: {
                        right: 'title',
                        // center: '',
                        left: ''
                    },
                    height: '100%',
                    contentHeight: 'auto',
                    events: [
                        {
                            title: 'ساعات مكتبية asdas',
                            start: '2024-12-25',
                            // className: "fc-event-warning",
                            color: '#f2c04e',   // an option!
                            description: 'Lorem ipsum conse ctetur adipi scing',
                            allDay: true
                        },

                        {
                            title: 'Event 2',
                            start: '2024-12-31',
                        },
                    ],
                    locale: 'ar',
                    direction: 'rtl',

                })
                calendar.render()
            })

        </script>--%>
