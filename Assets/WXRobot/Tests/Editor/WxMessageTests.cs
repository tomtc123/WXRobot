using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using WXRobot.Runtime;

namespace WXRobot.Tests.Editor
{
    public class WxMessageTests
    {
        private const string WebHookUrl =
            "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=693axxx6-7aoc-4bc4-97a0-0ec2sifa5aaa";

        [Test]
        public void PostPlainText()
        {
            WxRobotHelper.PostText(WebHookUrl, "Hello Robot!", "@all");
        }

        [Test]
        public void PostMarkdown()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<font color=\"red\">颜色文本</font>");
            sb.AppendLine("<font color=\"green\">颜色文本</font>");
            sb.AppendLine("<font color=\"blue\">颜色文本</font>");
            sb.AppendLine("<font color=\"info\">企业微信内置颜色</font>");
            sb.AppendLine("<font color=\"comment\">企业微信内置颜色</font>");
            sb.AppendLine("<font color=\"warning\">企业微信内置颜色</font>");
            WxRobotHelper.PostMarkdown(WebHookUrl, sb.ToString());
        }

        [Test]
        public void PostImage()
        {
            const int width = 128;
            const int height = 128;
            var r = 1f / width;
            var g = 1f / height;
            var colors = new List<Color>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    colors.Add(new Color(x * r, y * g, 0f));
                }
            }

            var texture = new Texture2D(width, height);
            texture.SetPixels(colors.ToArray());
            texture.Apply();
            File.WriteAllBytes($"{Application.dataPath}/test.png", texture.EncodeToPNG());
            WxRobotHelper.PostImage(WebHookUrl, texture);
        }
    }
}