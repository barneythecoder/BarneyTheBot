using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;

namespace SimpleEchoBot
{
    [Serializable]
    public class BadDriverDialog : IDialog<object>
    {
        //protected int count = 1;
        //protected bool sessionStarted = false;
        protected string licensePlate;
        protected List<string> badReportsList = new List<string> {
            "Did Signal",
            "Lacks Driving IQ",
            "Lane Hogging",
            "Illegal/Wrong Parking",
            "Improper Pick-up/Drop-off",
            "Blocked Intersection",
            "Smoke Belching",
            "Counterflow",
            "Ran a Red Light",
            "Slow in a Fast Lane",
            "Horn and Light Abuse",
            "Dangerous Driver",
            "PUV Refused Passenger",
            "PUV Charged Wrong Fare",
            "I change my mind"};

        public async Task StartAsync(IDialogContext context)
        {

            //await context.PostAsync("Welcome!");
            context.UserData.TryGetValue<string>("licensePlate", out licensePlate);

            PromptDialog.Choice(context,
                      this.OnOptionSelected,
                      badReportsList,
                      $"Report {licensePlate} for?",
                      "Not a valid option", 3);
        }
        public async Task OnOptionSelected(IDialogContext context, IAwaitable<string> argument)
        {
            try
            {
                var confirm = await argument;
                bool validOption = false;

                if (confirm == "I change my mind")
                {
                    await context.PostAsync("You opted to cancel reporting. <br/><br/>" +
                        "Report drivers by replying with their license plate or reply with 'chat' if you want to chat instead :)");
                    context.Done(true);
                }

                else
                {
                    foreach (var item in badReportsList)
                    {
                        if (confirm == item)
                        {
                            validOption = true;
                        }
                    }

                    if (validOption)
                    {
                        await context.PostAsync($"You reported {licensePlate} for: {confirm}.  <br/><br/>" +
                            $"Thanks for your report! If you register with us (www.conscience.ac) you can earn rewards and keep track of all the reports sent about you. <br/><br/>"
                            + "Download our Android app to make reporting easier: https://play.google.com/store/apps/details?id=com.bbb.autoconscience&hl=en <br/><br/>" +
                            "To report another vehicle  type in its license number or conduction sticker. If you'd like to chat with us just type 'chat' (without quotation marks).");
                        context.Done(true);
                    }
                }

            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Seems like you're having trouble reporting :( <br/><br/>" +
                    $"Reply with the license plate you want to report to start over. Or reply with 'chat' to chat instead.");
                context.Done(true);
            }
            catch (Exception ex)
            {

            }











            #region MyRegion
            //try
            //{
            //    var confirm = await argument;
            //    bool validOption = false;
            //    foreach (var item in badReportsList)
            //    {
            //        if (confirm == item)
            //        {
            //            validOption = true;
            //        }
            //    }

            //    if (validOption)
            //    {

            //        await context.PostAsync($"You reported {licensePlate} for: {confirm}.  <br/><br/>" +
            //            $"Thanks for your report! If you register with us (www.conscience.ac) you can earn rewards and keep track of all the reports sent about you. <br/><br/>"               
            //            + "Download our Android app to make reporting easier: https://play.google.com/store/apps/details?id=com.bbb.autoconscience&hl=en <br/><br/>" +
            //            "To report another vehicle  type in its license number or conduction sticker. If you'd like to chat with us just type 'chat' (without quotation marks).");
            //        context.Done(true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await context.PostAsync($"Failed with message: {ex.Source}, {ex.StackTrace}");
            //} 
            #endregion
        }
    }

}