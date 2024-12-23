using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryStock queryStock)
        {
            var stocks = await _stockRepo.GetAllAsync(queryStock);
            var stocksDto = stocks.Select(x => x.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);

            return stock == null ? NotFound() : Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockData)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stock = stockData.ToStockFromCreateRequestDto();
            await _stockRepo.CreateAsync(stock);

            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockData)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stock = await _stockRepo.UpdateAsync(id, stockData);

            return stock == null ? NotFound() : Ok(stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepo.DeleteAsync(id);

            return stock == null ? NotFound() : NoContent();
        }
    }
}