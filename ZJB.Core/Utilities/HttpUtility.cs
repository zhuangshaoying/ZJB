using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace ZJB.Core.Utilities
{
    public static class HttpUtility
    {
        private static readonly string boundary = "---------------------------PhOTopiCkER-mUlTiPaRtFoRm";
        private static readonly string defaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:16.0) Gecko/20100101 Firefox/16.0";

        public static string GetHtml(string url, NameValueCollection headers = null, bool gzip = false)
        {
            return GetHtml(url, Encoding.UTF8, headers, gzip);
        }

        public static string GetHtml(string url, Encoding encoding, NameValueCollection headers = null, bool gzip = false)
        {
            return encoding.GetString(GetData(url, headers, gzip));
        }

        public static byte[] GetData(string url, NameValueCollection headers = null, bool gzip = false)
        {

            using (WebClient client = GetWebClient(gzip))
            {
                AddHeaders(client.Headers, headers);
                return client.DownloadData(url);
            }

        }
        public static byte[] GetData(string url, NameValueCollection headers = null, bool gzip = false,IWebProxy proxy = null)
        {
            using (WebClient client = GetWebClient(gzip))
            {
                client.Proxy = proxy;
                AddHeaders(client.Headers, headers);
                return client.DownloadData(url);
            }
        }
        private static void AddHeaders(WebHeaderCollection headerCollection, NameValueCollection headers)
        {
            headerCollection.Set("User-Agent", defaultUserAgent);
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    headerCollection.Set(key, headers[key]);
                }
            }
        }

        private static WebClient GetWebClient(bool gzip)
        {
            return gzip ? new GzipWebClient() : new WebClient();
        }


        public static string PostHtml(string url, NameValueCollection formData, Encoding encoding, NameValueCollection headers = null)
        {
            return encoding.GetString(PostData(url, formData, headers));
        }

        public static string PostHtml(string url, NameValueCollection formData, NameValueCollection headers = null)
        {
            return PostHtml(url, formData, Encoding.UTF8, headers);
        }

        public static HttpStatusCode GetHttpStatus(string url, int timeout, string userAgent = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            request.UserAgent = string.IsNullOrEmpty(userAgent) ? defaultUserAgent : userAgent;
            request.Timeout = timeout;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            HttpStatusCode statusCode = response.StatusCode;
            response.Close();
            return statusCode;

        }

        public static string UploadFile(string url, NameValueCollection form, PostFile file, string userAgent = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            request.ContentType = "multipart/form-data; boundary=" + boundary;

            request.UserAgent = string.IsNullOrEmpty(userAgent) ? defaultUserAgent : userAgent;

            using (Stream stream = request.GetRequestStream())
            {
                foreach (string key in form.AllKeys)
                {
                    byte[] formData =
                        Encoding.UTF8.GetBytes(
                            string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; \r\n\r\n{2}\r\n", boundary,
                                          key, form[key]));

                    stream.Write(formData, 0, formData.Length);
                }

                byte[] fileInfo = Encoding.UTF8.GetBytes(string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                    boundary, file.FormName, file.FileName, file.ContentType
                    ));
                stream.Write(fileInfo, 0, fileInfo.Length);
                stream.Write(file.Data, 0, file.Data.Length);

                byte[] end =
                        Encoding.UTF8.GetBytes(
                            string.Format("\r\n--{0}--\r\n", boundary));

                stream.Write(end, 0, end.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string PostString(string url, string formData, NameValueCollection headers = null,
         IWebProxy proxy = null)
        {
            return PostData(url, formData, headers, proxy: proxy);
        }

        public static string PostString(string url, NameValueCollection formData, NameValueCollection headers = null,
         IWebProxy proxy = null)
        {
            return PostString(url, formData, Encoding.UTF8, headers, proxy);
        }
        public static string PostString(string url, NameValueCollection formData, Encoding encoding,
              NameValueCollection headers = null, IWebProxy proxy = null)
        {
            return encoding.GetString(PostData(url, formData, headers, proxy: proxy));
        }
        public static byte[] PostData(string url, NameValueCollection formData, NameValueCollection headers = null,
           bool gzip = false, IWebProxy proxy = null)
        {
            using (WebClient client = GetWebClient(gzip))
            {
                client.Proxy = proxy;
                AddHeaders(client.Headers, headers);
                return client.UploadValues(url, "POST", formData);
            }
        }

        public static string PostData(string url, string formData, NameValueCollection headers = null, bool gzip = false,IWebProxy proxy = null)
        {
            using (WebClient client = GetWebClient(gzip))
            {
                client.Proxy = proxy;
                AddHeaders(client.Headers,
                    new NameValueCollection { { "Content-Type", "application/x-www-form-urlencoded; charset=UTF-8" } });
                AddHeaders(client.Headers, headers);
                return client.UploadString(url, "POST", formData);
            }
        }

        public static string GetString(string url, NameValueCollection headers = null, bool gzip = false,IWebProxy proxy = null)
        {
            return GetString(url, Encoding.UTF8, headers, gzip, proxy);
        }

        public static string GetString(string url, Encoding encoding, NameValueCollection headers = null,bool gzip = false, IWebProxy proxy = null)
        {
            return encoding.GetString(GetData(url, headers, gzip, proxy));
        }

    }

    public class PostFile
    {
        public string FormName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }


    [System.ComponentModel.DesignerCategory("")]
    internal class GzipWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            return request;
        }
    }

  
}