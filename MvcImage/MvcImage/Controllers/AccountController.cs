/*
 * Comments.
 * ---------------------------------------------------------------------------------------------------
 * Date         |  Who          |  Version      | Description of Change
 * ---------------------------------------------------------------------------------------------------
 * 28/02/12      Gareth B           Alpha 0.0.2   Added Thumbail Support. 
 * 20/03/12      Gareth B           Alpha 0.0.3   Added UserList Support (Dashboard).
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MvcImage.Models;
using MvcImage.ViewModels;
using Bradaz.OpenSource.Images;
using Bradaz.OpenSource.IO;

namespace MvcImage.Controllers
{
    public class AccountController : Controller
    {
        private ImageContext db = new ImageContext();
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public ActionResult UserListDashboard()
        {
            
            var viewModel = Membership
                .GetAllUsers()
                .Cast<MembershipUser>()
                .Select(vm => new UserListDashboard
                {
                    UniqueId = vm.ProviderUserKey.ToString(),
                    UserName = vm.UserName,
                    LastActivityDate = vm.LastLoginDate
                });
                
            return View(viewModel);
        }
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            var viewModel = new RegisterViewModel
            {
                Thumbnail = true
            };

            return View(viewModel);
        }



        // POST: /Account/Register

        [HttpPost]
        public ActionResult RegisterDebug(bool Thumbnail,[Bind(Prefix="RegisterModel")]RegisterModel RegisterModel, HttpPostedFileBase imageLoad2)
        {

            return View(RegisterModel);
        }
        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(bool Thumbnail, [Bind(Prefix = "RegisterModel")]RegisterModel registermodel, 
            HttpPostedFileBase imageLoad2)
        {
   

            var AccountImage = new ImageModel();


            if (imageLoad2 != null)
            {
                using (Image img = Image.FromStream(imageLoad2.InputStream))
                {

                    //--Initalise the size of the array
                    byte[] file = new byte[imageLoad2.InputStream.Length];

                    //--Create a new BinaryReader and set the InputStream for the Images InputStream to the
                    //--beginning, as we create the img using a stream.
                    BinaryReader reader = new BinaryReader(imageLoad2.InputStream);
                    imageLoad2.InputStream.Seek(0, SeekOrigin.Begin);

                    //--Load the image binary.
                    file = reader.ReadBytes((int)imageLoad2.InputStream.Length);

                    //--Create a new image to be added to the database
                    AccountImage.RecordCreatedDate = DateTime.Now;
                    AccountImage.RecordAmendedDate = DateTime.Now;
                    AccountImage.Source = file;
                    AccountImage.FileSize = imageLoad2.ContentLength;
                    AccountImage.FileName = imageLoad2.FileName;
                    AccountImage.ContentType = imageLoad2.ContentType;
                    AccountImage.Height = img.Height;
                    AccountImage.Width = img.Width;
                    
#if DEBUG
                   AccountImage.RecordStatus = "T";            //--Testing.
#else
                   AccountImage.RecordStatus = " ";            //--Live.
#endif
                   //-- Now we create the thumbnail and save it.
                   if (Thumbnail == true)
                   {
                       byte[] thumbnail = Images.CreateThumbnailToByte(imageLoad2.InputStream, 100, 100);
                       AccountImage.ThumbnailSource = thumbnail;
                       AccountImage.ThumbnailFileSize = thumbnail.Length;
                       AccountImage.ThumbnailContentType = Files.GetContentType(imageLoad2.FileName);
                       AccountImage.ThumbnailHeight = Images.FromByteHeight(thumbnail);
                       AccountImage.ThumbnailWidth = Images.FromByteWidth(thumbnail);
                   }

                }
      
            }

            if (ModelState.IsValid)
            {
                registermodel.UserId = Guid.NewGuid();
                // Attempt to register the user
                // MembershipCreateStatus createStatus;
                MembershipCreateStatus createStatus = MembershipService.CreateUser(registermodel.UserName, registermodel.Password, registermodel.Email, registermodel.UserId);
                // Membership.CreateUser(registermodel.UserName, registermodel.Password, registermodel.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(registermodel.UserName, false /* createPersistentCookie */);
                    //--If the registeration is an success then
                    //--save the Image away.
                    AccountImage.UniqueId = registermodel.UserId;
                    AccountImage.TableLink = registermodel.UserId;
                    db.Images.Add(AccountImage);
                    db.SaveChanges();
                 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            var viewModel = new RegisterViewModel
            {
                RegisterModel = registermodel,
                Thumbnail = Thumbnail,
                UniqueKey = registermodel.UserId.ToString()

            };
            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }




        
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
