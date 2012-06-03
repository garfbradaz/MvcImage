using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcImage.Models
{
    public class ImageContext : DbContext
    {
        public DbSet<ImageModel> Images { get; set; }

    }

}