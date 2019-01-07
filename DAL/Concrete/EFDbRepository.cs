﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Abstract;
using DAL.Entities;

namespace DAL.Concrete
{
    public class EFDbRepository : IRepository
    {
        TagDbContext context = new TagDbContext();

        public IEnumerable<TagValue> GetTagValue(string tagName, DateTime start, DateTime end)
        {
            var tag = context.Tags.FirstOrDefault(t => t.TagName == tagName);
            if (tag != null) return GetTagValue(tag.TagID, start, end);
            return new List<Entities.TagValue>() { };
        }

        public IEnumerable<Entities.TagValue> GetTagValue(int tagID, DateTime start, DateTime end)
        {
            var tag = context.Tags.FirstOrDefault(t => t.TagID == tagID);
            if (tag.TagName.Trim().EndsWith("_LIMIT") || tag.TagName.Trim().EndsWith("_NORM"))
            {
                List<Entities.TagValue> result = new List<Entities.TagValue>();
                var value = context.TagValues.Where(v => v.TagID == tagID).LastOrDefault();
                if (value == null) return result;
                TimeSpan inc = new TimeSpan(1, 0, 0);
                while (start <= end)
                {
                    result.Add(new Entities.TagValue() { DateTime = start, TagID = tagID, Tag = value.Tag, Value = value.Value });
                    start = start.Add(inc);
                }
                return result;
            }
            return context.TagValues.Where(v => v.TagID == tagID && v.DateTime > start && v.DateTime <= end).AsEnumerable();
        }

        public IEnumerable<Entities.Param> GetParams()
        {
            return context.Params.AsEnumerable();
        }

        public IEnumerable<Entities.Group> GetGroups()
        {
            return context.Groups.Include("Tags.Param").AsEnumerable();
        }

        public IEnumerable<Entities.Comment> GetComments(int tagKey)
        {
            return context.Comments.Where(c => c.TagKey == tagKey).AsEnumerable();
        }

        public void AddComment(int tagKey, string author, string text)
        {
            Comment comment = new Comment() { TagKey = tagKey, Author = author, Text = text, Posted = DateTime.Now };
            context.Comments.Add(comment);
            context.SaveChanges();
        }


        public void AddTagValue(int tagKey, double value)
        {
            var entity = context.TagValues.First(v => v.TagKey == tagKey);
            entity.Value = value;
            context.SaveChanges();
        }


        public void AddTagValue(int tagID, DateTime date, double value)
        {
            var entity = context.TagValues.FirstOrDefault(v => v.TagID == tagID && v.DateTime == date);
            if (entity == null)
            {
                context.TagValues.Add(new TagValue() { TagID = tagID, DateTime = date, Value = value });
            }
            else
            {
                entity.Value = value;
            }
            context.SaveChanges();
        }
    }
}
