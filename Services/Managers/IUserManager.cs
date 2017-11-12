using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public interface IUserManager : IEntityManager<IUser>
    {
        IUser Get(string login);
        bool Check(string login, string password);
    }
}
