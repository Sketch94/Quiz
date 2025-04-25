using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace QuizApp.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var jsonString = session.GetString(key);
            return jsonString == null ? default(T) : JsonSerializer.Deserialize<T>(jsonString)!;
        }
    }
}
