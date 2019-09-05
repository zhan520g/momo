using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace momo.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "Permission")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [EnableCors("any")]
    public class BaseAPIController : Controller
    {
    }
}
