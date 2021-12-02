using System;
using System.Collections.Generic;
using System.Text;

namespace StaticSharpEngine {
    public static class StringUtils {

        /// <summary>
        /// Convert size in bytes to human-readable string
        /// </summary>
        /// <param name="size">Size in bytes</param>
        /// <returns></returns>
        public static string HumanizeSize(long byteCount) {
            string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suffix[0];
            long byteCountAbs = Math.Abs(byteCount);
            int order = Convert.ToInt32(Math.Floor(Math.Log(byteCountAbs, 1024)));
            double num = Math.Round(byteCountAbs / Math.Pow(1024, order), 1);
            return string.Format("{0:0.#}", Math.Sign(byteCount) * num) + suffix[order];
        }
    }
}
