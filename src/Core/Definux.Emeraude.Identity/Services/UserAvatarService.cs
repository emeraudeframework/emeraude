﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Files;
using Definux.Emeraude.Application.Identity;
using Definux.Emeraude.Application.Persistence;
using Definux.Emeraude.Domain.Entities;
using Definux.Emeraude.Essentials.Base;
using Definux.Emeraude.Identity.Entities;
using Definux.Utilities.Functions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Definux.Emeraude.Identity.Services
{
    /// <inheritdoc cref="IUserAvatarService"/>
    public class UserAvatarService : IUserAvatarService
    {
        private readonly IEmContext context;
        private readonly UserManager<User> userManager;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ISystemFilesService systemFilesService;
        private readonly IRootsService rootsService;
        private readonly ILogger<UserAvatarService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAvatarService"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="hostEnvironment"></param>
        /// <param name="systemFilesService"></param>
        /// <param name="rootsService"></param>
        /// <param name="logger"></param>
        public UserAvatarService(
            IEmContext context,
            UserManager<User> userManager,
            IWebHostEnvironment hostEnvironment,
            ISystemFilesService systemFilesService,
            IRootsService rootsService,
            ILogger<UserAvatarService> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
            this.systemFilesService = systemFilesService;
            this.rootsService = rootsService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task ApplyAvatarToUserAsync(Guid userId, string avatarUrl)
        {
            try
            {
                var user = await this.userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    user.AvatarUrl = avatarUrl;
                    this.context.Set<User>().Update(user);

                    await this.context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured during applying avatar to a user");
            }
        }

        /// <inheritdoc/>
        public async Task<string> CreateAvatarFromUrlAsync(string externalSourceAvatarUrl)
        {
            try
            {
                string targetFileDirectoryPath = this.rootsService.GetPathFromPublicRoot(EmFolders.UploadFolderName, EmFolders.ImagesFolderName);
                string avatarName = FilesFunctions.GetUniqueFileName();
                string avatarNameWithExtension = $"{avatarName}.jpg";
                string targetFilePath = Path.Combine(targetFileDirectoryPath, avatarNameWithExtension);

                using (var client = new HttpClient())
                using (var response = await client.GetAsync(externalSourceAvatarUrl))
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                using (FileStream fileStream = System.IO.File.Create(targetFilePath))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        await stream.CopyToAsync(fileStream);

                        return $"/{EmFolders.UploadFolderName}/{EmFolders.ImagesFolderName}/{avatarNameWithExtension}";
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured during applying avatar to a user");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<string> CreateUserAvatarAsync(byte[] avatarFileByteArray)
        {
            try
            {
                string avatarRelativePath = Path.Combine(EmFolders.UploadFolderName, EmFolders.ImagesFolderName);
                string targetFileDirectoryPath = Path.Combine(this.hostEnvironment.ContentRootPath, EmFolders.PublicRootFolderName, avatarRelativePath);
                string avatarName = FilesFunctions.GetUniqueFileName();
                string avatarNameWithExtension = $"{avatarName}.jpg";
                string targetFilePath = Path.Combine(targetFileDirectoryPath, avatarNameWithExtension);

                using (Stream stream = new MemoryStream(avatarFileByteArray))
                using (FileStream fileStream = System.IO.File.Create(targetFilePath))
                {
                    await stream.CopyToAsync(fileStream);

                    return $"/{EmFolders.UploadFolderName}/{EmFolders.ImagesFolderName}/{avatarNameWithExtension}";
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "An error occured during creating user avatar");
                return null;
            }
        }

        /// <inheritdoc/>
        public string GetUserAvatarRelativePath(IUser user)
        {
            return ((User)user)?.AvatarRelativePath;
        }
    }
}
