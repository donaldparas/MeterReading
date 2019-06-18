using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace MeterReading.Mobile.Services.Camera
{
    public interface ICameraService
    {
        Task<string> TakePhotoAsync(PhotoSize photoSize, string imageDirectory, string imageFileNameSuffix);
    }
}
