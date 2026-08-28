<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucAICalenderNew.ascx.cs" Inherits="PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges.AICenter.ucAICalenderNew" %>

<style>
    .ai-calendar-box {
        width: 100%;
        background-color: #fff;
    }
    .ai-calendar-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 20px;
    }
    .ai-calendar-days-header {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 6px;
        text-align: center;
        margin-bottom: 8px;
    }
    .ai-calendar-day-head {
        font-weight: 600;
        font-size: 0.875rem;
        padding: 8px 4px;
        background-color: var(--bs-primary-25, #f0f7f3);
        color: var(--bs-primary, #006923);
        border-radius: 6px;
    }
    .ai-calendar-days-grid {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 6px;
    }
    .ai-calendar-day-cell {
        min-height: 80px;
        padding: 6px;
        border-radius: 6px;
        background-color: #fafafa;
        border: 1px solid #eee;
        transition: background-color 0.2s ease;
        display: flex;
        flex-direction: column;
        align-items: flex-start;
    }
    .ai-calendar-day-cell:hover {
        background-color: #f2f2f2;
    }
    .ai-calendar-day-number {
        font-size: 0.85rem;
        font-weight: 600;
        margin-bottom: 4px;
        color: #444;
    }
    .ai-event-label {
        display: block;
        width: 100%;
        margin-top: 3px;
        font-size: 0.72rem;
        color: #fff;
        padding: 2px 5px;
        border-radius: 4px;
        white-space: normal;
        word-break: break-word;
        line-height: 1.2;
    }
    .track-legend-circle {
        width: 14px;
        height: 14px;
        border-radius: 50%;
        display: inline-block;
        flex-shrink: 0;
    }
</style>

<%
    var currentLanguage2 = SPContext.Current.Web.Language;
    bool isArLang = (currentLanguage2 == 1025);
    
    string GamificationandMetaverseTrack = isArLang ? "مسار التلعيب والميتافيرس": "Gamification and Metaverse Track";
    string InformationSecurityTrack = isArLang ? "مسار أمن المعلومات": "Information Security Track";
    string HealthcareBioinformaticsTrack = isArLang ? "مسار الصحة والمعلوماتية الحيوية": "Healthcare & Bioinformatics Track";
    string ComputationalLearningTheoryTrack = isArLang ? "مسار نظرية التعلم الحوسبي": "Computational Learning Theory Track";
    string InternetofThingsTrack = isArLang ? "مسار انترنت الأشياء": "Internet of Things Track";
    string ComputerVisionTrack = isArLang ? "مسار رؤية الحاسب": "Computer Vision Track";
    string GenerativeAINLPTrack = isArLang ? "مسار الذكاء الاصطناعي التوليدي ومعالجة اللغات الطبيعية": "Generative AI & NLP Track";
    string AICenterTrack = isArLang ? "مركز الذكاء الاصطناعي": "AI center";
    
    string Sunday = isArLang ? "الأحد": "Sun";
    string Monday = isArLang ? "الاثنين": "Mon";
    string Tuesday = isArLang ? "الثلاثاء": "Tue";
    string Wednesday = isArLang ? "الأربعاء": "Wed";
    string Thursday = isArLang ? "الخميس": "Thu";
    string Friday = isArLang ? "الجمعة": "Fri";
    string Saturday = isArLang ? "السبت": "Sat";
%>

<div class="d-flex flex-column gap-4">
    <h2 class="mb-4 fw-semibold">
        <asp:Literal runat="server" Text="<%$ Resources: PNUres, AICalender %>" />
    </h2>

    <div class="row g-4">
        <!-- Calendar Main Grid -->
        <div class="col-12 col-xl-8">
            <div class="card border rounded-3 p-4 shadow-sm ai-calendar-box">
                <!-- Filters & Controls -->
                <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4 pb-3 border-bottom">
                    <div class="d-flex align-items-center gap-2">
                        <select id="monthSelect" class="form-select form-select-sm" style="width: auto; min-width: 120px;"></select>
                        <select id="yearSelect" class="form-select form-select-sm" style="width: auto; min-width: 90px;"></select>
                    </div>

                    <div class="d-flex align-items-center gap-2">
                        <button id="prevMonth" type="button" class="btn btn-sm btn-outline-primary" aria-label="الشهر السابق">
                            <i class="hgi hgi-stroke hgi-arrow-right-01 fs-6" aria-hidden="true"></i>
                        </button>
                        <h3 id="monthName" class="h6 fw-bold mb-0 text-center px-2" style="min-width: 140px;"></h3>
                        <button id="nextMonth" type="button" class="btn btn-sm btn-outline-primary" aria-label="الشهر القادم">
                            <i class="hgi hgi-stroke hgi-arrow-left-01 fs-6" aria-hidden="true"></i>
                        </button>
                    </div>
                </div>

                <!-- Days Header -->
                <div class="ai-calendar-days-header">
                    <div class="ai-calendar-day-head"><%= Sunday %></div>
                    <div class="ai-calendar-day-head"><%= Monday %></div>
                    <div class="ai-calendar-day-head"><%= Tuesday %></div>
                    <div class="ai-calendar-day-head"><%= Wednesday %></div>
                    <div class="ai-calendar-day-head"><%= Thursday %></div>
                    <div class="ai-calendar-day-head"><%= Friday %></div>
                    <div class="ai-calendar-day-head"><%= Saturday %></div>
                </div>

                <!-- Days Grid -->
                <div id="days" class="ai-calendar-days-grid"></div>
            </div>
        </div>

        <!-- Legend Sidebar -->
        <div class="col-12 col-xl-4">
            <div class="card border rounded-3 p-4 shadow-sm bg-light h-100">
                <div class="d-flex align-items-center gap-2 mb-3 pb-2 border-bottom">
                    <i class="hgi hgi-stroke hgi-filter fs-5 text-primary" aria-hidden="true"></i>
                    <h3 class="h6 fw-bold mb-0"><%= isArLang ? "مسارات الفعاليات" : "Event Tracks" %></h3>
                </div>
                <ul class="list-unstyled mb-0 d-flex flex-column gap-2 small">
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #a8c3c3;"></span>
                        <span class="text-secondary"><%= GamificationandMetaverseTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #3b4cc0;"></span>
                        <span class="text-secondary"><%= InformationSecurityTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #9f74c1;"></span>
                        <span class="text-secondary"><%= HealthcareBioinformaticsTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #56b0b6;"></span>
                        <span class="text-secondary"><%= ComputationalLearningTheoryTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #7a5a9c;"></span>
                        <span class="text-secondary"><%= InternetofThingsTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #d15555;"></span>
                        <span class="text-secondary"><%= ComputerVisionTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #f2c04e;"></span>
                        <span class="text-secondary"><%= GenerativeAINLPTrack %></span>
                    </li>
                    <li class="d-flex align-items-center gap-2 py-1">
                        <span class="track-legend-circle" style="background-color: #006923;"></span>
                        <span class="text-secondary"><%= AICenterTrack %></span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    (function () {
        const events = [];
        const currentLanguage = <%= SPContext.Current.Web.Language %>;
        const isArabic = currentLanguage === 1025;

        const categoryColors_AR = {
            "مسار التلعيب والميتافيرس": "#a8c3c3",
            "مسار أمن المعلومات": "#3b4cc0",
            "مسار الصحة والمعلوماتية الحيوية": "#9f74c1",
            "مسار نظرية التعلم الحوسبي": "#56b0b6",
            "مسار انترنت الأشياء": "#7a5a9c",
            "مسار رؤية الحاسب": "#d15555",
            "مركز الذكاء الاصطناعي": "#006923",
            "مسار الذكاء الاصطناعي التوليدي ومعالجة اللغات الطبيعية": "#f2c04e"
        };

        const categoryColors_EN = {
            "Gamification and Metaverse Track": "#a8c3c3",
            "Information Security Track": "#3b4cc0",
            "Healthcare & Bioinformatics Track": "#9f74c1",
            "Computational Learning Theory Track": "#56b0b6",
            "Internet of Things Track": "#7a5a9c",
            "Computer Vision Track": "#d15555",
            "AI center": "#006923",
            "Generative AI & NLP Track": "#f2c04e"
        };

        const months_AR = ['يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو', 'يوليو', 'أغسطس', 'سبتمبر', 'أكتوبر', 'نوفمبر', 'ديسمبر'];
        const months_EN = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
        const months = isArabic ? months_AR : months_EN;
        const categoryColors = isArabic ? categoryColors_AR : categoryColors_EN;

        function formatDate(dateStr) {
            if (!dateStr) return '';
            const parts = dateStr.split('/');
            if (parts.length === 3) {
                return parts[2] + '-' + parts[1].padStart(2, '0') + '-' + parts[0].padStart(2, '0');
            }
            return dateStr;
        }

        var BindEventsData = function (data) {
            if (data && data.length > 0) {
                data.forEach(function (event) {
                    events.push({
                        title: event.EventTitle || event.Title,
                        startDate: formatDate(event.startDate),
                        endDate: formatDate(event.endDate),
                        category: event.Category
                    });
                });
            }
            renderCalendar();
        };

        var LoadDataFromSharePoint = function (params, success) {
            if (typeof $util === 'undefined' || !window.jQuery) return;
            var query = '';
            var i = 0;
            for (var key in params) {
                if (params.hasOwnProperty(key)) {
                    query += (i == 0 ? '' : '&') + key + '=' + encodeURIComponent(params[key]);
                    i++;
                }
            }
            jQuery.ajax({
                type: "GET",
                contentType: "application/json; charset=utf-8",
                url: ($util.LangUrl || '') + '/_LAYOUTS/15/NewPortal/PortalHandler.ashx?' + query,
                dataType: "json",
                async: true,
                cache: true,
                success: function (data) {
                    success(data);
                },
                error: function () {
                    renderCalendar();
                }
            });
        };

        let currentMonth = new Date().getMonth();
        let currentYear = new Date().getFullYear();

        function initFilters() {
            const yearSelect = document.getElementById('yearSelect');
            const monthSelect = document.getElementById('monthSelect');
            if (!yearSelect || !monthSelect) return;

            yearSelect.innerHTML = '';
            monthSelect.innerHTML = '';

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

        function isDateInRange(date, startDate, endDate) {
            if (!startDate) return false;
            if (!endDate) return date === startDate;
            return date >= startDate && date <= endDate;
        }

        function renderCalendar() {
            const firstDayOfMonth = new Date(currentYear, currentMonth, 1);
            const lastDayOfMonth = new Date(currentYear, currentMonth + 1, 0);
            const daysInMonth = lastDayOfMonth.getDate();
            const startingDay = firstDayOfMonth.getDay();

            const monthHeading = document.getElementById('monthName');
            if (monthHeading) {
                monthHeading.textContent = months[currentMonth] + ' ' + currentYear;
            }

            const daysContainer = document.getElementById('days');
            if (!daysContainer) return;
            daysContainer.innerHTML = '';

            // Empty slots
            for (let i = 0; i < startingDay; i++) {
                const emptyDiv = document.createElement('div');
                emptyDiv.className = 'ai-calendar-day-cell bg-transparent border-0';
                daysContainer.appendChild(emptyDiv);
            }

            for (let day = 1; day <= daysInMonth; day++) {
                const dayDiv = document.createElement('div');
                dayDiv.classList.add('ai-calendar-day-cell');

                const numSpan = document.createElement('span');
                numSpan.classList.add('ai-calendar-day-number');
                numSpan.textContent = day;
                dayDiv.appendChild(numSpan);

                const currentDate = currentYear + '-' + (currentMonth + 1).toString().padStart(2, '0') + '-' + day.toString().padStart(2, '0');

                events.forEach(event => {
                    if (isDateInRange(currentDate, event.startDate, event.endDate)) {
                        const eventLabel = document.createElement('span');
                        eventLabel.classList.add('ai-event-label');
                        eventLabel.textContent = event.title;
                        eventLabel.style.backgroundColor = categoryColors[event.category] || '#006923';
                        dayDiv.appendChild(eventLabel);
                    }
                });

                daysContainer.appendChild(dayDiv);
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            initFilters();
            renderCalendar();

            if (typeof $util !== 'undefined') {
                var bannersObject = {
                    'op': 'GetEventsData',
                    'listUrl': ($util.LangUrl || '') + '/Lists/Events',
                    'viewName': 'Home',
                    'pageURL': document.location.href
                };
                LoadDataFromSharePoint(bannersObject, BindEventsData);
            }

            const yearSelect = document.getElementById('yearSelect');
            if (yearSelect) {
                yearSelect.addEventListener('change', (e) => {
                    currentYear = parseInt(e.target.value);
                    renderCalendar();
                });
            }

            const monthSelect = document.getElementById('monthSelect');
            if (monthSelect) {
                monthSelect.addEventListener('change', (e) => {
                    currentMonth = parseInt(e.target.value);
                    renderCalendar();
                });
            }

            const prevBtn = document.getElementById('prevMonth');
            if (prevBtn) {
                prevBtn.addEventListener('click', function (e) {
                    e.preventDefault();
                    if (currentMonth === 0) {
                        currentMonth = 11;
                        currentYear--;
                    } else {
                        currentMonth--;
                    }
                    if (monthSelect) monthSelect.value = currentMonth;
                    if (yearSelect) yearSelect.value = currentYear;
                    renderCalendar();
                });
            }

            const nextBtn = document.getElementById('nextMonth');
            if (nextBtn) {
                nextBtn.addEventListener('click', function (e) {
                    e.preventDefault();
                    if (currentMonth === 11) {
                        currentMonth = 0;
                        currentYear++;
                    } else {
                        currentMonth++;
                    }
                    if (monthSelect) monthSelect.value = currentMonth;
                    if (yearSelect) yearSelect.value = currentYear;
                    renderCalendar();
                });
            }
        });
    })();
</script>
