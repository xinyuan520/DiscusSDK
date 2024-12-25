using Discus.SDK.Tools.PageResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.Application.Contracts.QueryParam
{
    public class UserPageParam : PageParam
    {
        public string? UserName { get; set; }
    }
}
