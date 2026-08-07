using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.MediaCenter.News;
using Portal.Main.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Colleges
{
    public class clsFacultyMembers
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public string Bio { get; set; }
        public string SubjectTitle1 { get; set; }
        public string SubjectDesc1 { get; set; }


    
    
    }
    [Serializable]
    public class clsCollMembersListName
    {
        public string ID { get; set; }
        //public string Title { get; set; }
        //public string full_name { get; set; }
        //public string english_name { get; set; }
        //public string employee_number { get; set; }
        //public string Email_address { get; set; }
        //public string phone_number { get; set; }
        //public string extention_number { get; set; }
        //public string profession { get; set; }

        //public string college { get; set; }
        //public string specialization { get; set; }
        //public string qualification_name { get; set; }
        //public string minor { get; set; }
        //public string section { get; set; }
        //public string employee_status { get; set; }
        //public string location_name { get; set; }
        //public string location_code { get; set; }
        //public string floor { get; set; }
        //public string office { get; set; }
        //public string college_id { get; set; }
        //public string section_id { get; set; }

        //public string GRADE_NAME { get; set; }


        public string TITLE { get; set; }
        public string FULL_NAME { get; set; }
        public string ENGLISH_NAME { get; set; }
        public string EMPLOYEE_NUMBER { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string EXTENTION_NUMBER { get; set; }
        public string PROFESSION { get; set; }
        public string PROFESSION_EN { get; set; }
        public string GRADE_NAME { get; set; }
        public string GRADE_NAME_EN { get; set; }
        public string COLLEGE_ID { get; set; }
        public string COLLEGE { get; set; }
        public string COLLEGE_EN { get; set; }
        public string SPECIALIZATION { get; set; }
        public string SPECIALIZATION_EN { get; set; }
        public string SPECIAL_CPECIALIZATION { get; set; }
        public string SPECIAL_CPECIALIZATION_EN { get; set; }
        public string QUALIFICATION_NAME { get; set; }
        public string QUALIFICATION_NAME_EN { get; set; }
        public string NATIONALITY { get; set; }
        public string NATIONALITY_EN { get; set; }
        public string SECTION_ID { get; set; }
        public string SECTION { get; set; }
        public string SECTION_EN { get; set; }
        public string SCHOLARSHIP { get; set; }
        public string POSITION_DELEG { get; set; }
        public string POSITION { get; set; }
        public string DELEG_DEPARTMENT { get; set; }
        public string DELEG_START_DATE { get; set; }
        public string DURATION { get; set; }
        public string ORDERDATE { get; set; }
        public string END_DATE { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public string TERMINATION_REASON { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LOCATION_NAME_EN { get; set; }
        public string FLOOR { get; set; }
        public string OFFICE { get; set; }



    }


    public class clsFacultyMembersRepeaterData
    {
        public string ID { get; set; }
        public string MainCategory { get; set; }
        public string MainCategory_EN { get; set; }
        public List<clsCollMembersListName> FacultyMembers { get; set; }

        public string ClassActive
        {
            get
            {
                if (this.ID == "1")
                    return " active";
                else
                    return "";

            }
        }
        public string ClassActiveShow
        {
            get
            {
                if (this.ID == "1")
                    return " active show";
                else
                    return "";

            }
        }

    }
    public class AllDataCollMembersListName
    {
        public string ID { get; set; }
        public string CountGoogle { get; set; }
        public string CountORCID { get; set; }
        public string CountTweets { get; set; }

        public string CountAnnounssements { get; set; }


        public string TITLE { get; set; }
        public string FULL_NAME { get; set; }
        public string ENGLISH_NAME { get; set; }
        public string EMPLOYEE_NUMBER { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string EXTENTION_NUMBER { get; set; }
        public string PROFESSION { get; set; }
        public string PROFESSION_EN { get; set; }
        public string GRADE_NAME { get; set; }
        public string GRADE_NAME_EN { get; set; }
        public string COLLEGE_ID { get; set; }
        public string COLLEGE { get; set; }
        public string COLLEGE_EN { get; set; }
        public string SPECIALIZATION { get; set; }
        public string SPECIALIZATION_EN { get; set; }
        public string SPECIAL_CPECIALIZATION { get; set; }
        public string SPECIAL_CPECIALIZATION_EN { get; set; }
        public string QUALIFICATION_NAME { get; set; }
        public string QUALIFICATION_NAME_EN { get; set; }
        public string NATIONALITY { get; set; }
        public string NATIONALITY_EN { get; set; }
        public string SECTION_ID { get; set; }
        public string SECTION { get; set; }
        public string SECTION_EN { get; set; }
        public string SCHOLARSHIP { get; set; }
        public string POSITION_DELEG { get; set; }
        public string POSITION { get; set; }
        public string DELEG_DEPARTMENT { get; set; }
        public string DELEG_START_DATE { get; set; }
        public string DURATION { get; set; }
        public string ORDERDATE { get; set; }
        public string END_DATE { get; set; }
        public string EMPLOYEE_STATUS { get; set; }
        public string TERMINATION_REASON { get; set; }
        public string LOCATION_CODE { get; set; }
        public string LOCATION_NAME { get; set; }
        public string LOCATION_NAME_EN { get; set; }
        public string FLOOR { get; set; }
        public string OFFICE { get; set; }



    }

    public static class busclsFacultyMembers
    {
        public static List<clsCollMembersListName> GetAllItems()
        {
            try
            {//List<clsSlider> _AllItems = SPFactory.GetAllItems<clsSlider>(Settings.SliderList);
                return SPFactory.GetAllItemsWithoutItemOrder<clsCollMembersListName>(Settings.FacultyMembersList, PortalHelper.ParentLangSite);




            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsFacultyMembers - GetAllItems", ex.Message);
                return null;
            }
            

        }


        public static clsCollMembersListName GetFacultyMemberByEmail(string email)
        {
            try
            {
                email = email.ToLower().Trim();

                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='EMAIL_ADDRESS' />
                                 <Value Type='Text'>" + email + @"</Value>
                              </Eq>
                           </Where>");

                SPListItemCollection objNew = null;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.OpenWeb("Admin"))
                        {
                            SPList reqList = web.Lists["CollMembersListName"];
                            objNew = reqList.GetItems(query);

                        }
                    }
                });
                if (objNew == null)
                    return null;
                if (objNew.Count == 0)
                    return null;

                List<clsCollMembersListName> _Data = SPFactory.MapListItemsToClass<clsCollMembersListName>(objNew);
                if (_Data != null && _Data.Count > 0)
                    return _Data[0];



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsFacultyMembers - GetFacultyMemberByEmail", ex.Message);
            }
            

            return null;

            

        }

        public static List<clsCollMembersListName> GetAllSectionFacultyMembers(string SecName)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Contains>
                                 <FieldRef Name='SECTION' />
                                 <Value Type='Text'>" + SecName + @"</Value>
                              </Contains>
                           </Where>");

                List<clsCollMembersListName> _Data = new List<clsCollMembersListName>();//SPFactory.GetAllItemsByQuery<clsCollMembersListName>("Admin", Settings.FacultyMembersList, query);
                SPListItemCollection objNew = null;
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb("Admin"))
                    {
                        SPList reqList = web.GetList(Settings.FacultyMembersList);
                        objNew = reqList.GetItems(query);

                    }
                }


                if (objNew != null && objNew.Count > 0)
                    return SPFactory.MapListItemsToClass<clsCollMembersListName>(objNew);

                return null;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsFacultyMembers - GetAllSectionFacultyMembers", ex.Message);
                return null;
            }

            



        }
    }




   


}
