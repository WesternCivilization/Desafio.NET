using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.NET.Domain.Entities
{
    public class UserPhone : BaseEntity
    {
        public long UserId { get; set; }

        public string AreaCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}