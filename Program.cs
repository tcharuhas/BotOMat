using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace API {
    public class Program {
        public static void Main (string[] args) {
            // this is for debugging in prod when IIS starts the program.
            // It's difficult to tell if it's actually been started or not when
            // there's an error.
            File.WriteAllText("startupentry.txt", $"Starting Server");
            try {
                BuildWebHost (args).Run ();
            } catch (Exception e) {
                File.WriteAllText("startuperror.txt", $"{e.Message}\n\n{e.StackTrace}");
            }
        }

        public static IWebHost BuildWebHost (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseIISIntegration()
            .UseStartup<Startup> ()
            .UseUrls(urls: "http://localhost:5000")
            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.1&tabs=aspnetcore2x 
            .Build ();
    }
}