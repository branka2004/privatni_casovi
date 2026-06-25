using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        LoginViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient();

        var content =
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    email = model.Email,
                    lozinka = model.Lozinka
                }),
                Encoding.UTF8,
                "application/json");

        var response =
            await client.PostAsync(
                "https://localhost:7076/api/Auth/login",
                content);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error =
                "Pogrešan email ili lozinka.";

            return View(model);
        }

        var json =
            await response.Content.ReadAsStringAsync();

        var loginResponse =
            JsonSerializer.Deserialize<LoginResponseDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        HttpContext.Session.SetString(
            "token",
            loginResponse!.Token);

        var role =
            JwtHelper.GetRole(
                loginResponse.Token);

        HttpContext.Session.SetString(
            "tipKorisnika",
            role);

        var korisnikId =
            JwtHelper.GetKorisnikId(
                loginResponse.Token);

        HttpContext.Session.SetString(
            "korisnikId",
            korisnikId.ToString());

        return RedirectToAction(
            "Index",
            "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult RegisterUcenik()
    {
        return View(
            new RegisterUcenikViewModel());
    }

    [HttpGet]
    public IActionResult RegisterPredavac()
    {
        return View(
            new RegisterPredavacViewModel());
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUcenik(
        RegisterUcenikViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient();

        var content =
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    ime = model.Ime,
                    prezime = model.Prezime,
                    email = model.Email,
                    password = model.Lozinka,
                    role = "Ucenik",

                    biografija = (string?)null,
                    cenaPoSatu = (decimal?)null,

                    skola = model.Skola,
                    razred = model.Razred
                }),
                Encoding.UTF8,
                "application/json");

        var response =
            await client.PostAsync(
                "https://localhost:7076/api/Auth/register",
                content);

        if (!response.IsSuccessStatusCode)
        {
            var json =
                await response.Content.ReadAsStringAsync();

            try
            {
                var root =
                    JsonNode.Parse(json);

                var errors =
                    root?["errors"]?.AsArray();

                if (errors != null)
                {
                    foreach (var error in errors)
                    {
                        model.Errors.Add(
                            error?.ToString() ?? "");
                    }
                }
                else
                {
                    model.Errors.Add(
                        "Greška prilikom registracije.");
                }
            }
            catch
            {
                model.Errors.Add(
                    "Greška prilikom registracije.");
            }

            return View(model);
        }

        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPredavac(
        RegisterPredavacViewModel model)
    {
        var client =
            _httpClientFactory.CreateClient();

        var content =
            new StringContent(
                JsonSerializer.Serialize(new
                {
                    ime = model.Ime,
                    prezime = model.Prezime,
                    email = model.Email,
                    password = model.Lozinka,
                    role = "Predavac",

                    biografija = model.Biografija,
                    cenaPoSatu = model.CenaPoSatu,

                    skola = (string?)null,
                    razred = (int?)null
                }),
                Encoding.UTF8,
                "application/json");

        var response =
            await client.PostAsync(
                "https://localhost:7076/api/Auth/register",
                content);

        if (!response.IsSuccessStatusCode)
        {
            var json =
                await response.Content.ReadAsStringAsync();

            var validation =
                JsonSerializer.Deserialize<ValidationResponse>(
                    json,
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

            return View(model);
        }

        return RedirectToAction("Login");
    }
}