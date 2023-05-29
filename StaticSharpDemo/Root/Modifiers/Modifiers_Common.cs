using Octokit;
using StaticSharp.Gears;
using StaticSharp.Js;
using System.Collections.Generic;
using System.Linq;
namespace StaticSharpDemo.Root.Modifiers {
    abstract partial class Modifiers_Common : ArticlePage {

        public override Block? MainVisual => new Image("ModifiersPoster.psd");


        protected Blocks BottonExample => new() {
            CodeBlockFromThisFileRegion("bottonExample"),
#region bottonExample
            new Paragraph("Click me!"){
                BackgroundColor = Color.Violet,
                Modifiers = {
                    new Button{
                        Script = """alert("clicked")"""
                    }
                }
            },
#endregion
            
        };

        protected Blocks ToggleExample => new() { 
            CodeBlockFromThisFileRegion("toggleExample"),
#region toggleExample
            new Paragraph("Click me!"){
                BackgroundColor = new(e=>e.AsToggle().Value ? Color.Violet : Color.BlueViolet) ,
                Modifiers = {
                    new Toggle()
                }
            },
#endregion
        };

        protected Blocks HoverExample => new() {
            CodeBlockFromThisFileRegion("hoverExample"),
#region hoverExample
            new Paragraph("Hover me!"){
                BackgroundColor = new(e=>e.AsHover().Value ? Color.PaleVioletRed : Color.BlueViolet) ,
                Modifiers = {
                    new Hover()
                }
            },
#endregion
        };

        protected Blocks CursorExample => new() {
            CodeBlockFromThisFileRegion("cursorExample"),

            new LinearLayout{
                Vertical = false,
                Children = {
#region cursorExample
                    new Paragraph("None"){
                        BackgroundColor = Color.Violet,
                        Modifiers = {
                            new Cursor(CursorOption.None)
                        }
                    },
                    new Paragraph("Pointer"){
                        BackgroundColor = Color.MediumVioletRed,
                        Modifiers = {
                            new Cursor(CursorOption.Pointer)
                        }
                    },
                    new Paragraph("Wait"){
                        BackgroundColor = Color.BlueViolet,
                        Modifiers = {
                            new Cursor(CursorOption.Wait)
                        }
                    },
#endregion
                }
            }.Modify(x=>{
                foreach (var i in x.Children.OfType<Paragraph>()){
                    i.TextAlignmentHorizontal = TextAlignmentHorizontal.Center;
                }
            }),
        };
        protected Blocks BorderRadiusExample => new() {
            CodeBlockFromThisFileRegion("borderRadiusExample"),
#region borderRadiusExample
            new Paragraph("BorderRadius"){
                BackgroundColor = Color.BlueViolet,
                MarginsHorizontal = 12,
                Modifiers = {
                    new BorderRadius(){
                        Radius = 10,
                        RadiusTopRight = 0,
                    }
                }
            },            
#endregion
        };
        protected Blocks MaterialShadowExample => new() {
            CodeBlockFromThisFileRegion("materialShadowExample"),
#region materialShadowExample
            new Paragraph("Hover me!"){
                MarginsHorizontal = 12,
                BackgroundColor = Color.BlueViolet,
                Modifiers = {
                    new MaterialShadow(){
                        Elevation = new(e=>e.AsHover().Value ? 10: 3)
                    },
                    new BorderRadius(){
                        Radius = 10,
                    },
                    new Hover()

                }
            },
#endregion
        };
        protected Blocks UserSelectExample => new() {
            CodeBlockFromThisFileRegion("userSelectExample"),
            new LinearLayout{
                Vertical = false,
                Children = {
#region userSelectExample
                    new Paragraph("None"){
                        BackgroundColor = Color.Violet,
                        Modifiers = {
                            new UserSelect(UserSelectOption.None)
                        }
                    },
                    new Paragraph("Text"){
                        BackgroundColor = Color.MediumVioletRed,
                        Modifiers = {
                            new UserSelect(UserSelectOption.Text)
                        }
                    }
#endregion
                }
            }
        };
        protected Blocks UsageExample => new() {
#region usageExample
            new Paragraph("Click me!"){
                Depth = 1,
                MarginsHorizontal = 12,
                BackgroundColor = Color.OrangeRed,
                Modifiers = {
                    new MaterialShadow(){
                        Elevation = new(
                            e=>Animation.SpeedLimit(
                                50,
                                e.AsHover().Value ? 10: 3
                                )
                            )
                    },
                    new BorderRadius(){
                        Radius = 4,
                    },
                    new Hover(),
                    new Cursor(CursorOption.Pointer),
                    new UserSelect(UserSelectOption.None),
                    new Toggle(),
                }
            }.Assign(out var CodeVisibilityToggle),

            CodeBlockFromThisFileRegion("usageExample")
                .Modify(x=> x.Exists = new(e=>CodeVisibilityToggle.Value.AsToggle().Value)),
#endregion
        };
        protected Blocks BackgroundImageExample_Simple => new() {
            CodeBlockFromThisFileRegion("backgroundImageExample_Simple"),
#region backgroundImageExample_Simple
            new Block{
                Height = 90,
                BackgroundColor = Color.LightGray,
                Modifiers = {
                    new BackgroundImage{
                        ImageGenome = LoadFile("../FavIconWhiteInBlack.svg"),
                    }
                }
            },
#endregion
        };

