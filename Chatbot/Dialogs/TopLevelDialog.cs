// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace Microsoft.BotBuilderSamples
{
    public class TopLevelDialog : ComponentDialog
    {
        // Define a "done" response for the exercise selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string UserInfo = "value-userInfo";

        public TopLevelDialog()
            : base(nameof(TopLevelDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));

            AddDialog(new ReviewSelectionDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                CustomStartSelectionStepAsync,
                AcknowledgementStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> CustomStartSelectionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[UserInfo] = new UserProfile() { Name = "Pablo" };

            return await stepContext.BeginDialogAsync(nameof(ReviewSelectionDialog), null, cancellationToken);
        }

        private async Task<DialogTurnResult> AcknowledgementStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Set the user's exercise selection to what they entered in the review-selection dialog.
            var userProfile = (UserProfile)stepContext.Values[UserInfo];
            userProfile.ExercisesToReview = stepContext.Result as List<string> ?? new List<string>();

            // Thank them for participating.
            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text($"Thank you for reviewing your exercises, {((UserProfile)stepContext.Values[UserInfo]).Name}."),
                cancellationToken);

            // Exit the dialog, returning the collected user information.
            return await stepContext.EndDialogAsync(stepContext.Values[UserInfo], cancellationToken);
        }
    }
}