using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dtos.Stock;
using WebApi.Mappers;

namespace WebApi.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    
    public StockController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _dbContext.Stocks.ToListAsync();

        var stockDtos = stocks.Select(s => s.ToStockDto());

        return Ok(stockDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _dbContext.Stocks.FindAsync(id);
        
        if (stock == null)
        {
            return NotFound();
        }
        
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        await _dbContext.Stocks.AddAsync(stockModel);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto() );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockDto)
    {
        var stockModel = await _dbContext.Stocks.FindAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = updateStockDto.Symbol;
        stockModel.CompanyName = updateStockDto.CompanyName;
        stockModel.Purchase = updateStockDto.Purchase;
        stockModel.LastDiv = updateStockDto.LastDiv;
        stockModel.Industry = updateStockDto.Industry;
        stockModel.MarketCap = updateStockDto.MarketCap;

        await _dbContext.SaveChangesAsync();
        
        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stock = await _dbContext.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }
        
        _dbContext.Remove(stock);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}