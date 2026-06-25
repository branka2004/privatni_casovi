using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web.Models;

namespace Web.Controllers;

public class PredavaciController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PredavaciController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client =
            _httpClientFactory.CreateClient();

        var response =
            await client.GetAsync(
                "https://localhost:7076/api/Predavaci");

        var json =
            await response.Content.ReadAsStringAsync();

        var predavaci =
            JsonSerializer.Deserialize<List<PredavacDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return View(predavaci);
    }

    [HttpGet]
    public async Task<IActionResult> Detalji(int id)
    {
        var client =
            _httpClientFactory.CreateClient();

        var responsePredavac =
            await client.GetAsync(
                $"https://localhost:7076/api/Predavaci/{id}");

        if (!responsePredavac.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var predavacJson =
            await responsePredavac.Content.ReadAsStringAsync();

        var predavac =
            JsonSerializer.Deserialize<PredavacDto>(
                predavacJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        var responsePredmeti =
            await client.GetAsync(
                $"https://localhost:7076/api/Predavaci/{id}/predmeti");

        var predmetiJson =
            await responsePredmeti.Content.ReadAsStringAsync();

        var predmeti =
            JsonSerializer.Deserialize<List<PredmetPredavacaDto>>(
                predmetiJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        ViewBag.Predmeti =
            predmeti ?? new List<PredmetPredavacaDto>();

        return View(predavac);
    }
}