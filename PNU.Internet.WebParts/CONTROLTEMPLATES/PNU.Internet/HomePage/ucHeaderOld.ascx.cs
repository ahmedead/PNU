using Microsoft.IdentityModel.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.ApplicationPages.Calendar.Exchange;
using Microsoft.SharePoint.Client;
using Org.BouncyCastle.Ocsp;
using Portal.Main.Helper;
using Portal.Main.Helper.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace PNU.Internet.WebParts.CONTROLTEMPLATES.PNU.Internet.HomePage
{
    public partial class ucHeaderOld : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    imgLogo.Attributes["src"] = SPFactory.GetPNUresResource("Logo");
                    img2.Attributes["src"] = SPFactory.GetPNUresResource("Logo");
                    aLinkHome.Attributes.Add("href", SPFactory.GetSiteURL());

                    LoadMenu();

                }
                divSignIn.Visible = false;
                divSignOut.Visible = false;
                //lnkbtn_signout.Visible = false;
                if (SPContext.Current.Web.CurrentUser == null)
                {
                    divSignIn.Visible = true;
                }
                else
                {
                    divSignOut.Visible = true;
                    //lnkbtn_signout.Visible = true;
                }


                Publics.AddVisitorsCount(HttpContext.Current.Request.Url.ToString(), this.Page.Title);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        private void LoadMenu()
        {
            try
            {
                string MenuLevel1 = "TopMenuLevel1";
                string MenuLevel2 = "TopMenuLevel2";
                using (SPSite site = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        List<ItemWithSub> items = new List<ItemWithSub>();
                        SPList mainLlist = null;
                        //mainLlist = web.Lists.TryGetList(MenuLevel1);
                        string Parentweb = web.Url + "/";
                        if (Parentweb.Contains(site.Url))
                        {
                            Parentweb = Parentweb.Replace(site.Url, "");
                        }
                        while (mainLlist == null)
                        {
                            try
                            {
                                string HTML = "";
                                using (SPWeb Pweb = site.OpenWeb(Parentweb))
                                {
                                    mainLlist = Pweb.Lists.TryGetList(MenuLevel1);
                                    if (mainLlist != null)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        SPQuery query = new SPQuery()
                                        {
                                            Query = $@"<Where><Eq><FieldRef Name='Visibility' /><Value Type='Boolean'>1</Value></Eq><</Where>
                                                    <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>"
                                        };
                                        SPListItemCollection sPListItemCollection = mainLlist.GetItems(query);
                                        List<TopMenuLevel1> _Level1 = new List<TopMenuLevel1>();
                                        List<TopMenuLevel1> _AllData = new List<TopMenuLevel1>();
                                        List<TopMenuLevel2> _Level2 = new List<TopMenuLevel2>();
                                        List<TopMenuLevel3> _Level3 = new List<TopMenuLevel3>();
                                        _Level1 = SPFactory.MapListItemsToClass<TopMenuLevel1>(sPListItemCollection);

                                        for (int i=0; i<_Level1.Count;i++)
                                        {

                                            TopMenuLevel1 _item1 = new TopMenuLevel1();
                                            _item1.LVL2 = new List<TopMenuLevel2>();
                                            query = new SPQuery()
                                            {
                                                //Query = $@"<Where><And><Contains><FieldRef Name='parent' /><Value Type='Lookup'>{_Level1[i].Title}</Value></Contains><Eq><FieldRef Name='Visibility' /><Value Type='Boolean'>1</Value></Eq></And></Where>
                                                //    <OrderBy><FieldRef Name='ItemOrder' Ascending='TRUE' /></OrderBy>"

                                                Query = $@"<Where>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='Parent' />
                                                                    <Value Type='Lookup'>{_Level1[i].Title}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='Visibility' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </Where>
                                                           <OrderBy>
                                                              <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                                           </OrderBy>"
                                            };
                                            SPList Sublist = Pweb.Lists.TryGetList(MenuLevel2);
                                            SPListItemCollection sPListItemCollectionLVL2 = Sublist.GetItems(query);

                                            _Level2 = new List<TopMenuLevel2>();
                                            _Level2 = SPFactory.MapListItemsToClass<TopMenuLevel2>(sPListItemCollectionLVL2);
                                            _Level1[i].LVL2 = new List<TopMenuLevel2>();
                                            
                                            if (_Level2 != null && _Level2.Count > 0)
                                            {
                                                _Level1[i].LVL2 = new List<TopMenuLevel2>();
                                                _Level1[i].LVL2 = _Level2;
                                                //Adding level3
                                                for (int j = 0;j< _Level2.Count;j++)
                                                {

                                                    var sub = new Dictionary<string, object>();
                                                    SPQuery Subquery = new SPQuery()
                                                    {
                                                        Query = $@"<Where>
                                                              <And>
                                                                 <Eq>
                                                                    <FieldRef Name='Parent' />
                                                                    <Value Type='Lookup'>{_Level2[j].Title}</Value>
                                                                 </Eq>
                                                                 <Eq>
                                                                    <FieldRef Name='Visibility' />
                                                                    <Value Type='Boolean'>1</Value>
                                                                 </Eq>
                                                              </And>
                                                           </Where>
                                                           <OrderBy>
                                                              <FieldRef Name='ItemOrder' Ascending='TRUE' />
                                                           </OrderBy>"
                                                    };
                                                    string MenuLevel3 = "TopMenuLevel3";
                                                    SPList SubSublist = Pweb.Lists.TryGetList(MenuLevel3);
                                                    if (SubSublist != null)
                                                    {
                                                        SPListItemCollection sPListItemCollectionLVL3 = SubSublist.GetItems(Subquery);
                                                        _Level3 = new List<TopMenuLevel3>();
                                                        _Level3 = SPFactory.MapListItemsToClass<TopMenuLevel3>(sPListItemCollectionLVL3);
                                                        _Level2[j].LVL3 = new List<TopMenuLevel3>();
                                                        _Level2[j].LVL3 = _Level3;
                                                    }

                                                }
                                            }

                                            _Level1[i].LVL2 = _Level2;
                                        }



                                        /////////////////
                                        List<MenuLevel1> menuList = new List<MenuLevel1>();

                                        foreach (var level1 in _Level1)
                                        {
                                            MenuLevel1 menu1 = new MenuLevel1
                                            {
                                                Title = level1.Title,
                                                URL = level1.URL,
                                                LVL2 = new List<MenuLevel2>(),
                                                LVL3 = new List<MenuLevel3>() // Initialize the consolidated list
                                            };
                                            if (level1.LVL2 != null && level1.LVL2.Count > 0)
                                            {
                                                foreach (var level2 in level1.LVL2)
                                                {
                                                    MenuLevel2 menu2 = new MenuLevel2
                                                    {
                                                        Title = level2.Title,
                                                        URL = level2.URL
                                                    };

                                                    // Adding Level 2 to Level 1 list
                                                    menu1.LVL2.Add(menu2);

                                                    if (level2.LVL3 != null && level2.LVL3.Count > 0)
                                                    {
                                                        // Flatten all Level 3 items from this Level 2 into Level 1's LVL3 list
                                                        foreach (var level3 in level2.LVL3)
                                                        {
                                                            MenuLevel3 menu3 = new MenuLevel3
                                                            {
                                                                Title = level3.Title,
                                                                URL = level3.URL,
                                                                ParentTitle = level2.Title // Assigning the parent title
                                                            };
                                                            menu1.LVL3.Add(menu3);
                                                        }

                                                    }

                                                }

                                            }

                                            menuList.Add(menu1);
                                        }
                                        ////////////////

                                        Get_MenuHTML_New(menuList);
                                        Get_MenuHTML(_Level1);
                                    }
                                       
                                    Parentweb = Pweb.ParentWeb.Url + "/";   
                                    if (Parentweb.Contains(site.Url))   
                                    {    
                                        Parentweb = Parentweb.Replace(site.Url, ""); 
                                    }
                                    }



                                }
                            catch (Exception ex)
                            {
                                break;
                            }

                        }



                        if (mainLlist == null)
                            return;


                        //return items;

                    }
                }

            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
            }

        }

        public string GetDynamicURL(string URL)
        {
            try
            {
                return String.Format("{0}{1}", SPFactory.GetSiteURL(), URL);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(), this.Page.Title, ex.Message);
                return "";
            }


        }

        public void Get_MenuHTML_New(List<MenuLevel1> _AllData)
        {
            string HTML = "";
            string LVL1 = "";
            if (_AllData != null && _AllData.Count > 0)
            {
                
                foreach (MenuLevel1 item in _AllData)
                {

                    if (item.LVL2 != null && item.LVL2.Count > 0)
                    {
                        if(item.LVL3 != null && item.LVL3.Count > 0)
                        {
                            LVL1 = $@"<li class=""nav-item dropdown my-2 my-md-0"">
                                            <a class=""nav-link dropdown-toggle py-0 align-content-around h-100 h-sm-30px position-relative font-ehsan-semibold d-flex align-items-center justify-content-between pe-1 pe-md-0""
                                              href=""#"" id=""navbarDropdownMenuLink"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                                              <div class=""align-items-baseline d-flex d-md-block"">
                                                <img src="""" class=""d-block d-md-none me-2""
                                                  alt=""{item.Title}"" />
                                                {item.Title}
                                              </div>
                                            </a>
                                            <div class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100""
                                              data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                                              <div class=""container"">
                                                <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap "">

                                                  @LVL2
                      
                                                </ul>
                                              </div>


                                            </div>
                                          </li>";
                            string LVL2 = "";
                            foreach (MenuLevel2 item2 in item.LVL2)
                            {

                                List<MenuLevel3> itemsLVL3 = item.LVL3.Where(e => e != null && e.ParentTitle == item2.Title).ToList();
                                if(itemsLVL3 != null && itemsLVL3.Count > 0)
                                {
                                    LVL2 += $@"<li class=""flex-fill list-unstyled"">
                                                      <div>
                                                        <a class=""dga-submenutitle  text-decoration-none  fw-semibold text-sa-600 text-start mb-2"">
                                                          <div class=""menu-item-title fs-6 px-2 "">{item2.Title}</div>

                                                        </a>

                                                        <div class=""tree-menu mt-2"">
                                                          <ul class=""list-unstyled text-start"">
                                                            @LVL3

                                                        </div>
                                                      </div>


                                                    </li>
                                                    ";
                                    string LVL3 = "";
                                    foreach(MenuLevel3 item3 in itemsLVL3)
                                    {
                                        LVL3 += $@"<li><a class=""p-2""  href=""{item3.URL}"">{item3.Title}</a></li>";
                                        
                                    }
                                    LVL2 = LVL2.Replace("@LVL3", LVL3);
                                }
                                else
                                {
                                    LVL2 += $@"<li class=""flex-fill list-unstyled"">
                                                      <div>
                                                        <a class=""dga-submenutitle  text-decoration-none  fw-semibold text-sa-600 text-start mb-2"">
                                                          <div class=""menu-item-title fs-6 px-2 "">{item2.Title}</div>

                                                        </a>

                                                        <div class=""tree-menu mt-2"">
                                                          <ul class=""list-unstyled text-start"">
                                                            <li><a class=""p-2""  href=""{item2.URL}"">{item2.Title}</a></li>

                                                        </div>
                                                      </div>


                                                    </li>
                                                    ";
                                }
                            
                            }
                            LVL1 = LVL1.Replace("@LVL2", LVL2);
                        }

                        else
                        {
                            //LVL1 = $@"<li class=""nav-item dropdown my-2 my-md-0"">
                            //                <a class=""nav-link dropdown-toggle py-0 align-content-around h-100 h-sm-30px position-relative font-ehsan-semibold d-flex align-items-center justify-content-between pe-1 pe-md-0""
                            //                  href=""#"" id=""navbarDropdownMenuLink"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                            //                  <div class=""align-items-baseline d-flex d-md-block"">
                            //                    <img src="""" class=""d-block d-md-none me-2""
                            //                      alt=""{item.Title}"" />
                            //                    {item.Title}
                            //                  </div>
                            //                </a>
                            //                <div class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100""
                            //                  data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                            //                  <div class=""container"">
                            //                    <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap "">

                            //                      @LVL2
                      
                            //                    </ul>
                            //                  </div>


                            //                </div>
                            //              </li>";
                            LVL1 = $@"<li class=""nav-item dropdown my-2 my-md-0"">
                                <a class=""nav-link dropdown-toggle py-0 align-content-around h-100 h-sm-30px position-relative font-ehsan-semibold d-flex align-items-center justify-content-between pe-1 pe-md-0""
                                  href=""#"" id=""navbarDropdownMenuLink"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                                  <div class=""align-items-baseline d-flex d-md-block"">
                                    <img src="""" class=""d-block d-md-none me-2""
                                      alt=""{item.Title}"" />
                                    {item.Title}
                                  </div>
                                </a>
                                <div
                                  class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100""
                                  data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                                  <ul class=""container d-flex list-unstyled flex-wrap"">
                                    @LVL2
                                  </ul>
                                </div>
                              </li>"; 
                            string LVL2 = "";

                            foreach (MenuLevel2 item2 in item.LVL2)
                            {
                                
                               List <MenuLevel3> itemsLVL3 = item.LVL3.Where(e => e != null && e.ParentTitle == item2.Title).ToList();
                                LVL2 += $@" <li class=""w-fit-content"">
                                          <a class=""align-items-center d-flex dropdown-item h-40px justify-content-center""
                                            href=""{item2.URL}"">{item2.Title}</a>
                                        </li>";
                            }

                            LVL1 = LVL1.Replace("@LVL2", LVL2);

                        }

                        HTML += LVL1;
                    }
                    else
                    {
                        //HTML += $@"<li class=""nav-item""><a href=""{item.URL}"" >{item.Title}</a></li>";
                        HTML += $@"<li class=""menu-item""><a class=""menu-drop"" href=""{item.URL}""><div class=""menu-item-title"">{item.Title}</div></a></li>";
                    }


                }
            }
            menuList.InnerHtml = HTML;

        }


        public void Get_MenuHTML(List<TopMenuLevel1> _AllData)
        {
            string HTML = "";
            string HTMLMobile = "";
            if (_AllData != null && _AllData.Count > 0)
            {
                foreach (TopMenuLevel1 item in _AllData)
                {

                    if(item.LVL2 != null && item.LVL2.Count > 0)
                    {
                        string LVL2Data = "";
                        string LVL2Mobile = $@"<li class=""nav-item dropdown"">
                                <a href=""{item.URL}"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false""
                                  class=""nav-link dropdown-toggle px-1 link-dark d-inline-flex align-items-center"">{item.Title}
                                  <svg class=""bi ms-2"" width=""10"" height=""6"">
                                    <use xlink:href=""#chevronDown"" />
                                  </svg>
                                </a>
                                <ul class=""dropdown-menu py-0"">
                                  @LVL2
                                </ul>
                              </li>";

                        string LVL2DataMobile = "";


                        string LVL2 = "";

                        LVL2 = $@"<li class=""nav-item dropdown my-2 my-md-0"">
                                    <a class=""nav-link dropdown-toggle py-0 align-content-around h-100 h-sm-30px position-relative font-ehsan-semibold d-flex align-items-center justify-content-between pe-1 pe-md-0""
                                      href=""#"" id=""navbarDropdownMenuLink"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                                      <div class=""align-items-baseline d-flex d-md-block"">
                                        <img src="""" class=""d-block d-md-none me-2""
                                          alt=""{item.Title}"" />
                                        {item.Title}
                                      </div>
                                    </a>
                                    <div class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100""
                                      data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                                      <ul class=""container d-flex list-unstyled flex-wrap"">
                                       @LVL2
                                      </ul>
                                    </div>
                                  </li>";

                        foreach (TopMenuLevel2 item2 in item.LVL2)
                        {

                            if (item2.LVL3 != null && item2.LVL3.Count > 0)
                            {
                                LVL2 = $@"<li class=""nav-item dropdown my-2 my-md-0"">
                                    <a class=""nav-link dropdown-toggle py-0 align-content-around h-100 h-sm-30px position-relative font-ehsan-semibold d-flex align-items-center justify-content-between pe-1 pe-md-0""
                                      href=""#"" id=""navbarDropdownMenuLink"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                                      <div class=""align-items-baseline d-flex d-md-block"">
                                        <img src="""" class=""d-block d-md-none me-2""
                                          alt=""{item.Title}"" />
                                        {item.Title}
                                      </div>
                                    </a>
                                    <div class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100""
                                      data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                                      <ul class=""container d-flex list-unstyled flex-wrap"">
                                       @LVL2
                                      </ul>
                                    </div>
                                  </li>";
                                string LVL3 = "";
                                string LVL3Mobile = "";
                                if (PortalHelper.IsArabic)
                                {
                                    //LVL3 = $@"<li class=""nav-item dropdown"">
                                    //    <a class=""dropdown-item d-inline-flex align-items-center"" href=""{item2.URL}"">
                                    //       {item2.Title}
                                    //      <svg class=""bi ms-2"" width=""10"" height=""6"" style="" transform: rotate(90deg);"">
                                    //        <use xlink:href=""#chevronDown"" />
                                    //      </svg>
                                    //    </a>
                                    //    <ul class=""submenu dropdown-menu"">
                                    //      @LVL3
                                    //    </ul>
                                    //  </li>";

                                    LVL3 = $@"
                <div class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100"" data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                  <div class=""container"">
                    <ul class=""list-unstyled w-100 d-flex m-auto flex-wrap "">

                      <li class=""flex-fill list-unstyled"">
                        <div>
                          <a class=""dga-submenutitle  text-decoration-none  fw-semibold text-sa-600 text-start mb-2"">
                            <div class=""menu-item-title fs-6 px-2 "">{item2.Title}</div>

                          </a>

                          <div class=""tree-menu mt-2"">
                            <ul class=""list-unstyled text-start"">
                              @LVL3

                            </ul>

                          </div>
                        </div>


                      </li>
                    </ul>
                  </div>


                </div>
              ";

                                    LVL3Mobile = $@"<li class=""nav-item dropdown"">
                                        <a class=""dropdown-item d-inline-flex align-items-center"" href=""{item2.URL}"">
                                           {item2.Title}
                                          <svg class=""bi ms-2"" width=""10"" height=""6"" style="" transform: rotate(90deg);"">
                                            <use xlink:href=""#chevronDown"" />
                                          </svg>
                                        </a>
                                        <ul class=""submenu dropdown-menu"">
                                          @LVL3
                                        </ul>
                                      </li>";
                                }



                                else
                                { 
                                    LVL3 = $@"<li class=""nav-item dropdown"">
                                            <a class=""dropdown-item d-inline-flex align-items-center"" href=""{item2.URL}"">
                                               {item2.Title}
                                              <svg class=""bi ms-2"" width=""10"" height=""6"" style="" transform: rotate(-90deg);"">
                                                <use xlink:href=""#chevronDown"" />
                                              </svg>
                                            </a>
                                            <ul class=""submenu dropdown-menu"">
                                              @LVL3
                                            </ul>
                                          </li>";
                                    LVL3Mobile = $@"<li class=""nav-item dropdown"">
                                            <a class=""dropdown-item d-inline-flex align-items-center"" href=""{item2.URL}"">
                                               {item2.Title}
                                              <svg class=""bi ms-2"" width=""10"" height=""6"" style="" transform: rotate(-90deg);"">
                                                <use xlink:href=""#chevronDown"" />
                                              </svg>
                                            </a>
                                            <ul class=""submenu dropdown-menu"">
                                              @LVL3
                                            </ul>
                                          </li>";
                            
                                }

                                string LVL3Data = "";
                                string LVL3DataMobile = "";
                                foreach (TopMenuLevel3 item3 in item2.LVL3)
                                {
                                    if (item3 != null)
                                    {
                                        //LVL3Data += $@"<li class=""nav-item dropdown""><a class=""dropdown-item"" href=""{item3.URL}"">{item3.Title}</a></li>";
                                        LVL3Data += $@"<li><a class=""p-2""  href=""{item3.URL}"">{item3.Title}</a></li>";
                                        LVL3DataMobile += $@"<li class=""nav-item dropdown""><a class=""dropdown-item"" href=""{item3.URL}"">{item3.Title}</a></li>";
                                    }
                                }

                                LVL3 = LVL3.Replace("@LVL3", LVL3Data);
                                LVL3Mobile = LVL3Mobile.Replace("@LVL3", LVL3DataMobile);
                                LVL2Data += LVL3;
                                LVL2DataMobile += LVL3Mobile;
                            }
                            else
                            {
                                LVL2 = $@"<li class=""nav-item dropdown my-2 my-md-0"">
                                    <a class=""nav-link dropdown-toggle py-0 align-content-around h-100 h-sm-30px position-relative font-ehsan-semibold d-flex align-items-center justify-content-between pe-1 pe-md-0""
                                      href=""#"" id=""navbarDropdownMenuLink"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                                      <div class=""align-items-baseline d-flex d-md-block"">
                                        <img src="""" class=""d-block d-md-none me-2""
                                          alt=""{item.Title}"" />
                                        {item.Title}
                                      </div>
                                    </a>
                                    <div class=""dropdown-menu border-0 border-top justify-content-around py-md-4 shadow-sm shadow-sm-none show w-100""
                                      data-bs-popper=""static"" aria-labelledby=""navbarDropdownMenuLink"">
                                      <ul class=""container d-flex list-unstyled flex-wrap"">
                                       @LVL2
                                      </ul>
                                    </div>
                                  </li>";
                                LVL2DataMobile += $@"<li><a class=""dropdown-item"" href=""{item2.URL}"">{item2.Title}</a></li>";
                                LVL2Data += $@" <li class=""w-fit-content"">
                                   <a class=""align-items-center d-flex dropdown-item h-40px justify-content-center""
                                     href=""{item2.URL}"">{item2.Title}</a>
                                 </li>";
                            }

                            

                        }

                        LVL2 = LVL2.Replace("@LVL2", LVL2Data);
                        HTML += LVL2;

                        LVL2Mobile = LVL2Mobile.Replace("@LVL2", LVL2DataMobile);
                        HTMLMobile += LVL2Mobile;
                    }
                    else
                    {
                        HTML += $@"<li class=""nav-item""><a href=""{item.URL}"" >{item.Title}</a></li>";
                        HTMLMobile += $@"<li class=""nav-item""><a href=""{item.URL}"" >{item.Title}</a></li>";

                    }


                }
            }
            //menuList.InnerHtml = HTML;

            HTMLMobile += $@"<hr>
                    <a href=""{SPFactory.GetSiteURL()}Pages/AllServices.aspx"" class=""mb-3  nav-link d-block w-100 justify-content-between fs-5 text-dark"">
                    {SPFactory.GetPNUresResource("EServices")}
                    </a>
                    <a href=""{SPFactory.GetSiteURL()}AcademicCalendar/Pages/default.aspx"" class=""mb-3  nav-link d-block w-100 justify-content-between fs-5 text-dark"">
                    {SPFactory.GetPNUresResource("UniversityCalender")}
                    </a>";
//<a href=""{SPFactory.GetSiteURL()}FAQs/Pages/FAQ.aspx"" class=""mb-3  nav-link d-block w-100 justify-content-between fs-5 text-dark"">الأسئلة الشائعة</a>

            divMenuMobile.InnerHtml = HTMLMobile;
        }

        protected void lnkbtn_signout_Click1(object sender, EventArgs e)
        {
            try
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                FormsAuthentication.SignOut();
                if (HttpContext.Current == null)
                    return;
                HttpContext.Current.Response.Redirect(PortalHelper.ParentLangSite, false);
            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),this.Page.Title, ex.Message);
            }
            
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {

        }
    }

    public class ItemWithSub
    {
        public object Item { get; set; }
        public object SubItem { get; set; }

        public ItemWithSub(object _item, object _subItem)
        {
            Item = _item;
            SubItem = _subItem;
        }
    }

    public class TopMenuLevel1
    {
        public string Title { get; set; }
        public string ItemOrder { get; set; }
        public string Visibility { get; set; }
        public string URL { get; set; }
        public string ID { get; set; }

        public List<TopMenuLevel2> LVL2 { get; set; }
    }

    public class TopMenuLevel2
    {
        public string Title { get; set; }
        public string ItemOrder { get; set; }
        public string Visibility { get; set; }
        public string URL { get; set; }
        public string ID { get; set; }
        public string Parent { get; set; }

        public List<TopMenuLevel3> LVL3 { get; set; }

    }

    public class TopMenuLevel3
    {
        public string Title { get; set; }
        public string ItemOrder { get; set; }
        public string Visibility { get; set; }
        public string URL { get; set; }
        public string ID { get; set; }
        public string Parent { get; set; }

    }


    public class MenuLevel3
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public string ParentTitle { get; set; } // New property to store the parent title
    }

    public class MenuLevel2
    {
        public string Title { get; set; }
        public string URL { get; set; }
    }

    public class MenuLevel1
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public List<MenuLevel2> LVL2 { get; set; }
        public List<MenuLevel3> LVL3 { get; set; } // Consolidated list of all LVL3 related to LVL2
    }
}
