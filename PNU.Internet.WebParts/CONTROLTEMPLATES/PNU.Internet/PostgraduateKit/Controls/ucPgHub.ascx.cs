using System;
using System.ComponentModel;
using System.Web.UI;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.PostgraduateKit.Controls
{
    /// <summary>
    /// The kit landing page: the hero carousel, the overview and the three station cards.
    /// This is index.html of the design; the three stations themselves are rendered by
    /// ucPostgraduateKit with the matching Phase.
    /// </summary>
    public partial class ucPgHub : UserControl
    {
        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowHero { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowIntro { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowCards { get; set; }

        [Browsable(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public bool ShowStyles { get; set; }

        public ucPgHub()
        {
            ShowHero = true;
            ShowIntro = true;
            ShowCards = true;
            ShowStyles = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                PgListProvisioner.EnsureAllListsExist();

                if (phHero != null) phHero.Visible = ShowHero;
                if (phIntro != null) phIntro.Visible = ShowIntro;
                if (phCards != null) phCards.Visible = ShowCards;
                if (ucStyles != null) ucStyles.Visible = ShowStyles;
            }
            catch (Exception ex)
            {
                PgLog.Write("ucPgHub.OnInit", ex);
            }
        }
    }
}
