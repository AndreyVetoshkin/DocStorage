using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Entities
{
    public interface IUser : IEntity
    {
        string Login { get; set; }
        string Password { get; set; }
    }
}
