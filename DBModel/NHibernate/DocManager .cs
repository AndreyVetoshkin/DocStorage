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

        public IList<IDoc> GetList(string name)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                var docs = session.QueryOver<IDoc>()
                    .WhereRestrictionOn(d => d.Name).IsLike($"%{name}%")
                    .List<IDoc>();


                return docs;
            }
        }

        public IList<IDoc> GetList(DateTime date)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                //DateTime searchDate;
                //CultureInfo culture = CultureInfo.CurrentCulture;

                //DateTimeStyles styles = DateTimeStyles.None;
                IList<IDoc> docs = null;
                //if (DateTime.TryParse(date, culture, styles, out searchDate))
                {
                    docs = session.QueryOver<IDoc>()
                    .Where(d => d.Date.Date == date)
                    .List<IDoc>();
                }

                return docs;
            }
        }
    }
}
