using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;
using SimpleEchoBot;
using System.Text.RegularExpressions;

namespace Microsoft.Bot.Sample.SimpleEchoBot
{
    [Serializable]
    public class EchoDialog : IDialog<object>
    {
        protected int count = 0;
        protected string licensePlate = "";
        protected bool sessionStarted = false;

        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("Welcome!");
            context.Wait(MessageReceivedAsync);
            
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;


            if (message.Text == "chat"|| message.Text == "Chat")
            {
                context.Call(new ChatDialog(), this.ResumeAfterOptionDialog);
            }
            else
            {
                if (message.Text.Length < 4 || message.Text.Length > 8)
                {
                    await context.PostAsync($"Hmmm, I seem to be having trouble with the License Plate {message.Text}, can you repeat that? <br/><br/> Reply with 'chat' if you want to chat instead :)" +
                           $"<br/><br/>EX: ABC 123");
                    context.Wait(MessageReceivedAsync);
                }
                else
                {
                    
                    //check if lp has digits yo
                    bool isDigitPresent = Regex.IsMatch(message.Text, @".*([\d]+).*");
                    if (!isDigitPresent)
                    {
                        await context.PostAsync($"Hmmm, License Plate {message.Text}, doesn't contain any numbers. <br/><br/> Reply with 'chat' if you want to chat instead :)" +
                           $"<br/><br/>EX: ABC 123");
                        context.Wait(MessageReceivedAsync);
                    }
                    else
                    {
                        this.licensePlate = message.Text;
                        await context.PostAsync($"You are reporting {message.Text}.");

                          PromptDialog.Choice(
                          context,
                          this.OnOptionSelected,
                          new List<string>() { "A good one!", "A bad one", "I change my mind" },
                          $"Is {message.Text} a good driver or a bad one?",
                          "Not a valid option :(", 3);

                    }
                }
            }
            #region MyRegion
            //else
            //{
            //    if (message.Text.Substring(6).Length > 9 || message.Text.Substring(6).Length < 4)
            //    {
            //        await context.PostAsync($"Hmmm, I seem to be having trouble with the License Plate {message.Text.Substring(6)}, can you repeat that?" +
            //            $"<br><br>EX: ABC 123");
            //    }
            //    else
            //    {
            //        PromptDialog.Choice(
            //                  context,
            //                  this.OnOptionSelected,
            //                  new List<string>() { "Good Driving", "Bad Driving" },
            //                  $"Report {message.Text} for?",
            //                  "Not a valid option", 3);
            //    }
            //}




            //if (message.Text.StartsWith("REPORT"))
            //{
            //    if (message.Text.Substring(6).Length>9|| message.Text.Substring(6).Length < 4)
            //    {
            //        await context.PostAsync($"Hmmm, I seem to be having trouble with the License Plate {message.Text.Substring(6)}, please report again with REPORT followed by the license plate. " +
            //            $"<br><br>EX: REPORT ABC 123");
            //    }
            //    else
            //    {
            //        this.licensePlate = message.Text.Substring(6);
            //        /*PromptDialog.Confirm(
            //            context,
            //            AfterResetAsync,
            //            "Are you sure you want to reset the count?",
            //            "Didn't get that!",
            //            promptStyle: PromptStyle.Auto);*/

            //        PromptDialog.Choice(
            //              context,
            //              this.OnOptionSelected,
            //              new List<string>() { "Good Driving", "Bad Driving" },
            //              "Report Driver for?",
            //              "Not a valid option", 3);

            //        /*await context.PostAsync($"KUNIN MO YUNG SUSI");*/
            //        //context.Wait(MessageReceivedAsync);
            //    }

            //}            
            //else
            //{
            //    /*await context.PostAsync($"{this.count++}: You said {message.Text}");*/
            //    this.count++;

            //    if (!this.sessionStarted)
            //    {
            //        await context.PostAsync($"Thank you for messaging AutoConscience! We will get back to you shortly <br/> <br/>You can report drivers on the road by replying with REPORT followed by the license plate"
            //                        + " (REPORT ABC 123)");
            //        this.sessionStarted = true;
            //    }                 

            //    context.Wait(MessageReceivedAsync);
            //} 
            #endregion
        }
        

        public async Task OnOptionSelected(IDialogContext context, IAwaitable<string> argument)
        {
            var confirm = await argument;
            try
            {
                if (confirm == "A good one!")
                {
                    try
                    {
                        context.UserData.SetValue<string>("licensePlate", this.licensePlate);
                        context.Call(new GoodDriverDialog(), this.ResumeAfterOptionDialog);
                    }
                    catch (Exception ex)
                    {
                        await context.PostAsync($"Failed with message: {ex.Source}, {ex.StackTrace}");
                    }




                    #region MyRegion
                    /*this.count = 1;*/
                    /*PromptDialog.Choice(context, 
                    this.OnOptionSelected, 
                    new List<string>() { "Used Signal", "Allowed Lane Merge", "Kept Intersection Open", "Followed Right of Way","Patient Driver","Defensive Driver","Nice Car"}, 
                    "Report Driver for?", 
                    "Not a valid option", 3);*/
                    //context.Wait(MessageReceivedAsync);
                    //await context.PostAsync("Good"); 
                    #endregion
                }
                else if (confirm == "A bad one")
                {
                    #region MyRegion
                    /*this.count = 1;*/
                    /*PromptDialog.Choice(context, 
                    this.OnOptionSelected, 
                    new List<string>() { "Did not Signal", "Lacks Driving IQ", "Lane Hogging", "Illegal or Wrong Parking","Improper Pick-up or Drop-off","Blocked Intersectoin","Smoke Belching","Counterflow","Ran a Red Light", "Horn or Light Abuse","Dangerous Driver"}, 
                    "Report Driver for?", 
                    "Not a valid option", 3);*/
                    //await context.PostAsync("Good"); 
                    #endregion

                    try
                    {
                        context.UserData.SetValue<string>("licensePlate", this.licensePlate);
                        context.Call(new BadDriverDialog(), this.ResumeAfterOptionDialog);
                    }
                    catch (Exception ex)
                    {
                        await context.PostAsync($"Failed with message: {ex.Source}, {ex.StackTrace}");
                    }
                }
                else if (confirm == "I change my mind")
                {
                    await context.PostAsync("You opted to cancel reporting.");
                    await context.PostAsync("Report a vehicle by replying with their license plate or reply with 'chat' to chat instead.");
                    context.Wait(MessageReceivedAsync);
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync($"Seems like you're having trouble reporting :( <br/><br/>" +
                    $"Reply with the license plate you want to report to start over. Or reply with 'chat' to chat instead.");
                context.Wait(MessageReceivedAsync);
            }
            //context.Wait(MessageReceivedAsync);
        }
        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                this.sessionStarted = false;
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Source}, {ex.StackTrace}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }

    }
}