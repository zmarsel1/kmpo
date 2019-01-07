using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;

namespace DAL.Abstract
{
    public interface IRepository
    {
        IEnumerable<TagValue> GetTagValue(string tagName, DateTime start, DateTime end);
        IEnumerable<TagValue> GetTagValue(int tagID, DateTime start, DateTime end);
        IEnumerable<Param> GetParams();
        IEnumerable<Group> GetGroups();
        IEnumerable<Comment> GetComments(int tagKey);
        void AddComment(int tagKey, string author, string text);
        void AddTagValue(int tagID, DateTime date, double value);
        void AddTagValue(int tagKey, double value);
    }
}
