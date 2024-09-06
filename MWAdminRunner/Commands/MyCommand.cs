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

              await   VS.MessageBox.ShowErrorAsync("Solution File Not Found", "Unable to Find the Middleware SolutionFile, Check from ur side macha!!");
            }
            else
            {
                 bool isRequiredFilesAreThere  =ib.CheckingRequiredFileForModification(currentSolutionInfo);

                if (isRequiredFilesAreThere) {

                    bool filesModified = ib.RequiredFileModification();
                    if (filesModified) {
                        bool ProjectBuild = await ib.BuildingSolution();
                        if (ProjectBuild) {

                          if (  await ib.SetMultipleStartupProjectsAsync())
                            {
                                try
                                {
                                    List<string> jokes = new List<string>
        {
                                        "This Developer wanted to tell u one joke at final step, but he saw his salary and he is still laughing",
            "Why don’t some couples go to the gym? - Because some relationships don’t work out, ಕೆಲವು ಜೋಡಿಗಳು ಜಿಮ್ ಗೆ ಹೋಗುವುದಿಲ್ಲ ಏಕೆ? ಏಕೆಂದರೆ ಕೆಲವು ಸಂಬಂಧಗಳು ಕೆಲಸ ಮಾಡುವುದಿಲ್ಲ!",
            "Why did the Indian chef break up with his partner? - Because they couldn’t 'meat' each other’s expectations! ಭಾರತೀಯ ಶೆಫ್ ತನ್ನ ಜೊತೆಯಾದವನನ್ನು ಏಕೆ ತಲುಪಿದನು? - ಏಕೆಂದರೆ ಅವರು ಪರಸ್ಪರ ನಿರೀಕ್ಷೆಗಳನ್ನು 'ಮೀಟ್' ಮಾಡಲಾರೆ!",
            "Why did the dosa go to the doctor? - Because it had a 'crack' in it! ಡೋಸಾ ಡಾಕ್ಟರ್ ಗೆ ಏಕೆ ಹೋದನು? - ಏಕೆಂದರೆ ಇದರಲ್ಲಿ 'ಕ್ರಾಕ್' ಇದ್ದು!",
            "Why do Indian men love their biryani so much? - Because it’s always 'well-seasoned'! ಭಾರತೀಯ ಪುರುಷರು ತಮ್ಮ ಬಿರಿಯಾನಿ ಅನ್ನು ಇಷ್ಟಪಡುವುದಕ್ಕೆ ಏನು? - ಏಕೆಂದರೆ ಇದು ಸದಾ 'ಚೆನ್ನಾಗಿ ಸೀಸನ್' ಆಗಿರುತ್ತದೆ!",
            "What did one ocean say to the other ocean? - Nothing, they just waved, ಒಂದು ಸಮುದ್ರವು ಇನ್ನೊಂದು ಸಮುದ್ರಕ್ಕೆ ಏನನ್ನು ಹೇಳಿದರು? - ಏನೂ ಇಲ್ಲ, ಅವರು ಕೇವಲ ಕೈಚಲಾಯಿಸಿದರು.",
            "Why don’t programmers like nature? - It has too many bugs, ಕಾರ್ಯಕ್ರಮಕಾರರು ನೈಸರ್ಗಿಕ ಪ್ರಿಯರಾಗುವುದಿಲ್ಲ ಏಕೆ? - ಇದರಲ್ಲಿ demasiada bugs ಇವೆ.",
          "What did the samosa say to the chutney? - 'You’re the spice of my life!' ಸಮೋಸಾ ಚಟ್ನಿ ಗೆ ಏನನ್ನು ಹೇಳಿತು? - 'ನೀನು ನನ್ನ ಜೀವನದ ಮಸಾಲೆ!'",
            "Why did the Indian man bring a curry to the party? - Because he wanted to add some 'spice' to the evening! ಭಾರತೀಯ ವ್ಯಕ್ತಿ ಪಕ್ಷಕ್ಕೆ ಒಂದು ಕ್ಯಾರಿ ತಂದನು ಏಕೆ? - ಏಕೆಂದರೆ ಅವರು ಸಂಜೆಗೆ 'ಮಸಾಲೆ' ಸೇರಿಸಲು ಬಯಸಿದ್ದರು!",
            "What do you call a spicy Indian dish made with potatoes? - A 'hot potato'! ಬಟಾಟೆಗಳಿಂದ ಮಾಡಲ್ಪಟ್ಟ ಉರಿಯುವ ಭಾರತೀಯ ವಾಣಿಜ್ಯವನ್ನು ನೀವು ಏನನ್ನು ಕರೆಯುತ್ತೀರಿ? - 'ಹಾಟ್ ಪಟೇಟೋ'!",
            "Why did the Indian chef love his spices so much? - Because they always add 'flavor' to his life! ಭಾರತೀಯ ಶೆಫ್ ತನ್ನ ಮಸಾಲೆಗಳನ್ನು ಇಷ್ಟಪಡುವುದಕ್ಕೆ ಏನು? - ಏಕೆಂದರೆ ಅವರು ಸದಾ ತನ್ನ ಜೀವನಕ್ಕೆ 'ಫ್ಲೇವರ್' ಸೇರಿಸುತ್ತವೆ!",
            "How does an Indian prepare for a big dinner? - By making sure there’s enough 'gravy'! ಭಾರತೀಯನು ದೊಡ್ಡ ಊಟಕ್ಕಾಗಿ ಹೇಗೆ ತಯಾರಾಗುತ್ತಾನೆ? - 'ಗ್ರೇವಿ' ಸಾಕಷ್ಟು ಇದೆ ಎಂದು ಖಚಿತಪಡಿಸಿಕೊಂಡು!",
            "Why was the chicken always calm at the Indian wedding? - Because it was 'tandoori' prepared! ಕೋಳಿ ಭಾರತೀಯ ಮದುವೆಯಲ್ಲಿ ಯಾವಾಗಲೂ ಶೀಘ್ರವಾಗಿದ್ದೆ ಏಕೆ? - ಏಕೆಂದರೆ ಇದು 'ತಂದೂರಿ' ತಯಾರಾಗಿತ್ತು!",
            "What did the Indian student say to his teacher about his food? - 'It’s a bit 'spicy', just like my homework!' ಭಾರತೀಯ ವಿದ್ಯಾರ್ಥಿ ತನ್ನ ಆಹಾರವನ್ನು ಬಗ್ಗೆ ತನ್ನ ಶಿಕ್ಷಕರಿಗೆ ಏನನ್ನು ಹೇಳಿದರು? - 'ಇದು ಸ್ವಲ್ಪ 'ಮಸಾಲೆ', ನನ್ನ ಹೋಮ್ವರ್ಕ್ ಹೀಗೆಯೇ!'",
            "Why did the Indian man refuse to eat at the buffet? - Because he didn't want to 'chicken' out! ಭಾರತೀಯ ವ್ಯಕ್ತಿ ಬಫೇನಲ್ಲಿ ಆಹಾರ ಸೇವಿಸಲು ನಿರಾಕರಿಸಿದನು ಏಕೆ? - ಏಕೆಂದರೆ ಅವನು 'ಚಿಕನ್' ಹೊರಗೊಮ್ಮಲು ಬಯಸಲಿಲ್ಲ!",
            "Why did the idli refuse to share its chutney? - Because it was 'too precious'! ಇಡ್ಲಿ ತನ್ನ ಚಟ್ನಿಯನ್ನು ಹಂಚಲು ನಿರಾಕರಿಸಿದನು ಏಕೆ? - ಏಕೆಂದರೆ ಇದು 'ಹೆಚ್ಚಿನ ಅಮೂಲ್ಯ'!",
            "Why don’t Indian chefs play poker? - Because they can’t handle a 'full house'! ಭಾರತೀಯ ಶೆಫ್‌ಗಳು ಪೋಕರ್ನಲ್ಲಿ ಆಟವಿಲ್ಲ ಏಕೆ? - ಏಕೆಂದರೆ ಅವರು 'ಫುಲ್ ಹೌಸ್' ಅನ್ನು ನಿರ್ವಹಿಸಲು ಸಾಧ್ಯವಿಲ್ಲ!",
            "What’s an Indian’s favorite way to eat chicken? - With a side of 'tandoori' and 'masala'! ಭಾರತೀಯನಿಗೆ ಚಿಕನ್ ತಿನ್ನಲು ಮೆಚ್ಚಿನ ಮಾರ್ಗ ಯಾವುದು? - 'ತಂದೂರಿ' ಮತ್ತು 'ಮಸಾಲೆ' ಜೊತೆಗೆ!",
            "Why did the dosa break up with the sambhar? - Because it was too 'watery'! ಡೋಸಾ ಸಾಮ್‌ಬರ್ ನೊಂದಿಗೆ ಬದಲಾಗಿದ ಏಕೆ? - ಏಕೆಂದರೆ ಅದು ಹೆಚ್ಚು 'ಜಲಮಯ'ವಾಗಿತ್ತು!",
            "Why did the Indian chef always have a big smile? - Because he loved to 'stir' things up! ಭಾರತೀಯ ಶೆಫ್ ಯಾವಾಗಲೂ ದೊಡ್ಡ ನಗುವನ್ನು ಹೊಂದಿದನು ಏಕೆ? - ಏಕೆಂದರೆ ಅವನು ವಿಷಯಗಳನ್ನು 'ಸ್ಟರ್' ಮಾಡಲು ಇಚ್ಛಿಸುತ್ತಾನೆ!",
             "What’s a common Indian’s favorite sport? - ‘Cricket’ and ‘Ludo’, because they both come with a lot of 'strategy' and 'drama'! ಸಾಮಾನ್ಯ ಭಾರತೀಯನಿಗೆ ಮೆಚ್ಚಿನ ಕ್ರೀಡೆ ಯಾವುದು? - 'ಕ್ರಿಕೆಟ್' ಮತ್ತು 'ಲುಡೋ', ಏಕೆಂದರೆ ಅವು ಎರಡೂ ಬಹಳಷ್ಟು 'ಆಯೋಜನೆ' ಮತ್ತು 'ನಾಟಕ' ಹೊಂದಿವೆ!",
            "Why don’t skeletons fight each other? - They don’t have the guts!, ಇಮ್ಮಡಿಗಳ ನಡುವೆ ಯಾಕೆ ಯುದ್ಧವಿಲ್ಲ? - ಅವರು ಜಿಗಿಯಲು ಸಿದ್ಧವಾಗಿಲ್ಲ!",
        };
                                    Random random = new Random();
                                    int randomIndex = random.Next(jokes.Count);
                                    await VS.MessageBox.ShowWarningAsync("Enjoy madi 😊 Project setup done", $"{jokes[randomIndex]}");
                                }
                                catch (Exception)
                                {

                                    await VS.MessageBox.ShowWarningAsync("Enjoy madi 😊 Project setup done", "This Developer wanted to tell u one joke at final step, but he saw his salary and he is still laughing");
                                }
                            }
                           else
                            {
                               await VS.MessageBox.ShowErrorAsync("Multiple Project Selection Error","Bhaiii! facing problem to set mulitple project as startup project, all changes done ,only this step u do manually plz bhai in free u are using this extension at least this u can do urself");
                             
                            }
                        }

                    }
                    else
                    {
                      await   VS.MessageBox.ShowErrorAsync("Dont know What Happend !!!", "Something Got Wrong While Modifying the files");
                    }
                }
                else
                {
                   await  VS.MessageBox.ShowErrorAsync("Kuch to Gadbad Hai", "Sorry Bro! something went wrong Please Contact at 8604470501 ");
                }
            }
            

       
        }
    }


}
