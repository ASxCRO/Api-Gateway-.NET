﻿using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Core.HttpServices
{
    public interface IAPIHttpService
    {
        public HttpRequestMessage ForgeRequest(object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials);
        public Task<HttpResponseMessage> SendRequest(string clientName, HttpRequestMessage request, string token = null);
        public Task<HttpResponseMessage> Fetch(string clientName, object requestModel, HttpMethod httpMethod, string uri, BrowserRequestMode browserRequestMode, BrowserRequestCredentials browserRequestCredentials, string token = null);
    }
}
