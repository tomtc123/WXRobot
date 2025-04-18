using System.Threading.Tasks;
using UnityEngine;

namespace WXRobot.Runtime
{
    public static class WxRobotHelper
    {
        public static void PostText(string url, string content, params string[] mobiles)
        {
            var message = new WxTextMessage(content);
            if (mobiles != null)
            {
                message.AddMentionedMobiles(mobiles);
            }

            Task.Run(async () =>
            {
                var result = await message.PostAsync(url);
                Debug.Log(result);
            });
        }

        public static void PostMarkdown(string url, string content)
        {
            var message = new WxMarkdownMessage(content);
            Task.Run(async () =>
            {
                var result = await message.PostAsync(url);
                Debug.Log(result);
            });
        }

        public static void PostImage(string url, Texture2D texture)
        {
            var message = new WxImageMessage(texture);
            Task.Run(async () =>
            {
                var result = await message.PostAsync(url);
                Debug.Log(result);
            });
        }
    }
}