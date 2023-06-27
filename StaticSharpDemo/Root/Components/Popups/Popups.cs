using StaticSharp.Resources.Text;
using System.Linq;

namespace StaticSharpDemo.Root.Components.Popups {

    [Representative]
    public partial class Popups : ArticlePage {

        public override Inlines? Description => "Popups, dialogs, menus, GDPR popup";


        public Block MenuPopup => new Block {


        }.FillHeight().FillWidth();



        public override Blocks UnmanagedChildren => new(){
            base.UnmanagedChildren,
            new Block{ 
                UnmanagedChildren = {
                    new LinearLayout {
                        BackgroundColor = Color.White,
                        Visibility = 1,
                        Children = {
                            "Warning",
                            new Paragraph("Ok"){ 
                                TextAlignmentHorizontal = TextAlignmentHorizontal.Center,
                                BackgroundColor = Color.Green,
                                Margins = 10,
                                Modifiers = {
                                    new SessionStorageBoolean {
                                        Name = "PopupShown",
                                        ValueToStore = new(e=>e.AsToggle().Value)
                                    },
                                    new Toggle{
                                        Value = new(e=>e.AsSessionStorageBoolean().StoredValue)
                                    },

                                    new BorderRadius{Radius = 5},
                                    new UserSelect(),
                                    new Cursor()
                                }
                            }.Assign(out var okButton)
                        },
                        Modifiers = {                            
                            new MaterialShadow(),
                            new BorderRadius{ 
                                Radius = 5
                            },
                            new BoxShadow{ 
                                Spread = 1,
                                Color = Color.FromGrayscale(0.5),
                            }                            
                        },
                        
                    }.Center(),
                },
                Exists = new(e=>!okButton.Value.AsToggle().Value),
                Modifiers = {
                    new BackdropBlur{}
                },                

            }.FillHeight().FillWidth()            
        };

        public override Blocks Article => new Blocks {

            //Enumerable.Range(0,20).Select(x=>new Paragraph(TextUtils.LoremIpsum(50))),


            new LinearLayout {
                //FontSize = 50,
                Vertical = false,
                Overflow = LinearLayoutOverflow.Remove,
                ItemGrow = 0,
                GapGrow = 1,
                Children = {
                    "One",
                    "Two",
                    "Three",
                    "Four",
                    "Five",
                    "Six",
                    "Seven",
                    "Eight",
                    "Nine",
                    "Ten"
                },

            }.Modify(x=>{
                x.Ellipsis = new Menu{
                    Button = new SvgIconBlock(SvgIcons.MaterialDesignIcons.DotsHorizontal){
                        Width = 24,
                        MarginsHorizontal = 10
                    },
                    Popup = new LinearLayout {
                        Vertical = true,
                        Overflow = LinearLayoutOverflow.Remove,
                        BackgroundColor = Color.White,
                        Children = {
                            "One",
                            "Two",
                            "Three",
                            "Four",
                            "Five",
                            "Six",
                            "Seven",
                            "Eight",
                            "Nine",
                            "Ten"
                        },
                        Modifiers = { 
                            new MaterialShadow(),
                            new BorderRadius(5)
                        }
                    }.Modify(x=>{
                        foreach (var (item, index) in x.Children.Select((item, index) => (item, index))){
                            item.Exists = new(e=>!e.Parent.Parent.Parent.AsBlockWithChildren().Children.Skip(index).First().Exists);
                        }

                    })
                };
            }),

            



            /*new Block{ 
                BackgroundColor = new(e=>e.AsHover().Value? Color.DeepPink: Color.GreenYellow),
                Height = 500,
                Modifiers = { 
                    new Modal{ },
                    new Hover()
                },
                UnmanagedChildren = {
                    new Block{
                        BackgroundColor = new(e=>e.AsHover().Value? Color.DeepPink: Color.GreenYellow),
                        Height = 150,
                        Width = 150,
                        Modifiers = {
                            new Hover()
                        }
                    }
                }
            }*/



        };

        
    }
}