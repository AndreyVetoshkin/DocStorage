using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entities
{
    public interface IDoc : IEntity
    {
        string Name { get; set; }
        DateTime Date { get; set; }
        string Author { get; set; }
        string FileName { get; set; }
    }
}
