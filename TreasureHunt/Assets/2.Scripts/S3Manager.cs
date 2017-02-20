﻿using UnityEngine;
using System.Collections;
using Amazon;
using Amazon.CognitoIdentity;
using System.IO;
using Amazon.S3.Model;
using Amazon.S3;
using UnityEngine.UI;

public class S3Manager : MonoBehaviour {
    public static S3Manager instance = null;
    public string url = "https://s3.ap-northeast-2.amazonaws.com/treasures1/";
    public Image img;
    public string IDENTITY_POOL_ID = "";
    public string ANALYTICS_APP_ID = "";
    public RegionEndpoint ENDPOINT = RegionEndpoint.APNortheast2;
    public Texture2D testTex;
    private CognitoAWSCredentials credentials;
    private AmazonS3Client s3Client;
    void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);
    }

    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        ConnectAtCognito();
        ConnectToS3();
        //GetImageFromS3("cute");
    }
    public void GetImageFromS3(string fileName)
    {
        StartCoroutine(GetImage(fileName));
    }
    IEnumerator GetImage(string fileName)
    {
        url += fileName;
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return (www);
        Texture2D tex = www.texture;
        //TextureScale.Bilinear(tex, 300, 300);
        //var newTex = Instantiate(tex);
        //TextureScale.Bilinear(newTex, tex.width * 2, tex.height * 2);
        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
    

    public void ConnectAtCognito()
    {
        // a Credentials provider that uses Cognito Identity
        credentials = new CognitoAWSCredentials(IDENTITY_POOL_ID, ENDPOINT);
    }

    void ConnectToS3()
    {
        s3Client = new AmazonS3Client(credentials, ENDPOINT);
        //PostObject(testTex, "test");        
    }
    public void PostObject(Texture2D tex, string fileName)
    {
        //ResultText.text = "Retrieving the file";
        byte[] image = tex.EncodeToPNG();
        var stream = new MemoryStream(image);
        //ResultText.text += "\nCreating request object";
        var request = new PostObjectRequest()
        {
            Bucket = "treasures1",
            Key = fileName,
            InputStream = stream,
            CannedACL = S3CannedACL.PublicReadWrite
        };

        //ResultText.text += "\nMaking HTTP post call";

        s3Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                //ResultText.text += string.Format("\nobject {0} posted to bucket {1}",
                // responseObj.Request.Key, responseObj.Request.Bucket);
                Debug.Log("fail");
            }
            else
            {
                //ResultText.text += "\nException while posting the result object";
                //ResultText.text += string.Format("\n receieved error {0}",
                //responseObj.Response.HttpStatusCode.ToString());
                Debug.Log("success");
            }
        });
    }   
}