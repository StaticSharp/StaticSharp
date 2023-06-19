using System.Linq;

namespace StaticSharpDemo.Root.Components.Popups {

    [Representative]
    public partial class Popups : ArticlePage {

        public override Inlines? Description => "Popups, dialogs, menus, GDPR popup";


        public Block MenuPopup => new Block {


        }.FillHeight().FillWidth();



        /*public override Blocks UnmanagedChildren => new(){
            base.UnmanagedChildren,

            new Block{ 
                Exists = new(e=>!e.UnmanagedChildren.First().AsToggle().Value),

                UnmanagedChildren = {                   

                    new LinearLayout {
                        Visibility = 1,
                        //BackgroundColor = new(e=>e.AsToggle().Value ? Color.Green : Color.Red),
                        Children = {
                            "Warning"
                        },
                        Modifiers = {
                            new SessionStorageBoolean {
                                Name = "PopupShown",
                                ValueToStore = new(e=>e.AsToggle().Value)
                            },
                            new Toggle{
                                Value = new(e=>e.AsSessionStorageBoolean().StoredValue)
                            }.Assign(out var visible),
                            
                        }
                    }.Center(),

                    new Block{
                        BackgroundColor = new Color(1,1,1,0.1),
                        Depth = -1,
                    }.FillHeight().FillWidth(),
                },
                Modifiers = {
                    new BackdropFilter{ 
                        //Enabled = new(e=>visible.Value.Value)
                    }
                }
            }.FillHeight().FillWidth()
            
        };*/

        public override Blocks Article => new Blocks {


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