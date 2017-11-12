using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public interface IEntityManager<T> where T : IEntity
    {
        void Save(T entity);
        void Delete(T entity);
        IEntity Get(long id);
    }
}
