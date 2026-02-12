using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.Stock;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Mappers;

namespace WebApi.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);   
        
        var stocks = await _stockRepository.GetAllAsync(query);

        var stockDtos = stocks.Select(s => s.ToStockDto());

        return Ok(stockDtos);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);  
        
        var stock = await _stockRepository.GetByIdAsync(id);
        
        if (stock == null)
        {
            return NotFound();
        }
        
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto createDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);  
        
        var stockModel = createDto.ToStockFromCreateDTO();
        await _stockRepository.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto() );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);  
        
        var stockModel = await _stockRepository.UpdateAsync(id, updateDto);

        if (stockModel == null)
        {
            return NotFound();
        }
        
        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);  
        
        var stock = await _stockRepository.DeleteAsync(id);

        if (stock == null)
        {
            return NotFound();
        } 
        
        return NoContent();
    }
}