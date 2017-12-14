using Microsoft.AspNetCore.Mvc;

namespace FluentCodeAPI.AspNetCore.Blockchains.Host.Controllers
{
    public class ChainController : Controller
    {
        private readonly Blockchain _blockchain;

        public ChainController(Blockchain blockchain)
        {
            _blockchain = blockchain;
        }

        // Get /chain
        [HttpGet]
        public JsonResult Index() => Json(_blockchain.Chain);
    }
}