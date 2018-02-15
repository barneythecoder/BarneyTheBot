using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Net.Http;

namespace SimpleEchoBot
{
    [Serializable]
    public class ChatDialog : IDialog<object>
    {
        
        public async Task StartAsync(IDialogContext context)
        {

            await context.PostAsync("Welcome!");
            await context.PostAsync("What's on your mind? We'll get back to you ASAP! <br/><br/> " +
                    "And hey, if you want to report drivers, just reply with 'REPORT' without quotation marks :)");
            context.Wait(MessageReceivedAsync);

        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text == "REPORT")
            {
                await context.PostAsync("Ok, I'll help you with that!");
                await context.PostAsync("Reply with the license plate of the vehicle you want to report");
                context.Done(true);
            }
            else
            {
                context.Wait(MessageReceivedAsync);
            }
            
        }
    }

}