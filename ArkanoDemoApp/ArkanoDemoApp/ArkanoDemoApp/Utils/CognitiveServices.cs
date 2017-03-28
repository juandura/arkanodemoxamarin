using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArkanoDemoApp.Utils
{
    public static class CognitiveServices
    {
        #region Public Methods

        public static async Task<List<FaceDetectResult>> FaceDetect(MediaFile mediaFileResult)
        {
            //TODO: cambiar para usar httpclient.

            Stream photoStream = mediaFileResult.GetStream();
            mediaFileResult.Dispose();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.projectoxford.ai/face/v1.0/detect");
            request.ContentType = "application/octet-stream";
            request.Method = "POST";
            request.Headers["Ocp-Apim-Subscription-Key"] = Constants.CognitiveServicesKey;
            byte[] bytearr = ReadFully(photoStream);
            if (bytearr != null)
            {
                Stream requestStream = await request.GetRequestStreamAsync();
                requestStream.Write(bytearr, 0, bytearr.Length);
            }
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string resultResponse = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                resultResponse = reader.ReadToEnd();
            }
            List<FaceDetectResult> faceDetectResults = await DeserializeObjectAsync<List<FaceDetectResult>>(resultResponse);
            return faceDetectResults;
        }

        public static async Task<List<FaceIdentifyResult>> FaceIdentify(string personGroupId, string faceId)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                FaceIdentifyBody faceIdentifyBody = new FaceIdentifyBody { personGroupId = personGroupId, faceIds = new List<string> { faceId }, confidenceThreshold = 0.5, maxNumOfCandidatesReturned = 1 };
                string jsonBody = await SerializeObjectAsync<FaceIdentifyBody>(faceIdentifyBody);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.CognitiveServicesKey);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                var uri = "https://api.projectoxford.ai/face/v1.0/identify";
                var result = await client.PostAsync(uri, content);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string jsonErrorResult = await result.Content.ReadAsStringAsync();
                    return null;
                }
                string jsonResult = await result.Content.ReadAsStringAsync();
                return await DeserializeObjectAsync<List<FaceIdentifyResult>>(jsonResult);
            }
        }

        public static async Task<PersonResult> GetPerson(string personGroupId, string personId, double confidence)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.CognitiveServicesKey);
                var uri = string.Format("https://api.projectoxford.ai/face/v1.0/persongroups/{0}/persons/{1}", personGroupId, personId);
                var result = await client.GetAsync(uri);
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return null;
                }
                string jsonResult = await result.Content.ReadAsStringAsync();
                PersonResult personResult = await DeserializeObjectAsync<PersonResult>(jsonResult);
                personResult.confidence = confidence;
                return personResult;
            }
        }

        #endregion Public Methods

        #region Private Methods

        //TODO: eliminar el metodo.
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private static Task<string> SerializeObjectAsync<T>(T data)
        {
            return Task.Run(() => JsonConvert.SerializeObject(data));
        }

        private static Task<T> DeserializeObjectAsync<T>(string value)
        {
            return Task.Run(() => JsonConvert.DeserializeObject<T>(value));
        }

        #endregion Private Methods

        #region Sub Types

        public class FaceDetectResult
        {
            public string faceId { get; set; }
        }

        public class FaceIdentifyBody
        {
            public string personGroupId { get; set; }
            public List<string> faceIds { get; set; }
            public int maxNumOfCandidatesReturned { get; set; }
            public double confidenceThreshold { get; set; }
        }

        public class FaceIdentifyResult
        {
            public string faceId { get; set; }
            public List<Candidate> candidates { get; set; }
            public class Candidate
            {
                public string personId { get; set; }
                public double confidence { get; set; }
            }
        }

        public class PersonResult
        {
            public string personId { get; set; }
            public List<string> persistedFaceIds { get; set; }
            public string name { get; set; }
            public string userData { get; set; }
            public double confidence { get; set; }
        }

        #endregion Sub Types
    }
}
