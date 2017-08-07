﻿using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Urlshortener.Models;
using Urlshortener.Functions;

namespace urlshortener
{
    public class PostNewUrl
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string name)
        {
            Assembly assembly = typeof(PostNewUrl).GetTypeInfo().Assembly;

            var builder = new ConfigurationBuilder()
                .AddJsonFile(new EmbeddedFileProvider(assembly, "post_new_url"), "appsettings.json", true, false);

            var configuration = builder.Build();

            var request = new ShortUrlRequest { OriginalUrl = name };

            var result = new ShortUrlResponse(
                (shortUrl) => ShortenUrlFunctions.SaveShortUrl(() => configuration["StorageConnection"], shortUrl),
                ShortenUrlFunctions.ShortenUrl,
                (hash) => ShortenUrlFunctions.RetrieveShortUrl(() => configuration["StorageConnection"], hash),
                request);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
