using eXtensionSharp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class SecretController : ControllerBase
{
    private readonly SecretDbContext _dbContext;

    public SecretController(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("{key}/{id}")]
    public async Task<IActionResult> GetJson(string key, int id)
    {
        var item = await _dbContext.Secrets.FirstOrDefaultAsync(m => m.Id == id);
        if (item.xIsEmpty()) return NotFound();
        if (item.SecretKey != key) return Unauthorized();
        return Ok(item.Json);
    }
}