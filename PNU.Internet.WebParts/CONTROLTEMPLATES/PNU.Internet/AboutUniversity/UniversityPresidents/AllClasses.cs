using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.AboutUniversity
{


    public class PresidentRepository
    {
        public List<PresidentProfileModel> GetProfile(SPWeb web)
        {
            var list = web.Lists.TryGetList(ListNames.PresidentProfile);
            if (list == null || list.ItemCount == 0)
                return new List<PresidentProfileModel>();

            SPQuery query = new SPQuery
            {
                RowLimit = 1,
                Query = "<OrderBy><FieldRef Name='ID' Ascending='TRUE' /></OrderBy>"
            };

            SPListItemCollection items = list.GetItems(query);
            if (items.Count == 0)
                return new List<PresidentProfileModel>();


            var result = new List<PresidentProfileModel>();
            if (items != null && items.Count > 0)
            {
                result = SPFactory.MapListItemsToClass<PresidentProfileModel>(items);
            }


            return result;
        }

        public List<SpeechItem> GetSpeechItems(SPWeb web)
        {
            var result = new List<SpeechItem>();
            SPList list = web.Lists.TryGetList(ListNames.PresidentSpeech);
            if (list == null) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new SpeechItem
                {
                    Title = Convert.ToString(item["Title"]),
                    ParagraphTextAr = Convert.ToString(item["ParagraphTextAr"]),
                    SortOrder = ToInt(item["SortOrder"])
                });
            }

            return result;
        }

        public List<ContactItem> GetContacts(SPWeb web)
        {
            var result = new List<ContactItem>();
            SPList list = web.Lists.TryGetList(ListNames.PresidentContacts);
            if (list == null) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new ContactItem
                {
                    Title = Convert.ToString(item["Title"]),
                    ContactLabelAr = Convert.ToString(item["ContactLabelAr"]),
                    ContactValue = Convert.ToString(item["ContactValue"]),
                    ContactType = Convert.ToString(item["ContactType"]),
                    SortOrder = ToInt(item["SortOrder"])
                });
            }

            return result;
        }

        public List<MembershipItem> GetMemberships(SPWeb web)
        {
            var result = new List<MembershipItem>();
            SPList list = web.Lists.TryGetList(ListNames.PresidentMemberships);
            if (list == null) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new MembershipItem
                {
                    Title = Convert.ToString(item["Title"]),
                    MembershipTextAr = Convert.ToString(item["MembershipTextAr"]),
                    SortOrder = ToInt(item["SortOrder"])
                });
            }

            return result;
        }

        public List<AwardItem> GetAwards(SPWeb web)
        {
            var result = new List<AwardItem>();
            SPList list = web.Lists.TryGetList(ListNames.PresidentAwards);
            if (list == null) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            foreach (SPListItem item in list.GetItems(query))
            {
                result.Add(new AwardItem
                {
                    Title = Convert.ToString(item["Title"]),
                    AwardTextAr = Convert.ToString(item["AwardTextAr"]),
                    SortOrder = ToInt(item["SortOrder"])
                });
            }

            return result;
        }

        public List<ExperienceItem> GetExperiences(SPWeb web)
        {
            var result = new List<ExperienceItem>();
            SPList list = web.Lists.TryGetList(ListNames.PresidentExperiences);
            if (list == null) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };
            SPListItemCollection collitem = list.GetItems(query);

            if (collitem != null && collitem.Count > 0)
            {
                result = SPFactory.MapListItemsToClass<ExperienceItem>(collitem);
            }



            return result;
        }

        private string GetUrlValue(SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return string.Empty;

            SPFieldUrlValue urlValue = new SPFieldUrlValue(item[fieldName].ToString());
            return urlValue.Url;
        }

        private int ToInt(object value)
        {
            if (value == null) return 0;
            int.TryParse(value.ToString(), out int result);
            return result;
        }
    }


    public class SharePointListInitializer
    {
        public  SPList MapListFieldsFromClass<T>(SPList list)
        {
            try
            {
                T obj = default(T);
                obj = Activator.CreateInstance<T>();
                PropertyInfo[] objprops = obj.GetType().GetProperties();
                obj = Activator.CreateInstance<T>();
                SPView view = list.DefaultView;
                for (int i = 0; i < objprops.Length; i++)
                {


                    PropertyInfo prop = objprops[i];
                    //if (prop.Name == "ID")
                    //    continue;
                    //Type PrpType;
                    if (prop != null)
                    {
                        if (prop.Name == "ID" || prop.Name == "Title")
                            continue;
                        if (prop.Name.Contains("Text") )
                            list.Fields.Add(prop.Name, SPFieldType.Note, false);
                        else
                            list.Fields.Add(prop.Name, SPFieldType.Text, false);
                        view.ViewFields.Add(prop.Name);
                    }



                }

                view.Update();

                return list;



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), "SPFactory - MapListFieldsFromClass", ex.Message);
            }
            return list;

        }
        public void EnsureLists(SPWeb web)
        {

            web.AllowUnsafeUpdates = true;
            EnsurePresidentProfileList(web);
            EnsurePresidentSpeechList(web);
            EnsurePresidentContactsList(web);
            EnsurePresidentMembershipsList(web);
            EnsurePresidentAwardsList(web);
            EnsurePresidentExperiencesList(web);

        }

        private void EnsurePresidentProfileList(SPWeb web)
        {
            if (web.Lists.TryGetList(ListNames.PresidentProfile) != null)
                return;

            Guid listId = web.Lists.Add(
                ListNames.PresidentProfile,
                "Stores the main president profile data",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];

            if (list != null)
            {
                list = MapListFieldsFromClass<PresidentProfileModel>(list);
            }

            //list.Fields.Add("PresidentNameAr", SPFieldType.Text, false);
            //list.Fields.Add("PresidentTitleAr", SPFieldType.Text, false);
            //list.Fields.Add("UniversityNameAr", SPFieldType.Text, false);
            //list.Fields.Add("ImageUrl", SPFieldType.URL, false);
            //list.Fields.Add("ExternalLink", SPFieldType.URL, false);
            //list.Fields.Add("BiographyTitleAr", SPFieldType.Text, false);
            //list.Fields.Add("BiographyTextAr", SPFieldType.Note, false);


            list.Update();
        }

        private void EnsurePresidentSpeechList(SPWeb web)
        {
            if (web.Lists.TryGetList(ListNames.PresidentSpeech) != null)
                return;

            Guid listId = web.Lists.Add(
                ListNames.PresidentSpeech,
                "Stores speech paragraphs",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];

            if (list != null)
            {
                list = MapListFieldsFromClass<SpeechItem>(list);
            }

            //list.Fields.Add("ParagraphTextAr", SPFieldType.Note, false);
            //list.Fields.Add("SortOrder", SPFieldType.Number, false);
            list.Update();

        }

        private void EnsurePresidentContactsList(SPWeb web)
        {
            if (web.Lists.TryGetList(ListNames.PresidentContacts) != null)
                return;

            Guid listId = web.Lists.Add(
                ListNames.PresidentContacts,
                "Stores contact items",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<ContactItem>(list);
            }
            //list.Fields.Add("ContactLabelAr", SPFieldType.Text, false);
            //list.Fields.Add("ContactValue", SPFieldType.Text, false);
            //list.Fields.Add("ContactType", SPFieldType.Text, false);
            //list.Fields.Add("SortOrder", SPFieldType.Number, false);
            list.Update();

        }

        private void EnsurePresidentMembershipsList(SPWeb web)
        {
            if (web.Lists.TryGetList(ListNames.PresidentMemberships) != null)
                return;

            Guid listId = web.Lists.Add(
                ListNames.PresidentMemberships,
                "Stores memberships",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<MembershipItem>(list);
            }

            //list.Fields.Add("MembershipTextAr", SPFieldType.Note, false);
            //list.Fields.Add("SortOrder", SPFieldType.Number, false);

            list.Update();

        }

        private void EnsurePresidentAwardsList(SPWeb web)
        {
            if (web.Lists.TryGetList(ListNames.PresidentAwards) != null)
                return;

            Guid listId = web.Lists.Add(
                ListNames.PresidentAwards,
                "Stores awards",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<AwardItem>(list);
            }
            //list.Fields.Add("AwardTextAr", SPFieldType.Note, false);
            //list.Fields.Add("SortOrder", SPFieldType.Number, false);

            list.Update();

        }

        private void EnsurePresidentExperiencesList(SPWeb web)
        {
            if (web.Lists.TryGetList(ListNames.PresidentExperiences) != null)
                return;

            Guid listId = web.Lists.Add(
                ListNames.PresidentExperiences,
                "Stores work experiences",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<ExperienceItem>(list);
            }
            //list.Fields.Add("JobTitleAr", SPFieldType.Text, false);
            //list.Fields.Add("OrganizationAr", SPFieldType.Text, false);
            //list.Fields.Add("LocationAr", SPFieldType.Text, false);
            //list.Fields.Add("PeriodAr", SPFieldType.Text, false);
            //list.Fields.Add("SortOrder", SPFieldType.Number, false);

            list.Update();

        }
    }




    public static class ListNames
    {
        public const string PresidentProfile = "PresidentProfile";
        public const string PresidentSpeech = "PresidentSpeech";
        public const string PresidentContacts = "PresidentContacts";
        public const string PresidentMemberships = "PresidentMemberships";
        public const string PresidentAwards = "PresidentAwards";
        public const string PresidentExperiences = "PresidentExperiences";
    }
    public class PresidentProfileModel
    {
        public string ID { get; set; }
        public string PresidentNameAr { get; set; }
        public string PresidentTitleAr { get; set; }
        public string UniversityNameAr { get; set; }
        public string ImageUrl { get; set; }
        public string ExternalLink { get; set; }
        public string BiographyTitleAr { get; set; }
        public string BiographyTextAr { get; set; }
        public string ParagraphTextAr { get; set; }
        public string ParagraphTextEn { get; set; }
        public string ParagraphTitleAr { get; set; }
        public string ParagraphTitleEn { get; set; }


    }

    public class SpeechItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ParagraphTextAr { get; set; }
        public int SortOrder { get; set; }
    }

    public class ContactItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string ContactLabelAr { get; set; }
        public string ContactValue { get; set; }
        public string ContactType { get; set; }
        public int SortOrder { get; set; }
    }

    public class MembershipItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string MembershipTextAr { get; set; }
        public int SortOrder { get; set; }
    }

    public class AwardItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string AwardTextAr { get; set; }
        public int SortOrder { get; set; }
    }

    public class ExperienceItem
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string JobTitleAr { get; set; }
        public string OrganizationAr { get; set; }
        public string LocationAr { get; set; }
        public string PeriodAr { get; set; }
        public int SortOrder { get; set; }
    }

}
