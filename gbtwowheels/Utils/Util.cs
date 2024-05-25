using System;
using gbtwowheels.Models;

namespace gbtwowheels.Utils
{
	public class Util
	{
        public async void SaveImageInLocalStorage(IFormFile imageFile, string fileName, string fileExtension, string targetPath)
        {
            var filePath = Path.Combine(targetPath, fileName + "." + fileExtension);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
        }
    }
}

