using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.CommonService.Authority.Application.Contracts.Dtos
{
    public class LoginPhoneRequestDto
    {
        public string Phone { get; set; }

        public string Code { get; set; }
    }
}
