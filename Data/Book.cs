using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookitties.Data
{
    public sealed record Book(string Title, string Author, DateTime Published)
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = Title;
        public string Author { get; set; } = Author;
        public DateTime Published { get; set; } = Published;
    }
}
