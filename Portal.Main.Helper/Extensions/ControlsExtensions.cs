// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlsExtensions.cs" company="SURE International Technology">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   The controls extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Extensions
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Web.UI.WebControls;

    /// <summary>
    ///     The controls extensions.
    /// </summary>
    public static class ControlsExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Binds the control with the data source.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        /// <author>
        ///     Ahmed Magdy (amagdy@sure.com.sa)
        /// </author>
        /// <created>03/09/2015</created>
        public static void BindWith(this DataBoundControl control, ICollection dataSource)
        {
            control.DataSource = dataSource;
            control.DataBind();
        }

        /// <summary>
        /// Binds the dropdown list with a data source with Text and Value properties.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        /// <param name="isRequired">
        /// if set to <c>true</c> [is required].
        /// </param>
        /// <author>Ahmed Magdy (amagdy@sure.com.sa)</author>
        /// <created>03/09/2015</created>
        public static void BindWith(this DropDownList control, ICollection dataSource, bool isRequired = true)
        {
            var dataTextField = string.IsNullOrWhiteSpace(control.DataTextField) ? "Text" : control.DataTextField;
            var dataValueField = string.IsNullOrWhiteSpace(control.DataValueField) ? "Value" : control.DataValueField;
            control.BindWith(dataSource, dataValueField, dataTextField, isRequired);
        }

        /// <summary>
        /// Binds the dropdown list with the data source.
        /// </summary>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="dataSource">
        /// The data source.
        /// </param>
        /// <param name="dataValueField">
        /// The data value field.
        /// </param>
        /// <param name="dataTextField">
        /// The data text field.
        /// </param>
        /// <param name="isRequired">
        /// if set to <c>true</c> [is required].
        /// </param>
        /// <author>Ahmed Magdy (amagdy@sure.com.sa)</author>
        /// <created>03/09/2015</created>
        public static void BindWith(
            this DropDownList control, 
            ICollection dataSource, 
            string dataValueField, 
            string dataTextField, 
            bool isRequired = true)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (!string.IsNullOrWhiteSpace(dataTextField))
            {
                control.DataTextField = dataTextField;
            }

            if (!string.IsNullOrWhiteSpace(dataValueField))
            {
                control.DataValueField = dataValueField;
            }

            control.DataSource = dataSource;
            control.DataBind();

            if (!isRequired)
            {
                return;
            }

            // Later we can add another defualt text.
            var defaultText = string.Empty;
            control.Items.Insert(0, new ListItem(defaultText, string.Empty));
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">
        /// The type
        /// </typeparam>
        /// <param name="textBox">
        /// The text box.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// Get the value/text of the control with the value
        /// </returns>
        /// <author>
        ///     Ahmed Magdy (amagdy@sure.com.sa)
        /// </author>
        /// <created>08/09/2015</created>
        public static T GetValue<T>(this TextBox textBox, T defaultValue = default(T))
        {
            return textBox.TrimmedText().To(defaultValue);
        }

        /// <summary>
        /// Gets the select value of the dropdown list.
        /// </summary>
        /// <typeparam name="T">
        /// The type
        /// </typeparam>
        /// <param name="dropDownList">
        /// The drop down list.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// Get the value of the control with the value.
        /// </returns>
        /// <author>Ahmed Magdy (amagdy@sure.com.sa)</author>
        /// <created>08/09/2015</created>
        public static T GetValue<T>(this DropDownList dropDownList, T defaultValue = default(T))
        {
            return dropDownList.SelectedValue.To(defaultValue);
        }

        /// <summary>
        /// Sets the value of the dropdown list without thowing exception.
        /// </summary>
        /// <param name="dropDownList">
        /// The drop down list.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// True if the item was found; otherwise false.
        /// </returns>
        /// <author>
        ///     Ahmed Magdy (amagdy@sure.com.sa)
        /// </author>
        /// <created>13/09/2015</created>
        public static bool SetValue(this DropDownList dropDownList, object value)
        {
            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            dropDownList.SelectedIndex = -1;
            foreach (ListItem listItem in dropDownList.Items)
            {
                if (listItem.Value == valueString)
                {
                    listItem.Selected = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="listControl">
        /// The list control.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <author>Ahmed Magdy (amagdy@sure.com.sa)</author>
        /// <created>03/11/2015</created>
        public static bool SetValue(this ListControl listControl, object value)
        {
            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            listControl.SelectedIndex = -1;
            foreach (ListItem listItem in listControl.Items)
            {
                if (listItem.Value == valueString)
                {
                    listItem.Selected = true;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the value/Text to the TextBox.
        /// </summary>
        /// <param name="textBox">
        /// The text box.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <author>
        ///     Ahmed Magdy (amagdy@sure.com.sa)
        /// </author>
        /// <created>15/09/2015</created>
        public static void SetValue(this TextBox textBox, object value)
        {
            var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
            textBox.Text = valueString;
        }

        /// <summary>
        /// Gets the trimmed text.
        /// </summary>
        /// <param name="textBox">
        /// The text box.
        /// </param>
        /// <returns>
        /// The trimmed text
        /// </returns>
        /// <author>Ahmed Magdy (amagdy@sure.com.sa)</author>
        /// <created>08/09/2015</created>
        public static string TrimmedText(this TextBox textBox)
        {
            return textBox.Text.Trim();
        }

        #endregion
    }
}