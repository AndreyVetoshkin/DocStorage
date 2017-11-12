using DBModel.Models;
using NHibernate;
using NHibernate.Criterion;
using Services.Entities;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.NHibernate
{
    public class UserManager : EntityManager<IUser>, IUserManager
    {
        public IUser Get(string login)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                var user = session.QueryOver<User>()
                    .And(u => u.Login == login)
                    .SingleOrDefault();

                return user;
            }
        }

        public bool Check(string login, string password)
        {
            using (ISession session = NHHelper.OpenSession())
            {
                //var criteria = session.CreateCriteria<User>();
                //criteria.Add(Restrictions.Eq("Login", login));
                //criteria.Add(Restrictions.Eq("Password", password));

                //var user = criteria.UniqueResult<User>();
                var user = session.QueryOver<User>()
                    .And(u => u.Login == login)
                    .And(u => u.Password == password)
                    .SingleOrDefault();
                return user != null;
            }
        }

    }
}
