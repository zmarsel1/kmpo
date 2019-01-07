using System;
using System.Collections.Generic;
using System.Linq;
using TagDBContext.Models;

namespace TagDBContext
{
    /// <summary>
    /// Нет доступной документации по метаданным.
    /// </summary>
    partial class TagDBEntities
    {
        public List<Tag> GetForecastTags()
        {
            return this.Tags.Where(t => t.TagName.Trim().EndsWith("_FORECAST")).ToList();
        }
        public IList<PlanFactRecord> GetPlanFact(int groupid, DateTime date)
        {
            DateTime end = date.AddDays(1);
            //var _group = this.Groups.First(g => g.GroupID == groupid);
            var _planTagID = this.Tags.Where(t => t.Groups.FirstOrDefault(g => g.GroupID == groupid) != null && t.TagName.Trim().EndsWith("_PLAN")).First().TagID;
            var _factTagID = this.Tags.Where(t => t.Groups.FirstOrDefault(g => g.GroupID == groupid) != null && t.TagName.Trim().EndsWith("_FACT")).First().TagID;

            var _planData = this.TagValues.Where(d => d.Tag.TagID == _planTagID && d.DateTime > date && d.DateTime <= end);
            var _factData = this.TagValues.Where(d => d.Tag.TagID == _factTagID && d.DateTime > date && d.DateTime <= end);

            var result = from f in _factData
                         join p in _planData on f.DateTime equals p.DateTime
                         select new PlanFactRecord() { DateTime = p.DateTime,
                             Plan = p.Value,
                             Fact = f.Value,
                             AbsDev = p.Value - f.Value,
                             RelDev = 100 * (p.Value - f.Value) / (p.Value == 0 ? 1 : p.Value),
                             Hour = p.DateTime.Hour == 0 ? 24 : p.DateTime.Hour };
            return result.ToList();
        }
        public IList<DocumentBodyEntity> GetPlanFactData(int groupid, DateTime date)
        {
            var tags = this.Groups
                .Where(g => g.GroupID == groupid)
                .First()
                .Tags
                .Where(t => t.TagName.Trim().EndsWith("_PLAN") || t.TagName.Trim().EndsWith("_FACT"))
                .Select(t => t.TagID)
                .ToArray();
            DateTime end = date.AddDays(1);
            var data = this.TagValues
                .Where(t => tags.Contains(t.TagID) && t.DateTime > date && t.DateTime <= end)
                .OrderBy(t => t.DateTime)
                .ToList();
            return data
                .Select(t => new DocumentBodyEntity()
                {
                    ParamName = t.Tag.Description.Trim(),
                    Time = string.Format("{0} - {1}", t.DateTime.AddHours(-1).ToString("HH:mm"), t.DateTime.ToString("HH:mm")),
                    Value = t.Value
                })
                .ToList(); ;
        }
        public List<DocumentBodyEntity> GetPrintDocumentData(int tagid, DateTime date)
        {
            DateTime end = date.AddDays(1);
            var data = this.TagValues
                .Where(t => t.TagID == tagid && t.DateTime > date && t.DateTime <= end)
                .OrderBy(t => t.DateTime)
                .ToList();
            return data
                .Select(t => new DocumentBodyEntity()
                {
                    ParamName = t.Tag.Description.Trim(),
                    Time = string.Format("{0} - {1}", t.DateTime.AddHours(-1).ToString("HH:mm"), t.DateTime.ToString("HH:mm")),
                    Value = t.Value
                })
                .ToList();
        }
        public IList<DocumentBodyEntity> GetPrintDocumentData(int groupid, DateTime date, string doctype)
        {
            Tag tag = this.Groups
                .Where(g => g.GroupID == groupid)
                .First()
                .Tags
                .Where(t => t.TagName.Trim().EndsWith(doctype))
                .FirstOrDefault();
            if (tag == null) return new List<DocumentBodyEntity>();
            DateTime end = date.AddDays(1);
            var data = this.TagValues
                .Where(t => t.TagID == tag.TagID && t.DateTime > date && t.DateTime <= end)
                .OrderBy(t => t.DateTime)
                .ToList();
            return data
                .Select(t => new DocumentBodyEntity()
                {
                    ParamName = t.Tag.Description.Trim(),
                    Time = string.Format("{0} - {1}", t.DateTime.AddHours(-1).ToString("HH:mm"), t.DateTime.ToString("HH:mm")),
                    Value = t.Value
                })
                .ToList();
            ;
        }
    }
}