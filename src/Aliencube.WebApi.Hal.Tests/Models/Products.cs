﻿using System.Collections.Generic;
using Aliencube.WebApi.Hal.Resources;

namespace Aliencube.WebApi.Hal.Tests.Models
{
    /// <summary>
    /// This represents the entity for product set.
    /// </summary>
    public class Products : LinkedResourceCollection<Product>
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="Products" /> class.
        /// </summary>
        public Products()
            : base()
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="Products" /> class.
        /// </summary>
        /// <param name="items">List of <see cref="Product" /> objects.</param>
        public Products(List<Product> items)
            : base(items)
        {
        }
    }
}