using DevHelper;
using System;
using System.Web.UI.WebControls;
using XtractPro.Multimedia;

namespace Portal.Main.WebApp.Controls.Common
{
    public partial class Captcha : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Image width
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// To be used by captcha control
        /// </summary>
        public const string CAPTCHA_ID = "__CaptchaImage__";

        /// <summary>
        /// Validation group name
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                cvCaptcha.ValidationGroup = value;
                txtCaptcha.ValidationGroup = value;
                rvtxtCaptcha.ValidationGroup = value;
                rxptxtCaptcha.ValidationGroup = value;
            }
        }

        /// <summary>
        /// Custom error message
        /// </summary>
        public string ErrorMessage
        {
            set
            {
                cvCaptcha.ErrorMessage = value;
            }
        }

        /// <summary>
        /// Required error messae
        /// </summary>
        public string RequirdErrorMessage
        {
            set
            {
                rvtxtCaptcha.ErrorMessage = value;
            }
        }

        /// <summary>
        /// Tab index on the form
        /// </summary>
        public short TabIndex
        {
            set
            {
                txtCaptcha.TabIndex = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cvCaptcha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (IsValid())
                {
                    args.IsValid = true;
                    txtCaptcha.Text = string.Empty;
                    //lblError.Visible = false;
                }
                else
                {
                    args.IsValid = false;
                    txtCaptcha.Text = string.Empty;
                    //lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Fatal(ex);
                txtCaptcha.Text = String.Empty;
                args.IsValid = false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the entered characters are valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            try
            {
                CaptchaImage image = Session[CAPTCHA_ID] as CaptchaImage;
                if (image == null)
                    return false;
                bool result = (String.Compare(image.Text, CommonHelper.ConvertToEnglishNumeral(txtCaptcha.Text), false) == 0);
				txtCaptcha.Text = string.Empty;
				Session.Remove(CAPTCHA_ID);
				return result;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Fatal(ex);
                txtCaptcha.Text = string.Empty;
                return false;
            }
        }

        public void ClearCapatchaText()
        {
            txtCaptcha.Text = string.Empty;
			Session.Remove(CAPTCHA_ID);
		}

        #endregion


    }
}