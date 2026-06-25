using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Web.Models;

namespace Web.Controllers;

public class PredmetiController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PredmetiController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client =
            _httpClientFactory.CreateClient();

        var token =
    HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var tipKorisnika =
            HttpContext.Session.GetString(
                "tipKorisnika");

        var korisnikId =
            HttpContext.Session.GetString(
                "korisnikId");

        string url;

        if (tipKorisnika == "Predavac")
        {
            url =
                $"https://localhost:7076/api/Predmeti/predavac/{korisnikId}";
        }
        else
        {
            url =
                "https://localhost:7076/api/Predmeti";
        }

        var response =
            await client.GetAsync(url);

        var json =
            await response.Content.ReadAsStringAsync();
       

        var predmeti =
            JsonSerializer.Deserialize<List<PredmetDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        ViewBag.TipKorisnika =
            tipKorisnika;

        return View(predmeti);
    }

    [HttpGet]
    public async Task<IActionResult> DodajPredmet()
    {
        var predavacId =
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

        var response =
            await client.GetAsync(
                $"https://localhost:7076/api/Predmeti/dostupni/{predavacId}");

        var json =
            await response.Content.ReadAsStringAsync();

        var predmeti =
            JsonSerializer.Deserialize<List<PredmetDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        var vm =
            new AddPredmetPredavacuViewModel
            {
                Predmeti = predmeti ?? new()
            };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> DodajPredmet(
    AddPredmetPredavacuViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient();

        var token =
            HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var predavacId =
            int.Parse(
                HttpContext.Session.GetString(
                    "korisnikId")!);

        HttpResponseMessage response;

        if (!string.IsNullOrWhiteSpace(model.Naziv))
        {
            var content =
                new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        predavacId,
                        naziv = model.Naziv,
                        oblast = model.Oblast,
                        godineIskustva =
                            model.GodineIskustva,
                        nivo = model.Nivo
                    }),
                    Encoding.UTF8,
                    "application/json");

            response =
                await client.PostAsync(
                    "https://localhost:7076/api/Predaje/novi-predmet",
                    content);
        }
        else
        {
            var content =
                new StringContent(
                    JsonSerializer.Serialize(new
                    {
                        predavacId,
                        predmetId = model.PredmetId,
                        godineIskustva =
                            model.GodineIskustva,
                        nivo = model.Nivo
                    }),
                    Encoding.UTF8,
                    "application/json");

            response =
                await client.PostAsync(
                    "https://localhost:7076/api/Predaje",
                    content);
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorJson =
     await response.Content.ReadAsStringAsync();

            var validation =
                JsonSerializer.Deserialize<ValidationResponse>(
                    errorJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            if (validation?.Errors != null)
            {
                foreach (var error in validation.Errors)
                {
                    model.Errors.AddRange(error.Value);
                }
            }

            model.Predmeti =
                await UcitajPredmete(predavacId);

            return View(model);
            return Content(errorJson);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Obrisi(int predmetId)
    {
        var predavacId =
            HttpContext.Session.GetString(
                "korisnikId");

        var token =
            HttpContext.Session.GetString(
                "token");

        var client =
            _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await client.DeleteAsync(
                $"https://localhost:7076/api/Predaje/{predavacId}/{predmetId}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Greska"] =
                "Ne možete ukloniti predmet jer postoje zakazani časovi.";

            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Predavaci(
    int predmetId)
    {
        var client =
            _httpClientFactory.CreateClient();

        var response =
            await client.GetAsync(
                $"https://localhost:7076/api/Predavaci/predmet/{predmetId}");

        var json =
            await response.Content.ReadAsStringAsync();

        var predavaci =
            JsonSerializer.Deserialize<List<PredavacDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        ViewBag.PredmetId = predmetId;

        return View(predavaci);
    }

    private async Task<List<PredmetDto>> UcitajPredmete(
    int predavacId)
    {
        var client =
            _httpClientFactory.CreateClient();

        var response =
            await client.GetAsync(
                $"https://localhost:7076/api/Predmeti/dostupni/{predavacId}");

        var json =
            await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<PredmetDto>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
    }
}