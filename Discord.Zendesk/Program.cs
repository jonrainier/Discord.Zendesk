// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Discord.Zendesk.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Discord.Zendesk
{
    public class Program
    {
        public static DiscordModel Discord { get; set; }

        public static JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(SetHost)
                .UseStartup<Startup>()
                .Build();
        }

        private static void SetHost(KestrelServerOptions options)
        {
            var configuration = (IConfiguration) options.ApplicationServices.GetService(typeof(IConfiguration));
            var host = configuration.GetSection("WebHost").Get<HostModel>();

            Discord = configuration.GetSection("Discord").Get<DiscordModel>();

            foreach (var endpointKvp in host.Endpoints)
            {
                var endpointName = endpointKvp.Key;
                var endpoint = endpointKvp.Value;
                if (!endpoint.IsEnabled) continue;

                var address = IPAddress.Parse(endpoint.Address);
                options.Listen(address, endpoint.Port, opt =>
                {
                    if (endpoint.Certificate == null) return;

                    var path = Path.Combine(Directory.GetCurrentDirectory(), endpoint.Certificate.Path);
                    var cert = new X509Certificate2(path, endpoint.Certificate.Password,
                        X509KeyStorageFlags.MachineKeySet);

                    switch (endpoint.Certificate.Source)
                    {
                        case "File":
                            opt.UseHttps(cert);
                            break;
                        default:
                            throw new NotImplementedException(
                                $"The source {endpoint.Certificate.Source} is not yet implemented");
                    }
                });

                options.UseSystemd(); //?
            }
        }
    }
}