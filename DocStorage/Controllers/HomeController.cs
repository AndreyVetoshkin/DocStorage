using DBModel.Models;
using Services.Entities;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.OwnerSortParm = sortOrder == "Owner" ? "Owner desc" : "Owner";

            var docs = docManager.GetAll();
            IOrderedEnumerable<IDoc> sortedDocs;

            switch (sortOrder)
            {
                case "Name desc":
                    sortedDocs = docs.OrderByDescending(d => d.Name);
                    break;
                case "Date":
                    sortedDocs = docs.OrderBy(d => d.Date);
                    break;
                case "Date desc":
                    sortedDocs = docs.OrderByDescending(d => d.Date);
                    break;
                case "Owner":
                    sortedDocs = docs.OrderBy(d => d.Author);
                    break;
                case "Owner desc":
                    sortedDocs = docs.OrderByDescending(d => d.Author);
                    break;
                default:
                    sortedDocs = docs.OrderBy(d => d.Name);
                    break;
            }

            ViewBag.SortedDocs = sortedDocs;
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
            if (ModelState.IsValid)
            {
                DateTime current = DateTime.Now;
                if (file != null)
                {
                    if (file.FileName.Substring(0, file.FileName.LastIndexOf('.')).Length <= 50)
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
                    else
                    { ModelState.AddModelError("", "Слишком длинное имя файла"); }
                }
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
        public ActionResult Search(string searchParam, string url, string searchOptions)
        {
            IList<IDoc> docs = null;
            string _Action = url.Substring(url.LastIndexOf("/"));
            string user = HttpContext.User.Identity.Name;
            switch (searchOptions)
            {
                case "По имени":


                    switch (_Action)
                    {
                        default:
                            docs = docManager.GetList(searchParam);
                            break;
                        case "/AddFile":                           
                            docs = docManager.GetList(searchParam).Where(d => d.Author == user).ToList();
                            break;
                    }
                    break;
                case "По дате":
                    DateTime searchDate;
                    CultureInfo culture = CultureInfo.CurrentCulture;
                    DateTimeStyles styles = DateTimeStyles.None;
                    switch (_Action)
                    {
                        default:
                            if (DateTime.TryParse(searchParam, culture, styles, out searchDate))
                            {
                                docs = docManager.GetList(searchDate);
                            }
                            if (docs == null)
                            {
                                docs = docManager.GetAll().ToList();
                            }
                            break;
                        case "/AddFile":
                            if (DateTime.TryParse(searchParam, culture, styles, out searchDate))
                            {
                                docs = docManager.GetList(searchDate).Where(d => d.Author == user).ToList();
                            }
                            if (docs == null)
                            {
                                docs = docManager.GetAll().Where(d => d.Author == user).ToList();
                            }
                            break;
                    }
                    break;
            }
            return PartialView(docs);
        }
    }
}