using FluentNHibernate.Mapping;
using Services.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel.Models
{
    public class Doc:IDoc
    {
        public virtual long Id { get; set; }
        [Display(Name = "Имя файла")]
        [StringLength(50,ErrorMessage ="Имя файла слишком длинное")]
        public virtual string Name { get; set; }
        [Display(Name = "Дата создания")]
        public virtual DateTime Date { get; set; }
        [Display(Name = "Хозяин")]
        public virtual string Author { get; set; }
        [Display(Name = "Имя на сервере")]
        public virtual string FileName { get; set; }

        public virtual User User { get; set; }
    }

    public class DocMap : ClassMap<Doc>
    {
        public DocMap()
        {
            Table("Docs");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Date);
            Map(x => x.Author);
            Map(x => x.FileName);
            //Отношение многие-к-одному
            References(x => x.User).Cascade.SaveUpdate();
        }
    }
}
