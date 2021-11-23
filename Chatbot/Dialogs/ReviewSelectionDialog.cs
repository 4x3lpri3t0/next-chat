// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples
{
    public class ReviewSelectionDialog : ComponentDialog
    {
        // Define a "done" response for the exercise selection prompt.
        private const string DoneOption = "done";

        // Define value names for values tracked inside the dialogs.
        private const string ExercisesSelected = "value-exercisesSelected";

        private FoundChoice chosenExercise;

        // Define value name for the selected choice in the quiz.
        private const string ChoiceSelected = "value-choiceSelected";

        // Define the exercise choices for the exercise selection prompt.
        private readonly List<string> _exerciseOptions = new List<string>()
        {
            "School Conversation",
            "English Small Talk",
        };

        private readonly Dictionary<string, string> exerciseVideoUrl = new Dictionary<string, string>()
        {
             { "School Conversation", "https://www.youtube.com/watch?v=7isSwerYaQc" },
             { "English Small Talk", "https://www.youtube.com/watch?v=0jhPrERjTxU" },
        };

        private readonly Dictionary<string, string[]> exerciseQuestions = new Dictionary<string, string[]>()
        {
            {
                "School Conversation",
                new string[] {
                    "Who is the blah blah",     // Q1
                    "Question 2",                // Q2
                    "Question 3"                // Q3
                }
            },
            {
                "English Small Talk",
                new string[] {
                    "Who is the blah blah",     // Q1
                    "Question 2",                // Q2
                    "Question 3"                // Q3
                }
            }
        };

        private readonly Dictionary<string, List<string>[]> exerciseOptions = new Dictionary<string, List<string>[]>()
        {
            {
                "School Conversation",
                new List<string>[] {
                    new List<string> { "Correct", "Incorrect", "Incorrect" },
                    new List<string> { "Correct", "Incorrect", "Incorrect" },
                    new List<string> { "Correct", "Incorrect", "Incorrect" },
                }
            },
            {
                "English Small Talk",
                new List<string>[] {
                    new List<string> { "Correct", "Incorrect", "Incorrect" },
                    new List<string> { "Correct", "Incorrect", "Incorrect" },
                    new List<string> { "Correct", "Incorrect", "Incorrect" },
                }
            },
        };

        private readonly Dictionary<string, string[]> exerciseAnswers = new Dictionary<string, string[]>()
        {
            {
                "School Conversation",
                new string[] {
                    "Correct",    // Correct answer to Q1
                    "Correct",   // Correct answer to Q2
                    "Correct"   // Correct answer to Q3
                }
            },
            {
                "English Small Talk",
                new string[] {
                    "Correct",   // Correct answer to Q1
                    "Correct",    // Correct answer to Q2
                    "Correct"    // Correct answer to Q3
                }
            }
        };

        public ReviewSelectionDialog()
            : base(nameof(ReviewSelectionDialog))
        {
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    SelectionStepAsync,
                    ExerciseStepAsync,
                    Question1Async,
                    Question2Async,
                    Question3Async,
                    LoopStepAsync,
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SelectionStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Continue using the same selection list, if any, from the previous iteration of this dialog.
            var list = stepContext.Options as List<string> ?? new List<string>();
            stepContext.Values[ExercisesSelected] = list;

            // Create a prompt message.
            string message;
            if (list.Count is 0)
            {
                message = $"Hi! Please choose an exercise to review, or `{DoneOption}` to finish.";
            }
            else
            {
                message = $"You have just reviewed the exercise **{list[0]}**. You can review an additional exercise, " +
                    $"or choose `{DoneOption}` to finish.";
            }

            if (_exerciseOptions.Count > 0 && chosenExercise != null)
            {
                // Remove the last exercise to reduce the size of the review list.
                _exerciseOptions.Remove(chosenExercise.Value);
            }

            if (_exerciseOptions.Count == 0)
            {
                message = $"Congratulations! That was your last exercise for today. Remember, by practicing daily we improve our language skills :)";
                return await stepContext.EndDialogAsync(message, cancellationToken);
            }

            // Create the list of options to choose from.
            var options = _exerciseOptions.ToList();
            options.Add(DoneOption);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text(message),
                RetryPrompt = MessageFactory.Text("Please choose an option from the list."),
                Choices = ChoiceFactory.ToChoices(options),
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> ExerciseStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Retrieve the choice they made.
            var list = stepContext.Values[ExercisesSelected] as List<string>;
            chosenExercise = (FoundChoice)stepContext.Result;

            var done = chosenExercise.Value == DoneOption;
            if (done)
            {
                // If they're done, exit and return their list.
                return await stepContext.EndDialogAsync(list, cancellationToken);
            }

            string exerciseChoiceValue = chosenExercise.Value;
            string exerciseChoiceValueVideoUrl = exerciseVideoUrl[exerciseChoiceValue];

            string status = $"You selected the {exerciseChoiceValue} unit. Please watch the following video related to it:";
            await stepContext.Context.SendActivityAsync(status);

            // Send video URL.
            var videoCard = new VideoCard()
            {
                Title = exerciseChoiceValue,
                Subtitle = $"Exercise: {exerciseChoiceValue}",
                Text = "Watch this video and reply the questions.",
                Image = new ThumbnailUrl(exerciseChoiceValueVideoUrl),
                Media = new MediaUrl[] { new MediaUrl(exerciseChoiceValueVideoUrl) },
            };
            var reply = MessageFactory.Attachment(videoCard.ToAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text($"Are you ready to answer some questions about the video?")
            };

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> Question1Async(
            WaterfallStepContext stepContext,
            CancellationToken cancellationToken)
        {
            // Display cute quiz png.
            var reply = MessageFactory.Attachment(GetImageInlineAttachment());
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            // Retrieve question text and possible options that the user can select from.
            string questionText = exerciseQuestions[chosenExercise.Value][0];
            List<string> questionOptions = exerciseOptions[chosenExercise.Value][0];

            var promptOptions = new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(questionOptions),
                Prompt = MessageFactory.Text($"Ok time for a short quiz! Choose the correct answer to this question: " + questionText),
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> Question2Async(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
        {
            // Retrieve the choice they made.
            string choice = ((FoundChoice)stepContext.Result).Value;
            string correctAnswer = exerciseAnswers[chosenExercise.Value][0];
            bool choiceIsCorrect = choice == correctAnswer;

            // Give feedback.
            string status = $"Correct! '{choice}' was the correct answer :)";
            if (!choiceIsCorrect)
            {
                status = $"Incorrect. You chose '{choice}' The correct answer was '{correctAnswer}'.";
            }
            await stepContext.Context.SendActivityAsync(status);

            // Retrieve question text and possible options that the user can select from.
            string questionText = exerciseQuestions[chosenExercise.Value][1];
            List<string> questionOptions = exerciseOptions[chosenExercise.Value][1];

            var promptOptions = new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(questionOptions),
                Prompt = MessageFactory.Text($"Ok, let's move on to the next question: " + questionText),
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> Question3Async(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
        {
            // Retrieve the choice they made.
            string choice = ((FoundChoice)stepContext.Result).Value;
            string correctAnswer = exerciseAnswers[chosenExercise.Value][1];
            bool choiceIsCorrect = choice == correctAnswer;

            // Give feedback.
            string status = $"Correct! '{choice}' was the correct answer :)";
            if (!choiceIsCorrect)
            {
                status = $"Incorrect. You chose '{choice}' The correct answer was '{correctAnswer}'.";
            }
            await stepContext.Context.SendActivityAsync(status);

            // Retrieve question text and possible options that the user can select from.
            string questionText = exerciseQuestions[chosenExercise.Value][2];
            List<string> questionOptions = exerciseOptions[chosenExercise.Value][2];

            var promptOptions = new PromptOptions
            {
                Choices = ChoiceFactory.ToChoices(questionOptions),
                Prompt = MessageFactory.Text($"Ok, let's move on to the LAST question: " + questionText),
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> LoopStepAsync(
        WaterfallStepContext stepContext,
        CancellationToken cancellationToken)
        {
            // Retrieve the choice they made.
            string choice = ((FoundChoice)stepContext.Result).Value;
            string correctAnswer = exerciseAnswers[chosenExercise.Value][2];
            bool choiceIsCorrect = choice == correctAnswer;

            // Give feedback.
            string status = $"Correct! '{choice}' was the correct answer :)";
            if (!choiceIsCorrect)
            {
                status = $"Incorrect. You chose '{choice}' The correct answer was '{correctAnswer}'.";
            }
            await stepContext.Context.SendActivityAsync(status);
            await stepContext.Context.SendActivityAsync("That was the last question for that exercise, good job!");

            //// Retrieve their selection list, the choice they made, and whether they chose to finish.
            var list = stepContext.Values[ExercisesSelected] as List<string>;
            //var choice = (FoundChoice)stepContext.Result;
            //var done = choice.Value == DoneOption;

            //if (done)
            //{
            //    // If they're done, exit and return their list.
            //    return await stepContext.EndDialogAsync(list, cancellationToken);
            //}

            // Add the chosen exercise to the list.
            list.Add(chosenExercise.Value);
            // Repeat this dialog, passing in the list from this iteration.
            return await stepContext.ReplaceDialogAsync(nameof(ReviewSelectionDialog), list, cancellationToken);
        }

        private static Attachment GetImageInlineAttachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, "Resources", "test.jpg");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Resources\test.jpg",
                ContentType = "image/jpg",
                ContentUrl = $"data:image/jpg;base64,{imageData}",
            };
        }
    }
}