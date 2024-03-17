using System.IO;

namespace BlogMVC.Services
{
    public class ImageService
    {
        public async Task<byte[]?> EncodeImageAsync(IFormFile file)
        {
            if (file is null) return null;
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<byte[]?> DecodeImageAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var file = $"{Directory.GetCurrentDirectory()}/wwwroot/images/{fileName}";
            return await File.ReadAllBytesAsync(file);
        }

        public string DecodeImage(byte[] data, string type)
        {
            if (data is null || string.IsNullOrEmpty(type)) return string.Empty;
            return $"data:image/{type};base64,{Convert.ToBase64String(data)}";
        }
    }

}
