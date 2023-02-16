namespace StaticSharp.VideoUtils.Test
{
    [TestClass]
    public class VideoDownloaderTests
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var vd = new VideoDownloader();
            var result = await vd.GetVideoFormats("https://www.youtube.com/watch?v=5oMDAfUpWyQ");
            Assert.IsNotNull(result);
        }
    }
}