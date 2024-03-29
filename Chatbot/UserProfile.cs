﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.BotBuilderSamples
{
    using System.Collections.Generic;

    /// <summary>Contains information about a user.</summary>
    public class UserProfile
    {
        public string Name { get; set; }
        public int Age { get; set; }

        // The list of exercises the user wants to review.
        public List<string> ExercisesToReview { get; set; } = new List<string>();
    }
}