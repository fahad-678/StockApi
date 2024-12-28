using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = User.GetUsername();
            
            var user = await _userManager.FindByNameAsync(userName);
            
            var portfolio = await _portfolioRepo.GetUserPortfolio(user);

            return Ok(portfolio);
        }

        [HttpPost("{stockId}")]
        [Authorize]
        public async Task<IActionResult> AddPortfolio([FromRoute] int stockId)
        {
            var userName = User.GetUsername();
            var user = await _userManager.FindByNameAsync(userName);

            var stock = await _stockRepo.GetByIdAsync(stockId);
            if (stock == null) return BadRequest("Stock does not exist");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);
            if(userPortfolio.Any(x => x.Id == stockId)) return BadRequest("Stock already exists in portfolio");

            var portfolio = new Portfolio { AppUserId = user.Id, StockId = stockId };
            await _portfolioRepo.CreateAsync(portfolio);

            return Created();
        }

        [HttpDelete("{stockId}")]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio([FromRoute] int stockId)
        {
            var userName = User.GetUsername();
            var user = await _userManager.FindByNameAsync(userName);

            var stock = await _stockRepo.GetByIdAsync(stockId);
            if (stock == null) return BadRequest("Stock does not exist");

            var portfolio = await _portfolioRepo.DeleteAsync(user, stock);
            if (portfolio == null) return BadRequest("Stock does not exist in portfolio");

            return NoContent();
        }
    }
}