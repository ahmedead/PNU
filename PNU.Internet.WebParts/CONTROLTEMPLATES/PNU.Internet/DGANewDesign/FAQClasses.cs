using Microsoft.SharePoint;
using PNU.Internet.WebParts.CONTROLTEMPLATES.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.DGANewDesign
{
    #region ListNames

    public static class FAQListNames
    {
        public const string FAQCategories   = "FAQCategories";
        public const string FAQQuestions    = "FAQQuestions";
        public const string FAQAnswerItems  = "FAQAnswerItems";
    }

    /// <summary>
    /// ItemType values for FAQAnswerItems.
    /// - "paragraph" : rendered as a <p> block
    /// - "bullet"    : rendered as a <li> inside a shared <ul>
    /// - "intro"     : rendered as a <p> that introduces the list that follows
    /// </summary>
    public static class FAQAnswerTypes
    {
        public const string Paragraph = "paragraph";
        public const string Bullet    = "bullet";
        public const string Intro     = "intro";
    }

    #endregion

    #region Models

    public class FAQCategoryItem
    {
        public string ID { get; set; }
        public string Title { get; set; }         // Category display name (Ar)
        public string IconClass { get; set; }     // e.g. hgi-validation-approval
        public int SortOrder { get; set; }
    }

    public class FAQQuestionItem
    {
        public string ID { get; set; }
        public string Title { get; set; }          // mirror of the question
        public string CategoryName { get; set; }   // FK by name -> FAQCategoryItem.Title
        public string QuestionTextAr { get; set; } // Note field (because name contains "Text")
        public int SortOrder { get; set; }

        // Not persisted — filled after loading answer rows and grouping them
        public List<FAQAnswerItem> AnswerItems { get; set; }
    }

    public class FAQAnswerItem
    {
        public string ID { get; set; }
        public string Title { get; set; }         // short label (can mirror content)
        public string QuestionTitle { get; set; } // FK by title -> FAQQuestionItem.Title
        public string ItemType { get; set; }      // paragraph | bullet | intro
        public string ContentTextAr { get; set; } // Note field (HTML allowed, e.g. <a>, <span>)
        public int SortOrder { get; set; }
    }

    #endregion

    #region Repository

    public class FAQRepository
    {
        public List<FAQCategoryItem> GetCategories(SPWeb web)
        {
            var result = new List<FAQCategoryItem>();

            SPList list = web.Lists.TryGetList(FAQListNames.FAQCategories);
            if (list == null || list.ItemCount == 0) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            SPListItemCollection items = list.GetItems(query);
            if (items != null && items.Count > 0)
            {
                result = SPFactory.MapListItemsToClass<FAQCategoryItem>(items);
            }

            return result;
        }

        public List<FAQQuestionItem> GetQuestions(SPWeb web)
        {
            var result = new List<FAQQuestionItem>();

            SPList list = web.Lists.TryGetList(FAQListNames.FAQQuestions);
            if (list == null || list.ItemCount == 0) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            SPListItemCollection items = list.GetItems(query);
            if (items != null && items.Count > 0)
            {
                result = SPFactory.MapListItemsToClass<FAQQuestionItem>(items);
            }

            return result;
        }

        public List<FAQAnswerItem> GetAnswerItems(SPWeb web)
        {
            var result = new List<FAQAnswerItem>();

            SPList list = web.Lists.TryGetList(FAQListNames.FAQAnswerItems);
            if (list == null || list.ItemCount == 0) return result;

            SPQuery query = new SPQuery
            {
                Query = "<OrderBy><FieldRef Name='SortOrder' Ascending='TRUE' /></OrderBy>"
            };

            SPListItemCollection items = list.GetItems(query);
            if (items != null && items.Count > 0)
            {
                result = SPFactory.MapListItemsToClass<FAQAnswerItem>(items);
            }

            return result;
        }

        public List<FAQQuestionItem> GetQuestionsByCategory(List<FAQQuestionItem> all, string categoryName)
        {
            if (all == null || all.Count == 0) return new List<FAQQuestionItem>();

            return all
                .Where(q => string.Equals(q.CategoryName, categoryName, StringComparison.Ordinal))
                .OrderBy(q => q.SortOrder)
                .ToList();
        }

        public List<FAQAnswerItem> GetAnswersForQuestion(List<FAQAnswerItem> all, string questionTitle)
        {
            if (all == null || all.Count == 0) return new List<FAQAnswerItem>();

            return all
                .Where(a => string.Equals(a.QuestionTitle, questionTitle, StringComparison.Ordinal))
                .OrderBy(a => a.SortOrder)
                .ToList();
        }

        /// <summary>
        /// Attaches answer items to their parent question (in-memory join) so the UI
        /// can render each question's answer block from a grouped list.
        /// </summary>
        public void AttachAnswersToQuestions(List<FAQQuestionItem> questions, List<FAQAnswerItem> answers)
        {
            if (questions == null) return;
            if (answers == null) answers = new List<FAQAnswerItem>();

            foreach (var q in questions)
            {
                q.AnswerItems = answers
                    .Where(a => string.Equals(a.QuestionTitle, q.Title, StringComparison.Ordinal))
                    .OrderBy(a => a.SortOrder)
                    .ToList();
            }
        }
    }

    #endregion

    #region List Initializer

    public class FAQListInitializer
    {
        public SPList MapListFieldsFromClass<T>(SPList list)
        {
            try
            {
                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] objprops = obj.GetType().GetProperties();
                SPView view = list.DefaultView;

                for (int i = 0; i < objprops.Length; i++)
                {
                    PropertyInfo prop = objprops[i];
                    if (prop == null) continue;
                    if (prop.Name == "ID" || prop.Name == "Title") continue;

                    // Skip complex/navigation properties (e.g. AnswerItems on FAQQuestionItem)
                    if (!IsSimpleType(prop.PropertyType)) continue;

                    if (prop.Name.Contains("Text"))
                        list.Fields.Add(prop.Name, SPFieldType.Note, false);
                    else
                        list.Fields.Add(prop.Name, SPFieldType.Text, false);

                    view.ViewFields.Add(prop.Name);
                }

                view.Update();
                return list;
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "FAQListInitializer - MapListFieldsFromClass", ex.Message);
            }
            return list;
        }

        private bool IsSimpleType(Type t)
        {
            if (t == null) return false;
            if (t.IsPrimitive) return true;
            if (t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime)) return true;

            // Nullable<T> -> unwrap
            Type underlying = Nullable.GetUnderlyingType(t);
            if (underlying != null) return IsSimpleType(underlying);

            return false;
        }

        public void EnsureLists(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;

            bool categoriesCreated   = EnsureFAQCategoriesList(web);
            bool questionsCreated    = EnsureFAQQuestionsList(web);
            bool answerItemsCreated  = EnsureFAQAnswerItemsList(web);

            // Seed every list only if it was just created now. This guarantees data is added
            // once (right after creation), and never duplicated on subsequent page loads.
            if (categoriesCreated)  SeedCategories(web);
            if (questionsCreated)   SeedQuestions(web);
            if (answerItemsCreated) SeedAnswerItems(web);
        }

        private bool EnsureFAQCategoriesList(SPWeb web)
        {
            if (web.Lists.TryGetList(FAQListNames.FAQCategories) != null)
                return false;

            Guid listId = web.Lists.Add(
                FAQListNames.FAQCategories,
                "Stores FAQ categories (tabs)",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<FAQCategoryItem>(list);
            }
            list.Update();
            return true;
        }

        private bool EnsureFAQQuestionsList(SPWeb web)
        {
            if (web.Lists.TryGetList(FAQListNames.FAQQuestions) != null)
                return false;

            Guid listId = web.Lists.Add(
                FAQListNames.FAQQuestions,
                "Stores FAQ questions",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<FAQQuestionItem>(list);
            }
            list.Update();
            return true;
        }

        private bool EnsureFAQAnswerItemsList(SPWeb web)
        {
            if (web.Lists.TryGetList(FAQListNames.FAQAnswerItems) != null)
                return false;

            Guid listId = web.Lists.Add(
                FAQListNames.FAQAnswerItems,
                "Stores one row per answer line / bullet / paragraph, linked to a question by title",
                SPListTemplateType.GenericList);

            SPList list = web.Lists[listId];
            if (list != null)
            {
                list = MapListFieldsFromClass<FAQAnswerItem>(list);
            }
            list.Update();
            return true;
        }

        #region Seed Data

        private void SeedCategories(SPWeb web)
        {
            try
            {
                SPList list = web.Lists.TryGetList(FAQListNames.FAQCategories);
                if (list == null) return;

                var categories = new List<FAQCategoryItem>
                {
                    new FAQCategoryItem { Title = "القبول والتسجيل",            IconClass = "hgi-validation-approval", SortOrder = 1 },
                    new FAQCategoryItem { Title = "الدراسة والخدمات الأكاديمية", IconClass = "hgi-calendar-03",         SortOrder = 2 },
                    new FAQCategoryItem { Title = "التحويل والمعادلة",           IconClass = "hgi-mentor",              SortOrder = 3 },
                    new FAQCategoryItem { Title = "المكافآت",                  IconClass = "hgi-diploma",             SortOrder = 4 },
                    new FAQCategoryItem { Title = "الوثائق",                   IconClass = "hgi-diploma",             SortOrder = 5 },
                    new FAQCategoryItem { Title = "الخدمات التقنية والأنظمة",    IconClass = "hgi-search-01",           SortOrder = 6 }
                };

                foreach (var c in categories)
                {
                    SPListItem item = list.Items.Add();
                    item["Title"]     = c.Title;
                    item["IconClass"] = c.IconClass;
                    item["SortOrder"] = c.SortOrder;
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "FAQListInitializer - SeedCategories", ex.Message);
            }
        }

        private void SeedQuestions(SPWeb web)
        {
            try
            {
                SPList list = web.Lists.TryGetList(FAQListNames.FAQQuestions);
                if (list == null) return;

                foreach (var q in GetSeedQuestions())
                {
                    SPListItem item = list.Items.Add();
                    item["Title"]          = q.Title;
                    item["CategoryName"]   = q.CategoryName;
                    item["QuestionTextAr"] = q.QuestionTextAr;
                    item["SortOrder"]      = q.SortOrder;
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "FAQListInitializer - SeedQuestions", ex.Message);
            }
        }

        private void SeedAnswerItems(SPWeb web)
        {
            try
            {
                SPList list = web.Lists.TryGetList(FAQListNames.FAQAnswerItems);
                if (list == null) return;

                foreach (var a in GetSeedAnswerItems())
                {
                    SPListItem item = list.Items.Add();
                    item["Title"]         = a.Title;
                    item["QuestionTitle"] = a.QuestionTitle;
                    item["ItemType"]      = a.ItemType;
                    item["ContentTextAr"] = a.ContentTextAr;
                    item["SortOrder"]     = a.SortOrder;
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),
                    "FAQListInitializer - SeedAnswerItems", ex.Message);
            }
        }

        #region Seed Data — Questions

        private List<FAQQuestionItem> GetSeedQuestions()
        {
            return new List<FAQQuestionItem>
            {
                // ===== القبول والتسجيل =====
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 1, Title = "ما هي شروط القبول في الجامعة؟",               QuestionTextAr = "ما هي شروط القبول في الجامعة؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 2, Title = "ما هي معايير وآلية المفاضلة في القبول؟",       QuestionTextAr = "ما هي معايير وآلية المفاضلة في القبول؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 3, Title = "ما الوثائق المطلوبة للتقديم؟",                   QuestionTextAr = "ما الوثائق المطلوبة للتقديم؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 4, Title = "هل يمكن التقديم على أكثر من جامعة في نفس الوقت؟", QuestionTextAr = "هل يمكن التقديم على أكثر من جامعة في نفس الوقت؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 5, Title = "ماذا أفعل إذا لم تصلني رسالة رقم طلب القبول؟",    QuestionTextAr = "ماذا أفعل إذا لم تصلني رسالة رقم طلب القبول؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 6, Title = "ما الفرق بين إلغاء القبول والانسحاب من القبول؟",  QuestionTextAr = "ما الفرق بين إلغاء القبول والانسحاب من القبول؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 7, Title = "هل يستطيع المقيمون التقديم؟",                     QuestionTextAr = "هل يستطيع المقيمون التقديم؟" },
                new FAQQuestionItem { CategoryName = "القبول والتسجيل", SortOrder = 8, Title = "هل يوجد حد أدنى للمعدل للقبول؟",                  QuestionTextAr = "هل يوجد حد أدنى للمعدل للقبول؟" },

                // ===== الدراسة والخدمات الأكاديمية =====
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 1, Title = "كيف يتم تسجيل المقررات أو الجدول الدراسي؟",          QuestionTextAr = "كيف يتم تسجيل المقررات أو الجدول الدراسي؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 2, Title = "ما طريقة الحذف والإضافة للمقررات؟",                    QuestionTextAr = "ما طريقة الحذف والإضافة للمقررات؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 3, Title = "ما الفرق بين الاعتذار عن فصل دراسي وتأجيل الدراسة؟", QuestionTextAr = "ما الفرق بين الاعتذار عن فصل دراسي وتأجيل الدراسة؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 4, Title = "ما شروط الاعتذار عن مقرر أو فصل دراسي؟",              QuestionTextAr = "ما شروط الاعتذار عن مقرر أو فصل دراسي؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 5, Title = "ما شروط إعادة القيد؟",                                 QuestionTextAr = "ما شروط إعادة القيد؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 6, Title = "ما الحد الأقصى للساعات الدراسية في الفصل الدراسي؟",    QuestionTextAr = "ما الحد الأقصى للساعات الدراسية في الفصل الدراسي؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 7, Title = "ما الحد الأقصى للساعات في الفصل الصيفي؟",              QuestionTextAr = "ما الحد الأقصى للساعات في الفصل الصيفي؟" },
                new FAQQuestionItem { CategoryName = "الدراسة والخدمات الأكاديمية", SortOrder = 8, Title = "كيف يتم تقديم طلب إعادة تصحيح؟",                        QuestionTextAr = "كيف يتم تقديم طلب إعادة تصحيح؟" },

                // ===== التحويل والمعادلة =====
                new FAQQuestionItem { CategoryName = "التحويل والمعادلة", SortOrder = 1, Title = "ما شروط التحويل الداخلي بين التخصصات؟",             QuestionTextAr = "ما شروط التحويل الداخلي بين التخصصات؟" },
                new FAQQuestionItem { CategoryName = "التحويل والمعادلة", SortOrder = 2, Title = "ما شروط التحويل الخارجي من جامعة أخرى؟",             QuestionTextAr = "ما شروط التحويل الخارجي من جامعة أخرى؟" },
                new FAQQuestionItem { CategoryName = "التحويل والمعادلة", SortOrder = 3, Title = "هل يمكن الدراسة كطالبة زائرة في جامعة أخرى؟",         QuestionTextAr = "هل يمكن الدراسة كطالبة زائرة في جامعة أخرى؟" },
                new FAQQuestionItem { CategoryName = "التحويل والمعادلة", SortOrder = 4, Title = "هل تحتسب المقررات التي درستها في جامعة أخرى ضمن المعدل؟", QuestionTextAr = "هل تحتسب المقررات التي درستها في جامعة أخرى ضمن المعدل؟" },
                new FAQQuestionItem { CategoryName = "التحويل والمعادلة", SortOrder = 5, Title = "هل يمكن التراجع عن التحويل بعد قبوله؟",                QuestionTextAr = "هل يمكن التراجع عن التحويل بعد قبوله؟" },
                new FAQQuestionItem { CategoryName = "التحويل والمعادلة", SortOrder = 6, Title = "هل يمكن التحويل لمن سبق لها التحويل؟",                QuestionTextAr = "هل يمكن التحويل لمن سبق لها التحويل؟" },

                // ===== المكافآت =====
                new FAQQuestionItem { CategoryName = "المكافآت", SortOrder = 1, Title = "ما شروط استحقاق المكافأة الجامعية؟",  QuestionTextAr = "ما شروط استحقاق المكافأة الجامعية؟" },
                new FAQQuestionItem { CategoryName = "المكافآت", SortOrder = 2, Title = "هل تصرف المكافأة عند الاعتذار أو التأجيل؟", QuestionTextAr = "هل تصرف المكافأة عند الاعتذار أو التأجيل؟" },
                new FAQQuestionItem { CategoryName = "المكافآت", SortOrder = 3, Title = "هل تصرف مكافأة للطالبات الزائرات؟",   QuestionTextAr = "هل تصرف مكافأة للطالبات الزائرات؟" },
                new FAQQuestionItem { CategoryName = "المكافآت", SortOrder = 4, Title = "كيف أقوم بتعديل الآيبان؟",           QuestionTextAr = "كيف أقوم بتعديل الآيبان؟" },
                new FAQQuestionItem { CategoryName = "المكافآت", SortOrder = 5, Title = "ما هي شروط مكافأة التفوق؟",          QuestionTextAr = "ما هي شروط مكافأة التفوق؟" },

                // ===== الوثائق =====
                new FAQQuestionItem { CategoryName = "الوثائق", SortOrder = 1, Title = "كيف يمكن استخراج السجل الأكاديمي؟",    QuestionTextAr = "كيف يمكن استخراج السجل الأكاديمي؟" },
                new FAQQuestionItem { CategoryName = "الوثائق", SortOrder = 2, Title = "كيف يمكن إصدار وثيقة تخرج بدل فاقد؟",   QuestionTextAr = "كيف يمكن إصدار وثيقة تخرج بدل فاقد؟" },
                new FAQQuestionItem { CategoryName = "الوثائق", SortOrder = 3, Title = "كيف يتم تعديل الاسم في وثيقة التخرج؟",  QuestionTextAr = "كيف يتم تعديل الاسم في وثيقة التخرج؟" },
                new FAQQuestionItem { CategoryName = "الوثائق", SortOrder = 4, Title = "ما هي شروط مراتب الشرف؟",               QuestionTextAr = "ما هي شروط مراتب الشرف؟" },

                // ===== الخدمات التقنية والأنظمة =====
                new FAQQuestionItem { CategoryName = "الخدمات التقنية والأنظمة", SortOrder = 1, Title = "كيف يمكن استعادة كلمة المرور أو اسم المستخدم؟", QuestionTextAr = "كيف يمكن استعادة كلمة المرور أو اسم المستخدم؟" },
                new FAQQuestionItem { CategoryName = "الخدمات التقنية والأنظمة", SortOrder = 2, Title = "لم تصلني رسالة التحقق على الجوال، ماذا أفعل؟",  QuestionTextAr = "لم تصلني رسالة التحقق على الجوال، ماذا أفعل؟" },
                new FAQQuestionItem { CategoryName = "الخدمات التقنية والأنظمة", SortOrder = 3, Title = "كيف أنشئ بريدًا إلكترونيًا جامعيًا؟",             QuestionTextAr = "كيف أنشئ بريدًا إلكترونيًا جامعيًا؟" }
            };
        }

        #endregion

        #region Seed Data — Answer Items

        private List<FAQAnswerItem> GetSeedAnswerItems()
        {
            var list = new List<FAQAnswerItem>();

            // ===== Q: ما هي شروط القبول في الجامعة؟ =====
            AddBullets(list, "ما هي شروط القبول في الجامعة؟", new[]
            {
                "أن تكون الطالبة سعودية الجنسية أو من أم سعودية.",
                "الحصول على شهادة الثانوية العامة أو ما يعادلها من داخل المملكة أو خارجها.",
                "يسمح لمن مضى على شهادة الثانوية العامة أو ما يعادلها أكثر من خمس سنوات بالتقدم على البرامج المدفوعة فقط وفق الشروط المنظمة لذلك.",
                "ألا يكون قد مضى على الثانوية العامة أو ما يعادلها أكثر من سنة واحدة للمتقدمات على السنة التأسيسية للكليات الصحية.",
                "أن تكون الطالبة قد تقدمت لاختباري القدرات والتحصيلي المقدمين من المركز الوطني للقياس وفق الضوابط المعتمدة.",
                "أن تجتاز بنجاح أي اختبار أو مقابلة شخصية يحددها مجلس الجامعة.",
                "أن تكون الطالبة لائقة طبيًا.",
                "مناسبة الحالة الصحية والنفسية للتخصصات الصحية، وفي حال ثبوت خلاف ذلك يحق للجامعة تحويل القبول إلى تخصصات أخرى.",
                "مناسبة الحالة الصحية والبدنية لكلية علوم الرياضة والنشاط البدني، وألا يزيد مؤشر كتلة الجسم عن 27.",
                "ألا تكون الطالبة مقيدة أو مقبولة أو حاصلة سابقًا على البكالوريوس أو مفصولة تأديبيًا من جامعة حكومية أو أي جامعة أخرى.",
                "ألا يكون للطالبة سجل سابق في جامعة الأميرة نورة بنت عبدالرحمن."
            });

            // ===== Q: ما هي معايير وآلية المفاضلة في القبول؟ =====
            AddIntro  (list, "ما هي معايير وآلية المفاضلة في القبول؟", "معايير القبول لجميع كليات الجامعة ما عدا كلية اللغات:", 1);
            AddBullets(list, "ما هي معايير وآلية المفاضلة في القبول؟", new[]
            {
                "30% الثانوية العامة.",
                "30% اختبار القدرات.",
                "40% الاختبار التحصيلي."
            }, startOrder: 2);
            AddIntro  (list, "ما هي معايير وآلية المفاضلة في القبول؟", "معيار القبول لكلية اللغات:", 5);
            AddBullets(list, "ما هي معايير وآلية المفاضلة في القبول؟", new[]
            {
                "50% الثانوية العامة.",
                "15% اختبار القدرات.",
                "15% الاختبار التحصيلي.",
                "20% اختبار STEP.",
                "تتم المفاضلة بين المتقدمات تنافسيًا وفق النسبة المركبة والطاقة الاستيعابية لكل كلية."
            }, startOrder: 6);

            // ===== Q: ما الوثائق المطلوبة للتقديم؟ =====
            AddParagraph(list, "ما الوثائق المطلوبة للتقديم؟", "لا توجد وثائق مطلوبة، حيث يتم التقديم عبر المنصة الوطنية للقبول الموحد.");

            // ===== Q: هل يمكن التقديم على أكثر من جامعة في نفس الوقت؟ =====
            AddParagraph(list, "هل يمكن التقديم على أكثر من جامعة في نفس الوقت؟", "يتم التقديم عبر المنصة الوطنية للقبول الموحد، وتتيح للطالبات التقديم على أكثر من جامعة، ويكون القبول النهائي على تخصص واحد وجامعة واحدة.");

            // ===== Q: ماذا أفعل إذا لم تصلني رسالة رقم طلب القبول؟ =====
            AddParagraph(list, "ماذا أفعل إذا لم تصلني رسالة رقم طلب القبول؟", "يمكنك التواصل مع الدعم الفني عبر المنصة الوطنية للقبول الموحد.");

            // ===== Q: ما الفرق بين إلغاء القبول والانسحاب من القبول؟ =====
            AddBullets(list, "ما الفرق بين إلغاء القبول والانسحاب من القبول؟", new[]
            {
                "إلغاء القبول: يتم قبل بدء الدراسة.",
                "الانسحاب من القبول: يتم تنفيذه أيضًا قبل بدء الدراسة وفق الإجراء المعتمد."
            });

            // ===== Q: هل يستطيع المقيمون التقديم؟ =====
            AddParagraph(list, "هل يستطيع المقيمون التقديم؟", "يمكن للمقيمين التقديم عبر منصة <span>ادرس في السعودية</span>.");

            // ===== Q: هل يوجد حد أدنى للمعدل للقبول؟ =====
            AddParagraph(list, "هل يوجد حد أدنى للمعدل للقبول؟", "لا يوجد حد أدنى عام، لأن القبول يتم بالمفاضلة التنافسية بين المتقدمات، ما عدا مسار قبول الطالبات المتميزات أكاديميًا حيث يجب ألا تقل النسبة المركبة عن 95%.");

            // ===== Q: كيف يتم تسجيل المقررات أو الجدول الدراسي؟ =====
            AddParagraph(list, "كيف يتم تسجيل المقررات أو الجدول الدراسي؟", "يتم التسجيل عبر الخدمة الذاتية حسب <a href='academic-calendar.html'><span>التقويم الجامعي</span> <i class='hgi hgi-stroke hgi-link-square-02'></i></a>.");

            // ===== Q: ما طريقة الحذف والإضافة للمقررات؟ =====
            AddParagraph(list, "ما طريقة الحذف والإضافة للمقررات؟", "يتم الحذف والإضافة عبر الخدمة الذاتية حسب <a href='academic-calendar.html'><span>التقويم الجامعي</span> <i class='hgi hgi-stroke hgi-link-square-02'></i></a>.");

            // ===== Q: ما الفرق بين الاعتذار عن فصل دراسي وتأجيل الدراسة؟ =====
            AddBullets(list, "ما الفرق بين الاعتذار عن فصل دراسي وتأجيل الدراسة؟", new[]
            {
                "لا يحتسب التأجيل من المدة النظامية للتخرج.",
                "يحتسب الاعتذار من المدة النظامية للتخرج."
            });

            // ===== Q: ما شروط الاعتذار عن مقرر أو فصل دراسي؟ =====
            AddIntro  (list, "ما شروط الاعتذار عن مقرر أو فصل دراسي؟", "شروط الاعتذار عن مقرر:", 1);
            AddBullets(list, "ما شروط الاعتذار عن مقرر أو فصل دراسي؟", new[]
            {
                "ألا يقل عدد الساعات عن الحد الأدنى للعبء الدراسي.",
                "لا يسمح بالاعتذار عن مقررات السنة الأولى.",
                "يسمح لطالبة البكالوريوس بالاعتذار عن خمسة مقررات خلال الدراسة، على ألا تزيد عن مقررين في الفصل الواحد.",
                "يسمح لطالبة الدبلوم بالاعتذار عن ثلاثة مقررات خلال الدراسة، على ألا تزيد عن مقرر واحد في الفصل الواحد.",
                "لا يسمح بالاعتذار عن مقرر لطالبات معهد اللغة العربية."
            }, startOrder: 2);

            // ===== Q: ما شروط إعادة القيد؟ =====
            AddIntro  (list, "ما شروط إعادة القيد؟", "ضوابط إعادة القيد:", 1);
            AddBullets(list, "ما شروط إعادة القيد؟", new[]
            {
                "يحق للطالبة التقدم بطلب إعادة القيد إذا لم تتجاوز أربعة فصول دراسية لمرحلة البكالوريوس أو فصلين دراسيين لمرحلة الدبلوم من تاريخ الانقطاع.",
                "يشترط للموافقة على الطلب أن تتمكن الطالبة من إكمال متطلبات التخرج بعد إعادة القيد خلال المدة المسموح بها نظامًا.",
                "لا يحق إعادة القيد للطالبات المنقطعات في فصل القبول، أو من سبق إعادة قيدهن، أو المفصولات أكاديميًا، أو المطوي قيدهن في السنة التأسيسية."
            }, startOrder: 2);

            // ===== Q: ما الحد الأقصى للساعات الدراسية في الفصل الدراسي؟ =====
            AddParagraph(list, "ما الحد الأقصى للساعات الدراسية في الفصل الدراسي؟", "الحد الأقصى هو 22 ساعة معتمدة.");

            // ===== Q: ما الحد الأقصى للساعات في الفصل الصيفي؟ =====
            AddParagraph(list, "ما الحد الأقصى للساعات في الفصل الصيفي؟", "الحد الأقصى هو 9 ساعات معتمدة.");

            // ===== Q: كيف يتم تقديم طلب إعادة تصحيح؟ =====
            AddParagraph(list, "كيف يتم تقديم طلب إعادة تصحيح؟", "تتقدم الطالبة إلى رئيسة القسم الذي يتبعه المقرر بطلب إعادة تصحيح ورقة الإجابة خلال مدة لا تتجاوز خمسة عشر يومًا من تاريخ إعلان النتيجة.");

            // ===== Q: ما شروط التحويل الداخلي بين التخصصات؟ =====
            AddParagraph(list, "ما شروط التحويل الداخلي بين التخصصات؟", "يمكن الاطلاع على دليل التحويل الداخلي عبر موقع الجامعة.");

            // ===== Q: ما شروط التحويل الخارجي من جامعة أخرى؟ =====
            AddBullets(list, "ما شروط التحويل الخارجي من جامعة أخرى؟", new[]
            {
                "تقديم الطلب في المدة المحددة للتحويل من خارج الجامعة في التقويم الدراسي.",
                "الالتزام بشروط وضوابط وإجراءات التحويل المعلنة على موقع الجامعة.",
                "أن تكون الطالبة قد درست في جامعة أو كلية محلية أو أجنبية مرخصة من جهة الاختصاص في بلد الدراسة.",
                "ألا تكون الطالبة منقطعة أو منسحبة أو مفصولة من الجامعة المحولة منها.",
                "وجود أسباب مقنعة تستدعي التحويل إلى جامعة الأميرة نورة بنت عبدالرحمن مع تقديم ما يثبت ذلك.",
                "ألا يكون للطالبة سجل سابق في جامعة الأميرة نورة بنت عبدالرحمن.",
                "ألا يقل المعدل التراكمي عن الحد المعلن في شروط التحويل وقت التقديم.",
                "أن يكون التحويل على التخصص الذي كانت الطالبة مقيدة فيه في الجامعة المحولة منها.",
                "أن تكون الطالبة قد أمضت عامًا دراسيًا على الأقل في الجامعة التي ترغب التحويل منها.",
                "أن تدرس الطالبة في جامعة الأميرة نورة ما لا يقل عن 60% من متطلبات التخرج.",
                "أن تحقق المتقدمة شروط ومؤشرات القبول أو التخصيص للبرنامج المراد التحويل إليه في نفس العام الدراسي.",
                "استيفاء أي شروط أخرى يحددها مجلس الكلية المعنية وتقرها عمادة القبول والتسجيل."
            });

            // ===== Q: هل يمكن الدراسة كطالبة زائرة في جامعة أخرى؟ =====
            AddParagraph(list, "هل يمكن الدراسة كطالبة زائرة في جامعة أخرى؟", "نعم، يمكن ذلك وفق شروط وضوابط محددة.");

            // ===== Q: هل تحتسب المقررات التي درستها في جامعة أخرى ضمن المعدل؟ =====
            AddParagraph(list, "هل تحتسب المقررات التي درستها في جامعة أخرى ضمن المعدل؟", "تثبت في السجل الأكاديمي للطالبة بيانات المقررات التي عودلت لها، ولا تدخل تقديراتها في احتساب المعدل الفصلي أو التراكمي في جامعة الأميرة نورة بنت عبدالرحمن.");

            // ===== Q: هل يمكن التراجع عن التحويل بعد قبوله؟ =====
            AddParagraph(list, "هل يمكن التراجع عن التحويل بعد قبوله؟", "لا يحق للطالبة العدول عن التحويل بعد قبوله.");

            // ===== Q: هل يمكن التحويل لمن سبق لها التحويل؟ =====
            AddBullets(list, "هل يمكن التحويل لمن سبق لها التحويل؟", new[]
            {
                "يكون التحويل بين كليات الجامعة لمرة واحدة فقط طوال مدة دراسة الطالبة في الجامعة.",
                "يكون التحويل من تخصص إلى آخر داخل الكلية لمرة واحدة فقط طوال مدة دراسة الطالبة في الجامعة."
            });

            // ===== Q: ما شروط استحقاق المكافأة الجامعية؟ =====
            AddBullets(list, "ما شروط استحقاق المكافأة الجامعية؟", new[]
            {
                "أن تكون الطالبة منتظمة.",
                "أن تكون سعودية أو من أم سعودية أو أم مواطن سعودي أو طالبة منحة رسمية من خارج المملكة.",
                "ألا يقل المعدل التراكمي عن 2 من 5.",
                "تصرف المكافأة خلال مدة البرنامج الدراسي."
            });

            // ===== Q: هل تصرف المكافأة عند الاعتذار أو التأجيل؟ =====
            AddParagraph(list, "هل تصرف المكافأة عند الاعتذار أو التأجيل؟", "يتوقف صرف المكافأة عند التأجيل أو الاعتذار عن دراسة الفصل الدراسي.");

            // ===== Q: هل تصرف مكافأة للطالبات الزائرات؟ =====
            AddParagraph(list, "هل تصرف مكافأة للطالبات الزائرات؟", "تصرف المكافأة الشهرية لطالبة جامعة الأميرة نورة بنت عبدالرحمن الزائرة لجامعة أخرى إذا كانت مستحقة لها، بعد تقديم نتائجها إلى عمادة القبول والتسجيل.");

            // ===== Q: كيف أقوم بتعديل الآيبان؟ =====
            AddParagraph(list, "كيف أقوم بتعديل الآيبان؟", "يتم تعديل رقم الآيبان عن طريق إدارة المكافآت، وذلك بإرسال خطاب برقم الآيبان الجديد من البريد الجامعي إلى البريد الخاص بالإدارة <a href='mailto:dsa_rewards@pnu.edu.sa'><span>dsa_rewards@pnu.edu.sa</span> <i class='hgi hgi-stroke hgi-link-square-02'></i></a>.");

            // ===== Q: ما هي شروط مكافأة التفوق؟ =====
            AddIntro  (list, "ما هي شروط مكافأة التفوق؟", "تصرف مكافأة التفوق في نهاية كل عام دراسي بالشروط التالية:", 1);
            AddBullets(list, "ما هي شروط مكافأة التفوق؟", new[]
            {
                "أن تكون الطالبة مسجلة وحاصلة على معدل فصلي 4.50 فأعلى في كل فصل من فصلين متتاليين.",
                "ألا يقل عدد ساعات المعدل عن 12 ساعة في كل فصل من فصلين متتاليين، ويستثنى من ذلك الخريجات في فصل التخرج.",
                "أن تكون الطالبة غير متجاوزة للمدة النظامية للبرنامج.",
                "ألا يكون لدى الطالبة مقرر بتقدير غير مكتمل (IC)."
            }, startOrder: 2);
            AddParagraph(list, "ما هي شروط مكافأة التفوق؟", "مقدار مكافأة التفوق هو 1000 ريال لجميع التخصصات، ويتم إيداعها تلقائيًا في حساب الطالبة المستحقة دون الحاجة إلى المطالبة بها.", order: 10);

            // ===== Q: كيف يمكن استخراج السجل الأكاديمي؟ =====
            AddBullets(list, "كيف يمكن استخراج السجل الأكاديمي؟", new[]
            {
                "مراجعة إدارة المحاسبة في عمادة القبول والتسجيل بمبنى الإدارة المركزية (P05) لدفع بدل فاقد السجل الأكاديمي وقدره 50 ريالًا.",
                "مراجعة إدارة الوثائق لطباعة السجل الأكاديمي."
            });

            // ===== Q: كيف يمكن إصدار وثيقة تخرج بدل فاقد؟ =====
            AddBullets(list, "كيف يمكن إصدار وثيقة تخرج بدل فاقد؟", new[]
            {
                "مراجعة إدارة المحاسبة في عمادة القبول والتسجيل بمبنى الإدارة المركزية (P05) لدفع بدل فاقد الوثيقة وقدره 50 ريالًا.",
                "مراجعة إدارة الوثائق لطباعة الوثيقة بدل فاقد."
            });

            // ===== Q: كيف يتم تعديل الاسم في وثيقة التخرج؟ =====
            AddBullets(list, "كيف يتم تعديل الاسم في وثيقة التخرج؟", new[]
            {
                "مراجعة إدارة الدعم والمساندة في عمادة القبول والتسجيل بمبنى الإدارة المركزية (P05) لتعديل الاسم في النظام الأكاديمي.",
                "مراجعة إدارة المحاسبة في عمادة القبول والتسجيل لدفع مبلغ تعديل الاسم في الوثيقة وقدره 50 ريالًا.",
                "مراجعة إدارة الوثائق لطباعة الوثيقة بعد التعديل."
            });

            // ===== Q: ما هي شروط مراتب الشرف؟ =====
            AddParagraph(list, "ما هي شروط مراتب الشرف؟", "تمنح جامعة الأميرة نورة بنت عبدالرحمن طالباتها مرتبة الشرف الأولى أو الثانية، وتسجل في وثيقة التخرج للطالبة وفق الضوابط التالية:", order: 1);
            AddIntro    (list, "ما هي شروط مراتب الشرف؟", "الشروط العامة:", 2);
            AddBullets  (list, "ما هي شروط مراتب الشرف؟", new[]
            {
                "ألا تكون الطالبة قد رسبت في أي مقرر درسته في الجامعة أو في جامعة أخرى.",
                "أن تكون قد أكملت متطلبات التخرج في المدة النظامية.",
                "أن تكون قد درست في الجامعة ما لا يقل عن 60% من متطلبات التخرج.",
                "ألا تكون قد فصلت فصلًا تأديبيًا."
            }, startOrder: 3);
            AddIntro    (list, "ما هي شروط مراتب الشرف؟", "شروط المعدل التراكمي عند التخرج:", 7);
            AddBullets  (list, "ما هي شروط مراتب الشرف؟", new[]
            {
                "تمنح مرتبة الشرف الأولى إذا حصلت الطالبة على معدل تراكمي لا يقل عن 4.75.",
                "تمنح مرتبة الشرف الثانية إذا حصلت الطالبة على معدل تراكمي من 4.25 إلى 4.74."
            }, startOrder: 8);

            // ===== Q: كيف يمكن استعادة كلمة المرور أو اسم المستخدم؟ =====
            AddParagraph(list, "كيف يمكن استعادة كلمة المرور أو اسم المستخدم؟", "يمكن استعادة كلمة المرور من خلال الرابط <a href='https://eservice.pnu.edu.sa' target='_blank' rel='noopener'><span>eservice.pnu.edu.sa</span> <i class='hgi hgi-stroke hgi-link-square-02'></i></a>.");

            // ===== Q: لم تصلني رسالة التحقق على الجوال، ماذا أفعل؟ =====
            AddParagraph(list, "لم تصلني رسالة التحقق على الجوال، ماذا أفعل؟", "يمكنك تحديث الصفحة، أو تغيير المتصفح، أو اختيار استلام رمز التحقق من خلال البريد البديل.");

            // ===== Q: كيف أنشئ بريدًا إلكترونيًا جامعيًا؟ =====
            AddParagraph(list, "كيف أنشئ بريدًا إلكترونيًا جامعيًا؟", "يتم إرسال الطلب من خلال بريد إدارتك الرئيسية إلى الإدارة العامة للتحول الرقمي.");

            return list;
        }

        // ---------- Seed helpers (keep GetSeedAnswerItems tidy) ----------

        private void AddParagraph(List<FAQAnswerItem> list, string questionTitle, string text, int order = 1)
        {
            list.Add(new FAQAnswerItem
            {
                Title         = Truncate(text, 80),
                QuestionTitle = questionTitle,
                ItemType      = FAQAnswerTypes.Paragraph,
                ContentTextAr = text,
                SortOrder     = order
            });
        }

        private void AddIntro(List<FAQAnswerItem> list, string questionTitle, string text, int order)
        {
            list.Add(new FAQAnswerItem
            {
                Title         = Truncate(text, 80),
                QuestionTitle = questionTitle,
                ItemType      = FAQAnswerTypes.Intro,
                ContentTextAr = text,
                SortOrder     = order
            });
        }

        private void AddBullets(List<FAQAnswerItem> list, string questionTitle, string[] bullets, int startOrder = 1)
        {
            int order = startOrder;
            foreach (var b in bullets)
            {
                list.Add(new FAQAnswerItem
                {
                    Title         = Truncate(b, 80),
                    QuestionTitle = questionTitle,
                    ItemType      = FAQAnswerTypes.Bullet,
                    ContentTextAr = b,
                    SortOrder     = order++
                });
            }
        }

        private string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Length <= max ? s : s.Substring(0, max);
        }

        #endregion

        #endregion
    }

    #endregion
}
