// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System.Collections.Generic;

namespace Discord.Zendesk.Models
{
    public class HostModel
    {
        public Dictionary<string, Endpoint> Endpoints { get; set; }
    }

    public class Endpoint
    {
        public bool IsEnabled { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

        public Certificate Certificate { get; set; }
    }

    public class Certificate
    {
        public string Source { get; set; }

        public string Path { get; set; }

        public string Password { get; set; }
    }
}