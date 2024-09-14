using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private readonly string[] RequiredFilesAdmin = new string[] { "appsettings.json", "Program.cs", "Properties/launchSettings.json" };
        private readonly string[] RequiredFilesWeb = new string[] { "appsettings.json" };
        private string postGresPassword = "Incadea@321";
        private List<string> RequiredFileToModifedForAdmin = new List<string> { };
        private List<string> RequiredFileToModifedForWeb = new List<string> { };
        private readonly string lineToCommentInProgrameCSFile = "builder.Services.AddHostedService<RunBackGroundJob>();";
        private readonly string[] startUpProject = new string[] { "incadea.api.middleware.admin\\incadea.api.middleware.admin.csproj", "incadea.api.middleware.web\\incadea.api.middleware.web.csproj" };

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
                VS.MessageBox.ShowWarning("SolutionFile Not Opend", $"Aree! Yaar, {middleWareSolutionFileName}.sln file  is not Opened,  Kindly Open it then perform the operation.. come on machha open it 😊 ");
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
            if (allProjectsInsideSolutionWithPath.Length > 0)

            {
                // "C:/MW/Datastream/WAP-API Components/incadea.API Middleware/incadea.api.middleware.admin/appsettings.json"
                //"C:\\MW\\Datastream\\WAP-API Components\\incadea.API Middleware\\incadea.api.middleware.admin\\appsettings.json"
                //C:\MW\CoreLogs\WAP-API Components\incadea.API Middleware\incadea.api.middleware.admin
                string[] allProjectInsideSolutionOnlyPath = allProjectsInsideSolutionWithPath.Select(i => Path.GetDirectoryName(i)).ToArray();//damiler will get skipped
                string[] allProjectInsideSolutionWithoutPath = allProjectsInsideSolutionWithPath.Select(y => Path.GetFileNameWithoutExtension(y)).ToArray();
                bool allPresent = this.RequiredProjects.All(element => allProjectInsideSolutionWithoutPath.Contains(element));
                if (allPresent)
                {
                    int CountForAdmin = 0;
                    int CountForWeb = 0;
                    for (int i = 0; i < allProjectInsideSolutionOnlyPath.Length; i++)
                    {
                        //admin
                        if (allProjectInsideSolutionOnlyPath[i].Contains(this.RequiredProjects[0]))
                        {
                            for (int j = 0; j < this.RequiredFilesAdmin.Length; j++)
                            {
                                var FilePathToCheckInsideProject = (allProjectInsideSolutionOnlyPath[i] + $"\\{this.RequiredFilesAdmin[j]}").Replace('\\', '/');
                                var isThere = File.Exists(FilePathToCheckInsideProject);
                                if (isThere)
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
                                var FilePathToCheckInsideProjectWeb = (allProjectInsideSolutionOnlyPath[i] + $"\\{this.RequiredFilesAdmin[j]}").Replace('\\', '/');
                                var isThereWeb = File.Exists(FilePathToCheckInsideProjectWeb);
                                if (isThereWeb)
                                {
                                    CountForWeb++;
                                    this.RequiredFileToModifedForWeb.Add(FilePathToCheckInsideProjectWeb);
                                }
                            }

                        }
                    }
                    if (CountForWeb == this.RequiredFilesWeb.Length && CountForAdmin == this.RequiredFilesAdmin.Length)
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
                VS.MessageBox.ShowError("Oh Bhai!!!", $"No project found inside the {GetSolutionName}.sln");
                return false;
            }





        }

        //this method will modified the required files for mw and web
        public bool RequiredFileModification()
        {
            List<bool> status = new List<bool> { };
            if (this.RequiredFileToModifedForWeb.Count == 0 && this.RequiredFileToModifedForAdmin.Count == 0 && this.RequiredFileToModifedForWeb.Count == this.RequiredFilesWeb.Length && this.RequiredFileToModifedForAdmin.Count == this.RequiredFilesAdmin.Length)
            {
                VS.MessageBox.ShowError("Dev Error", "Something got Wrong buddy!! This error can be understood  by the only stupid owner of this extension!! call him at 8604470501");
                return false;
            }
            else
            { //Modification For Admin Files
                for (int i = 0; i < this.RequiredFileToModifedForAdmin.Count; i++)
                {
                    if (this.RequiredFileToModifedForAdmin[i].Contains(this.RequiredFilesAdmin[0]))//appsettings.json
                    {
                        bool isModifiedJson = ModifyJsonFiles(this.RequiredFileToModifedForAdmin[i], this.postGresPassword);
                        if (isModifiedJson)
                            status.Add(true);
                        else
                        {
                            VS.MessageBox.ShowError("Oh FILE OP Error!!", "Error While Editing the appsettings.json in Admin");
                            return false;
                        }
                    }
                    else if (this.RequiredFileToModifedForAdmin[i].Contains(this.RequiredFilesAdmin[1]))//Program.cs
                    {
                        var lines = File.ReadAllLines(RequiredFileToModifedForAdmin[i]);
                        var modifiedLines = lines.Select(line =>
                                                                    line.Contains(this.lineToCommentInProgrameCSFile) ? "// " + line : line).ToArray();

                        // Write the modified lines back to the file
                        File.WriteAllLines(RequiredFileToModifedForAdmin[i], modifiedLines);
                        status.Append(true);

                    }
                    else if (this.RequiredFileToModifedForAdmin[i].Contains(this.RequiredFilesAdmin[2]))//Properties/launchSettings.json
                    {
                        //C:/Users/himanshu.y.sharma/Desktop/VisualStudioExtensionMW/MWAdminRunner/bin/Debug/Resoureces/IISExpress.json
                        //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                        //var c = Directory.GetCurrentDirectory();
                        //var c = Directory.Exists("./.");
                        // var jsonFileFromDataHastoBeTaken = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "IISExpress.json").Replace('\\', '/'); ;
                        // if (File.Exists(jsonFileFromDataHastoBeTaken))
                        //{
                        //     string jsonContent = File.ReadAllText(jsonFileFromDataHastoBeTaken);
                        //     File.WriteAllText(this.RequiredFileToModifedForAdmin[i], jsonContent);
                        //     status.Add(true);
                        // }
                        // else
                        // {
                        //     VS.MessageBox.ShowError("DEV Error", "Launchsettings.json content could not be found in code dir...stupid dev Yaar!!! he cant even handle proper file path.");
                        // }

                        var iisExpress = "{\n" +
"  \"iisSettings\": {\n" +
"    \"windowsAuthentication\": false,\n" +
"    \"anonymousAuthentication\": true,\n" +
"    \"iisExpress\": {\n" +
"      \"applicationUrl\": \" http://localhost:56494/\",\n" +
"      \"sslPort\": 44390\n" +
"    }\n" +
"  },\n" +
"  \"profiles\": {\n" +
"    \"IIS Express\": {\n" +
"      \"commandName\": \"IISExpress\",\n" +
"      \"launchBrowser\": true,\n" +
"      \"environmentVariables\": {\n" +
"        \"ASPNETCORE_ENVIRONMENT\": \"Development\"\n" +
"      }\n" +
"    },\n" +
"    \"incadea.api.middleware.admin\": {\n" +
"      \"commandName\": \"Project\",\n" +
"      \"launchBrowser\": true,\n" +
"      \"environmentVariables\": {\n" +
"        \"ASPNETCORE_ENVIRONMENT\": \"Development\"\n" +
"      },\n" +
"      \"applicationUrl\": \"https://localhost:5001;http://localhost:5000\"\n" +
"    }\n" +
"  }\n" +
"}"
;
                        File.WriteAllText(this.RequiredFileToModifedForAdmin[i], iisExpress);
                        status.Add(true);



                    }
                }
                for (int i = 0; i < this.RequiredFileToModifedForWeb.Count; i++)
                {
                    if (this.RequiredFileToModifedForWeb[i].Contains(this.RequiredFilesWeb[0]))//appsettings.json
                    {
                        bool isModifiedJson = ModifyJsonFiles(this.RequiredFileToModifedForWeb[i], this.postGresPassword);
                        if (isModifiedJson)
                            status.Add(true);
                        else
                        {
                            VS.MessageBox.ShowError("Oh FILE OP Error!!", "Error While Editing the appsettings.json in Web");
                            return false;
                        }
                    }
                }

                if (status.Where(s => s == true).ToList().Count != 3)
                    return false;
                else
                    return true;
            }

        }

        private bool ModifyJsonFiles(string filepath, string ModifiedValue)
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
                        string newConnectionString = mappingDatabase.Replace("Password=postgres", $"Password={ModifiedValue}");
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


        public async Task<bool> BuildingSolution()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = (EnvDTE.DTE)ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE));

            if (dte != null)
            {

                // Start the build
                dte.Solution.SolutionBuild.Build(true);

                // Wait for the build to complete
                // (You might need to implement a better waiting mechanism based on your requirements)
                await Task.Delay(5000);

                if (dte.Solution.SolutionBuild.LastBuildInfo == 0)
                {
                    return true;
                }
                else
                {
                    await VS.MessageBox.ShowErrorAsync("WTF! Build failed. But All Files are modified u can build manually", "Aree yaaar!!!! what the hell!!! you can run the project manually it will run fine.");
                    return false;
                }
            }
            else
            {
                await VS.MessageBox.ShowErrorAsync("Unable to access DTE.", "Hmmm.. don`t know y this is error came ....");
                return false;
            }

        }


        public async Task<bool> SetMultipleStartupProjectsAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = (DTE2)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            if (dte == null)
            {
                await VS.MessageBox.ShowErrorAsync("Error...", "Developer has no idea what is this error about..");
                return false;
            }

            var solution = dte.Solution;

            var solutionBuild = solution.SolutionBuild;
            var o = solutionBuild.StartupProjects;

            solutionBuild.StartupProjects = this.startUpProject;

            dte.ExecuteCommand("File.SaveAll");

            return true;
        }
    }
}
