using System;
using System.Web.UI;
using Microsoft.SharePoint;
using Portal.Main.Helper;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.EserviceList
{
    public partial class ucEserviceSections : UserControl
    {
        #region Public Properties (Configurable via ControlLoaderWebPart or Parent Page)

        public string InternalUniversityUrlAr { get; set; } = "/ar/Pages/InternalEservices.aspx";
        public string InternalUniversityUrlEn { get; set; } = "/en/Pages/InternalEservices.aspx";

        public string InternalNafathUrlAr { get; set; } = "/ar/Pages/InternalNafathEservices.aspx";
        public string InternalNafathUrlEn { get; set; } = "/en/Pages/InternalNafathEservices.aspx";

        public string ExternalUrlAr { get; set; } = "/ar/Pages/ExternalEservices.aspx";
        public string ExternalUrlEn { get; set; } = "/en/Pages/ExternalEservices.aspx";

        private string _internalUniversityUrl;
        public string InternalUniversityUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_internalUniversityUrl)) return _internalUniversityUrl;
                return IsArabic
                    ? (!string.IsNullOrEmpty(InternalUniversityUrlAr) ? InternalUniversityUrlAr : "/ar/Pages/InternalEservices.aspx")
                    : (!string.IsNullOrEmpty(InternalUniversityUrlEn) ? InternalUniversityUrlEn : "/en/Pages/InternalEservices.aspx");
            }
            set { _internalUniversityUrl = value; }
        }

        public string InternalUrl
        {
            get { return InternalUniversityUrl; }
            set { InternalUniversityUrl = value; }
        }

        private string _internalNafathUrl;
        public string InternalNafathUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_internalNafathUrl)) return _internalNafathUrl;
                return IsArabic
                    ? (!string.IsNullOrEmpty(InternalNafathUrlAr) ? InternalNafathUrlAr : "/ar/Pages/InternalNafathEservices.aspx")
                    : (!string.IsNullOrEmpty(InternalNafathUrlEn) ? InternalNafathUrlEn : "/en/Pages/InternalNafathEservices.aspx");
            }
            set { _internalNafathUrl = value; }
        }

        private string _externalUrl;
        public string ExternalUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_externalUrl)) return _externalUrl;
                return IsArabic
                    ? (!string.IsNullOrEmpty(ExternalUrlAr) ? ExternalUrlAr : "/ar/Pages/ExternalEservices.aspx")
                    : (!string.IsNullOrEmpty(ExternalUrlEn) ? ExternalUrlEn : "/en/Pages/ExternalEservices.aspx");
            }
            set { _externalUrl = value; }
        }

        public string InternalTitleAr { get; set; }
        public string InternalTitleEn { get; set; }

        public string InternalDescAr { get; set; }
        public string InternalDescEn { get; set; }

        public string ExternalTitleAr { get; set; }
        public string ExternalTitleEn { get; set; }

        public string ExternalDescAr { get; set; }
        public string ExternalDescEn { get; set; }

        public string UniversityLoginTextAr { get; set; }
        public string UniversityLoginTextEn { get; set; }

        public string NafathLoginTextAr { get; set; }
        public string NafathLoginTextEn { get; set; }

        private string _internalIcon;
        public string InternalIcon
        {
            get { return !string.IsNullOrEmpty(_internalIcon) ? _internalIcon : "hgi-user-group"; }
            set { _internalIcon = value; }
        }

        private string _externalIcon;
        public string ExternalIcon
        {
            get { return !string.IsNullOrEmpty(_externalIcon) ? _externalIcon : "hgi-globe-02"; }
            set { _externalIcon = value; }
        }

        #endregion

        #region Display Properties

        public bool IsArabic
        {
            get
            {
                try
                {
                    return SPContext.Current != null && SPContext.Current.Web != null && SPContext.Current.Web.Language == 1025;
                }
                catch
                {
                    return PortalHelper.IsArabic;
                }
            }
        }

        public string InternalTitle
        {
            get
            {
                if (IsArabic)
                    return !string.IsNullOrEmpty(InternalTitleAr) ? InternalTitleAr : "الخدمات الإلكترونية للمستفيد الداخلي";
                else
                    return !string.IsNullOrEmpty(InternalTitleEn) ? InternalTitleEn : "Internal Beneficiary E-Services";
            }
        }

        public string InternalDescription
        {
            get
            {
                if (IsArabic)
                    return !string.IsNullOrEmpty(InternalDescAr) ? InternalDescAr : "الطالبات، أعضاء الهيئة التعليمية، أعضاء الهيئة الإدارية";
                else
                    return !string.IsNullOrEmpty(InternalDescEn) ? InternalDescEn : "Students, Faculty Members, Administrative Staff";
            }
        }

        public string ExternalTitle
        {
            get
            {
                if (IsArabic)
                    return !string.IsNullOrEmpty(ExternalTitleAr) ? ExternalTitleAr : "خدمات المستفيد الخارجي";
                else
                    return !string.IsNullOrEmpty(ExternalTitleEn) ? ExternalTitleEn : "External Beneficiary Services";
            }
        }

        public string ExternalDescription
        {
            get
            {
                if (IsArabic)
                    return !string.IsNullOrEmpty(ExternalDescAr) ? ExternalDescAr : "خدمات الزوار";
                else
                    return !string.IsNullOrEmpty(ExternalDescEn) ? ExternalDescEn : "Visitor Services";
            }
        }

        public string UniversityLoginText
        {
            get
            {
                if (IsArabic)
                    return !string.IsNullOrEmpty(UniversityLoginTextAr) ? UniversityLoginTextAr : "الدخول عن طريق الحساب الجامعي";
                else
                    return !string.IsNullOrEmpty(UniversityLoginTextEn) ? UniversityLoginTextEn : "Login via University Account";
            }
        }

        public string NafathLoginText
        {
            get
            {
                if (IsArabic)
                    return !string.IsNullOrEmpty(NafathLoginTextAr) ? NafathLoginTextAr : "الدخول عن طريق نفاذ";
                else
                    return !string.IsNullOrEmpty(NafathLoginTextEn) ? NafathLoginTextEn : "Login via Nafath";
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
