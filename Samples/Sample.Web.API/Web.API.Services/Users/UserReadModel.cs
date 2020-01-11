﻿using Newtonsoft.Json;

namespace Synergy.Samples.Web.API.Services.Users
{
    public class UserReadModel
    {
        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("login")]
        public string Login { get; }

        public UserReadModel(string id, string login)
        {
            Id = id;
            Login = login;
        }
    }
}