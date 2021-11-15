﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Emeraude.Admin.Adapters;
using Emeraude.Admin.Models;
using Emeraude.Admin.UI.Models;

namespace Definux.Emeraude.Tests.Project.Adapters
{
    public class AdminMenusBuilder : IAdminMenusBuilder
    {
        public async Task<SidebarSchema> BuildAsync()
            => await Task.FromResult(new SidebarSchema
            {
                InsightsItems = new List<SidebarEssentialLink>
                {
                    new ()
                    {
                        Title = "Client Login",
                        Route = "/login",
                        Icon = "mdi mdi-account",
                        Blank = true
                    }
                },
                Menus = new List<SidebarMenuSection>
                {
                    new ()
                    {
                        Title = "Dashboard",
                        Icon = "mdi mdi-television",
                        Children = new List<SidebarMenuLink>()
                        {
                            new ("Index", "/admin"),
                        }
                    }
                }
            });
    }
}