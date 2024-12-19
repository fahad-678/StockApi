using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                IndustryCode = stockModel.IndustryCode,
                MarketCap = stockModel.MarketCap
            };
        }

        public static Stock ToStockFromCreateRequestDto(this CreateStockRequestDto stockModel)
        {
            return new Stock
            {
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                IndustryCode = stockModel.IndustryCode,
                MarketCap = stockModel.MarketCap
            };
        }
    }
}