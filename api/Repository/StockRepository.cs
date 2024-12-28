using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
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

        public async Task<List<Stock>> GetAllAsync(QueryStock queryStock)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryStock.Symbol))
            {
                stocks = stocks.Where(x => x.Symbol.ToLower().Contains(queryStock.Symbol.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(queryStock.CompanyName))
            {
                stocks = stocks.Where(x => x.CompanyName.ToLower().Contains(queryStock.CompanyName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(queryStock.IndustryCode))
            {
                stocks = stocks.Where(x => x.IndustryCode.ToLower().Contains(queryStock.IndustryCode.ToLower()));
            }
            if (queryStock.MarketCap > 0)
            {
                stocks = stocks.Where(x => x.MarketCap == queryStock.MarketCap);
            }
            if (queryStock.LastDiv > 0)
            {
                stocks = stocks.Where(x => x.LastDiv == queryStock.LastDiv);
            }
            if (queryStock.Purchase > 0)
            {
                stocks = stocks.Where(x => x.Purchase == queryStock.Purchase);
            }

            if (!string.IsNullOrWhiteSpace(queryStock.SortBy))
            {
                bool isDescending = queryStock.SortBy.StartsWith('-');
                string propertyName = isDescending ? queryStock.SortBy.Substring(1) : queryStock.SortBy;

                var property = typeof(Stock).GetProperty(propertyName);

                if (property != null)
                {
                    stocks = isDescending ? stocks.OrderByDescending(x => EF.Property<object>(x, propertyName)) : stocks.OrderBy(x => EF.Property<object>(x, propertyName));
                }
            }

            int skip = (queryStock.PageNumber - 1) * queryStock.PageSize;
            stocks = stocks.Skip(skip).Take(queryStock.PageSize);

            return await stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockData)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null) return null;

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