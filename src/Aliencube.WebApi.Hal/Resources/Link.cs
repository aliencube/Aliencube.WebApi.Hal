namespace Aliencube.WebApi.App.Resources
{
    /// <summary>
    /// This represents the entity for link.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Gets or sets the rel of the link.
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// Gets or sets the href of the link.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Gets a value indicating whether whether the <c>Href</c> is templated URI or not.
        /// </summary>
        public bool IsHrefTemplated
        {
            get { return this.Href.Contains("{") && this.Href.Contains("}"); }
        }

        /// <summary>
        /// Gets or sets the type of the link.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the alternative URI for deprecation.
        /// </summary>
        public string Deprecation { get; set; }

        /// <summary>
        /// Gets or sets the name of the link.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the profile of the link.
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Gets or sets the title of the link.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the language of the link href.
        /// </summary>
        public string HrefLang { get; set; }
    }
}