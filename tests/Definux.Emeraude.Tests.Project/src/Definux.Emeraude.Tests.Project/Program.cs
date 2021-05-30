using System;
using Definux.Emeraude;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Definux.Emeraude.Tests.Project
{
    public class Program : EmStartup
    {
        public static void Main(string[] args)
        {
            RunEmeraude(CreateHostBuilder(args));
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            bool useKestrel = false;

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (useKestrel)
                    {
                        webBuilder.UseKestrel();
                    }

                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
