using DBModel.Models;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.NHibernate
{
    public class NHHelper
    {
        public static ISession OpenSession()
        {
            ISessionFactory sessionFactory = Fluently.Configure()
     //Настройки БД. Строка подключения к БД MS Sql Server 2008
     .Database(MsSqlConfiguration.MsSql2008.ConnectionString(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\work\DocStorage\DBModel\AppData\DBDocs.mdf; Integrated Security = True; ")
            .ShowSql()
            )
            //Маппинг. Используя AddFromAssemblyOf NHibernate будет пытаться маппить КАЖДЫЙ класс в этой сборке (assembly). Можно выбрать любой класс. 
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>())
            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Doc>())
            //SchemeUpdate позволяет создавать/обновлять в БД таблицы и поля (2 поле ==true) 
            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
            .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
