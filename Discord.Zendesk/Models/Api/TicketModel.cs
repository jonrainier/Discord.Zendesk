// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System;

namespace Discord.Zendesk.Models.Api
{
    public class TicketModel
    {
        public string Status { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string Comment { get; set; }

        public string Sender { get; set; }

        public DateTime Updated { get; set; }
    }
}