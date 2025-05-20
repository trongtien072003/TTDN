using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Cart.Dto
{
    public class RemoveFromCartInput : FullAuditedEntity<long>
    {
        public long CartItemId { get; set; }
    }
}
