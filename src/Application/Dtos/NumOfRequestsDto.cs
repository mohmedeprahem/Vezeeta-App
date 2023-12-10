using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class NumOfRequestsDto
    {
        public int ConfirmedRequests { get; set; }
        public int PendingRequests { get; set; }
        public int CancelledRequests5 { get; set; }
    }
}
