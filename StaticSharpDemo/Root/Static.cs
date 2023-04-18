using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StaticSharpDemo {
    public static partial class Static {
        

        

        public static Inline GithubUrl(string text = "GitHub repository") {
            return new Inline {
                ExternalLink = "https://github.com/StaticSharp/StaticSharp",
                OpenLinksInANewTab = true,
                ForegroundColor = Color.FromIntChannelsRGB(172, 196, 53),
                Children = {
                    text
                }
            };
        }

        /*public Inline DiscordUrl(string text = "Discord server") {
            return new Inline {
                ExternalLink = "https://discord.gg/ZTqmfPsGEr",
                OpenLinksInANewTab = true,
                ForegroundColor = Color.FromIntRGB(139, 148, 245),
                Children = {
                    text
                }
            };
        }*/




    }

}
