using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dtos.Stock;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _dbContext;

    public StockRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _dbContext.Stocks.Include(s => s.Comments).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol == query.Symbol);
        }

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName == query.CompanyName);
        }
        
        return await stocks.ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _dbContext.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _dbContext.Stocks.AddAsync(stockModel);
        await _dbContext.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
        var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
        {
            return null;
        }
        
        stock.Symbol = stockDto.Symbol;
        stock.CompanyName = stockDto.CompanyName;
        stock.Purchase = stockDto.Purchase;
        stock.LastDiv = stockDto.LastDiv;
        stock.Industry = stockDto.Industry;
        stock.MarketCap = stockDto.MarketCap;
        
        await _dbContext.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
        {
            return null;
        }
        
        _dbContext.Remove(stock);
        await _dbContext.SaveChangesAsync();
        return stock;
    }

    public async Task<bool> IsStockExistAsync(int id)
    {
        return await _dbContext.Stocks.AnyAsync(s => s.Id == id);
    }
}