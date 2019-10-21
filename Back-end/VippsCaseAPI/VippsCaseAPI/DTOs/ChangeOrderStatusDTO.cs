using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VippsCaseAPI.DTOs
{
    public class ChangeOrderStatusDTO
    {
        public int OrderId { get; set; }
        public int Status { get; set; }
    }
}
