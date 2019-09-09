using System;
using Newtonsoft.Json.Linq;
using NuGet_CLI_Tools;

namespace WebhookDemo.Business
{
    public static class HookHandler
    {
        public static bool Handle(JObject jsonObject)
        {
            var url = (string) jsonObject["repository"]["html_url"];
            var name = (string) jsonObject["repository"]["name"];
            var description = (string) jsonObject["repository"]["description"];
            var author = (string) jsonObject["repository"]["owner"]["name"];
            var pushed = (long) jsonObject["repository"]["pushed_at"];
            
            Console.WriteLine($"Received webhook for {name}");
            
            var path = GitHelper.CloneRepository(url, (string) jsonObject["repository"]["master_branch"]);

            var packagePath = NuGetHelper.CreateNuGetPackage(path, name, $"1.0.0-{pushed}", description, author);

            bool success = NuGetHelper.PublishNuGetPackage(packagePath);

            if (success)
            {
                Console.WriteLine($"Published package for {name}...");
            }

            return success;
        }
    }
}