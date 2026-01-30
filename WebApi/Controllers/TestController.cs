using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Core.Entities.ListingModels;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController(
    ITestRepository testRepository
    ) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetListingsTest()
    {
        var listings = await testRepository.GetListings();
        return Ok(listings);
    }
}