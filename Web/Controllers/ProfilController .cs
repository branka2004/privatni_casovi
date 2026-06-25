using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Web.Models;
using System.Net.Http.Headers;

namespace Web.Controllers;

public class ProfilController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProfilController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var korisnikId =
            HttpContext.Session.GetString(
                "korisnikId");

        var client =
            _httpClientFactory.CreateClient();

        var token =
    HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var responsePredavac =
            await client.GetAsync(
                $"https://localhost:7076/api/Predavaci/{korisnikId}");

        var jsonPredavac =
            await responsePredavac
                .Content
                .ReadAsStringAsync();

        var predavac =
            JsonSerializer.Deserialize<PredavacDto>(
                jsonPredavac,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        var responseCasovi =
            await client.GetAsync(
                $"https://localhost:7076/api/Casovi/predavac/{korisnikId}");

        var jsonCasovi =
            await responseCasovi
                .Content
                .ReadAsStringAsync();

        var casovi =
            JsonSerializer.Deserialize<List<CasDto>>(
                jsonCasovi,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        Console.WriteLine(jsonCasovi);

        var model =
            new ProfilViewModel
            {
                Predavac = predavac!,
                Casovi = casovi ?? new List<CasDto>()
            };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(
        ProfilViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient();

        var content =
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    biografija =
                        model.Predavac.Biografija,

                    cenaPoSatu =
                        model.Predavac.CenaPoSatu
                }),
                Encoding.UTF8,
                "application/json");

        await client.PutAsync(
            $"https://localhost:7076/api/Predavaci/{model.Predavac.KorisnikId}",
            content);

        return RedirectToAction(nameof(Index));
    }
}