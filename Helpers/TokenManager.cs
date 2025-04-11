using System.Collections.Concurrent;

namespace best_hackathon_2025.Helpers;

public static class TokenManager
{
    private static readonly ConcurrentDictionary<string, DateTime> TokenStore = new();

    public static string GenerateToken()
    {
        var token = Guid.NewGuid().ToString();
        TokenStore[token] = DateTime.UtcNow.AddMinutes(10); // Токен дійсний 10 хвилин
        Console.WriteLine($"Згенеровано токен: {token}");
        return token;
    }

    public static bool ValidateToken(string token, out DateTime expiry)
    {
        return TokenStore.TryGetValue(token, out expiry) && expiry >= DateTime.UtcNow;
    }

    public static void RemoveToken(string token)
    {
        TokenStore.TryRemove(token, out _);
    }
}