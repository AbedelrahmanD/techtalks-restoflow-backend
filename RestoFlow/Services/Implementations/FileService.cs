using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using RestoFlow.Services.Interfaces;
using RestoFlow;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RestoFlow.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public FileService(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string destFolder, string[] allowedExtensions, long maxBytes)
        {
            if (file == null)
            {
                var msg = _localizer["file_required"].Value ?? "File is required";
                throw new ArgumentNullException(nameof(file), msg);
            }

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
            {
                var template = _localizer["invalid_extension"].Value ?? "Extension '{0}' is not allowed";
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, template, ext));
            }

            if (file.Length <= 0)
            {
                var msg = _localizer["file_empty"].Value ?? "File is empty";
                throw new InvalidOperationException(msg);
            }

            if (file.Length > maxBytes)
            {
                var template = _localizer["file_too_large"].Value ?? "File exceeds maximum allowed size of {0} bytes";
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, template, maxBytes));
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

        public Task<bool> RemoveFileAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return Task.FromResult(false);

            try
            {
                var normalized = path.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
                var full = Path.IsPathRooted(normalized) ? normalized : Path.Combine(Directory.GetCurrentDirectory(), normalized);

                if (File.Exists(full))
                {
                    File.Delete(full);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
