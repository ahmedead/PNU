using Microsoft.SharePoint;
using PNU.Internet.WebParts.Layouts.PNU.Internet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PNU.Internet.WebParts
{
    public static class busclsAcademicCredits
    {
        public static List<AcademicCreditsDto> GetAllAcademicCreditsByCollegeCode(string Coll_Code)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='COLL_CODE' />
                                 <Value Type='Text'>" + Coll_Code + @"</Value>
                              </Eq>
                           </Where>");

                List<AcademicCreditsDto> _Data = SPFactory.GetAllItemsByQuery<AcademicCreditsDto>("Admin", Settings.AcademicCredits, query);
                if (_Data == null || _Data.Count == 0)
                {
                    return null;
                }

                _Data = _Data.GroupBy(e => new { e.PRG_CRED_CLASS, e.PRG_CRED_SPN }).Select(group => group.First()).ToList();
                if (_Data == null || _Data.Count == 0)
                {
                    return null;
                }
                _Data = _Data.Where(e => e.PRG_CRED_SPN != null).ToList();
                if (_Data != null && _Data.Count > 0)
                {
                    for (int i = 0; i < _Data.Count; i++)
                    {
                        if (_Data[i].PRG_CRED_SPN == "ABET")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/ABET.jpg";
                        else if (_Data[i].PRG_CRED_SPN == "Equals")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/Eaquals.png";
                        else if (_Data[i].PRG_CRED_SPN == "المركز الوطني للتقويم والاعتماد الأكاديمي")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/NCAAE.png";
                    }
                    return _Data;
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsAcademicCredits - GetAllAcademicCreditsByCollegeCode", ex.Message);
            }


            return null;
        }


        public static List<AcademicCreditsDto> GetAllAcademicCreditsByDepartmentCode(string Dept_Code)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='DEPT_CODE' />
                                 <Value Type='Text'>" + Dept_Code + @"</Value>
                              </Eq>
                           </Where>");

                List<AcademicCreditsDto> _Data = SPFactory.GetAllItemsByQuery<AcademicCreditsDto>("Admin", Settings.AcademicCredits, query);
                if (_Data == null || _Data.Count == 0)
                {
                    return null;
                }
                _Data = _Data.GroupBy(e => new { e.PRG_CRED_CLASS, e.PRG_CRED_SPN }).Select(group => group.First()).ToList();
                if (_Data == null || _Data.Count == 0)
                {
                    return null;
                }
                _Data = _Data.Where(e => e.PRG_CRED_SPN != null).ToList();
                if (_Data != null && _Data.Count > 0)
                {
                    for (int i = 0; i < _Data.Count; i++)
                    {
                        if (_Data[i].PRG_CRED_SPN == "ABET")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/ABET.jpg";
                        else if (_Data[i].PRG_CRED_SPN == "Equals")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/Eaquals.png";
                        else if (_Data[i].PRG_CRED_SPN == "المركز الوطني للتقويم والاعتماد الأكاديمي")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/NCAAE.png";
                    }
                    return _Data;
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsAcademicCredits - GetAllAcademicCreditsByDepartmentCode", ex.Message);
            }

            
            return null;
        }


        public static List<AcademicCreditsDto> GetAllAcademicCreditsByProgCode(string Prog_Code)
        {
            try
            {
                SPQuery query = new SPQuery();
                query.Query = string.Concat(
                                 @"<Where>
                              <Eq>
                                 <FieldRef Name='PROG_CODE' />
                                 <Value Type='Text'>" + Prog_Code + @"</Value>
                              </Eq>
                           </Where>");

                List<AcademicCreditsDto> _Data = SPFactory.GetAllItemsByQuery<AcademicCreditsDto>("Admin", Settings.AcademicCredits, query);
                if (_Data == null || _Data.Count == 0)
                {
                    return null;
                }
                _Data = _Data.GroupBy(e => new { e.PRG_CRED_CLASS, e.PRG_CRED_SPN }).Select(group => group.First()).ToList();
                if (_Data == null || _Data.Count == 0)
                {
                    return null;
                }
                _Data = _Data.Where(e => e.PRG_CRED_SPN != null).ToList();
                if (_Data != null && _Data.Count > 0)
                {
                    for (int i = 0; i < _Data.Count; i++)
                    {
                        if (_Data[i].PRG_CRED_SPN == "ABET")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/ABET.jpg";
                        else if (_Data[i].PRG_CRED_SPN == "Equals")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/Eaquals.png";
                        else if (_Data[i].PRG_CRED_SPN == "المركز الوطني للتقويم والاعتماد الأكاديمي")
                            _Data[i].PRG_CRED_SPN = "/Admin/PublishingImages/NCAAE.png";
                    }
                    return _Data;
                }



            }

            catch (Exception ex)
            {
                Publics.WriteToLog(HttpContext.Current.Request.Url.ToString(),"busclsAcademicCredits - GetAllAcademicCreditsByProgCode", ex.Message);
            }

            
            return null;
        }
    }
}
