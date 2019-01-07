using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Concrete;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TagDbContext context = new TagDbContext();
            context.Database.CreateIfNotExists();
        }
    }
}
