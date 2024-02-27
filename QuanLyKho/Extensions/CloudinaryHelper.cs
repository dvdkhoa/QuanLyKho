﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace QuanLyKho.Extensions
{
    public class CloudinaryHelper
    {

        private const string _cloudName = "dqkpxkmip";
        private const string _apiKey = "372141596394814";
        private const string _apiSecret = "zDvxvy0zuhAaYmPj23RJPfpCfh0";
        private static Account _account = new Account(_cloudName, _apiKey, _apiSecret);


        public static Cloudinary Cloudinary = new Cloudinary(_account); 



        // Bổ sung việc trả vể publicId để dễ thực hiện thao tác xóa

        public async static Task<String> UploadFileToCloudinary(IFormFile formFile, string dir)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                // The formFile is the method parameter which type is IFormFile
                // Saves the files to the local file system using a file name generated by the app.
                await formFile.CopyToAsync(stream);
            }
            string newName = Guid.NewGuid().ToString();

            var uploadParams = new RawUploadParams
            {
                PublicId = $"luanvan/{dir}/{newName}",
                File = new FileDescription(filePath)
            };

            var uploadResults = Cloudinary.Upload(uploadParams);

            var url = uploadResults.Url;

            return url.ToString();
        }

        public async static Task<bool> DeteleImage(string fileUrl, string dir)
        {
            string[] arr = fileUrl.Split("/");

            var id = arr[arr.Length - 1].Substring(0, arr[arr.Length - 1].IndexOf("."));

            var publicId = $"luanvan/{dir}/{id}";

            DeletionParams @params = new DeletionParams(publicId);
            var result = await Cloudinary.DestroyAsync(@params);

            var status =  result.StatusCode;
            if (status == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }
    }
}
