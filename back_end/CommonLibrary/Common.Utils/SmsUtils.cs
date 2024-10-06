using log4net;
using Newtonsoft.Json;
using System.Net;

namespace Common.Utils;

public static class SmsUtils
{
    public static readonly ILog log = LogManager.GetLogger(typeof(SmsUtils));
    public static bool SendOTPToPhone(string phoneNo, string otpCode, string smsToken, string smsServiceUrl)
    {
        //string messageVI = $"{otpCode} la ma xac minh Cabin book. Anh/chi khong chia se ma nay voi bat ky ai."; //Vna va mobi ko su dung dc
        string messageVI = $"NP Digital: {otpCode} la ma xac minh Cabin book. Anh/chi khong chia se ma nay voi bat ky ai.";
        string messageEN = $"NP Digital: Your Cabin book Verification Code is {otpCode}. Don't share it with anyone.";

        Random random = new Random(2022);

        try
        {
            //Set not require http
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var ret = CallApi($"{smsToken}", smsServiceUrl, "webapi/sendSMS", new
            {
                from = "NP Digital",
                to = $"{phoneNo}",
                text = random.Next(2000, 2022) % 2 == 0 ? messageVI : messageEN
            });

            if (ret.Item1 == HttpStatusCode.OK)
            {
                log.Info($"[SMSHelperAPI] Send to {phoneNo}, result: {ret.Item1}, {ret.Item2}");
                try
                {
                    var result = JsonConvert.DeserializeObject<ExternalBrandNameSMS>(ret.Item2.ToString());
                    return result.status == 1;
                }
                catch (Exception ex)
                {
                    log.Error($"SMS - Brand Name, Parse result error", ex);
                }
                return true;
            }
            else
            {
                log.Error($"[SMSHelperAPI] Send to {phoneNo}, result: {ret.Item1}, {ret.Item2}");
            }
        }
        catch (Exception ex)
        {
            log.Error($"SMS - Brand Name", ex);
        }
        return false;
    }

    public static Tuple<HttpStatusCode, object> CallApi(string authorization, string urlAPI, string apiAction, object requestBody)
    {
        using (HttpClient _client = new HttpClient())
        {
            CreateBaseRequestClient(_client, urlAPI, "application/json", authorization);
            HttpResponseMessage response = HttpClientExtensions.PostAsJsonAsync(_client, apiAction, requestBody).Result;
            return CheckResponseStatus(response);
        }
    }

    private static void CreateBaseRequestClient(HttpClient _client, string urlAPI, string headerType, string authorization)
    {
        _client.BaseAddress = new Uri(urlAPI);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(headerType));
        if (!string.IsNullOrWhiteSpace(authorization)) _client.DefaultRequestHeaders.Add("Authorization", authorization);
        if (System.Diagnostics.Debugger.IsAttached)
        {
            _client.Timeout = TimeSpan.FromSeconds(600);
        }
        else
        {
            _client.Timeout = TimeSpan.FromSeconds(120);
        }
    }

    private static Tuple<HttpStatusCode, object> CheckResponseStatus(HttpResponseMessage response)
    {
        var result = response.Content.ReadAsStringAsync().Result;
        return Tuple.Create<HttpStatusCode, object>(response.StatusCode, result);
    }
}

public class ExternalBrandNameSMS
{
    public int status { get; set; }
    public int mnp { get; set; }
    public string carrier { get; set; }
}