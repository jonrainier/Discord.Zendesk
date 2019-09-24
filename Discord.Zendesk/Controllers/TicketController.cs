// Copyright (c) 2019 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
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
        public Task<HttpResponseMessage> Post([FromBody] TicketModel ticket)
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
                            color = Convert.ToInt32(ticketStatus.GetAttributeOfType<DescriptionAttribute>().Description,
                                16),
                            description = $"[Ticket #{ticket.Id} - {ticketStatus}]({ticket.Url})\r\n{ticket.Comment}",
                            footer = new
                            {
                                text = $"Updated at {ticket.Updated.ToLocalTime():G} by {ticket.Sender}"
                            }
                        }
                    }
                });

            return new HttpClient().PostAsync(Startup.WebHookUrl, content);
        }
    }
}