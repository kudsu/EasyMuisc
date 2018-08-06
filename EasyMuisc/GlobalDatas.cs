﻿using EasyMusic.Info;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Un4seen.Bass;

namespace EasyMusic
{
    public static class GlobalDatas
    {
        /// <summary>
        /// 支持的格式
        /// </summary>
        public static string[] supportExtension = { ".mp3", ".MP3", ".wav", ".WAV" };
        /// <summary>
        /// 支持的格式，过滤器格式
        /// </summary>
        public static string supportExtensionWithSplit;
        public static Properties.Settings Setting { get; } = new Properties.Settings();

        public static IntPtr windowHandle;
        /// <summary>
        /// 托盘图标
        /// </summary>
        public static WpfCodes.Program.TrayIcon trayIcon = null;
        /// <summary>
        /// 程序目录
        /// </summary>
        public static string programDirectory = new FileInfo(Process.GetCurrentProcess().MainModule.FileName).DirectoryName;
        public static ListenHistoryHelper listenHistory = new ListenHistoryHelper();

        public static string argPath = null;


        /// <summary>
        /// 创建并启动一个新的浮点数据类型动画
        /// </summary>
        /// <param name="obj">动画主体</param>
        /// <param name="property">更改的属性</param>
        /// <param name="to">目标值</param>
        /// <param name="duration">时间</param>
        /// <param name="decelerationRatio">减缓时间</param>
        /// <param name="completed">完成以后的事件</param>
        /// <returns></returns>
        public static Storyboard NewDoubleAnimation(FrameworkElement obj, DependencyProperty property, double to, double duration, double decelerationRatio = 0, EventHandler completed = null, bool stopAfterComplete = false, EasingFunctionBase easingFunction = null)
        {
            DoubleAnimation ani = new DoubleAnimation
            {
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(duration)),//动画时间1秒
                DecelerationRatio = decelerationRatio,
                FillBehavior = stopAfterComplete ? FillBehavior.Stop : FillBehavior.HoldEnd,
                EasingFunction = easingFunction,
            };


            Storyboard.SetTargetName(ani, obj.Name);
            Storyboard.SetTargetProperty(ani, new PropertyPath(property));
            Storyboard story = new Storyboard();
            //Debug.WriteLine(Timeline.GetDesiredFrameRate(story));

            story.Children.Add(ani);
            if (completed != null)
            {
                story.Completed += completed;
            }
            story.Begin(obj);
            return story;
        }
        /// <summary>
        /// 支持多个过滤器的枚举
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static string[] EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string i in searchPatterns)
            {
                files.AddRange(Directory.EnumerateFiles(path, i, searchOption));
            }
            return files.ToArray();
        }

        public static double ScreenHight => SystemParameters.PrimaryScreenHeight;
        public static double ScreenWidth => SystemParameters.PrimaryScreenWidth;
        public static int GetRandomNumber(int from, int smallerThan)
        {
            RNGCryptoServiceProvider r = new RNGCryptoServiceProvider();
            byte[] b = new byte[8];
            r.GetBytes(b);
            return (b[0] + b[1] + b[2] + b[3] + b[4] + b[5] + b[6] + b[7]) % (smallerThan - from) + from;
        }
    }
}