using FluentNHibernate.Mapping;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.Models
{
    public class User : IUser
    {
        public virtual long Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual IList<Doc> Docs { get; set; }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(x => x.Id);
            Map(x => x.Login);
            Map(x => x.Password);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            //Отношение один-ко-многим
           // HasMany(x => x.Docs).Cascade.All().Inverse();
        }
    }
}
