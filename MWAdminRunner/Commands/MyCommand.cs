using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using MWAdminRunner.CustomeService;
namespace MWAdminRunner
{


    [Command(PackageIds.MiddlewareRunnerCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        private IVsSolution vsSolution;
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
          
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            InfoDialogBox ib = new InfoDialogBox(vsSolution);

            List<string> currentSolutionInfo = ib.GetCurrentSolutionName();

            if (currentSolutionInfo == null) {

                VS.MessageBox.ShowError("Solution File Not Found", "Unable to Find the Middleware SolutionFile");
            }
            else
            {
                 bool isRequiredFilesAreThere  =ib.CheckingRequiredFileForModification(currentSolutionInfo);

                if (isRequiredFilesAreThere) {

                    bool filesModified = ib.RequiredFileModification();
                }
                else
                {
                    VS.MessageBox.ShowError("Kuch to Gadbad Hai", "Sorry Bro! something went wrong Please Contact at 8604470501 ");
                }
            }
            

       
        }
    }


}
