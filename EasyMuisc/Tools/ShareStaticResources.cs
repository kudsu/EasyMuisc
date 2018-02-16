﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Un4seen.Bass;

namespace EasyMuisc
{
    public static class ShareStaticResources
    {
        /// <summary>
        /// 音乐播放句柄
        /// </summary>
        public static int stream = 0;

        public static int Pitch
        {
            get
            {
                float value = 0;
                Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, ref value);
                return (int)value;
            }
            set
            {
                if (value < -50)
                {
                    value = -50;
                }
                if (value > 50)
                {
                    value = 50;
                }
                Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, value);

            }
        }
        public static int Tempo
        {
            get
            {
                float value = 0;
                Bass.BASS_ChannelGetAttribute(stream, BASSAttribute.BASS_ATTRIB_TEMPO, ref value);
                return (int)(value*100);
            }
            set
            {
                if (value < -95)
                {
                    value = -95;
                }
                if (value > 200)
                {
                    value = 200;
                }
                Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_TEMPO, ((float)value));

            }
        }
        public static  Properties.Settings set = new Properties.Settings();

        public static IntPtr windowHandle;
    }
}