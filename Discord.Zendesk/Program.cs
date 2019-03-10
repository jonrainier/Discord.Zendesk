// Copyright (c) 2018 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Discord.Zendesk
{
    public class Program
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        ///     Main entry for the application
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///     Builds an instance of the web host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            if (Debugger.IsAttached)
                return WebHost.CreateDefaultBuilder(args)
                    .UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 80);
                        options.Listen(IPAddress.Any, 443,
                            listenOptions => { listenOptions.UseHttps(args[0]); });
                    })
                    .UseStartup<Startup>();

            if (args.Length == 0)
                return WebHost.CreateDefaultBuilder(args)
                    .UseUrls("http://0.0.0.0:5000")
                    .UseStartup<Startup>();

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://0.0.0.0:{args[0]}")
                .UseStartup<Startup>();
        }
    }
}