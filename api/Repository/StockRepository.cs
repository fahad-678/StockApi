using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null) return null;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockData)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if(stock == null) return null;

            stock.Symbol = stockData.Symbol;
            stock.CompanyName = stockData.CompanyName;
            stock.Purchase = stockData.Purchase;
            stock.LastDiv = stockData.LastDiv;
            stock.IndustryCode = stockData.IndustryCode;
            stock.MarketCap = stockData.MarketCap;

            await _context.SaveChangesAsync();

            return stock;
        }
    }
}