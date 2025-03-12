using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Enums;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public StockRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _dbContext
                .Stocks.Include(comment => comment.Comments)
                .ThenInclude(stock => stock.AppUser)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(stock => stock.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(stock => stock.Symbol.Contains(query.Symbol));
            }

            if (!query.SortBy.Equals(null))
            {
                if (query.SortBy == SortByStock.Purchase)
                {
                    stocks = query.IsDecsending
                        ? stocks.OrderByDescending(stock => stock.Symbol)
                        : stocks.OrderBy(stock => stock.Symbol);
                }

                if (query.SortBy == SortByStock.MarketCap)
                {
                    stocks = query.IsDecsending
                        ? stocks.OrderByDescending(stock => stock.MarketCap)
                        : stocks.OrderBy(stock => stock.MarketCap);
                }
            }

            return await stocks.ToListAsync();
        }

        public async Task<List<Stock>> GetAllPaginatedAsync(PaginatedStockQueryObject query)
        {
            var stocks = await GetAllAsync(
                new QueryObject
                {
                    CompanyName = query.CompanyName,
                    Symbol = query.Symbol,
                    SortBy = query.SortBy,
                    IsDecsending = query.IsDecsending,
                }
            );

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skipNumber).Take(query.PageSize).ToList();

            return stocks;
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext
                .Stocks.Include(comment => comment.Comments)
                .FirstOrDefaultAsync(stock => stock.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dbContext.Stocks.AddAsync(stock);
            await _dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _dbContext.Stocks.FirstOrDefaultAsync(stock =>
                stock.Id == id
            );

            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Industry = stockDto.Industry;
            existingStock.MarketCap = stockDto.MarketCap;

            await _dbContext.SaveChangesAsync();

            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = _dbContext.Stocks.FirstOrDefault(stock => stock.Id == id);

            if (stock == null)
            {
                return null;
            }

            _dbContext.Stocks.Remove(stock);
            await _dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _dbContext.Stocks.AnyAsync(stock => stock.Id == id);
        }
    }
}
