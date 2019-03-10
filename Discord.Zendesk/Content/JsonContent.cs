// Copyright (c) 2019 Initial Servers LLC. All rights reserved.
// https://initialservers.com/

using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Discord.Zendesk.Content
{
    public class JsonContent : StringContent
    {
        public JsonContent(object obj) :
            base(JsonConvert.SerializeObject(obj, Program.JsonSerializerSettings), Encoding.UTF8, "application/json")
        {
        }
    }
}