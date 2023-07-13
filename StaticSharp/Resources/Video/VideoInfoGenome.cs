using FFMpegCore.Arguments;
using FFMpegCore.Pipes;
using FFMpegCore;

using StaticSharp.Gears;

using FFMpegCore.Exceptions;
using FFMpegCore.Helpers;
using System.Diagnostics;
using System.Text.Json;



namespace StaticSharp {



    public record VideoInfoGenome(Genome<IAsset> Source) : Genome<FFProbeAnalysis> {

        public class Data {
            public string SourceHash = null!;
        }


        protected override void Create(out FFProbeAnalysis value, out Func<bool>? verify) {

            Data data;
            var source = Source.Result;
            var slot = Cache.GetSlot(Key);
            string json;
            if (slot.LoadData(out data) && data.SourceHash == source.ContentHash) {
                json = slot.LoadContentText();
            } else {
                FFMpegCore.GlobalFFOptions.Configure(x => {
                    x.BinaryFolder = FFMpegInstaller.Discover.InstallationDirectory;
                });

                MemoryStream memoryStream = new MemoryStream(Source.Result.Data);

                data = new() {
                    SourceHash = source.ContentHash,
                };

                var lines = AnalyseAsync(memoryStream).Result;
                json = string.Join(string.Empty, lines);
                slot.StoreData(data).StoreContentText(json);
            }

            value = JsonSerializer.Deserialize<FFProbeAnalysis>(json, new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            });

            verify = () => {
                return data.SourceHash == Source.Result.ContentHash;
            };

        }














        private static void ThrowIfExitCodeNotZero(Instances.IProcessResult result) {
            if (result.ExitCode != 0) {
                string message = string.Format("ffprobe exited with non-zero exit-code ({0} - {1})", result.ExitCode, string.Join("\n", result.ErrorData));
                throw new FFMpegException(FFMpegExceptionType.Process, message, null, string.Join("\n", result.ErrorData));
            }
        }
        private static Instances.ProcessArguments PrepareStreamAnalysisInstance(string filePath, FFOptions ffOptions) {
            return PrepareInstance("-loglevel error -print_format json -show_format -sexagesimal -show_streams \"" + filePath + "\"", ffOptions);
        }

        public static async Task<IReadOnlyList<string>> AnalyseAsync(Stream stream, FFOptions? ffOptions = null, CancellationToken cancellationToken = default(CancellationToken)) {
            StreamPipeSource writer = new StreamPipeSource(stream);
            InputPipeArgument pipeArgument = new InputPipeArgument(writer);
            Instances.ProcessArguments processArguments = PrepareStreamAnalysisInstance(pipeArgument.PipePath, ffOptions ?? GlobalFFOptions.Current);
            pipeArgument.Pre();
            Task<Instances.IProcessResult> task = processArguments.StartAndWaitForExitAsync(cancellationToken);
            try {
                await pipeArgument.During(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (IOException) {
            }
            finally {
                pipeArgument.Post();
            }

            Instances.IProcessResult obj = await task.ConfigureAwait(continueOnCapturedContext: false);
            ThrowIfExitCodeNotZero(obj);
            pipeArgument.Post();
            return obj.OutputData;
        }

        private static Instances.ProcessArguments PrepareInstance(string arguments, FFOptions ffOptions) {
            FFProbeHelper.RootExceptionCheck();
            FFProbeHelper.VerifyFFProbeExists(ffOptions);
            return new Instances.ProcessArguments(new ProcessStartInfo(GlobalFFOptions.GetFFProbeBinaryPath(ffOptions), arguments) {
                StandardOutputEncoding = ffOptions.Encoding,
                StandardErrorEncoding = ffOptions.Encoding,
                WorkingDirectory = ffOptions.WorkingDirectory
            });
        }




    }



}

