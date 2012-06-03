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
using MvcImage.Models;


namespace MvcImage.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterModel RegisterModel { get; set; }
        public string UniqueKey { get; set; }
        public bool Thumbnail { get; set; }
 
    }
}