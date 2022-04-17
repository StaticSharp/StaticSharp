using System.Collections;
using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace StaticSharp;







class Material {

    public virtual Modifier? Style => null;
    public virtual IItem? Content => null;

    public virtual IItem? Footer => null;


}


class MyMaterial : Material {



    sealed override public Column? Content => new() { 
        
        new Modifier() {            
            ForgroundColor = Color.Blue,
            Children = {
                new Row(){

                }
            }
                   
        }
    };

}

class MyMaterial2 : MyMaterial { 
    

}

public static class Program {
    public static void Main(string[] args) {


        var item = new Item() {
            
            X = e => 2*5+e.Parent.Y,
            Y = e => new Symbolic.Item( e.Parent["Id"].ToString() ).Width,
            Margin = new() {
                Left = e => 10,
                Right = e => e.Margin.Top
            }
        };

        var script = item.PropertiesInitializationScript();



        var c2 = new Column() {


            Children = {
                new Space()
            }
        };

        var c = new Column() {
            //Width = c2.Parent.Width,
            Margin = new() {
                Left = (e) => e.X,
                Right = (e) => e.Margin.Left
                /*Left = { Binding =
                e => 0.5f * Math.Min(e.Width - 800, 0)
                },*/
                //Right = e => e.Padding.Left,
            },
            Children = {
                //new Space(),
                @$"Text
{8}
text",
                new Row {
                    "Text"
                }
            }
            
        };

        var propDecl = c.GeneratePropertyDeclaration();

        //Expression<Func<Column, float>> f = (element) => 0.5f * ((element.Width ?? 0) - (element.Width ?? 0));

        


        var myMaterial = new MyMaterial();
    
    }
}