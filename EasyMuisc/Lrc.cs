﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyMuisc
{
    public class Lrc
    {
        /// <summary>
        /// 歌曲
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// 歌词作者
        /// </summary>
        public string LrcBy { get; set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        public string Offset { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public Dictionary<double, string> LrcContent { get; set; }

        /// <summary>
        /// 获得歌词信息
        /// </summary>
        /// <param name="LrcPath">歌词路径</param>
        /// <returns>返回歌词信息(Lrc实例)</returns>
        public Lrc(string LrcPath)
        {
            Regex regex = new Regex(@"(?<time>\[[0-9.:\]\[\s]*\])(?<value>.*)", RegexOptions.Compiled);
            Regex timeRegex = new Regex(@"\[(?<time>[0-9.:]*)\]\s*", RegexOptions.Compiled);

            LrcContent = new Dictionary<double, string>();
            var tempDic = new Dictionary<double, string>();
            using (FileStream fs = new FileStream(LrcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                string line;
                using (StreamReader sr = new StreamReader(fs, EncodingType.GetType(LrcPath)))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("[ti:"))
                        {
                            Title = SplitInfo(line);
                        }
                        else if (line.StartsWith("[ar:"))
                        {
                            Artist = SplitInfo(line);
                        }
                        else if (line.StartsWith("[al:"))
                        {
                            Album = SplitInfo(line);
                        }
                        else if (line.StartsWith("[by:"))
                        {
                            LrcBy = SplitInfo(line);
                        }
                        else if (line.StartsWith("[offset:"))
                        {
                            Offset = SplitInfo(line);
                        }
                        else
                        {
                            try
                            {
                                Match match = regex.Match(line);//分割时间和内容
                                string word = match.Groups["value"].Value;
                                MatchCollection timeMatch = timeRegex.Matches(match.Groups["time"].Value);//分割多个时间
                                foreach (var i in timeMatch)
                                {
                                    double time = TimeSpan.Parse("00:" + (i as Match).Groups["time"].Value).TotalSeconds;
                                    tempDic.Add(time, word);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
            LrcContent = tempDic.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);

        }

        /// <summary>
        /// 处理信息
        /// </summary>
        /// <param name="line"></param>
        /// <returns>返回基础信息</returns>
        static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }
    }
}
