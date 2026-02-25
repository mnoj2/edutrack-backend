using System.Text.Json;

namespace EduTrack.EduTrack.Data.Helpers {
    public class FileHelper {
        public static async Task<T?> ReadFromJsonAsync<T>(string filePath) {

            var jsonData = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(jsonData); 
        }

        public static async Task WriteToJsonAsync<T>(string filePath, T data) {

            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonData = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(filePath, jsonData);
        }
    }
}
