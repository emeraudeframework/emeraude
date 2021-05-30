﻿using Newtonsoft.Json;

namespace Definux.Emeraude.Identity.ExternalProviders.Facebook
{
    /// <summary>
    /// Helper class for mapping Facebook profile picture data.
    /// </summary>
    public class ProfilePictureData
    {
        /// <summary>
        /// Url of the picture.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
