using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL.Abstract;
using WebUI.Models;
using DAL.Entities;

namespace WebUI.Controllers
{
    public class PlanController : Controller
    {
        IRepository repository;
        static string NORM = "_NORM";
        static string PLAN = "_PLAN";
        static string FACT = "_FACT";
        static string FORECAST = "_FORECAST";
        static string LIMIT = "_LIMIT";

        public PlanController(IRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index(SearchModel search)
        {
            var groups = repository.GetGroups().Where(g => g.ParentID != null).ToArray();
            ViewBag.Groups = groups;
            if (search.GroupID == 0) search = new SearchModel() { GroupID = groups.First().GroupID, Start = DateTime.Today.AddDays(-3), End = DateTime.Today.AddDays(3) };
            var tag = groups
                .Where(g => g.GroupID == search.GroupID)
                .First()
                .Tags
                .Where(t => t.TagName.Trim().EndsWith(PLAN))
                .FirstOrDefault();
            ViewBag.GroupName = groups.Where(g => g.GroupID == search.GroupID).First().Name; 
            PlanViewModel model = new PlanViewModel() { GroupID = search.GroupID, Start = search.Start, End = search.End };

            if (tag != null)
            {
                model.Documents = repository.GetTagValue(tag.TagID, search.Start, search.End.AddDays(1))
                    .GroupBy(v => v.DateTime.AddHours(-1).ToShortDateString())
                    .Select(g => new DocumentHead { GroupID = search.GroupID, Date = DateTime.Parse(g.Key), Value = g.Sum(e => e.Value) }).ToList();
            }
            else
                model.Documents = new List<DocumentHead>();
            return View(model);
        }

        public ActionResult Forecast(SearchModel search)
        {
            //var groups = repository.GetGroups().Where(g => g.ParentID != null).ToArray();
            //ViewBag.Groups = groups;
            //if (search.GroupID == 0) search = new SearchModel() { GroupID = groups.First().GroupID, Start = DateTime.Today.AddDays(-3), End = DateTime.Today };
            //var tag = groups
            //    .Where(g => g.GroupID == search.GroupID)
            //    .First()
            //    .Tags
            //    .Where(t => t.TagName.Trim().EndsWith(FORECAST))
            //    .FirstOrDefault();
            //ViewBag.GroupName = groups.Where(g => g.GroupID == search.GroupID).First().Name;
            //PlanViewModel model = new PlanViewModel() { GroupID = search.GroupID, Start = search.Start, End = search.End };

            //if (tag != null)
            //{
            //    model.Documents = repository.GetTagValue(tag.TagID, search.Start, search.End)
            //        .GroupBy(v => v.DateTime.AddHours(-1).ToShortDateString())
            //        .Select(g => new DocumentHead { GroupID = search.GroupID, Date = DateTime.Parse(g.Key), Value = g.Sum(e => e.Value) }).ToList();
            //}
            //else
            //    model.Documents = new List<DocumentHead>();
            //return View(model);
            ViewBag.Report = "PrintForecast";
            return View("Print", new DocumentHead() { Date = DateTime.Today });
        }

        public ActionResult Print(int id, DateTime date, string report = "PrintPlan")
        {
            var model = new DocumentHead() { Date = date, GroupID = id };
            ViewBag.Report = report;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var groups = repository.GetGroups().Where(g => g.ParentID != null).ToArray();
            ViewBag.Groups = groups;
            var model = new CreateDocumentModel() {
                GroupID = groups.First().GroupID,
                Date = DateTime.Today.AddDays(1),
                ParentDocumentDate = DateTime.Today
            }; 
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(CreateDocumentModel model)
        { 
            var tag = repository.GetGroups()
                .Where(g => g.GroupID == model.GroupID)
                .First()
                .Tags
                .Where(t => t.TagName.Trim().EndsWith(PLAN))
                .First();
            if (tag == null)
            {
                var errorinfo = new HandleErrorInfo(new Exception(string.Format("Тэга по планированию в группе id = {0} не существует", model.GroupID)), "Plan", "Create");
                return View("Error", errorinfo);
            }

            if (model.HasParent)
            {
                if (model.Source == CreateDocumentModel.DocSource.Plan) // основан на заявке
                {
                    var plan = repository.GetTagValue(tag.TagID, model.ParentDocumentDate, model.ParentDocumentDate.AddDays(1)).ToArray();
                    DateTime date = model.Date.AddHours(1);
                    while (date <= model.Date.AddDays(1))
                    {
                        double value = plan.Where(tv => tv.DateTime.Hour == date.Hour).First().Value;
                        repository.AddTagValue(tag.TagID, date, Math.Round(value,2));
                        date = date.AddHours(1);
                    }
                }
                else if (model.Source == CreateDocumentModel.DocSource.Forecast) // основан на прогнозе
                {
                    var forecasttag = repository.GetGroups()
                        .Where(g => g.GroupID == model.GroupID)
                        .First()
                        .Tags
                        .Where(t => t.TagName.Trim().EndsWith(FORECAST))
                        .First();
                    var forecast = repository.GetTagValue(forecasttag.TagID, model.ParentDocumentDate, model.ParentDocumentDate.AddDays(1)).ToArray();
                    DateTime date = model.Date.AddHours(1);
                    while (date <= model.Date.AddDays(1))
                    {
                        double value = forecast.Where(tv => tv.DateTime.Hour == date.Hour).First().Value;
                        repository.AddTagValue(tag.TagID, date, Math.Round(value, 2));
                        date = date.AddHours(1);
                    }
                }
                else // основан на факте
                {
                    var facttag = repository.GetGroups()
                        .Where(g => g.GroupID == model.GroupID)
                        .First()
                        .Tags
                        .Where(t => t.TagName.Trim().EndsWith(FACT))
                        .First();
                    var forecast = repository.GetTagValue(facttag.TagID, model.ParentDocumentDate, model.ParentDocumentDate.AddDays(1)).ToArray();
                    DateTime date = model.Date.AddHours(1);
                    while (date <= model.Date.AddDays(1))
                    {
                        double value = forecast.Where(tv => tv.DateTime.Hour == date.Hour).First().Value;
                        repository.AddTagValue(tag.TagID, date, Math.Round(value, 2));
                        date = date.AddHours(1);
                    }
                }
            }
            else
            {
                DateTime date = model.Date.AddHours(1);
                while (date <= model.Date.AddDays(1))
                {
                    repository.AddTagValue(tag.TagID, date, 0.0f);
                    date = date.AddHours(1);
                }
            }

            TempData["Message"] = "Документ успешно создан.";
            return RedirectToAction("Edit", new { id = model.GroupID, date = model.Date.ToString("yyyy-MM-dd") });
        }
        public ActionResult Edit(int id, DateTime date)
        {
            var model = new Document() { GroupID = id, Date = date };
            var tag = repository.GetGroups()
                .Where(g => g.GroupID == id)
                .First()
                .Tags
                .Where(t => t.TagName.Trim().EndsWith(PLAN))
                .First();

            model.Entities = repository.GetTagValue(tag.TagID, date, date.AddDays(1)).ToList();
            if (tag == null)
            {
                var errorinfo = new HandleErrorInfo(new Exception(string.Format("Тэга по планированию в группе id = {0} не существует", id)), "Plan", "Edit");
                return View("Error", errorinfo);
            }
            ViewBag.DateTime = date;
            ViewBag.GroupName = repository.GetGroups().Where(g => g.GroupID == id).First().Name;
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Document document)
        {
            try
            {
                foreach (var entity in document.Entities)
                {
                    repository.AddTagValue(entity.TagID, entity.DateTime, entity.Value);
                }
            }
            catch
            {
                
            }
            return RedirectToAction("Index");
        }
    }
}
