using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.Xml.Linq;
using Microsoft.SharePoint;
using System.IO;



namespace PNU.Integration
{
    public static class Config
    {
        public static string RootSiteUrl =>
            System.Configuration.ConfigurationManager.AppSettings["RootSiteUrl"];

        public static string OnlyThisWebUrl =>
            System.Configuration.ConfigurationManager.AppSettings["OnlyThisWebUrl"];

        public static string SqlConnectionString =>
            System.Configuration.ConfigurationManager.ConnectionStrings["SharePointExportDb"].ConnectionString;
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. SharePoint Architecture Check (Crucial for the 80040154 error)
            if (!Environment.Is64BitProcess)
            {
                OracleDBContext.LogToFile("ERROR: This application must run as x64 to access SharePoint DLLs.");
                return;
            }

            // 2. Help message if no arguments are provided
            if (args.Length == 0)
            {
                //OracleDBContext.LogToFile("PNU Integration Console - Usage:");
                //OracleDBContext.LogToFile("PNU.Integration.exe [jobname]");
                //OracleDBContext.LogToFile("\nAvailable Jobs:");
                //OracleDBContext.LogToFile(" - all               : Runs everything in sequence");
                //OracleDBContext.LogToFile(" - members           : CollMembersListName");
                //OracleDBContext.LogToFile(" - stats             : Statistics");
                //OracleDBContext.LogToFile(" - programs          : AllPrograms & Colleges");
                //OracleDBContext.LogToFile(" - studyplan         : NewStudyPlan");
                //OracleDBContext.LogToFile(" - credits           : AcademicCredits & Requisites");
                //OracleDBContext.LogToFile(" - courses           : Member Courses (AR & EN)");
                //OracleDBContext.LogToFile(" - gadeer            : Gadeer API Sync (AR & EN)");
                //OracleDBContext.LogToFile("=== Starting AllPrograms & Colleges ===");
                //OracleDBContext.AddProgramsForFirstTime("AllPrograms");
                //OracleDBContext.AddColleges("CollDeptListName");
                //OracleDBContext.AddCollegesNew("CollegeCategory");

                //OracleDBContext.LogToFile("=== Deploying Master Pages: UAT → Production ===");
                //MasterPageDeployment.DeployMasterPages();


                Console.ReadLine();
                return;
            }
            else
            {
                string job = args[0].ToLower();

                // Start Logging the specific task
                OracleDBContext.LogToFile($"--- TASK START: {job.ToUpper()} ---");
                try
                {
                    switch (job)
                    {

                        case "export-masterpages":
                            OracleDBContext.LogToFile("=== Exporting Master Pages from this farm ===");
                            MasterPageDeployment.ExportMasterPages();
                            break;

                        case "export-pagelayouts":
                            OracleDBContext.LogToFile("=== Exporting Page Layouts from this farm ===");
                            MasterPageDeployment.ExportPageLayouts();
                            break;

                        case "export-all-ui":
                            OracleDBContext.LogToFile("=== Exporting Master Pages + Page Layouts from this farm ===");
                            MasterPageDeployment.ExportAllUiArtifacts();
                            break;

                        case "import-masterpages":
                            OracleDBContext.LogToFile("=== Importing Master Pages into this farm ===");
                            MasterPageDeployment.ImportMasterPages();
                            break;

                        case "import-pagelayouts":
                            OracleDBContext.LogToFile("=== Importing Page Layouts into this farm ===");
                            MasterPageDeployment.ImportPageLayouts();
                            break;

                        case "import-all-ui":
                            OracleDBContext.LogToFile("=== Importing Master Pages + Page Layouts into this farm ===");
                            MasterPageDeployment.ImportAllUiArtifacts();
                            break;
                        case "members":
                            OracleDBContext.LogToFile("=== Starting CollMembersListName ===");
                            OracleDBContext.SaveCollMemeberDataToList("CollMembersListName");
                            break;

                        case "stats":
                            OracleDBContext.LogToFile("=== Starting Statistics & AcademicCredits & Requisites ===");
                            OracleDBContext.UpdatePowerPiStatisticsDataToList("Statistics");
                            OracleDBContext.LogToFile("=== Task Ended Statistics ===");
                            
                            OracleDBContext.LogToFile("=== Starting AddAcademicCredits ===");
                            OracleDBContext.AddAcademicCredits();
                            OracleDBContext.LogToFile("=== Task Ended AddAcademicCredits ===");
                            OracleDBContext.LogToFile("=== Starting AddProgramsRequisites ===");
                            OracleDBContext.AddProgramsRequisites();
                            OracleDBContext.LogToFile("=== Task Ended AddProgramsRequisites ===");
                            
                            break;

                        case "programs":
                            OracleDBContext.LogToFile("=== Starting AllPrograms & Colleges ===");

                            OracleDBContext.LogToFile("=== Starting AddProgramsForFirstTime ===");
                            OracleDBContext.AddProgramsForFirstTime("AllPrograms");
                            OracleDBContext.LogToFile("=== Task Ended AddProgramsForFirstTime ===");
                            OracleDBContext.LogToFile("=== Starting CollDeptListName ===");
                            OracleDBContext.AddColleges("CollDeptListName");
                            OracleDBContext.LogToFile("=== Task Ended CollDeptListName ===");
                            OracleDBContext.LogToFile("=== Starting CollegeCategory ===");
                            OracleDBContext.AddCollegesNew("CollegeCategory");
                            OracleDBContext.LogToFile("=== Task Ended CollegeCategory ===");
                            break;

                        case "studyplan":
                            OracleDBContext.LogToFile("=== Starting NewStudyPlan ===");
                            OracleDBContext.AddNewStudyPlan("NewStudyPlan");
                            break;

                        case "credits":
                            OracleDBContext.LogToFile("=== Starting AcademicCredits & Requisites ===");
                            OracleDBContext.AddAcademicCredits();
                            OracleDBContext.AddProgramsRequisites();
                            break;

                        case "courses":
                            OracleDBContext.LogToFile("=== Starting Member Courses (AR & EN) ===");
                            OracleDBContext.SaveInstructorCoursesDataToSP();
                            OracleDBContext.SaveInstructorCoursesData_ENToSP();
                            break;

                        case "gadeer":
                            OracleDBContext.LogToFile("=== Starting Gadeer API Sync (AR & EN) ===");
                            OracleDBContext.SaveGadeerCoursesDataToSP();
                            OracleDBContext.SaveGadeerCoursesData_ENToSP();
                            break;

                        case "all":
                            // Recursively call the Main method for all parts or just list them here
                            RunAllJobs();
                            break;

                        default:
                            OracleDBContext.LogToFile($"Unknown job argument: {job}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    OracleDBContext.LogToFile($"FATAL ERROR in {job}: {ex.Message} \n {ex.StackTrace}");
                }
                finally
                {
                    OracleDBContext.LogToFile($"--- TASK END: {job.ToUpper()} ---");
                    OracleDBContext.LogToFile("--------------------------------------------------");
                }

            }

            
            //LogToFile("=== Process Finished ===");
        }




        //static void Main(string[] args)
        //{
        //    // 1. SharePoint Architecture Check (Crucial for the 80040154 error)
        //    if (!Environment.Is64BitProcess)
        //    {
        //        OracleDBContext.LogToFile("ERROR: This application must run as x64 to access SharePoint DLLs.");
        //        return;
        //    }

        //    // 2. Help message if no arguments are provided
        //    if (args.Length == 0)
        //    {
        //        OracleDBContext.LogToFile("PNU Integration Console - Usage:");
        //        OracleDBContext.LogToFile("PNU.Integration.exe [jobname]");
        //        OracleDBContext.LogToFile("\nAvailable Jobs:");
        //        OracleDBContext.LogToFile(" - all               : Runs everything in sequence");
        //        OracleDBContext.LogToFile(" - members           : CollMembersListName");
        //        OracleDBContext.LogToFile(" - stats             : Statistics");
        //        OracleDBContext.LogToFile(" - programs          : AllPrograms & Colleges");
        //        OracleDBContext.LogToFile(" - studyplan         : NewStudyPlan");
        //        OracleDBContext.LogToFile(" - credits           : AcademicCredits & Requisites");
        //        OracleDBContext.LogToFile(" - courses           : Member Courses (AR & EN)");
        //        OracleDBContext.LogToFile(" - gadeer            : Gadeer API Sync (AR & EN)");
        //        //Console.ReadLine();
        //        return;
        //    }

        //    string job = args[0].ToLower();

        //    // Start Logging the specific task
        //    OracleDBContext.LogToFile($"--- TASK START: {job.ToUpper()} ---");
        //    try
        //    {
        //        switch (job)
        //        {
        //            case "members":
        //                OracleDBContext.LogToFile("=== Starting CollMembersListName ===");
        //                OracleDBContext.SaveCollMemeberDataToList("CollMembersListName");
        //                break;

        //            case "stats":
        //                OracleDBContext.LogToFile("=== Starting Statistics ===");
        //                OracleDBContext.UpdatePowerPiStatisticsDataToList("Statistics");
        //                break;

        //            case "programs":
        //                OracleDBContext.LogToFile("=== Starting AllPrograms & Colleges ===");
        //                OracleDBContext.AddProgramsForFirstTime("AllPrograms");
        //                OracleDBContext.AddColleges("CollDeptListName");
        //                OracleDBContext.AddCollegesNew("CollegeCategory");
        //                break;

        //            case "studyplan":
        //                OracleDBContext.LogToFile("=== Starting NewStudyPlan ===");
        //                OracleDBContext.AddNewStudyPlan("NewStudyPlan");
        //                break;

        //            case "credits":
        //                OracleDBContext.LogToFile("=== Starting AcademicCredits & Requisites ===");
        //                OracleDBContext.AddAcademicCredits();
        //                OracleDBContext.AddProgramsRequisites();
        //                break;

        //            case "courses":
        //                OracleDBContext.LogToFile("=== Starting Member Courses (AR & EN) ===");
        //                OracleDBContext.SaveInstructorCoursesDataToSP();
        //                OracleDBContext.SaveInstructorCoursesData_ENToSP();
        //                break;

        //            case "gadeer":
        //                OracleDBContext.LogToFile("=== Starting Gadeer API Sync (AR & EN) ===");
        //                OracleDBContext.SaveGadeerCoursesDataToSP();
        //                OracleDBContext.SaveGadeerCoursesData_ENToSP();
        //                break;

        //            case "all":
        //                // Recursively call the Main method for all parts or just list them here
        //                RunAllJobs();
        //                break;

        //            default:
        //                OracleDBContext.LogToFile($"Unknown job argument: {job}");
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        OracleDBContext.LogToFile($"FATAL ERROR in {job}: {ex.Message} \n {ex.StackTrace}");
        //    }
        //    finally
        //    {
        //        OracleDBContext.LogToFile($"--- TASK END: {job.ToUpper()} ---");
        //        OracleDBContext.LogToFile("--------------------------------------------------");
        //    }

        //    //LogToFile("=== Process Finished ===");
        //}

        private static void RunAllJobs()
        {
            // Copy-paste your original sequence here if you still want an 'all' option
        }


        


        //static void Main(string[] args)
        //{

        //    LogToFile("=== Starting CollMembersListName ===");

        //    OracleDBContext.SaveCollMemeberDataToList("CollMembersListName");

        //    LogToFile("=== Finished CollMembersListName ===");

        //    LogToFile("=== Starting Statistics ===");

        //    OracleDBContext.UpdatePowerPiStatisticsDataToList("Statistics");
        //    LogToFile("=== Finished Statistics ===");

        //    LogToFile("=== Starting AllPrograms ===");

        //    OracleDBContext.AddProgramsForFirstTime("AllPrograms");
        //    LogToFile("=== Finished AllPrograms ===");
        //    LogToFile("=== Starting CollDeptListName ===");

        //    OracleDBContext.AddColleges("CollDeptListName");

        //    LogToFile("=== Finished CollDeptListName ===");
        //    LogToFile("=== Starting CollegeCategory - Colleges - Departements - Programs ===");

        //    OracleDBContext.AddCollegesNew("CollegeCategory");
        //    LogToFile("=== Finished CollegeCategory - Colleges - Departements - Programs ===");

        //    LogToFile("=== Starting NewStudyPlan - NewStudyPlan_EN  ===");

        //    OracleDBContext.AddNewStudyPlan("NewStudyPlan");

        //    LogToFile("=== Finished NewStudyPlan - NewStudyPlan_EN  ===");

        //    LogToFile("=== Starting AcademicCredits  ===");

        //    OracleDBContext.AddAcademicCredits();
        //    LogToFile("=== Finished AcademicCredits  ===");
        //    LogToFile("=== Starting ProgramsRequisites  ===");
        //    OracleDBContext.AddProgramsRequisites();
        //    LogToFile("=== Finished ProgramsRequisites  ===");
        //    LogToFile("=== Starting Membere Courses  ===");
        //    OracleDBContext.SaveInstructorCoursesDataToSP();
        //    LogToFile("=== Finished Membere Courses  ===");


        //    LogToFile("=== Starting Membere Courses EN  ===");
        //    OracleDBContext.SaveInstructorCoursesData_ENToSP();
        //    LogToFile("=== Finished Membere Courses EN ===");

        //    LogToFile("Starting: SaveGadeerCoursesDataToDB...");
        //    OracleDBContext.SaveGadeerCoursesDataToSP();
        //    LogToFile("Finishing: SaveGadeerCoursesDataToDB...");

        //    LogToFile("Starting: SaveGadeerCoursesDataToDB...");
        //    OracleDBContext.SaveGadeerCoursesData_ENToSP();
        //    LogToFile("Finishing: SaveGadeerCoursesDataToDB...");



        //}





    }
}
