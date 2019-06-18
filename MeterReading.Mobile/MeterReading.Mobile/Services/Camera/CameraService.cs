using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace MeterReading.Mobile.Services.Camera
{
    public class CameraService : ICameraService
    {

        public static readonly ICameraService _instance = new CameraService();

        static CameraService() { }

        private CameraService() { }

        public static ICameraService Instance
        {
            get
            {
                return _instance;
            }
        }

        public async Task<string> TakePhotoAsync(PhotoSize photoSize, string imageDirectory, string imageFileNameSuffix)
        {
            string imagePath = null;

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                throw new ApplicationException("No camera available");
            }

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                throw new ApplicationException("Unable to take photos. Permission denied.");
            }

            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = photoSize,
                SaveToAlbum = true,
                Directory = imageDirectory,
                Name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + imageFileNameSuffix
            });

            App.Analytics.TrackEvent("After camera button click");

            if (photo != null)
            {
                imagePath = photo.Path;
            }

            return imagePath;
        }
    }
}
