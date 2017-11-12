using NHibernate;
using Services.Entities;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.NHibernate
{
    public class EntityManager<T> : IEntityManager<T> where T : IEntity
    {
        public void Delete(T entity)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                using (ITransaction tr = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete(entity);
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        return;
                    }
                    tr.Commit();
                }
            }
        }

        public IEntity Get(long id)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public void Save(T entity)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                using (ITransaction tr = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(entity);
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        return;
                    }
                    tr.Commit();
                }
            }
        }
    }
}
