using StaticSharp;

namespace StaticSharpDemo.Root.Modifiers {

    [Representative]
    partial class En : Page {
        public override Inlines? Description => "Modifiers is a tool for adding functionality to UI objects. Modifiers can be added to UI objects to modify their behavior or enhance their capabilities. Modifiers are capable of changing CSS properties to create effects such as shadows and gradients, as well as listening to events to add properties like \"Hovered\" or \"Pressed\". They provide a modular and reusable way for developers to add specific functionality to UI elements";

        public override Blocks? Content => new (){

            Description,

            new LayoutOverride{ 
                OverrideWidth = 100,
                OverrideHeight = 50,
                Child = new Block{
                    Height= 50,
                    BackgroundColor = new(e=>e.AsToggle().Value ? Color.Green : Color.Red),
                    Modifiers = { 
                        new Toggle(),
                        new BorderRadius{ Radius = 25}
                    }
                }
            },
            


            new Block{
                Width = 200,
                Height = 200,
                BackgroundColor =
                new(e=>
                (e as JHover).Value
                ?Color.MediumVioletRed
                :Color.OrangeRed
                ),

                Modifiers = {
                    new Hover(),
                    new BorderRadius{ 
                        Radius = new (e=>
                        Js.Animation.SpeedLimit(
                            10,
                            (e as JHover).Value? 10: 50
                            ))
                    }
                }
            },


            new Block{ 
                Width = 200,
                Height = 200,
                BackgroundColor = new(e=>
                Color.Lerp(
                    Color.MediumVioletRed,
                    Color.OrangeRed,
                    Js.Animation.SpeedLimit(
                        (e as JHover).Value?5:0.5,
                        (e as JHover).Value?1:0
                    )
                )
                ),
                Modifiers = { 
                    new Hover(),
                    /*new BorderRadius{ 
                        Radius = new(
                            e =>  Js.Animation.SpeedLimit(10,(e as Js.Hover)!.Value ? ((Js.Block)e).Height / 2 : 0) + 10                           
                        ),
                    },*/
                }
            }
        };
    }
}