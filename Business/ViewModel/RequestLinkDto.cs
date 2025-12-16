using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class RequestLinkDto
    {
        public int TransactionTypeId { get; set; }
        public long RequestId { get; set; }
    }

    public class FinalizeAttachmentsChunkVM
    {
        public string TempKey { get; set; } = "";
        public string? Reqno { get; set; } // ممكن يكون null وقت الإنشاء الأول
        public int TransactionTypeId { get; set; }
        public long RequestId { get; set; }
        public List<string> AttachInfos { get; set; } = new(); // كل عنصر: attachId|originalFileName|label|transactionTypeId
    }

}
