// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System;
using System.ComponentModel;
using System.Net.Http;
using Discord.Zendesk.Content;
using Discord.Zendesk.Enums;
using Discord.Zendesk.Extensions;
using Discord.Zendesk.Models.Api;
using Microsoft.AspNetCore.Mvc;

namespace Discord.Zendesk.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        [HttpPost]
        public async void Post([FromBody] TicketModel ticket)
        {
            var ticketStatus = Enum.Parse<ColorType>(ticket.Status);
            var content =
                new JsonContent(new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = $"{ticket.Title}",
                            type = "rich",
                            color = Convert.ToInt32(ticketStatus.GetAttributeOfType<DescriptionAttribute>().Description, 16),
                            description = $"[Ticket #{ticket.Id} - {ticketStatus}]({ticket.Url})\r\n{ticket.Comment}",
                            footer = new
                            {
                                text = $"Updated at {ticket.Updated:G} by {ticket.Sender}"
                            }
                        }
                    }
                });

            await new HttpClient().PostAsync(Program.Discord.WebHookUrl, content);
        }
    }
}