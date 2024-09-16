using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koi.Services.Services.GgDriveService
{
    public class GoogleDriveUpload
    {
        static string[] Scopes = { DriveService.Scope.DriveFile };
        static string ApplicationName = "Google Drive API .NET Upload";

        public static string UploadFileToDrive(string filePath)
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Tạo Drive API service
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath)
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, GetMimeType(filePath));
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);

            // Chia sẻ file công khai
            var permission = new Google.Apis.Drive.v3.Data.Permission()
            {
                Role = "reader", // Quyền chỉ đọc
                Type = "anyone", // Bất kỳ ai có đường dẫn
            };
            service.Permissions.Create(permission, file.Id).Execute();

            // Lấy link chia sẻ công khai
            string fileLink = $"https://drive.google.com/file/d/{file.Id}/view?usp=sharing";
            Console.WriteLine("File Link: " + fileLink);

            return fileLink;
        }

        // Hàm lấy MIME type dựa trên phần mở rộng của file
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/octet-stream";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}
