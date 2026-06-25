using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers;

public class CasoviController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CasoviController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {

       
        var client =
            _httpClientFactory.CreateClient();

       

        

        var token =
     HttpContext.Session.GetString(
         "token");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction(
                "Login",
                "Account");
        }

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token); 

        var tipKorisnika =
            JwtHelper.GetRole(token);

        var korisnikId =
            JwtHelper.GetKorisnikId(token);

        string url;

        if (tipKorisnika == "Ucenik")
        {
            url =
                $"https://localhost:7076/api/Casovi/ucenik/{korisnikId}";
        }
        else
        {
            url =
                $"https://localhost:7076/api/Casovi/predavac/{korisnikId}";
        }

        var response =
            await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Content(
                $"Greška API-ja: {(int)response.StatusCode}");
        }

        var json =
            await response.Content.ReadAsStringAsync();

        var casovi =
            JsonSerializer.Deserialize<List<CasDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        return View(casovi);
    }

    [HttpPost]
    public async Task<IActionResult> Otkazi(int id)
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
            JwtHelper.GetRole(token!);

        var response =
            await client.PutAsync(
                $"https://localhost:7076/api/Casovi/otkazi/{id}?otkazao={tipKorisnika}",
                null);

        if (!response.IsSuccessStatusCode)
        {
            var error =
                await response.Content.ReadAsStringAsync();

            return Content(
                $"Status={response.StatusCode}\n{error}");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Odrzan(int id)
    {
        var client =
            _httpClientFactory.CreateClient();

        var token =
            HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await client.PutAsync(
                $"https://localhost:7076/api/Casovi/odrzan/{id}",
                null);

        if (!response.IsSuccessStatusCode)
        {
            var error =
                await response.Content.ReadAsStringAsync();

            return Content(
                $"Status={response.StatusCode}\n{error}");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Zakazi(
     int predavacId,
     int predmetId)
    {
        var model =
            new ZakaziCasViewModel
            {
                PredavacId = predavacId,
                PredmetId = predmetId,
                Datum = DateTime.Today
            };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Zakazi(
    ZakaziCasViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient();

        var token =
            HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        var ucenikId =
            JwtHelper.GetKorisnikId(token!);

        var content =
            new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        ucenikId,
                        predavacId = model.PredavacId,
                        predmetId = model.PredmetId,
                        datum = model.Datum,
                        vremePocetka = model.VremePocetka,
                        vremeZavrsetka = model.VremeZavrsetka
                    }),
                Encoding.UTF8,
                "application/json");

        var response =
            await client.PostAsync(
                "https://localhost:7076/api/Casovi",
                content);

        if (!response.IsSuccessStatusCode)
        {
            var json =
                await response.Content.ReadAsStringAsync();

            if (json.Contains("Predavač već ima"))
            {
                model.Errors.Add(
                    "Predavač već ima zakazan čas u tom terminu.");
            }
            else
            {
                var validation =
                    JsonSerializer.Deserialize<
                        ValidationResponse>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (validation?.Errors != null)
                {
                    foreach (var error in validation.Errors)
                    {
                        model.Errors.AddRange(
                            error.Value);
                    }
                }
            }

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Potvrdi(int id)
    {
        var client =
            _httpClientFactory.CreateClient();

        var token =
            HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        await client.PutAsync(
            $"https://localhost:7076/api/Casovi/potvrdi/{id}",
            null);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Odbij(int id)
    {
        var client =
            _httpClientFactory.CreateClient();

        var token =
            HttpContext.Session.GetString("token");

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer",
                token);

        await client.PutAsync(
            $"https://localhost:7076/api/Casovi/odbij/{id}",
            null);

        return RedirectToAction(nameof(Index));
    }
}