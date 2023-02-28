

using System.Runtime.CompilerServices;

namespace StaticSharp;

static partial class Static {
    public static double BorderRadius = 5;

    public static Block FacebookMessengerButton(string username, string text = "Facebook", int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {
     
        return new Paragraph(callerLineNumber, callerFilePath) {
            ExternalLink = $"https://m.me/{username}",
            //Width = new (e=>e.InternalWidth),
            OpenLinksInANewTab = true,
            TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            PaddingsHorizontal = 12,
            Inlines = {
                new SvgIconInline(SvgIcons.SimpleIcons.Messenger) {
                    MarginRight = 0.4,
                    //BaselineOffset = 0
                },
                text
            },
            Radius = BorderRadius,
            BackgroundColor = new Color("1a6ed8")
        };

    }

    public static Block TelegramButton(string username, string text = "Telegram", int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {


        return new Paragraph(callerLineNumber, callerFilePath) {
            ExternalLink = $"https://t.me/{username}",
            OpenLinksInANewTab = true,
            TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            //Width = new(e => e.InternalWidth),
            PaddingsHorizontal = 12,
            Inlines = {
                new SvgIconInline(SvgIcons.SimpleIcons.Telegram) {
                    MarginRight = 0.4,
                    Scale = 1.2,
                    BaselineOffset = 0.2
                },
                text
            },
            Radius = BorderRadius,
            ForegroundColor = Color.White,
            BackgroundColor = new Color("26A5E4")
        };

    }

    public static Block DiscordButton(string invite, string? text = "Discord", int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "") {

        if (!Uri.TryCreate(invite, new UriCreationOptions(), out var uri)) {
            invite = "https://discord.com/invite/" + invite; 
        }

        return new Paragraph(callerLineNumber, callerFilePath) {
            ExternalLink = invite,
            OpenLinksInANewTab = true,
            TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
            //Width = new(e => e.InternalWidth),
            PaddingsHorizontal = 12,
            Inlines = {
                new SvgIconInline(SvgIcons.SimpleIcons.Discord) {
                    MarginRight = 0.4,
                    Scale = 1.2,
                    BaselineOffset = 0.2
                },
                text
            },
            Radius = BorderRadius,
            ForegroundColor = Color.White,
            BackgroundColor = new Color("5865F2")
        };

    }




}
