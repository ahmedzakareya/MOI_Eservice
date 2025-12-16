using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class NewsVM
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? SmallDescription { get; set; }

        public IFormFile? Image { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }

        public string? Description { get; set; }

        public bool? Status { get; set; }

        public DateTime? CreatedDate { get; set; }
        public List<NewsItem>? NewsList { get; set; }
    }

    public class NewsItem
    {

        public int? Id { get; set; }
        public string? Title { get; set; }

        public string? SmallDescription { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
