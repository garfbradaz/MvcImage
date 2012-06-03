/*
 * Comments.
 * ---------------------------------------------------------------------------------------------------
 * Date         |  Who          |  Version      | Description of Change
 * ---------------------------------------------------------------------------------------------------
 * 28/02/12      Gareth B           Alpha 0.0.2   Added Thumbail Support. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using DataAnnotationsExtensions.ClientValidation;
using Bradaz.OpenSource.Images;
namespace MvcImage.Models
{
    /// <summary> 
    /// This class represents the table for Images and it necessary columns
    /// </summary>
    public class ImageModel : IImageModel
    {
        [Key]
        public Guid UniqueId { get; set; }
        [Required]
        public Guid TableLink { get; set; }
        public String RecordStatus { get; set; }
        [Date]
        public DateTime RecordCreatedDate { get; set; }
        [Date]
        public DateTime RecordAmendedDate { get; set; }
        public Byte[] Source { get; set; }
        public Int32 FileSize { get; set; }
        public String FileName { get; set; }
        [FileExtensions("png|jpg|jpeg|gif")]
        public String FileExtension { get; set; }
        public String ContentType { get; set; }
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        
        //-- New in Alpha Release 0.0.2
        public Byte[] ThumbnailSource { get; set; }
        public Int32 ThumbnailFileSize { get; set; }
        public String ThumbnailContentType { get; set; }
        public Int32 ThumbnailHeight { get; set; }
        public Int32 ThumbnailWidth { get; set; }
        
        



    }
}