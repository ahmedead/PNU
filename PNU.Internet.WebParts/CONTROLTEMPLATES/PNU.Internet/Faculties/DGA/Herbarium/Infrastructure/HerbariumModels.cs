using System;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.Faculties.DGA.Herbarium
{
    public class HerbariumHeaderModel
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string ImageCaption { get; set; }
    }

    public class HerbariumOverviewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
    }

    public class HerbariumStatModel
    {
        public int Id { get; set; }
        public string StatValue { get; set; }
        public string Title { get; set; }
        public int ItemOrder { get; set; }
    }

    public class HerbariumMissionModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class HerbariumObjectiveModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ItemOrder { get; set; }

        public string TargetId
        {
            get { return "herbariumObjective" + Id + "Collapse"; }
        }

        public string HeadingId
        {
            get { return "herbariumObjective" + Id + "Heading"; }
        }
    }

    public class HerbariumServiceModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public int ItemOrder { get; set; }
    }

    public class HerbariumMilestoneModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BadgeText { get; set; }
        public string BadgeClass { get; set; }
        public int ItemOrder { get; set; }
    }

    public class HerbariumContactModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IconClass { get; set; }
        public string ButtonText { get; set; }
    }
}
