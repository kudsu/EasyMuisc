﻿using EasyMusic.Enum;
using EasyMusic.Helper;
using EasyMusic.Info;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControls.Dialog;
using static EasyMusic.GlobalDatas;
using static EasyMusic.Helper.MusicListHelper;
using static WpfControls.Dialog.DialogHelper;
using static Un4seen.Bass.Bass;
using Un4seen.Bass;
using System.Windows.Interop;

namespace EasyMusic.Helper
{
    public static class MusicControlHelper
    {

        private static BassModuleHelper bassInstance;
        public static BassModuleHelper Music
        {
            get
            {
                return bassInstance;
            }
            set
            {
                bassInstance?.Dispose();
                bassInstance = value;
            }
        }
        public static int Device
        {
            get =>BASS_GetDevice();
            set
            {
                var devices = BASS_GetDeviceInfos();
                if (value < devices.Length)
                {
                    var device = devices[value];
                    if (device.status.HasFlag(BASSDeviceInfo.BASS_DEVICE_ENABLED) && !device.status.HasFlag(BASSDeviceInfo.BASS_DEVICE_INIT))
                    {
                        BASS_Init(value, Setting.SampleRate, BASSInit.BASS_DEVICE_DEFAULT, MainWindow.Current.Handle);
                    }

                    if (BASS_SetDevice(value))
                    {
                        if (Music != null)
                        {
                            Music.Device = value;
                        }
                    }
                    else
                    {
                        ShowError("设置总输出设备错误："+BASS_ErrorGetCode().ToString());
                    }
                }

            }
        }

        static MusicControlHelper()
        {
            CycleMode = (CycleMode)Setting.CycleMode;
        }

        public static CycleMode CycleMode { get; set; }

        public static double Volumn
        {
            get => Setting.Volumn;
            set
            {
                Setting.Volumn = value;
                if (Music != null)
                {
                    Music.Volumn = value;
                }
                MainWindow.Current.controlBar.Notify("SliderVolumnBinding");
            }
        }
        public static void PlayOrPlayNew()
        {
            if (Music == null)
            {
                if (MusicCount != 0)
                {
                    PlayNew(MusicDatas.First());
                    return;
                }
            }

            Music?.Play();
        }
        /// <summary>
        /// 播放新的歌曲
        /// </summary>
        /// <param name="index">指定列表中的歌曲索引</param>
        /// <returns></returns>
        public static void PlayNew(MusicInfo music, bool playAtOnce = true)
        {
            Music = new BassModuleHelper(music);
            Music.Initialiaze();
            if (playAtOnce)
            {
                Music.Play();
            }
        }


        public static void PlayLast()
        {

            if (HistoryCount == 0)
            {
                PlayListLast();
            }
            else
            {
                CurrentHistoryIndex--;
                if (CurrentHistoryIndex != -1)
                {
                    PlayNew(GetHistory(CurrentHistoryIndex));
                }
            }
        }

        public static void PlayNext()
        {
            if (CurrentHistoryIndex == HistoryCount - 1)
            {
                PlayListNext();
            }
            else
            {
                PlayNew(GetHistory(++CurrentHistoryIndex));
            }
        }

        /// <summary>
        /// 根据不同的播放循环模式播放下一首
        /// </summary>
        public static void PlayListNext()
        {
            switch (CycleMode)
            {
                case CycleMode.ListCycle:
                    if (MusicCount > 1)
                    {if(Music!=null)
                        {
                            if (MusicDatas.Last() == Music.Info)
                            {
                                PlayNew(MusicDatas.First());
                            }
                            else
                            {
                                PlayNew(MusicDatas[MusicDatas.IndexOf(Music.Info) + 1]);
                            }
                        }
                        else
                        {
                            PlayNew(MusicDatas[0]);
                        }
                       
                    }
                    else
                    {
                        Music.PlayAgain();
                    }
                    break;
                case CycleMode.Shuffle:
                    if (MusicCount == 1)
                    {
                        Music.PlayAgain();
                        break;
                    }
                    int index;
                    Random r = new Random();
                    MusicInfo music = null;
                    do
                    {
                        index = r.Next(0, MusicCount);
                        music = MusicDatas[index];
                    }
                    while (music ==((Music==null)?null: Music.Info));
                    PlayNew(music);
                    break;
                case CycleMode.SingleCycle:
                    Music.PlayAgain();
                    break;
            }
        }
        public static void PlayListLast()
        {
            if (MusicCount > 1)
            {
                if (MusicDatas.First() == Music.Info)
                {
                    PlayNew(MusicDatas.Last());
                }
                else
                {
                    PlayNew(MusicDatas[MusicDatas.IndexOf(Music.Info) - 1]);
                }
            }
            else
            {
                Music.PlayAgain();
            }

        }

    }
}
