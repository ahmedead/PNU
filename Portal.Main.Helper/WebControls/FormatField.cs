using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.Main.Helper.WebControls
{
	[ToolboxData("<{0}:FormatField runat=\"server\" />")]
	public class FormatField : WebControl
	{
		private string _FieldName;

		[Bindable(true), DefaultValue(""), Localizable(false)]
		public string FieldName
		{
			get { return _FieldName; }
			set { _FieldName = value; }
		}

		private string _Format;

		[Bindable(true), DefaultValue(""), Localizable(false)]
		public string Format
		{
			get { return _Format; }
			set { _Format = value; }
		}

		private bool _Higri;

		[Bindable(true), DefaultValue(""), Localizable(false)]
		public bool Higri
		{
			get { return _Higri; }
			set { _Higri = value; }
		}

		public enum LanguageEnum
		{
			Arabic,
			English
		}

		public enum FieldType
		{
			String,
			Number,
			Date
		}
		public enum DateMode
		{
			Hijri,
			Miladi,
			Other
		}

		private FieldType _Type;

		[Bindable(true), DefaultValue(""), Localizable(false)]
		public FieldType Type
		{
			get { return _Type; }
			set { _Type = value; }
		}

		private DateMode _Mode;

		[Bindable(true), DefaultValue(""), Localizable(false)]
		public DateMode Mode
		{
			get { return _Mode; }
			set { _Mode = value; }
		}

		private LanguageEnum _Language;
		[Bindable(true), DefaultValue(""), Localizable(false)]
		public LanguageEnum Language
		{
			get { return _Language; }
			set { _Language = value; }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			try
			{
				SPListItem currentItem = SPContext.Current.List.Items.GetItemById(SPContext.Current.Item.ID);
				string fieldValue = currentItem[_FieldName].ToString();
				switch (_Type)
				{
					case FieldType.Date:
						DateTime date = DateTime.Parse(fieldValue);
						if (_Mode == DateMode.Other)
						{
							fieldValue = date.ToString(Format); ;
						}
						else if (_Mode == DateMode.Hijri)
						{
							fieldValue = date.ToString(this._Format, new CultureInfo("ar-SA"));
						}
						else if (_Mode == DateMode.Miladi)
						{
							fieldValue = (_Language == LanguageEnum.Arabic)
								? date.ToString(this._Format, new CultureInfo("ar-AE"))
								: date.ToString(this._Format, new CultureInfo("en-US"));

						}
						break;
				}

				writer.Write(fieldValue);
			}
			catch { }
		}
	}
}