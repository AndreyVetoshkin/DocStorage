using DBModel.Models;
using Services.Entities;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocStorage.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUserManager userManager { get; set; }
        private IDocManager docManager { get; set; }

        public HomeController(IUserManager userManager, IDocManager docManager)
        {
            this.userManager = userManager;
            this.docManager = docManager;
        }

        public ActionResult Index()
        {
            var docs = docManager.GetAll();
            ViewBag.Docs = docs;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult AddFile()
        {
            var user = userManager.Get(HttpContext.User.Identity.Name);
            if (user != null)
            {
                var docs = docManager.GetAll().Where(d => d.Author == user.Login);
                ViewBag.Docs = docs;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddFile(Doc doc, HttpPostedFileBase file)
        {
            var user = userManager.Get(HttpContext.User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            if (!ModelState.IsValid)
                return View(doc);
            DateTime current = DateTime.Now;
            if (file != null)
            {
                doc.Name = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));
                // Получаем расширение
                string ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                // сохраняем файл по определенному пути на сервере
                string path = current.ToString("dd/MM/yyyy H:mm:ss").Replace(":", "_").Replace("/", ".") + ext;
                file.SaveAs(Server.MapPath("~/Files/" + path));
                doc.FileName = path;
                doc.Date = current;
                doc.Author = user.Login;
                doc.User = user as User;
                docManager.Save(doc);
            }

            var docs = docManager.GetAll().Where(d => d.Author == user.Login);
            ViewBag.Docs = docs;
            return View(doc);
        }

        public ActionResult Delete(long id)
        {
            var doc = docManager.Get(id) as Doc;
            // получаем текущего пользователя
            var user = userManager.Get(HttpContext.User.Identity.Name) as User;
            if (doc != null && doc.User.Id == user.Id)
            {
                //Удалить сам файл с сервера
                string filename = Server.MapPath("~/Files/" + doc.FileName);
                System.IO.File.Delete(filename);
                docManager.Delete(doc);
            }
            return RedirectToAction("AddFile");
        }

        public ActionResult Download(int id)
        {
            var doc = docManager.Get(id) as Doc;
            if (doc != null)
            {
                string filename = Server.MapPath("~/Files/" + doc.FileName);
                string contentType = "application/msword";

                string ext = filename.Substring(filename.LastIndexOf('.'));
                switch (ext)
                {
                    case ".txt":
                        contentType = "text/plain";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".tiff":
                        contentType = "image/tiff";
                        break;
                    case ".docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                }
                return File(filename, contentType, doc.Name);
            }

            return Content("Файл не найден");
        }

        [HttpPost]
        public ActionResult Search(string name)
        {
            IList<IDoc> docs;
            docs = docManager.GetAll().Where(d=>d.Name.Contains(name)).ToList();
            if (docs.Count <= 0)
            {
                docs = docManager.GetAll().ToList();
                return PartialView(docs);
            }
            return PartialView(docs);
        }
    }
}