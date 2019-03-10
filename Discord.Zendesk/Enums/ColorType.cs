// Copyright (c) 2019 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System.ComponentModel;

namespace Discord.Zendesk.Enums
{
    public enum ColorType
    {
        [Description("59BBE0")] Pending,
        [Description("F5CA00")] New,
        [Description("E82A2A")] Open,
        [Description("828282")] Solved
    }
}