using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WXRobot.Runtime
{
    /// <summary>
    /// https://developer.work.weixin.qq.com/document/path/91770
    /// </summary>
    public abstract class WxMessage
    {
        protected abstract WxMessageType MessageType { get; }
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
        private class MessageInfo
        {
            public string content;
            public List<string> mentioned_list = new();
            public List<string> mentioned_mobile_list = new();
        }

        protected override WxMessageType MessageType => WxMessageType.Text;

        [SerializeField] private MessageInfo text = new();

        public WxTextMessage(string content)
        {
            text.content = content;
        }

        public void AddMentionedUser(string userid)
        {
            text.mentioned_list.Add(userid);
        }

        public void AddMentionedUsers(string[] users)
        {
            text.mentioned_list.AddRange(users);
        }

        public void AddMentionedMobile(string mobile)
        {
            text.mentioned_mobile_list.Add(mobile);
        }

        public void AddMentionedMobiles(string[] mobiles)
        {
            text.mentioned_mobile_list.AddRange(mobiles);
        }
    }

    [Serializable]
    public class WxMarkdownMessage : WxMessage
    {
        [Serializable]
        private class MessageInfo
        {
            public string content;
        }

        protected override WxMessageType MessageType => WxMessageType.Markdown;
        [SerializeField] private MessageInfo markdown = new();

        public WxMarkdownMessage(string content)
        {
            markdown.content = content;
        }
    }

    [Serializable]
    public class WxImageMessage : WxMessage
    {
        [Serializable]
        private class MessageInfo
        {
            public string base64;
            public string md5;
        }

        protected override WxMessageType MessageType => WxMessageType.Image;
        [SerializeField] private MessageInfo image = new();

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
        private class MessageInfo
        {
            public List<Article> articles = new();
        }

        [Serializable]
        public class Article
        {
            public string title;
            public string description;
            public string url;
            public string picurl;
        }

        protected override WxMessageType MessageType => WxMessageType.News;
        [SerializeField] private MessageInfo news = new();

        public void AddArticle(Article article)
        {
            news.articles.Add(article);
        }
    }
}