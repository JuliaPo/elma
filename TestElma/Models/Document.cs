using System;
using System.ComponentModel.DataAnnotations;

namespace TestElma.Models
{
    public class Document
    {
        public virtual Guid Id { get; set; }

        [Display(Name = "Название")]
        public virtual string Name { get; set; }

        [Display(Name = "Дата создания")]
        public virtual DateTime Date { get; set; }

        [Display(Name = "Автор")]
        public virtual string Author { get; set; }

        [Display(Name = "Файл")]
        public virtual string FileExt { get; set; }

    }
}
