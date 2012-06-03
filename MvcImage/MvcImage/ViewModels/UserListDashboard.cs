/*
 * Comments.
 * ---------------------------------------------------------------------------------------------------
 * Date         |  Who          |  Version      | Description of Change
 * ---------------------------------------------------------------------------------------------------
 * 28/02/12      Gareth B           Alpha 0.0.3   Started life.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcImage.Models;

namespace MvcImage.ViewModels
{
    public class UserListDashboard
    {
        public string UniqueId { get; set; }
        public string UserName { get; set; }
        public DateTime LastActivityDate {get; set;}
        public Guid Key { get; set; }
    }
}