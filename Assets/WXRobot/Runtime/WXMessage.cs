using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WXRobot.Runtime
{
    public abstract class WxMessage
    {
        public abstract WxMessageType MessageType { get; }
        public string msgtype;

        private string ToJson()
        {
            msgtype = MessageType.ToString().ToLower();
            return JsonUtility.ToJson(this);
        }

        public async Task<string> PostAsync(string url)
        {
            var client = new HttpClient();
            var json = ToJson();
            Debug.Log($"post:{json}");
            var content = new StringContent(ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var result = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                Debug.LogError($"error:{response.StatusCode}");
            }

            return result;
        }
    }

    [Serializable]
    public class WxTextMessage : WxMessage
    {
        [Serializable]
        public class HttpMessage
        {
            public string content;
            public List<string> mentioned_list = new();
            public List<string> mentioned_mobile_list = new();
        }

        public override WxMessageType MessageType => WxMessageType.Text;
        public HttpMessage text = new();
    }

    [Serializable]
    public class WxMarkdownMessage : WxMessage
    {
        [Serializable]
        public class HttpMessage
        {
            public string content;
        }

        public override WxMessageType MessageType => WxMessageType.Markdown;
        public HttpMessage markdown = new();
    }

    [Serializable]
    public class WxImageMessage : WxMessage
    {
        [Serializable]
        public class HttpMessage
        {
            public string base64;
            public string md5;
        }

        public override WxMessageType MessageType => WxMessageType.Image;
        public HttpMessage image = new();

        public WxImageMessage(Texture2D texture)
        {
            var bytes = texture.EncodeToPNG();
            image.base64 = Utility.ToBase64String(bytes);
            image.md5 = Utility.Md5(bytes);
        }
    }

    [Serializable]
    public class WxNewsMessage : WxMessage
    {
        [Serializable]
        public class Article
        {
            public string title;
            public string description;
            public string url;
            public string picurl;
        }

        [Serializable]
        public class HttpMessage
        {
            public List<Article> articles = new();
        }

        public override WxMessageType MessageType => WxMessageType.News;
        public HttpMessage news = new();
    }
}