        protected Blocks BackgroundImageExample_Complex => new() {
            CodeBlockFromThisFileRegion("backgroundImageExample_Complex"),
#region backgroundImageExample_Complex
            new Block{
                Height = 70,
                BackgroundColor = Color.OrangeRed,
                Modifiers = {
                    new BackgroundImage{
                        Height= 40,
                        ImageGenome = LogoGenome,
                        Repeat = BackgroundImageRepeat.RepeatX,
                        BlendMode = BlendMode.Saturation,
                    }.Center(),

                    new BackgroundImage{
                        X = new(e=>Animation.Loop(5,0,-e.Width)),
                        Height= new(e=>e.AsBlock().Height),
                        ImageGenome = LogoGenome,
                        Repeat = BackgroundImageRepeat.RepeatX,
                    },
                }
            },
#endregion
        };

        protected Blocks LinearGradientExample_Simple => new() {
            CodeBlockFromThisFileRegion("linearGradientExample_Simple"),
#region linearGradientExample_Simple
            new Block(){
                Height = 70,
                Modifiers = {
                    new LinearGradient{
                        Keys = {
                            Color.OrangeRed,
                            { Color.Orange, 0.25 },
                            Color.MediumVioletRed,
                        },
                    }
                }
            },
#endregion
        };
        protected Blocks LinearGradientExample_Complex => new() {
            CodeBlockFromThisFileRegion("linearGradientExample_Complex"),
#region linearGradientExample_Complex
            new Paragraph("Click me"){
                Height = 70,
                BackgroundColor= Color.Black,
                Modifiers = {
                    new LinearGradient{
                        Angle = 0.25,
                        Keys = {
                            { Color.OrangeRed, new(e=>Js.Animation.Duration(0.5, e.AsToggle().Value? 0.5: 0.25)) },
                            { Color.MediumVioletRed, new(e=>Js.Animation.Duration(0.5, e.AsToggle().Value? 0.5: 0.75)) },
                        },
                    },
                    new Toggle()
                }
            }
#endregion
        };



        protected Blocks BorderExample => new() {
            CodeBlockFromThisFileRegion("borderExample"),
#region borderExample
            new Block{
                Height = 70,
                Modifiers = {
                    new Border{
                        Sides = Sides.All, //Default value
                        Style= BorderStyle.Solid,                        
                        Width = new(e=> 1 / Js.Window.DevicePixelRatio)
                        //The width remains fixed at one pixel regardless of scaling.
                    },
                    new Border{
                        Sides = Sides.Left,
                        Color = Color.OrangeRed,
                        Width = new(e=>Animation.Loop(5,0,e.AsBlock().Width)),
                    },
                }
            }
        };
 #endregion



    }
}
