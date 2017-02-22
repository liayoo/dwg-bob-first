using System;
using System.IO;
using System.Net;   // WebRequest
using System.Collections;
using System.Security.Cryptography;

using Vuforia;
using JsonFx.Json;
using UnityEngine;
using UnityEngine.UI;

public class PostNewTrackableRequest
{
    public string name;
    public double width;
    public string image;
    public string application_metadata;
}

namespace IA.Plugin
{
    public class CloudUpLoadManager : MonoBehaviour
    {
        public Texture2D texture;

        // server access keys.
		static string access_key = "df72ea81a32af560a1949b00a34ea1b58bbdc6bc";
		static string secret_key = "5796fd7e0ff090d666862bc6256e8b46c77dca9d";
        static string url = @"https://vws.vuforia.com";
        static string targetName = "MyTarget"; // must change when upload another Image Target, avoid same as exist Image on cloud
        private byte[] requestBytesArray;

        public void CallPostTarget(Texture2D texture, string targetName)
        {
            Debug.Log("CallPostTarget");
            StartCoroutine(PostNewTarget(texture, targetName));
        }

        /// Makes the post request to upload a new image target to the cloud database
        public static IEnumerator PostNewTarget(Texture2D texture, string targetName)
        {
            Debug.Log("<color=red>Posting target: " + targetName + "</color>");
            string requestPath = "/targets";
            string serviceURI = url + requestPath;
            string httpAction = "POST";
            string contentType = "application/json";
            string date = string.Format("{0:r}", DateTime.Now.ToUniversalTime());

            // if your texture2d has RGb24 type, don't need to redraw new texture2d
            Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
            tex.SetPixels(texture.GetPixels());
            tex.Apply();
            byte[] image = tex.EncodeToPNG();

            string metadataStr = "Vuforia metadata";
            byte[] metadata = System.Text.ASCIIEncoding.ASCII.GetBytes(metadataStr);
            PostNewTrackableRequest model = new PostNewTrackableRequest();
            model.name = targetName;
            model.width = 100.0f;
            model.image = System.Convert.ToBase64String(image);

            model.application_metadata = System.Convert.ToBase64String(metadata);
            string requestBody = JsonWriter.Serialize(model);

            WWWForm form = new WWWForm();

            // set post request's headers
            var headers = form.headers;
            byte[] rawData = form.data;
            headers["Host"] = url;
            headers["Date"] = date;
            headers["Content-Type"] = contentType;

            HttpWebRequest httpWReq = (HttpWebRequest)HttpWebRequest.Create(serviceURI);

            MD5 md5 = MD5.Create();
            var contentMD5bytes = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(requestBody));
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < contentMD5bytes.Length; i++)
            {
                sb.Append(contentMD5bytes[i].ToString("x2"));
            }

            string contentMD5 = sb.ToString();
            string stringToSign = string.Format("{0}\n{1}\n{2}\n{3}\n{4}", httpAction, contentMD5, contentType, date, requestPath);

            // set 'Authorization' field of header        
            HMACSHA1 sha1 = new HMACSHA1(System.Text.Encoding.ASCII.GetBytes(secret_key));
            byte[] sha1Bytes = System.Text.Encoding.ASCII.GetBytes(stringToSign);
            MemoryStream stream = new MemoryStream(sha1Bytes);
            byte[] sha1Hash = sha1.ComputeHash(stream);
            string signature = System.Convert.ToBase64String(sha1Hash);

            headers["Authorization"] = string.Format("VWS {0}:{1}", access_key, signature);
            Debug.Log("Signature: " + signature);

            WWW request = new WWW(serviceURI, System.Text.Encoding.UTF8.GetBytes(JsonWriter.Serialize(model)), headers);
			TargetImageController.instance.targetImage = targetName;
            yield return request;

            if (request.error != null)
            {
                Debug.Log("<color=red>REQUEST ERROR: " + request.error + "</color>");
            }
            else
            {
                Debug.Log("<color=green>REQUEST SUCCESS:" + request.text + "</color>");
            }
            
            MakerSceneManager.instance.callback = true;
        }
    }
}