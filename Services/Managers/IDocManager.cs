using Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public interface IDocManager : IEntityManager<IDoc>
    {
        IDoc Get(string name);
        IList<IDoc> GetAll();
        IList<IDoc> GetList(string name);
        IList<IDoc> GetList(DateTime name);
    }
}
