using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Domain.Entities
{
    public class Token : BaseEntity
    {
        public long UserId { get; set; }

        public string Code { get; set; }

        public DateTime IssuedOn { get; set; }

        public DateTime ExpireOn { get; set; }
    }
}