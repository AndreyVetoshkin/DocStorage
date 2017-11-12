using DBModel.Models;
using NHibernate;
using NHibernate.Linq;
using Services.Entities;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.NHibernate
{
    public class DocManager : EntityManager<IDoc>, IDocManager
    {
        public IDoc Get(string name)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                var doc = session.QueryOver<Doc>()
                    .And(d => d.Name == name)
                    .SingleOrDefault();

                return doc;
            }
        }

        public IList<IDoc> GetAll()
        {
            using (ISession session = NHHelper.OpenSession())
            {
                var docs = session.Query<IDoc>().ToList();
                    
                return docs;
            }
        }
    }
}
