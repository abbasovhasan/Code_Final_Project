using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await _brandService.GetAllBrandsAsync();
        return Ok(brands);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById(int id)
    {
        var brand = await _brandService.GetBrandByIdAsync(id);
        if (brand == null)
            return NotFound();

        return Ok(brand);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrand([FromBody] BrandDto brandDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _brandService.AddBrandAsync(brandDto);
        return CreatedAtAction(nameof(GetBrandById), new { id = brandDto.Name }, brandDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDto brandDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _brandService.UpdateBrandAsync(id, brandDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand(int id)
    {
        await _brandService.DeleteBrandAsync(id);
        return NoContent();
    }
}
