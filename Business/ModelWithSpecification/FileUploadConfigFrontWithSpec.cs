using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class FileUploadConfigFrontWithSpec : Specification<FileUploadConfigurationsFront>
    {
        public FileUploadConfigFrontWithSpec(int id) : base(c => c.Id==id)
        {
            Includes.Add(f => f.RequestStatusLookup);
        }
        public FileUploadConfigFrontWithSpec() : base()
        {
            Includes.Add(f => f.RequestStatusLookup);
        }
    }
}
