using TestElma.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using System.Reflection;

namespace TestElma.Controllers
{
    [Authorize]
    public class DocController : Controller
    {
        private IDocumentManager Manager { get; set; }

        private List<string> Fields { get; set; }

        public DocController()
        {
           Manager = new DocumentManager();

            Fields = new List<string>();
            foreach (var p in typeof(Document).GetProperties())
            {
                if (p.Name == "Name" || p.Name == "Author")
                {
                    Fields.Add(p.Name);
                }
            }
        }

        // GET: Doc
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Documents(string field, string currentFilter, string sortOrder = "", string search = "")
        {
            ViewBag.field = new SelectList(Fields);

            if (string.IsNullOrWhiteSpace(search))
            {
                search = currentFilter;
                ViewBag.Searching = "y";
            }

            ViewBag.CurrentFilter = search;
            currentFilter = search;
            return View(SortDocuments(Manager.List(search, field), sortOrder, search));

        }

        private IEnumerable<Document> SortDocuments(IEnumerable<Document> documents, string sortOrder, string search)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "Name";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.AuthorSortParam = sortOrder == "Author" ? "author_desc" : "Author";
            var docs = from d in documents
                       select d;
            if (!String.IsNullOrEmpty(search))
            {
                docs = docs.Where(d => d.Name.Contains(search)
                                       || d.Author.Contains(search));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    docs = docs.OrderByDescending(d => d.Name);
                    break;
                case "Date":
                    docs = docs.OrderBy(d => d.Date);
                    break;
                case "date_desc":
                    docs = docs.OrderByDescending(d => d.Date);
                    break;
                case "Author":
                    docs = docs.OrderBy(d => d.Author);
                    break;
                case "author_desc":
                    docs = docs.OrderByDescending(d => d.Author);
                    break;
                default:
                    docs = docs.OrderBy(d => d.Name);
                    break;
            }
            return docs.ToList();
        }

        #region РАБОТА С ФАЙЛАМИ

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, Document mod)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                        var fileExt = file.ContentType.ToString();

                        //Проверяем, существует ли в директории такой файл
                        int count = 1;
                        while (System.IO.File.Exists(path))
                        {
                            fileName = string.Format("{0}({1})", Path.GetFileNameWithoutExtension(path), count++);
                            path = Path.Combine(Path.GetDirectoryName(path), fileName + Path.GetExtension(path));
                        }

                        file.SaveAs(path);
                        AddDocument(Path.GetFileName(path), fileExt);
                    }
                    ViewBag.Message = "Success";
                    return View();
                }
                catch
                {
                    ViewBag.Message = "Fail";
                    return RedirectToAction("UploadFile");
                }
            }
            return View(mod);
        }


        public ActionResult Download(string name, string author)
        {
            string path = "~/Upload/";
            var document = Server.MapPath(path);
            var file = File(document + name, System.Net.Mime.MediaTypeNames.Application.Octet, name);
            if (User.Identity.Name == author)
            {
                return file;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
        }

        #endregion

        #region РАБОТА С БД

        private void AddDocument(string name, string extension)
        {
            var document = new Document();

            document.Name = name;
            document.Date = DateTime.Now;
            document.Author = User.Identity.Name;
            document.FileExt = extension;

            Manager.Add(document);
        }
        #endregion
    }
}