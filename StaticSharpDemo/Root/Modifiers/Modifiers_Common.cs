using StaticSharp.Js;
using System.Linq;
namespace StaticSharpDemo.Root.Modifiers {
    abstract partial class Modifiers_Common : ArticlePage {

        public override Block? MainVisual => new Image("ModifiersPoster.psd");

        public override Blocks Article => new(){

            nameof(Button).ToSectionHeader(),

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
            
            nameof(Toggle).ToSectionHeader(),
             CodeBlockFromThisFileRegion("toggleExample"),
#region toggleExample
            new Paragraph("Click me!"){
                BackgroundColor = new(e=>e.AsToggle().Value ? Color.Violet : Color.BlueViolet) ,
                Modifiers = {
                    new Toggle()
                }
            },
#endregion

            nameof(Hover).ToSectionHeader(),
             CodeBlockFromThisFileRegion("hoverExample"),
#region hoverExample
            new Paragraph("Hover me!"){
                BackgroundColor = new(e=>e.AsHover().Value ? Color.PaleVioletRed : Color.BlueViolet) ,
                Modifiers = {
                    new Hover()
                }
            },
#endregion



            nameof(Cursor).ToSectionHeader(),
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


            nameof(BorderRadius).ToSectionHeader(),
            CodeBlockFromThisFileRegion("borderRadiusExample"),
#region borderRadiusExample
            new Paragraph("BorderRadius"){
                BackgroundColor = Color.BlueViolet,
                Modifiers = {
                    new BorderRadius(){
                        Radius = 10,
                        RadiusTopRight = 0,
                    }
                }
            },            
#endregion

            nameof(MaterialShadow).ToSectionHeader(),
            CodeBlockFromThisFileRegion("shadowExample"),
#region shadowExample
            new Paragraph("Hover me!"){
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

             (nameof(UserSelect)+" & friends").ToSectionHeader(),
#region userExample
            new Paragraph("Click me!"){
                Depth = 1,
                BackgroundColor = Color.LightPink,
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

            CodeBlockFromThisFileRegion("userExample")
                .Modify(x=> x.Exists = new(e=>CodeVisibilityToggle.Value.AsToggle().Value)),

#endregion
        };

        

    }
}
