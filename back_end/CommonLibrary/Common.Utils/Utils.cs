using Common.Constant;
using Common.Enum;
using Common.Utils;
using log4net;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Model.RequestModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Globalization;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utils
{
    public static class Utils
    {
        public static string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }

        public static string GenerateQRCode()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            return Convert.ToBase64String(time.Concat(key).ToArray());
        }

        public static string GenerateOneTimeOTP(int length = 6)
        {
            string characters = "1234567890";
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        public static string HashMd5(string input)
        {
            string result = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();
                // Loop through each byte of the hashed data
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                result = sBuilder.ToString();
            }
            return result;
        }

        public static bool VerifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = HashMd5(input);
            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

        public static string GetMessage(string prefix, int iCode, string language)
        {
            try
            {
                string name = prefix + iCode.ToString("0000");
                string lan = CultureHelper.GetImplementedCulture(language);
                // Modify current thread's cultures
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(lan);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                // Set the thread's CurrentUICulture.
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lan);
                Resources.Culture = cultureInfo;
                ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(cultureInfo, true, true);
                string msgResource = resourceSet.GetString(name);
                return msgResource;
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
            return string.Empty;
        }

        public static MyException GetMyException(Exception ex)
        {
            if (ex is MyException MyException) return MyException;
            return new MyException(SystemReturnCode.Error, $"{ex.GetExceptionMessage()}");
        }

        public static string GetResources(string resourceKey, string language)
        {
            try
            {
                string lan = CultureHelper.GetImplementedCulture(language);
                // Modify current thread's cultures
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(lan);
                //Thread.CurrentThread.CurrentCulture = cultureInfo;
                // Set the thread's CurrentUICulture.
                //Thread.CurrentThread.CurrentUICulture = new CultureInfo(lan);
                Resources.Culture = cultureInfo;
                ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(cultureInfo, true, true);
                string msgResource = resourceSet.GetString(resourceKey);
                return msgResource;
            }
            catch { }
            return string.Empty;
        }

        public static bool IsImageURL(string imgURL)
        {
            if (!string.IsNullOrWhiteSpace(imgURL) && imgURL.StartsWith("http://"))
            {
                string[] imageExtensions = new string[] { ".png", ".jpg", ".gif", ".jpeg" };
                string pathExtention = Path.GetExtension(imgURL);
                return imageExtensions.Any(p => p == pathExtention);
            }
            return false;
        }

        public static bool IsUrl(string sUrl)
        {
            Regex regex =
                new Regex(
                    @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$",
                    RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Match match = regex.Match(sUrl);
            return match.Success;
        }

        #region Normalize VietNam Character - Remove unicode

        public static string NormalizeUnicode(string unicodeText)
        {
            string normalText = unicodeText.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < normalText.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(normalText[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalText[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            normalText = sb.ToString().Normalize(NormalizationForm.FormD);
            return normalText;
        }

        #endregion

        public static string UppercaseString(string value)
        {
            try
            {
                value = value.ToLower();
                char[] array = value.ToCharArray();
                if (array.Length >= 1)
                {
                    if (char.IsLower(array[0]))
                    {
                        array[0] = char.ToUpper(array[0]);
                    }
                }
                for (int i = 1; i < array.Length; i++)
                {
                    if (array[i - 1] == ' ')
                    {
                        if (char.IsLower(array[i]))
                        {
                            array[i] = char.ToUpper(array[i]);
                        }
                    }
                }
                return new string(array);
            }
            catch { return value; }
        }

        #region Encrypt

        public static string EncryptStringBase64(string plainText)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return plainText;
        }

        public static string DecryptStringBase64(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                    return string.Empty;
                var base64EncodedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "";
        }

        #endregion

        #region Get Value Key

        public static string GetValueByKeyName(FormDataCollection formData, string keyName)
        {
            if (formData.GetValues(keyName) == null || (formData.GetValues(keyName) != null
                && string.IsNullOrWhiteSpace(formData.GetValues(keyName)[0])))
                return string.Empty;
            else
                return formData.GetValues(keyName)[0].ToString();
        }

        public static T GetValueByKeyName<T>(FormDataCollection formData, string keyName)
        {
            try
            {
                if (formData == null || formData.GetValues(keyName) == null || (formData.GetValues(keyName) != null && string.IsNullOrWhiteSpace(formData.GetValues(keyName)[0])))
                    return default(T);
                else
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(formData.GetValues(keyName)[0].ToString());
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

        #region Get Request Header
        public static string GetValueByKeyName(HttpRequestMessage request, string keyName)
        {
            try
            {
                if (request != null && request.Headers != null)
                {
                    var values = request.Headers.GetValues(keyName);
                    if (values != null)
                        return values.FirstOrDefault();
                }
            }
            catch { }
            return "";
        }
        public static string GetValueByKeyName(HttpRequest request, string keyName)
        {
            try
            {
                if (request != null && request.Headers != null)
                {
                    if (request.Headers.TryGetValue(keyName, out var values))
                    {
                        return values.FirstOrDefault();
                    }
                }
            }
            catch { }
            return "";
        }
        #endregion

        #region Get Value [FromBody] Dynamic

        public static string GetValueByKeyName(dynamic value, string keyName)
        {
            if (value != null)
                return value[keyName].Value + "";
            return "";
        }

        public static T GetValueByKey<T>(JToken request, string keyName, bool isRequireValue = false, bool isRequireField = false)
        {
            try
            {
                if (request == null || request[keyName] == null || (request[keyName] != null && string.IsNullOrWhiteSpace(request[keyName].ToString())))
                {
                    if (isRequireValue) throw new Exception($"{keyName} is required.");
                    return default;
                }
                else
                {
                    return request[keyName].ToObject<T>();
                }
            }
            catch
            {
                if (isRequireField) throw new Exception($"{keyName} is required.");
            }
            return default;
        }

        #endregion

        #region Common

        /// <summary>
        /// return '+' if A & B is different 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string NotationNextDay(DateTime? dat, DateTime? nextDay)
        {
            if (!dat.HasValue || !nextDay.HasValue) return "";
            if (dat.Value.Date != nextDay.Value.Date) return "+";
            return "";
        }

        public static string GetDateTimeStandard(DateTime? dt, DateTimeType type = DateTimeType.Long24H)
        {
            if (dt == null)
                return "";
            if (!dt.HasValue)
                return "";
            switch (type)
            {
                case DateTimeType.Long24H:
                    return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                case DateTimeType.Long12H:
                    return dt.Value.ToString("yyyy-MM-dd hh:mm:ss tt");
                case DateTimeType.OnlyDate:
                    return dt.Value.ToString("yyyy-MM-dd");
                case DateTimeType.OnlyDateVI:
                    return dt.Value.ToString("dd-MM-yyyy");
                case DateTimeType.OnlyLongTime12H:
                    return dt.Value.ToString("hh:mm:ss tt");
                case DateTimeType.OnlyLongTime24H:
                    return dt.Value.ToString("HH:mm:ss");
                case DateTimeType.OnlyShortTime12H:
                    return dt.Value.ToString("hh:mm tt");
                case DateTimeType.OnlyShortTime24H:
                    return dt.Value.ToString("HH:mm");
                case DateTimeType.OnlyDateInternational:
                    return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
                case DateTimeType.DDMMM:
                    return dt.Value.ToString("dd MMM").ToUpper();
                case DateTimeType.TimeOrDate:
                    if (dt.Value.Date == DateTime.Today) //return time
                        return dt.Value.ToString("HH:mm");
                    else
                        return dt.Value.ToString("ddd, dd MMM");
                default:
                    return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /*public static bool IsValidJson(string strInput)
        {
            try
            {
                strInput = strInput.Trim();
                if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                    (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
                {
                    try
                    {
                        var obj = JToken.Parse(strInput);
                        return true;
                    }
                    catch (JsonReaderException jex)
                    {
                        //Exception in parsing json
                        Console.WriteLine(jex.Message);
                        return false;
                    }
                    catch (Exception ex) //some other exception
                    {
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, ex);
                return false;
            }
        }

        public static string NormalizeString(string inputString)
        {
            try
            {
                if (inputString == null)
                    return "";
                else
                    return inputString.Trim();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(LogLevelL4N.ERROR, ex);
                return string.Empty;
            }
        }*/

        /// <summary>
        /// Base64 string
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encode64(string plainText)
        {
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return plainText;
        }

        /// <summary>
        /// Decode base64 to string
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string Decode64(string encryptedText)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedText))
                    return string.Empty;
                var base64EncodedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "";
        }

        public static string NormalizeMessageTime(DateTime? dt)
        {
            try
            {
                if (dt == null)
                    return "";
                if (!dt.HasValue)
                    return "";
                if (dt.Value.Date == DateTime.Today.Date)
                    return string.Format("{0} {1}", "Today", dt.Value.ToString("HH:mm"));
                else if ((DateTime.Today.Date - dt.Value.Date).TotalDays == 1)
                    return string.Format("{0} {1}", "Yesterday", dt.Value.ToString("HH:mm"));
                else if ((DateTime.Today.Date - dt.Value.Date).TotalDays >= 2 && (DateTime.Today.Date - dt.Value.Date).TotalDays <= 7)
                    return string.Format("{0} {1}", dt.Value.ToString("dddd"), dt.Value.ToString("HH:mm"));
                else
                    return string.Format("{0}, {1}", dt.Value.ToString("ddd, MMM dd"), dt.Value.ToString("HH:mm"));
            }
            catch { }
            return dt.Value.ToString("dd/MM HH:mm");
        }

        #endregion

        public static DateTime GetEndOfDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 23, 59, 59);
        }

        public static DateTime GetStartOfDate(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }

        #region Basic Response for Version 2

        public static BaseResponse<T> CreateResponseModel<T>(T data, int record = 0)
        {
            return new BaseResponse<T>
            {
                Message = StatusMessage.Success,
                Data = data,
                TotalRecord = record
            };
        }

        public static BaseResponse<T> CreateSuccessModel<T>(T data, string message = "", int record = 0)
        {
            return new BaseResponse<T>
            {
                Message = string.IsNullOrWhiteSpace(message) ? StatusMessage.Success : message,
                Data = data,
                TotalRecord = record
            };
        }

        public static BaseResponse<T> CreateErrorModel<T>(StatusCode statusCode = StatusCode.Error, string message = StatusMessage.Error, Exception? exception = null)
        {
            var sttCode = (int)statusCode;
            try
            {
                if (exception is DbEntityValidationException dbEntityValidationException)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var eve in dbEntityValidationException.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            stringBuilder.AppendLine(string.Format("* Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                    string sErrorMessage = stringBuilder.ToString();
                    if (!string.IsNullOrWhiteSpace(sErrorMessage)) message += $"\n{sErrorMessage}";
                }
                else if (exception is MyException ex)
                {
                    if (message == exception.Message) { }
                    else message += $"\n{exception.GetExceptionMessage()}";
                    sttCode = (int)ex.RetCode;
                }
                else if (exception != null)
                {
                    sttCode = exception.GetHashCode();
                    if (message == exception.Message) { }
                    else message += $"\n{exception.GetBaseException().Message}";
                }
            }
            catch { }

            return new BaseResponse<T>
            {
                StatusCode = sttCode,
                Message = message
            };
        }

        #endregion

        public static string HtmlToPlainText(string htmlString)
        {
            if (!string.IsNullOrWhiteSpace(htmlString))
            {
                string htmlTagPattern = "<.*?>";
                var regexCss = new Regex(@"(\\<script(.+?)\\)|(\\<style(.+?)\\)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                htmlString = regexCss.Replace(htmlString, string.Empty);
                htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
                //htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                //htmlString = htmlString.Replace(" ", string.Empty);
                return htmlString;
            }
            return htmlString;
        }

        public static String HexConverter(System.Drawing.Color c)
        {
            try
            {
                return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
                //string.Format("{0:x6}", c.ToArgb());
            }
            catch
            {

                return "#000000";
            }
        }

        #region WriteLog

        public static void WriteLogInfo(ILog log, string userId, HttpRequest request, object body)
        {
            //Write log HEADER & DATA & API Name
            string apiRequested = request.Path.Value ?? "";
            log.Info($"USERID: {userId} - URL: {apiRequested} - BODY: {JsonConvert.SerializeObject(body)} - MESSAGE: {StatusMessage.Success}");
        }

        public static void WriteLogError(ILog log, string userId, HttpRequest request, Exception ex)
        {
            //Write log
            string apiRequested = request.Path.Value ?? "";
            log.Error($"USERID: {userId} - URL: {apiRequested} failed. - BODY:  - MESSAGE: {ex.Message}");
        }

        #endregion

        #region Upload

        public static List<string> UploadImageToAzure(IFormFile file, string strStorage, string strContainerRefer, ModelUrlImage model)
        {
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();
            List<string> allowedExtensions = new List<string> { ".jpg", ".png" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Only JPEG (.jpg) and .png files are allowed.");
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(strStorage);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to the container
            CloudBlobContainer container = blobClient.GetContainerReference(strContainerRefer);

            #region Tách ảnh thành 3 ảnh kích thước khác nhau
            var sizes = new (int width, int height)[] { (480, 720), (720, 1280), (1080, 1920) };

            var blobNames = new string[] {
                "ImageSmall/" + Guid.NewGuid() + fileExtension,
                "ImageMedium/" + Guid.NewGuid() + fileExtension,
                "ImageLarge/" + Guid.NewGuid() + fileExtension
            };
            if (model.UrlSmall != null && model.UrlMedium != null && model.UrlBig != null)
            {
                blobNames = new string[] {
                    "ImageSmall/"+ Path.GetFileName(model.UrlSmall),
                    "ImageMedium/"+ Path.GetFileName(model.UrlMedium),
                    "ImageLarge/"+Path.GetFileName(model.UrlBig)
                };
            }

            // Create a list to store the URLs of the resized images
            var urls = new List<string>();

            // Resize the image and upload it to Azure Storage for each size
            for (int i = 0; i < sizes.Length; i++)
            {
                // Load the image from the stream
                using var image = Image.Load(file.OpenReadStream());

                // Resize the image using ImageSharp library
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(sizes[i].width, sizes[i].height),
                    Mode = ResizeMode.Max
                }));

                // Create a new MemoryStream to store the resized image
                using var resizedImageStream = new MemoryStream();

                image.SaveAsJpeg(resizedImageStream);

                // Reset the position of the stream
                resizedImageStream.Position = 0;

                // Upload the resized image to Azure Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobNames[i]);
                blockBlob.UploadFromStreamAsync(resizedImageStream).Wait();

                // Add the URL of the resized image to the list
                urls.Add(blockBlob.Uri.ToString());

                #region MIME type
                // Get the MIME type of the file
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileExtension, out string contentType))
                {
                    contentType = "application/octet-stream";
                }

                // Set the content type of the blob
                blockBlob.Properties.ContentType = contentType;
                blockBlob.SetPropertiesAsync().Wait();
                #endregion
            }
            #endregion
            return urls;
        }

        public static string UploadFileToAzure(IFormFile file, string strStorage, string strContainerRefer)
        {
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(strStorage);

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to the container
            CloudBlobContainer container = blobClient.GetContainerReference(strContainerRefer);

            var blobNames = Guid.NewGuid() + fileExtension;

            // Create a new MemoryStream to store the resized image
            using var resizedImageStream = new MemoryStream();

            file.CopyToAsync(resizedImageStream);

            // Reset the position of the stream
            resizedImageStream.Position = 0;

            // Upload the file to Azure Storage
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobNames);
            blockBlob.UploadFromStreamAsync(resizedImageStream).Wait();

            // Add the URL of the file
            var url = blockBlob.Uri.ToString();

            #region MIME type
            // Get the MIME type of the file
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileExtension, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            // Set the content type of the blob
            blockBlob.Properties.ContentType = contentType;
            blockBlob.SetPropertiesAsync().Wait();
            #endregion

            return url;
        }

        public static List<string> UploadImageToLocal(IFormFile file, string folderName, ModelUrlImage model)
        {
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();
            List<string> allowedExtensions = new List<string> { ".jpg", ".png" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Only JPEG (.jpg) and .png files are allowed.");
            }

            #region Tách ảnh thành 3 ảnh kích thước khác nhau
            var sizes = new (int width, int height)[] { (480, 720), (720, 1280), (1080, 1920) };

            var pathNames = new string[] {
                "ImageSmall/" + Guid.NewGuid() + fileExtension,
                "ImageMedium/" + Guid.NewGuid() + fileExtension,
                "ImageLarge/" + Guid.NewGuid() + fileExtension
            };
            if (model.UrlSmall != null && model.UrlMedium != null && model.UrlBig != null)
            {
                pathNames = new string[] {
                    "ImageSmall/"+ Path.GetFileName(model.UrlSmall),
                    "ImageMedium/"+ Path.GetFileName(model.UrlMedium),
                    "ImageLarge/"+Path.GetFileName(model.UrlBig)
                };
            }

            // Create a list to store the URLs of the resized images
            var urls = new List<string>();

            // Resize the image and upload it to Azure Storage for each size
            for (int i = 0; i < sizes.Length; i++)
            {
                // Load the image from the stream
                using var image = Image.Load(file.OpenReadStream());

                // Resize the image using ImageSharp library
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(sizes[i].width, sizes[i].height),
                    Mode = ResizeMode.Max
                }));

                // Create a new MemoryStream to store the resized image
                using var resizedImageStream = new MemoryStream();

                image.SaveAsJpeg(resizedImageStream);

                // Reset the position of the stream
                resizedImageStream.Position = 0;

                // Upload the resized image to Local Storage
                var path = Path.Combine(Directory.GetCurrentDirectory(), folderName, Guid.NewGuid() + fileExtension);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }

                // Add the URL of the resized image to the list
                urls.Add(path);
            }
            #endregion
            return urls;
        }

        public static string UploadFileToLocal(IFormFile file, string folderName)
        {
            string fileName = file.FileName;
            string fileExtension = Path.GetExtension(fileName).ToLower();
            // Save the file to disk
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName, Guid.NewGuid() + fileExtension);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }
            return path;
        }

        #endregion

        public static DeviceInfoRequest GetDeviceInfo(HttpRequest request)
        {
            DeviceInfoRequest? model = null;
            string udid = GetValueByKeyName(request, "X_REQUEST_UDID");//Unique Device ID
            string osVersion = GetValueByKeyName(request, "X_REQUEST_OS_VERSION");//iOS 11.2
            string platform = GetValueByKeyName(request, "X_REQUEST_PLATFORM");//Android or iOS or Other Platform
            string deviceType = GetValueByKeyName(request, "X_REQUEST_DEVICE_TYPE");//iPhone, iPad, Galaxy S8
            string deviceName = GetValueByKeyName(request, "X_REQUEST_DEVICE_NAME");//Harry's iPhone
            string description = GetValueByKeyName(request, "X_REQUEST_DESCRIPTION");//json format (other infomation app can get)

            if (!string.IsNullOrWhiteSpace(udid))
            {
                model = new DeviceInfoRequest
                {
                    UDID = udid,
                    OSVersion = osVersion,
                    OSName = platform,
                    DeviceType = deviceType,
                    DeviceName = deviceName,
                    DeviceDescription = description
                };
            }

            if (model == null)
                throw new Exception(string.Format(CommonMessage.Message_Required, "Device Info"));
            return model;
        }

        public static T? GetValueDynamic<T>(dynamic dyObject, string keyName, bool isRequireValue = false, bool isRequireField = true)
        {
            try
            {
                var requestData = CommonConvert.ConvertDynamicData(dyObject);
                if (CommonConvert.DoesPropertyExist(requestData, keyName))
                {
                    var values = (IDictionary<string, object>)requestData;
                    var value = values[keyName];
                    if (value != null)
                    {
                        try
                        {
                            if (typeof(T) == typeof(string))
                                return (T?)TypeDescriptor.GetConverter(typeof(T?)).ConvertFromString(value?.ToString() ?? "");
                            else if (typeof(T) == typeof(int) || typeof(T) == typeof(int?) || typeof(T) == typeof(DateTime?) || typeof(T) == typeof(DateTime) || typeof(T) == typeof(Guid?) || typeof(T) == typeof(Guid))
                                return (T?)TypeDescriptor.GetConverter(typeof(T?)).ConvertFromString(value?.ToString() ?? "");
                            return (T)value;
                        }
                        catch
                        {
                            if (isRequireValue) throw new KeyNotFoundException($"Not found value [key: {keyName}]");
                        }
                    }
                    else
                    {
                        if (isRequireValue) throw new KeyNotFoundException($"Not found value [key: {keyName}]");
                    }
                }
            }
            catch
            {
                if (isRequireField) throw new KeyNotFoundException($"Not found key [{keyName}]");
            }
            return default;
        }
    }

    public static class CommonConvert
    {
        public static long ToUnixTimeStamp(this DateTime? dateTime)
        {
            try
            {
                if (!dateTime.HasValue) return 0;
                var dateTimeOffset = new DateTimeOffset(dateTime.Value);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                return unixDateTime;
            }
            catch
            {
                return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            }
        }

        public static long ToUnixTimeStamp(this DateTime dateTime)
        {
            try
            {
                var dateTimeOffset = new DateTimeOffset(dateTime);
                var unixDateTime = dateTimeOffset.ToUnixTimeSeconds();
                return unixDateTime;
            }
            catch
            {
                return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            }
        }

        public static DateTime? ToDatetime(this long? unixDateTime)
        {
            try
            {
                if (unixDateTime.GetValueOrDefault(0) == 0) return null;
                var localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixDateTime.Value).DateTime.ToLocalTime();
                return localDateTimeOffset;
            }
            catch
            {
                return null;
            }
        }

        public static DateTime ToDatetime(this long unixDateTime)
        {
            try
            {
                var localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixDateTime).DateTime.ToLocalTime();
                return localDateTimeOffset;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        private static string ToFormatDate(this DateTime? dt, string format)
        {
            if (dt.HasValue)
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                return dt.Value.ToString(format);
            }
            return string.Empty;
        }

        private static string ToFormatDate(this DateTime dt, string format)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return dt.ToString(format);
        }

        public static string ToFormatDate(this DateTime? dt, string resourceKey, string language)
        {
            if (dt.HasValue)
                return ToFormatDate(dt, Utils.GetResources(resourceKey, language));
            return string.Empty;
        }

        public static string ToFormatDate(this DateTime dt, string resourceKey, string language)
        {
            return ToFormatDate(dt, Utils.GetResources(resourceKey, language));
        }

        public static string ToFormatShortDate(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                return dt.Value.ToString("dd/MM/yyyy");
            }
            return string.Empty;
        }

        public static string ToFormatShortDate(this DateTime dt)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return dt.ToString("dd/MM/yyyy");
        }

        public static string ToFormatLongDate(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                return dt.Value.ToString("dd/MM/yyyy HH:mm");
            }
            return string.Empty;
        }

        public static string ToFormatLongDate(this DateTime dt)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return dt.ToString("dd/MM/yyyy HH:mm");
        }

        public static string ToFormatFullDate(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                return dt.Value.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return string.Empty;
        }

        public static string ToFormatFullDate(this DateTime dt)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return dt.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string ToFormatFullTime(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                return dt.Value.ToString("HH:mm:ss");
            }
            return string.Empty;
        }

        public static string ToFormatFullTime(this DateTime dt)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return dt.ToString("HH:mm:ss");
        }

        public static string ToFormatChatTime(this DateTime dt, string language)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            DateTime newTime = dt;
            DateTime now = DateTime.Now;
            if (newTime.Date == now.Date)
            {
                return $"{newTime:HH:mm}";
            }
            else if (newTime.Year == now.Year)
            {
                return $"{newTime:dd/MM HH:mm}";
            }
            return ToFormatDate(newTime, Utils.GetResources(MyFormatConst.CM_Format_LongDate24_None_Second, language));
        }

        public static string ToFormatChatTime(this DateTime? dt, string language)
        {
            if (dt.HasValue)
                return ToFormatChatTime(dt.Value, language);
            return "N/A";
        }

        public static string ToFormatMoney(this decimal d, string formatStyle, string formatUnit)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return string.Format("{0}{1}", Math.Floor(d).ToString(formatStyle), formatUnit);
        }

        public static string ToFormatMoney(this decimal d, string resourceKeyStyle, string resourceKeyUnit, string language)
        {
            return ToFormatMoney(d, Utils.GetResources(resourceKeyStyle, language), Utils.GetResources(resourceKeyUnit, language));
        }

        public static string ToFormatMoney(this decimal? d, string resourceKeyStyle, string resourceKeyUnit, string language)
        {
            return ToFormatMoney(d ?? 0, Utils.GetResources(resourceKeyStyle, language), Utils.GetResources(resourceKeyUnit, language));
        }

        public static string ToFormatMoneyPayslip(this decimal d, string language)
        {
            return ToFormatMoney(d, Utils.GetResources(MyFormatConst.CM_Format_Money_Style, language), Utils.GetResources(MyFormatConst.CM_Format_Money_Unit_VND_ShortText, language));
        }

        public static string ToFormatMoneyPayslip(this decimal? d, string language)
        {
            return ToFormatMoney(d ?? 0, Utils.GetResources(MyFormatConst.CM_Format_Money_Style, language), Utils.GetResources(MyFormatConst.CM_Format_Money_Unit_VND_ShortText, language));
        }

        public static string ToFormatDecimal(this decimal d, string format)
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            return d.ToString(format);
        }

        public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable("dataTable");

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// Dùng để gọi store procedure mà có tham số truyền vào là danh sách Guid.
        /// Use for call store procedure have paramater is guid array. 
        /// </summary>
        /// <param name="arGuids"></param>
        /// <returns></returns>
        public static DataTable BuildGuidDataType(this IEnumerable<Guid> arGuids)
        {
            DataTable retVal = new DataTable("GuidIds");
            retVal.Columns.Add("Id", typeof(Guid));
            if (arGuids == null) return retVal;
            foreach (Guid g in arGuids)
            {
                var dr = retVal.NewRow();
                dr[0] = g;
                retVal.Rows.Add(dr);
            }
            return retVal;
        }

        public static DataTable BuildIntDataType(this IEnumerable<int> arInts)
        {
            DataTable retVal = new DataTable("IntIds");
            retVal.Columns.Add("Id", typeof(int));
            if (arInts == null) return retVal;
            foreach (int g in arInts)
            {
                var dr = retVal.NewRow();
                dr[0] = g;
                retVal.Rows.Add(dr);
            }
            return retVal;
        }

        public static DataTable BuildStringDataType(this IEnumerable<string> arString)
        {
            DataTable retVal = new DataTable("StringTable");
            retVal.Columns.Add("Value", typeof(string));
            if (arString == null) return retVal;
            foreach (var g in arString)
            {
                var dr = retVal.NewRow();
                dr[0] = g;
                retVal.Rows.Add(dr);
            }
            return retVal;
        }

        /// <summary>
        /// Get nonunicode string
        /// </summary>
        /// <param name="sText"></param>
        /// <returns>Nonunicode string</returns>
        public static string NonUnicode(this string sText)
        {
            try
            {
                string sFormD = sText.Normalize(NormalizationForm.FormD);
                StringBuilder sbNonUnicodeString = new StringBuilder();
                UnicodeCategory ucCategory;

                for (int i = 0; i < sFormD.Length; i++)
                {
                    ucCategory = CharUnicodeInfo.GetUnicodeCategory(sFormD[i]);
                    if (ucCategory != UnicodeCategory.NonSpacingMark)
                    {
                        sbNonUnicodeString.Append(sFormD[i]);
                    }
                }
                sbNonUnicodeString = sbNonUnicodeString.Replace('Đ', 'D');
                sbNonUnicodeString = sbNonUnicodeString.Replace('đ', 'd');
                return (sbNonUnicodeString.ToString().Normalize(NormalizationForm.FormC));
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
            return string.Empty;
        }

        /// <summary>
        /// Get nonunicode string in lower
        /// </summary>
        /// <param name="sText"></param>
        /// <returns>Nonunicode string</returns>
        public static string NonUnicodeLower(this string sText)
        {
            try
            {
                return sText.NonUnicode().ToLower();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
            return string.Empty;
        }

        /// <summary>
        /// Get nonunicode string in upper
        /// </summary>
        /// <param name="sText"></param>
        /// <returns>Nonunicode string</returns>
        public static string NonUnicodeUpper(this string sText)
        {
            try
            {
                return sText.NonUnicode().ToUpper();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
            return string.Empty;
        }

        public static DataTable ToDataTable<T>(this IList<T> arData, string tableName = "")
        {
            try
            {
                PropertyDescriptorCollection arPropertiesDescriptor =
                    TypeDescriptor.GetProperties(typeof(T));

                DataTable dtData = new DataTable(tableName);

                foreach (PropertyDescriptor propertyDescriptor in arPropertiesDescriptor)
                {
                    dtData.Columns.Add(propertyDescriptor.Name, Nullable.GetUnderlyingType(propertyDescriptor.PropertyType) ?? propertyDescriptor.PropertyType);
                }

                object[] arValues = new object[arPropertiesDescriptor.Count];

                foreach (T item in arData)
                {
                    for (int i = 0; i < arValues.Length; i++)
                    {
                        arValues[i] = arPropertiesDescriptor[i].GetValue(item);
                    }
                    dtData.Rows.Add(arValues);
                }
                return dtData;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
            return new DataTable(tableName);
        }

        public static decimal MakeRoundMoney(this decimal value)
        {
            try
            {
                var val = value % 1000 >= 500 ? value + 1000 - value % 1000 : value - value % 1000;
                return val;
                //var val = Math.Round(value / 1000.0M, 0) * 1000.0M;
                //val = Math.Round(val / 1000.0M, 2) * 1000.0M;
                //val = Math.Round(val / 1000.0M, 1) * 1000.0M;
                //val = Math.Round(val / 1000.0M, 0) * 1000.0M;
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
            return value;
        }

        public static decimal MakeRoundDecimal(this decimal value)
        {
            try
            {
                return Math.Round(value, 0, MidpointRounding.AwayFromZero);
                //return Math.Round(Math.Round(Math.Round(value, 2), 1), 0);
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
            return value;
        }

        public static decimal MakeFloorDecimal(this decimal value)
        {
            try
            {
                return Math.Floor(value);
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
            return value;
        }

        public static ExpandoObject ConvertDynamicData(dynamic jsonData)
        {
            JToken data = ConvertDynamicToJToken(jsonData);
            return data.ToObject<ExpandoObject>();
        }

        public static JToken ConvertDynamicToJToken(dynamic jsonData)
        {
            JToken data = JsonConvert.DeserializeObject<JToken>(jsonData.ToString());
            return data;
        }

        public static bool DoesPropertyExist(dynamic dyObject, string propertyName)
        {
            if (dyObject is ExpandoObject)
                return ((IDictionary<string, object>)dyObject).ContainsKey(propertyName);
            return dyObject.GetType().GetProperty(propertyName) != null;
        }
    }

    public static class ExceptionExtend
    {
        public static string GetExceptionMessage(this Exception ex)
        {
            if (ex is DbEntityValidationException dbEntityValidationException)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var eve in dbEntityValidationException.EntityValidationErrors)
                {
                    string s1 = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        string s2 = string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        stringBuilder.AppendLine(s2);
                    }
                }
                return stringBuilder.ToString();
            }
            else
            {
                return GetInnerMessage(ex);
            }
        }

        public static string GetInnerMessage(this Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetInnerMessage(ex.InnerException);
            }
            else
            {
                return ex.Message;
            }
        }
    }

    public static class UtilsExtension
    {
        #region For dynamic

        /// <summary>
        /// Convert json string to expando object. 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic JsonToExpandoObject(string json)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        }

        public static string ToJsonString(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        #endregion
    }

    public static class Caster
    {
        #region Cast Model

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                propertyInfo.SetValue(x, value, null);
            }
            return (T)x;
        }

        #endregion
    }

    public class BaseResponse<T>
    {
        public int StatusCode { get; set; } = (int)Common.Constant.StatusCode.Success;
        public string Message { get; set; } = "";
        public int TotalRecord { get; set; } = 0;
        public T? Data { get; set; }
    }

    public class ModelUrlImage
    {
        public string? UrlSmall { get; set; }
        public string? UrlMedium { get; set; }
        public string? UrlBig { get; set; }
    }
}