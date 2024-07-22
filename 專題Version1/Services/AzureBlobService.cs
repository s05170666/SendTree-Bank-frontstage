using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace 專題Version1.Services
{
    public class AzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;


        public AzureBlobService(string connectionString, string containerName)
        {

            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadFileAsync(string originalFileName, Stream stream)
        {
            try
            {
                string uniqueFileName = $"{Guid.NewGuid()}_{originalFileName}";

                BlobClient blobClient = _containerClient.GetBlobClient(uniqueFileName);
                await blobClient.UploadAsync(stream, true);
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                // 捕捉例外狀況並回傳錯誤訊息
                return "Upload failed";
            }

        }


        public async Task<Stream> DownloadFileAsync(string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);
            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;//回傳檔案串流
        }

        public string GetBlobUrl(string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }


    }
}