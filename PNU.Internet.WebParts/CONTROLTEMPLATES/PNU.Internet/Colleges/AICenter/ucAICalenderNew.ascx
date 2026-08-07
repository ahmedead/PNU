<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAICalenderNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAICalenderNew" %>






<style>
    .calendar {
        width: 100%;
        max-width: 900px;
        background-color: #fff;
        border-radius: 10px;
        box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
        padding: 20px;
    }
    .calendar-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
    }
    .calendar-header button {
        padding: 10px;
        background-color: #4a90e2;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        color: white;
    }
    .calendar-header button:hover {
        background-color: #357ab7;
    }
    .calendar-header h2 {
        font-size: 24px;
        margin: 0;
    }
    .calendar-filters {
        display: flex;
        justify-content: space-between;
        margin-bottom: 20px;
    }
    .calendar-filters select, .calendar-filters input {
        padding: 5px;
        border-radius: 5px;
        border: 1px solid #ccc;
    }
    .calendar-days {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 10px;
        text-align: center;
    }
    .calendar-day {
        padding: 10px;
        border-radius: 5px;
        cursor: pointer;
        background-color: #f0f0f0;
    }
    .calendar-day:hover {
        background-color: #d0d0d0;
    }
    .calendar-day.event {
        background-color: #4a90e2;
        color: white;
    }
    .calendar-day.event:hover {
        background-color: #357ab7;
    }
    .event-label {
        display: block;
        margin-top: 5px;
        font-size: 16px;
        color: #fff;
        padding: 3px;
        border-radius: 3px;
    }
    
    select#yearSelect,
    select#monthSelect {
        width: 80px;
        padding: 15px;
    }
	.fc a {
            text-decoration: unset !important;
        }

        .fc .fc-daygrid-day-top {
            flex-direction: row;
        }

        .fc-h-event .fc-event-title {
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


<%
    // Check the current site language
    var currentLanguage2 = SPContext.Current.Web.Language;
    string PrevContent = (currentLanguage2 == 1025) // Arabic Language LCID
        ? "▶"
        : "◀";
		
		string NextContent = (currentLanguage2 == 1025) // Arabic Language LCID
        ? "◀"
        : "▶";
		
		
		string GamificationandMetaverseTrack = (currentLanguage2 == 1025) ? "مسار التلعيب والميتافيرس": "Gamification and Metaverse Track";
		string InformationSecurityTrack = (currentLanguage2 == 1025) ? "مسار أمن المعلومات": "Information Security Track";
		string HealthcareBioinformaticsTrack = (currentLanguage2 == 1025) ? "مسار الصحة والمعلوماتية الحيوية": "Healthcare & Bioinformatics Track";
		string ComputationalLearningTheoryTrack = (currentLanguage2 == 1025) ? "مسار نظرية التعلم الحوسبي": "Computational Learning Theory Track";
		string InternetofThingsTrack = (currentLanguage2 == 1025) ? "مسار انترنت الأشياء": "Internet of Things Track";
		string ComputerVisionTrack = (currentLanguage2 == 1025) ? "مسار رؤية الحاسب": "Computer Vision Track";
		string GenerativeAINLPTrack = (currentLanguage2 == 1025) ? "مسار الذكاء الاصطناعي التوليدي ومعالجة اللغات الطبيعية": "Generative AI & NLP Track";
		string AICenterTrack = (currentLanguage2 == 1025) ? "مركز الذكاء الاصطناعي": "AI center";
		
		
		
		
	
	
	
		
		string Sunday = (currentLanguage2 == 1025) ? "الأحد": "Sunday";
		string Monday = (currentLanguage2 == 1025) ? "الاثنين": "Monday";
		string Tuesday = (currentLanguage2 == 1025) ? "الثلاثاء": "Tuesday";
		string Wednesday = (currentLanguage2 == 1025) ? "الأربعاء": "Wednesday";
		string Thursday = (currentLanguage2 == 1025) ? "الخميس": "Thursday";
		string Friday = (currentLanguage2 == 1025) ? "الجمعة": "Friday";
		string Saturday = (currentLanguage2 == 1025) ? "السبت": "Saturday";
		
%>

<section class="full-calendar">
    <div class="container py-5 my-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">
                <div class="calendar">
                    <div class="calendar-filters">
                        <input type="text" id="searchEvent" placeholder="بحث عن الفعالية..."  style="display:none"/>
                        <select id="yearSelect"></select>
                        <select id="monthSelect"></select>
                    </div>
                    
                    <div class="calendar-header">
                        <button id="prevMonth"><%= PrevContent %></button>
                        <h2 id="monthName"></h2>
                        <button id="nextMonth"><%= NextContent %></button>
                    </div>
                    <div class="calendar-days">
                        <div class="calendar-day"><%= Sunday %></div>
                        <div class="calendar-day"><%= Monday %></div>
                        <div class="calendar-day"><%= Tuesday %></div>
                        <div class="calendar-day"><%= Wednesday %></div>
                        <div class="calendar-day"><%= Thursday %></div>
                        <div class="calendar-day"><%= Friday %></div>
                        <div class="calendar-day"><%= Saturday %></div>
                    </div>
                    
                    <div id="days" class="calendar-days"></div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="d-flex flex-column">
                    <div>
                        <div class="p-2 px-3 my-2 border rounded-4">
                            <ul class="list-unstyled mb-0">
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #a8c3c3;"></span>
                                    <span><%= GamificationandMetaverseTrack %></span>
                                </li>
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #3b4cc0;"></span>
                                    <span><%= InformationSecurityTrack %></span>
                                </li>
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #9f74c1;"></span>
                                    <span><%= HealthcareBioinformaticsTrack %></span>
                                </li>
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #56b0b6;"></span>
                                    <span><%= ComputationalLearningTheoryTrack %></span>
                                </li>
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #7a5a9c;"></span>
                                    <span><%= InternetofThingsTrack %></span>
                                </li>
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #d15555;"></span>
                                    <span><%= ComputerVisionTrack %></span>
                                </li>
                                <li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #f2c04e;"></span>
                                    <span><%= GenerativeAINLPTrack %></span>
                                </li>
								<li class="d-flex align-items-center my-2">
                                    <span class="circle" style="background-color: #556B2F;"></span>
                                    <span><%= AICenterTrack %></span>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>

<script>
    const events = [];
    const currentLanguage = <%= SPContext.Current.Web.Language %>;

    $(function () {
        // load banners images
        var bannersObject = {
            'op': 'GetEventsData',
            'listUrl': $util.LangUrl + '/Lists/Events',
            'viewName': 'Home',
            'pageURL': document.location.href
        };
        LoadDataFromSharePoint(bannersObject, BindEventsData);
    });

    function formatDate(dateStr) {
        const [day, month, year] = dateStr.split('/');
        return `${year}-${month}-${day}`;
    }

    // Define event categories and their corresponding colors
    const categoryColors_AR = {
        "مسار التلعيب والميتافيرس": "#a8c3c3", // Light Blue
        "مسار أمن المعلومات": "#3b4cc0", // Dark Blue
        "مسار الصحة والمعلوماتية الحيوية": "#9f74c1", // Purple
        "مسار نظرية التعلم الحوسبي": "#56b0b6", // Teal
        "مسار انترنت الأشياء": "#7a5a9c", // Lavender
        "مسار رؤية الحاسب": "#d15555", // Red
        "مركز الذكاء الاصطناعي": "#556B2F", // Green
        "مسار الذكاء الاصطناعي التوليدي ومعالجة اللغات الطبيعية": "#f2c04e" // Yellow
    };

    const categoryColors_EN = {
        "Gamification and Metaverse Track": "#a8c3c3", // Light Blue
        "Information Security Track": "#3b4cc0", // Dark Blue
        "Healthcare & Bioinformatics Track": "#9f74c1", // Purple
        "Computational Learning Theory Track": "#56b0b6", // Teal
        "Internet of Things Track": "#7a5a9c", // Lavender
        "Computer Vision Track": "#d15555", // Red
        "AI center": "#556B2F", // Green
        "Generative AI & NLP Track": "#f2c04e" // Yellow
    };

    const isArabic = currentLanguage === 1025;

    const months_AR = ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'أغسطس', 'سبتمبر', 'أكتوبر', 'نوفمبر', 'ديسمبر'];
    const months_EN = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];


    const months = isArabic ? months_AR : months_EN;

    const categoryColors = isArabic ? categoryColors_AR : categoryColors_EN;
    var BindEventsData = function (data, listName) {
        if (data && data.length > 0) {
            const daysContainer = document.getElementById('days');
            const dayElements = daysContainer.querySelectorAll('.calendar-day');

            // Create an array to store event data for each day
            const eventSpans = [];

            // Loop through the data to match event dates with calendar days
            data.forEach(function (event) {
                const startDate = formatDate(event.startDate);  // Fix invalid date format
                const endDate = formatDate(event.endDate);  // Fix invalid date format
                const eventTitle = event.EventTitle;
                const eventCategory = event.Category; // Assuming event has a 'Category' property

                events.push({
                    title: eventTitle,
                    startDate: startDate,
                    endDate: endDate,
                    category: eventCategory
                });
            });

        } else {
            console.log("No events available");
        }

        renderCalendar();
    };

    // Load data from list
    var LoadDataFromSharePoint = function LoadDataFromFaculty(params, success, container, handler) {
        var listUrl = '';
        var query = '';
        var i = 0;
        for (var key in params) {
            if (params.hasOwnProperty(key)) {
                query += (i == 0 ? '' : '&') + key + '=' + params[key];
                i++;
            }
            if (key.toLowerCase() === 'listUrl'.toLowerCase()) {
                listUrl = params[key];
            }
        }
        var lang = $util.currentLang;

        $.ajax({
            type: "GET",
            contentType: "application/json; charset=utf-8",
            url: $util.LangUrl + '/_LAYOUTS/15/NewPortal/' + (handler ?? 'PortalHandler') + '.ashx?' + query,
            dataType: "json",
            async: true,
            cache: true,
            success: function (data) {
                success(data, listUrl, container)
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $(container).find('.internal_loader').html($util.getLocalString("NoDataContainer"));
            }
        })
    };

    let currentMonth = new Date().getMonth();
    let currentYear = new Date().getFullYear();

    function initFilters() {
        const yearSelect = document.getElementById('yearSelect');
        const monthSelect = document.getElementById('monthSelect');

        for (let i = 2020; i <= 2030; i++) {
            const option = document.createElement('option');
            option.value = i;
            option.textContent = i;
            if (i === currentYear) option.selected = true;
            yearSelect.appendChild(option);
        }

        months.forEach((month, index) => {
            const option = document.createElement('option');
            option.value = index;
            option.textContent = month;
            if (index === currentMonth) option.selected = true;
            monthSelect.appendChild(option);
        });
    }

    function renderCalendar() {
        const firstDayOfMonth = new Date(currentYear, currentMonth, 1);
        const lastDayOfMonth = new Date(currentYear, currentMonth + 1, 0);
        const daysInMonth = lastDayOfMonth.getDate();
        const startingDay = firstDayOfMonth.getDay();

        const monthName = firstDayOfMonth.toLocaleString('default', { month: 'long' });
        document.getElementById('monthName').textContent = `${monthName} ${currentYear}`;

        const daysContainer = document.getElementById('days');
        daysContainer.innerHTML = '';

        // Empty slots before the start of the month
        for (let i = 0; i < startingDay; i++) {
            const emptyDiv = document.createElement('div');
            daysContainer.appendChild(emptyDiv);
        }

        const eventSpans = [];

        for (let day = 1; day <= daysInMonth; day++) {
            const dayDiv = document.createElement('div');
            dayDiv.classList.add('calendar-day');
            dayDiv.textContent = day;

            const currentDate = `${currentYear}-${(currentMonth + 1).toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;

            events.forEach(event => {
                if (isDateInRange(currentDate, event.startDate, event.endDate)) {
                    if (!eventSpans[day]) {
                        eventSpans[day] = [];
                    }
                    eventSpans[day].push(event);
                }
            });

            daysContainer.appendChild(dayDiv);
        }

        createEventBars(eventSpans);
    }

    function createEventBars(eventSpans) {
        const daysContainer = document.getElementById('days');
        const dayElements = daysContainer.querySelectorAll('.calendar-day');

        eventSpans.forEach((eventsForDay, dayIndex) => {
            if (eventsForDay) {
                const eventBar = document.createElement('div');
                eventBar.classList.add('event-bar');

                eventsForDay.forEach(event => {
                    const eventLabel = document.createElement('span');
                    eventLabel.classList.add('event-label');
                    eventLabel.textContent = event.title;

                    // Apply background color based on category
                    eventLabel.style.backgroundColor = categoryColors[event.category] || '#ccc'; // Default color if category is not found

                    eventBar.appendChild(eventLabel);
                });

                dayElements[dayIndex - 1].appendChild(eventBar);
            }
        });
    }

    function isDateInRange(date, startDate, endDate) {
        return date >= startDate && date <= endDate;
    }

    document.getElementById('yearSelect').addEventListener('change', (e) => {
        currentYear = parseInt(e.target.value);
        renderCalendar();
    });

    document.getElementById('monthSelect').addEventListener('change', (e) => {
        currentMonth = parseInt(e.target.value);
        renderCalendar();
    });

    document.getElementById('searchEvent').addEventListener('input', (e) => {
        const searchTerm = e.target.value.toLowerCase();
        const allDays = document.querySelectorAll('.calendar-day');

        allDays.forEach((dayDiv) => {
            const eventLabel = dayDiv.querySelector('.event-label');
            if (eventLabel) {
                const eventText = eventLabel.textContent.toLowerCase();
                if (eventText.includes(searchTerm)) {
                    dayDiv.style.display = 'block';
                } else {
                    dayDiv.style.display = 'none';
                }
            }
        });
    });

    initFilters();
    renderCalendar();

    // Handle "Previous" and "Next" Month Button Click
    document.getElementById('prevMonth').addEventListener('click', function (e) {
        e.preventDefault();
        if (currentMonth === 0) {
            currentMonth = 11;
            currentYear--;
        } else {
            currentMonth--;
        }
        renderCalendar();
    });

    document.getElementById('nextMonth').addEventListener('click', function (e) {
        e.preventDefault();
        if (currentMonth === 11) {
            currentMonth = 0;
            currentYear++;
        } else {
            currentMonth++;
        }
        renderCalendar();
    });
</script>
