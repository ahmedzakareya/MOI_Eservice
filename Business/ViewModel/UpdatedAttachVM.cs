using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class UpdatedAttachVM
    {
        public FileSaveResponseVM FileSaveResponseVM { get; set; }
        public long RequestId { get; set; }
        public int AttachId { get; set; }

    }
}
