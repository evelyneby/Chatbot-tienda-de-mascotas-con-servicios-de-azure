// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using HuellitasCompleto.Common.Cards;
using Microsoft.AspNetCore.Http;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HuellitasCompleto
{
    public class HuellitasCompleto<T> : ActivityHandler where T: Dialog
    {

        private readonly BotState _userState;
        private readonly BotState _conversationState;
        private readonly Dialog _dialog;

        public HuellitasCompleto(UserState userState, ConversationState conversationState, T dialog)
        { 
            _userState = userState;
            _conversationState = conversationState;
            _dialog = dialog;
        
        }

        

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hola, para darte un mejor servicio puedes preguntarme que opciones tengo para ayudarte."), cancellationToken);

                }
            }
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);

        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await _dialog.RunAsync(
                turnContext,
                _conversationState.CreateProperty<DialogState>(nameof(DialogState)),
                cancellationToken
                );


        }

    }
}
