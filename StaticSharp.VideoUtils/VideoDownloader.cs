using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StaticSharp.VideoUtils
{
    public class VideoDownloader // TODO: make sigleton and consider common interface with YoutubeExplode
    {
        public const string YoutubeDlExePath = "./BinaryDependencies/youtube-dl.exe";

        public async Task<IList<VideoFormat>> GetVideoFormats(string videoPageUrl)
        {
            var outputBuilder = new StringBuilder();
            await CommandLineExecutor.ExecuteCommandAsync(
                YoutubeDlExePath, $"\"{videoPageUrl}\" -F", 
                async (line) => outputBuilder.AppendLine(line));

            var output = outputBuilder.ToString();
            var videoFormats = ParseVideoFormatsResuslt(output, videoPageUrl);
            return videoFormats;
        }

        public async Task DownloadAsync(string pageUrl, int formatCode, string outputFilePath, Action<object> onProgress = null)
        {
            var outputBuilder = new StringBuilder();
            await CommandLineExecutor.ExecuteCommandAsync(
                YoutubeDlExePath, $"\"{pageUrl}\" -f {formatCode} -o {outputFilePath}", 
                async (line) => await Task.CompletedTask/*outputBuilder.AppendLine(line)*/);
        }

        protected List<VideoFormat> ParseVideoFormatsResuslt(string rawResult, string videoPageUrl)
        {
            var result = new List<VideoFormat>();

            var error = "ERROR:";
            if (rawResult.Contains(error))
            {
                throw new Exception("Result contains error");
            }

            var tableHeader = "format code  extension  resolution note";

            var splitResult = rawResult.Split(tableHeader);
            if (splitResult.Length < 2) 
            {
                throw new Exception("Result does not contain list of available formats");
            }

            var formatsTable = splitResult[1];

            var formatRows = formatsTable.Split("\n").ToList();

            formatRows = formatRows.Where(_ => !string.IsNullOrWhiteSpace(_)).ToList();

            foreach (var row in formatRows)
            {
                ParseVideoFormatRow(row,
                    out int formatCode,
                    out string extension,
                    out int? width,
                    out int? height,
                    out bool hasAudio,
                    out bool hasVideo);


                var videoFormat = new VideoFormat
                {
                    VideoPageUrl = videoPageUrl,
                    FormatCode = formatCode,
                    Extension = extension,
                    Width = width,
                    Height = height,
                    HasAudio = hasAudio,
                    HasVideo = hasVideo
                };

                result.Add(videoFormat);
            }

           return result;
        }

        protected void ParseVideoFormatRow(string rowString, 
            out int formatCode,
            out string extension,
            out int? width,
            out int? height,
            out bool hasAudio,
            out bool hasVideo)
        {
            var rowRegex = new Regex("(?<id>\\d*)\\s*(?<extension>\\S*)\\s*(?<resolution>(audio only)|(\\S*))\\s(?<note>.*)");
            
            var match = rowRegex.Match(rowString);
            try
            {
                formatCode = int.Parse(match.Groups["id"].Value);
                extension = match.Groups["extension"].Value;
                   
                var resolution = match.Groups["resolution"].Value;
                var note = match.Groups["note"].Value;

                if (resolution == "audio only")
                {
                    hasVideo = false;
                    width = null;
                    height = null;
                }
                else
                {
                    hasVideo = true;
                    var resolutionSplit = resolution.Split('x');
                    width = int.Parse(resolutionSplit[0]);
                    height = int.Parse(resolutionSplit[1]);
                }

                hasAudio = !note.Contains("video only");
            }
            catch
            {
                throw new Exception("Failed to parse video formats table");
            }
        }
    }
}
