/*
 * Comments.
 * ---------------------------------------------------------------------------------------------------
 * Date         |  Who          |  Version      | Description of Change
 * ---------------------------------------------------------------------------------------------------
 * 28/02/12      Gareth B           Alpha 0.0.2   Added Thumbail Support. Added AjaxSubmit and ImageLoad
 *                                                logic to cope with this.
 * 21/03/12      Gareth B           Alpha 0.0.3   GetThumbnailImage controller to get the thumbail image only.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcImage.Models;
using Bradaz.OpenSource;
using Bradaz.OpenSource.Images;

namespace MvcImage.Controllers
{ 
    public class ImageController : Controller
    {
        private ImageContext db = new ImageContext();

        //
        // GET: /Image/

        public ViewResult Index()
        {
            return View(db.Images.ToList());
        }

        //
        // GET: /Image/Details/5

        public ViewResult Details(Guid id)
        {
            ImageModel imagemodel = db.Images.Find(id);
            return View(imagemodel);
        }

        //
        // GET: /Image/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Image/Create

        [HttpPost]
        public ActionResult Create(ImageModel imagemodel)
        {
            if (ModelState.IsValid)
            {
                imagemodel.UniqueId = Guid.NewGuid();
                db.Images.Add(imagemodel);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(imagemodel);
        }
        
        //
        // GET: /Image/Edit/5
 
        public ActionResult Edit(Guid id)
        {
            ImageModel imagemodel = db.Images.Find(id);
            return View(imagemodel);
        }

        //
        // POST: /Image/Edit/5

        [HttpPost]
        public ActionResult Edit(ImageModel imagemodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imagemodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imagemodel);
        }

        //
        // GET: /Image/Delete/5
 
        public ActionResult Delete(Guid id)
        {
            ImageModel imagemodel = db.Images.Find(id);
            return View(imagemodel);
        }

        //
        // POST: /Image/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {            
            ImageModel imagemodel = db.Images.Find(id);
            db.Images.Remove(imagemodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #region GetImage
        /// <summary>
        /// This controller method returns a binary image to be viewed client side.
        /// (HTTP GET) 
        /// </summary>
        /// <param name="id">Guid to lookup in the Database.</param>
        /// <param name="tableLink">true = Use the image to locate the image from a seperate linking table id false = Image tables id lookup</param>
        /// <returns>binary image</returns>
        [HttpGet]
        public ActionResult GetImage(Guid id, bool tableLink)
        {
            ImageModel image = new ImageModel();
            byte[] imageSource = null;
            int count = 0;

            if (tableLink == true)
            {
                //--We either need to lookup the Image in the Database via the Secondary Key.....
                count = db.Images.Count(c => c.TableLink == id);

                //--Found a record.
              
                //--TODO: ADD THIS AS A CLASS TO Bradaz.OpenSource.MVC
                if (count > 0)
                {
                    image = db.Images.SingleOrDefault(i => i.TableLink == id);

                    //--Convert the Image data into Memory, and return it to the client.
                    imageSource = image.Source;
                    return File(imageSource, image.ContentType, image.FileName);

                }
      
            }
            else
            {
                //--....or we can lookup using the id of the image
                count = db.Images.Count(c => c.UniqueId == id);

                //--Found a record
                if (count > 0)
                {
                    image = db.Images.SingleOrDefault(i => i.UniqueId == id);

                    //--Convert the Image data into Memory, and return it to the client.
                    imageSource = image.Source;
                    return File(imageSource, image.ContentType, image.FileName);

                }
            }

            return File(new byte[0], null);
        }
        #endregion

        #region GetThumbnailImage
        /// <summary>
        /// This controller method returns a binary thumbnail image to be viewed client side.
        /// (HTTP GET) 
        /// </summary>
        /// <param name="id">Guid to lookup in the Database.</param>
        /// <param name="tableLink">true = Use the image to locate the image from a seperate linking table id false = Image tables id lookup</param>
        /// <returns>binary thumbnail image</returns>
        [HttpGet]
        public ActionResult GetThumbnailImage(Guid id, bool tableLink)
        {
            ImageModel image = new ImageModel();
            byte[] imageSource = null;
            int count = 0;

            if (tableLink == true)
            {
                //--We either need to lookup the Image in the Database via the Secondary Key.....
                count = db.Images.Count(c => c.TableLink == id);

                //--Found a record.

                //--TODO: ADD THIS AS A CLASS TO Bradaz.OpenSource.MVC
                if (count > 0)
                {
                    image = db.Images.SingleOrDefault(i => i.TableLink == id);

                    //--Convert the Image data into Memory, and return it to the client.
                    imageSource = image.ThumbnailSource;
                    return File(imageSource, image.ThumbnailContentType, image.FileName);

                }

            }
            else
            {
                //--....or we can lookup using the id of the image
                count = db.Images.Count(c => c.UniqueId == id);

                //--Found a record
                if (count > 0)
                {
                    image = db.Images.SingleOrDefault(i => i.UniqueId == id);

                    //--Convert the Image data into Memory, and return it to the client.
                    imageSource = image.ThumbnailSource;
                    return File(imageSource, image.ThumbnailContentType, image.FileName);

                }
            }

            return File(new byte[0], null);
        }
        #endregion


    
        #region Ajax Submit
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AjaxSubmit(int id)
        {

            Session["Image.ContentLength"] = Request.Files[0].ContentLength;
            Session["Image.ContentType"] = Request.Files[0].ContentType;
            byte[] b = new byte[Request.Files[0].ContentLength];
            Request.Files[0].InputStream.Read(b, 0, Request.Files[0].ContentLength);
            Session["Image.ContentStream"] = b;


            if (id > 0)
            {
                byte[] thumbnail = Images.CreateThumbnailToByte(Request.Files[0].InputStream, 100, 100);


                Session["Thumbnail.ContentLength"] = thumbnail.Length;
                Session["Thumbnail.ContentType"] = Request.Files[0].ContentType;

                byte[] c = new byte[thumbnail.Length];
                Request.Files[0].InputStream.Read(c, 0, Request.Files[0].ContentLength);
                Session["Thumbnail.ContentStream"] = thumbnail;

            }



            return Content(Request.Files[0].ContentType + ";" + Request.Files[0].ContentLength);
        }
              
          
        #endregion

        #region ImageLoad
        public ActionResult ImageLoad(int id)
        {
            if(id == 0)
            {
                byte[] b = (byte[])Session["Image.ContentStream"];
                int length = (int)Session["Image.ContentLength"];
                string type = (string)Session["Image.ContentType"];
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = type;
                Response.BinaryWrite(b);
                Response.Flush();
                Response.End();
                Session["Image.ContentLength"] = null;
                Session["Image.ContentType"] = null;
                Session["Image.ContentStream"] = null;
            }

            //--The following is the Thumnbail id.

            if (id == 1)
            {
                byte[] b = (byte[])Session["Thumbnail.ContentStream"];
                int length = (int)Session["Thumbnail.ContentLength"];
                string type = (string)Session["Thumbnail.ContentType"];
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = type;
                Response.BinaryWrite(b);
                Response.Flush();
                Response.End();
                Session["Thumbnail.ContentLength"] = null;
                Session["Thumbnail.ContentType"] = null;
                Session["Thumbnail.ContentStream"] = null;
            }
            return Content("");
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}