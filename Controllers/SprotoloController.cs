using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sprotolo13a.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprotoloController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13a";

    }
}
