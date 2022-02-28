using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using YazilimNedirBSCToken.Models;
using YazilimNedirBSCToken.TokenService;
using YazilimNedirBSCToken.Utils;

namespace YazilimNedirBSCToken.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        private readonly BSCTokenService tokenService;

        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration,
            BSCTokenService tokenService)
        {
            _logger = logger;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }

        public IActionResult Index()
        {
            var resultModel = new DashboardViewModel();

            var balance = tokenService.GetBalanceOf(this.configuration.GetValue<string>("CommissionWalletAddress")).Result;
            string symbol = tokenService.GetSymbol().Result;
            var decimals = tokenService.GetDecimal().Result;

            resultModel.CommissionWalletData = new CommissionWalletModel { Symbol = symbol, FormattedBalance = balance.ToFormatted(decimals) };
            return View(resultModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
