using Microsoft.VisualStudio.Shell.Interop;
using System;
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
            return new List<string>() { root,slnFile,slnUserFile};
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
                VS.MessageBox.ShowWarning("SolutionFile Not Opend",  $"Aree! Yaar, {middleWareSolutionFileName}.sln file  is not Opened,  Kindly Open it then perform the operation.. come on dude open it 😊 ");
                return false ;
            }

            //BackgroundSolution = { Microsoft.VisualStudio.CommonIDE.Solutions.BackgroundSolutionService}
            //getting the list of projects
            List<IVsHierarchy> listProjectsInsideSln = solution.GetAllProjectHierarchies().ToList();

            foreach (var item  in listProjectsInsideSln)
            {
                var hierarchy = item as IVsHierarchy;
                Console.WriteLine(hierarchy);
            }
         

            return true;


        }
    }
}
