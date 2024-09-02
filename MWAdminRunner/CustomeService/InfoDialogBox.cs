﻿using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace MWAdminRunner.CustomeService
{
    public class InfoDialogBox
    {

        private readonly IVsSolution _vsSolution;
        private readonly string middleWareSolutionFileName = "incadea.api.middleware";
        private readonly uint maxProjectInsideProject = 100;
        private readonly string[] RequiredProjects = new string[] { "incadea.api.middleware.admin", "incadea.api.middleware.web" };
        private readonly string[] RequiredFilesAdmin = new string[] { "appsettings.json","Program.cs", "Properties/launchSettings.json" };
        private readonly string[] RequiredFilesWeb = new string[] { "appsettings.json" };
        private List<string>  RequiredFileToModifedForAdmin = new List<string> { };
        private List<string> RequiredFileToModifedForWeb = new List<string> { };

        public InfoDialogBox(IVsSolution vsSolution)
        {
            _vsSolution = vsSolution;

        }


        public List<string> GetCurrentSolutionName()
        {
            string root = null;
            string slnFile;
            string slnUserFile;
            var solution = (IVsSolution)Package.GetGlobalService(typeof(SVsSolution));
            if (solution == null)
            {
                return null; // failed to find a solution
            }
            solution.GetSolutionInfo(out root, out slnFile, out slnUserFile);
            return new List<string>() { root, slnFile, slnUserFile };
        }

        //Getting list Of Projects inside Solution and checking Require File to Modiefied so that it will run locally 

        public bool CheckingRequiredFileForModification(List<string> infoAboutSolutionFile)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var solution = (IVsSolution)Package.GetGlobalService(typeof(SVsSolution));
            string GetSolutionName = Path.GetFileNameWithoutExtension(infoAboutSolutionFile[1]);
            //checking Weather SolutionFile is opend or not
            if (!string.Equals(middleWareSolutionFileName, GetSolutionName))
            {
                VS.MessageBox.ShowWarning("SolutionFile Not Opend", $"Aree! Yaar, {middleWareSolutionFileName}.sln file  is not Opened,  Kindly Open it then perform the operation.. come on dude open it 😊 ");
                return false;
            }
            string[] cProjects = new string[this.maxProjectInsideProject];
            uint pcProjectsFetched = 0;
            solution.GetProjectFilesInSolution(
                                    (uint)__VSGETPROJFILESFLAGS.GPFF_SKIPUNLOADEDPROJECTS,
                                    this.maxProjectInsideProject,
                                       cProjects,
                                       out pcProjectsFetched

                                );
            //"C:\\MW\\Datastream\\WAP-API Components\\incadea.API Middleware\\incadea.api.middleware.admin\\incadea.api.middleware.admin.csproj\\appsettings.json"
            //removing null values
            string[] allProjectsInsideSolutionWithPath = cProjects.Where(i => i != null).ToArray();
            if(allProjectsInsideSolutionWithPath.Length > 0)

            {
               // "C:/MW/Datastream/WAP-API Components/incadea.API Middleware/incadea.api.middleware.admin/appsettings.json"
                //"C:\\MW\\Datastream\\WAP-API Components\\incadea.API Middleware\\incadea.api.middleware.admin\\appsettings.json"
                //C:\MW\CoreLogs\WAP-API Components\incadea.API Middleware\incadea.api.middleware.admin
                string[] allProjectInsideSolutionOnlyPath = allProjectsInsideSolutionWithPath.Select(i=>Path.GetDirectoryName(i)).ToArray();//damiler will get skipped
                string[] allProjectInsideSolutionWithoutPath = allProjectsInsideSolutionWithPath.Select(y => Path.GetFileNameWithoutExtension(y)).ToArray();
                bool allPresent = this.RequiredProjects.All(element => allProjectInsideSolutionWithoutPath.Contains(element));
                if (allPresent)
                {
                    int CountForAdmin=0;
                    int CountForWeb = 0;
                    for (int i = 0; i< allProjectInsideSolutionOnlyPath.Length;i++)
                    {
                        //admin
                        if (allProjectInsideSolutionOnlyPath[i].Contains(this.RequiredProjects[0]))
                        {
                            for (int j = 0; j < this.RequiredFilesAdmin.Length; j++)
                            {
                                var FilePathToCheckInsideProject = (allProjectInsideSolutionOnlyPath[i] + $"\\{this.RequiredFilesAdmin[0]}").Replace('\\', '/');
                                var isThere = File.Exists(FilePathToCheckInsideProject); 
                                if(isThere)
                                   {
                                    CountForAdmin++;
                                    this.RequiredFileToModifedForAdmin.Add(FilePathToCheckInsideProject);
                                }
                            }
                           
                        }
                        //web
                        if (allProjectInsideSolutionOnlyPath[i].Contains(this.RequiredProjects[1]))
                        {
                            for (int j = 0; j < this.RequiredFilesWeb.Length; j++)
                            {
                                var FilePathToCheckInsideProjectWeb = (allProjectInsideSolutionOnlyPath[i] + $"\\{this.RequiredFilesAdmin[0]}").Replace('\\', '/');
                                var isThereWeb = File.Exists(FilePathToCheckInsideProjectWeb);
                                if(isThereWeb)
                                {
                                   CountForWeb++;
                                    this.RequiredFileToModifedForWeb.Add(FilePathToCheckInsideProjectWeb);
                                }
                            }

                        }
                    }
                    if(CountForWeb == this.RequiredFilesWeb.Length && CountForAdmin== this.RequiredFilesAdmin.Length)
                            return true;
                    else
                    {
                        VS.MessageBox.ShowError("File Not Found", "Oh! Some Required Files need to be modified are not found!!! [appsettings.json(Admin/Web),Programe.cs(Admin) ,launchsettings.json (Admin)]");
                        return false;
                    }
                }

                else
                {
                    VS.MessageBox.ShowError("Yaar!!", $"No Required Project Found inside your {GetSolutionName}.sln to modify, Please Check Once");
                    return false;
                }
            }
            else
            {
                VS.MessageBox.ShowError("Oh Bhai!!!",$"No project found inside the {GetSolutionName}.sln");
                return false;
            }
        

         


        }

        //this method will modified the required files for mw and web
        public bool RequiredFileModification()
        {       
            if (this.RequiredFileToModifedForWeb.Count == 0 && this.RequiredFileToModifedForAdmin.Count == 0 && this.RequiredFileToModifedForWeb.Count == this.RequiredFilesWeb.Length && this.RequiredFileToModifedForAdmin.Count==this.RequiredFilesAdmin.Length)
            {
                VS.MessageBox.ShowError("Dev Error", "Something got Wrong buddy!! This error can be understood  by the only stupid owner of this extension!! call him at 8604470501");
                return false;
            }
            else
            { //Modification For Admin Files
                for(int i=0;i<this.RequiredFileToModifedForAdmin.Count;i++)
                {
                    for (int j = 0; j < this.RequiredFilesAdmin.Length; j++)
                        if (string.Equals(Path.GetFileNameWithoutExtension(this.RequiredFileToModifedForAdmin[i]), this.RequiredFilesAdmin[j]))
                        {
                            //from here need to update
                        }
                }
                return true;
            }    
         
        }

        private bool ModifyJsonFiles(string filepath)
        {
            {
                string filePath = filepath;
                string jsonString = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonString);
                var connectionStrings = jsonObject["ConnectionStrings"] as JObject;
                if (connectionStrings != null)
                {
                    var mappingDatabase = connectionStrings["MappingDatabase"]?.ToString();
                    if (mappingDatabase != null)
                    {
                        string newConnectionString = mappingDatabase.Replace("Password=postgres", "Password=123");
                        connectionStrings["MappingDatabase"] = newConnectionString;
                        string updatedJsonString = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(filePath, updatedJsonString);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            
            }
        }
    }
}
