using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EmotionDetection.Controllers;

public class EmotionController : Controller
{
    private HttpClient _httpClient;
    private IHttpClientFactory _httpClientFactory;
    private IConfiguration _configuration;

    public EmotionController(HttpClient httpClient, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("Emotion");
    }

    [HttpPost]
    public async Task<IActionResult> Emotion()
    {
        var file = Request.Form.Files[0];

        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(file.OpenReadStream());
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        content.Add(fileContent, "file", file.FileName);
        var response = await _httpClient.PostAsync("http://localhost:8000/api", content);
        
        var result = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(result);
        if (json.ContainsKey("emotion"))
        {
            var emotion = json["emotion"]?.ToString();
            return Json(new { success = true, emotion });
        }

        return Json(new { success = false, message = "Yüzünü Görelim Algılayamıyorum!!!!" });
    }
    
    public async Task<IActionResult> happy()
    {
        string gifUrl = await GetMutluOl();
        ViewBag.GifUrl = gifUrl;
        return View("MutluOl");
    }

    private async Task<string> GetMutluOl()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=funny&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }
    
    public async Task<IActionResult> angry()
    {
        string gifUrl = await GetSakinOl();
        ViewBag.GifUrl = gifUrl;
        return View("SakinOl");
    }

    private async Task<string> GetSakinOl()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=angry&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }
    
    public async Task<IActionResult> disgust()
    {
        string gifUrl = await GetIgrenmeBenden();
        ViewBag.GifUrl = gifUrl;
        return View("IgrenmeBenden");
    }

    private async Task<string> GetIgrenmeBenden()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=disgust&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }
    
    public async Task<IActionResult> fear()
    {
        string gifUrl = await GetKorku();
        ViewBag.GifUrl = gifUrl;
        return View("Korku");
    }

    private async Task<string> GetKorku()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=scared&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }
    
    public async Task<IActionResult> neutral()
    {
        string gifUrl = await GetNotr();
        string fikra = await GetFikraUret();
        ViewBag.Fikra = fikra;
        ViewBag.GifUrl = gifUrl;
        return View("gulmelisin");
    }

    private async Task<string> GetNotr()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=tired&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }
    
    public async Task<IActionResult> sad()
    {
        string gifUrl = await GetSad();
        string fikra = await GetFikraUret();
        ViewBag.GifUrl = gifUrl;
        ViewBag.Fikra = fikra;
        return View("Uzulme");
    }

    private async Task<string> GetSad()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=crying&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }

    private async Task<string> GetFikraUret()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fikralar.txt");
        var fikra = await System.IO.File.ReadAllTextAsync(filePath);
        var fikraList = fikra.Split("---", StringSplitOptions.RemoveEmptyEntries);
        Random random = new Random();
        int randomIndex = random.Next(fikraList.Length);

        return fikraList[randomIndex].Trim();
    }
    
    public async Task<IActionResult> surprise()
    {
        string gifUrl = await GetSuprise();
        ViewBag.GifUrl = gifUrl;
        return View("Supriz");
    }

    private async Task<string> GetSuprise()
    {
        string apiKey = _configuration["MutluOlGiphyApiKey"];
        string apiUrl = $"https://api.giphy.com/v1/gifs/random?api_key={apiKey}&tag=surprise&rating=g";
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(apiUrl);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return data["data"]?["images"]?["original"]?["url"]?.ToString();
        }

        return null;
    }
    
    
}