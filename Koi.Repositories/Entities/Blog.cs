using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Repositories.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public bool IsNews { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; } // Foreign key to User

        // public DateTime CreatedDate { get; set; } already have CreatedAT
        public string? Tags { get; set; }

        public bool IsPublished { get; set; }

        // Navigation property
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}