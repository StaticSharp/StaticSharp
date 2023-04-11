namespace StaticSharpDemo.Root.Modifiers {

    [Representative]
    partial class En : Page {
        public override Inlines? Description => "Modifiers is a tool for adding functionality to UI objects. Modifiers can be added to UI objects to modify their behavior or enhance their capabilities. Modifiers are capable of changing CSS properties to create effects such as shadows and gradients, as well as listening to events to add properties like \"Hovered\" or \"Pressed\". They provide a modular and reusable way for developers to add specific functionality to UI elements";

        public override Blocks? Content => new (){

            Description,


            new Block{ 
                Width = 200,
                Height = 200,
                BackgroundColor = Color.MediumVioletRed,
                Modifiers = { 
                    new Hover(),
                    new BorderRadius{ 
                        Radius = new(
                            e=>  Js.Animation.SpeedLimit(1000, ((Js.Hover)e).Value ? ((Js.Block)e).Height / 2 : 0)                            
                        ),
                    },
                }
            }
        };
    }
}