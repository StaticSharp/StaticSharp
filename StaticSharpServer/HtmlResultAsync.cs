using System.Net.Mime;
using System.Text;

namespace StaticSharp.Gears {
    class ResultAsync : IResult {
        private readonly Func<Task<IResult>> ResultGenerator;

        public ResultAsync(Func<Task<IResult>> resultGenerator) {
            ResultGenerator = resultGenerator;
        }

        public async Task ExecuteAsync(HttpContext httpContext) {
            await (await ResultGenerator()).ExecuteAsync(httpContext);
        }
    }
}
