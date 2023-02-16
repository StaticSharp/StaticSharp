using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace StaticSharp.VideoUtils
{
    public class VideoFormat
    {
        public int FormatCode { get; init; }
        
        public string VideoPageUrl {get; init; }

        

        public bool HasAudio { get; init; }

        public bool HasVideo { get; init; }

        public int? Width { get; init; }
        
        public int? Height { get; init; }

        public string Extension { get; init; }
    }
}
