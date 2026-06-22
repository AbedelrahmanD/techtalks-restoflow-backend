using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
namespace RestoFlow.Helpers
{
    public static class FileHelper
    {

        public static async Task<string> SaveFileAsync(IFormFile file, string destFolder, string[] allowedExtensions, long maxBytes)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File is required");
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                throw new InvalidOperationException($"Extension '{ext}' is not allowed");
            }

            if (file.Length <= 0)
            {
                throw new InvalidOperationException("File is empty");
            }

            if (file.Length > maxBytes)
            {
                throw new InvalidOperationException($"File exceeds maximum allowed size of {maxBytes} bytes");
            }

            if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

            var name = Path.GetFileNameWithoutExtension(file.FileName);
            var safeName = string.Concat(name.Where(c => !Path.GetInvalidFileNameChars().Contains(c))).Replace(' ', '_');
            var fileName = $"{safeName}_{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(destFolder, fileName);

            using var stream = new FileStream(full, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
