namespace Web.Models;

public class ValidationResponse
{
    public Dictionary<string, string[]> Errors { get; set; }
        = new();
}