using StaticSharp.Gears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticSharpDemo.Root {

    [Representative]
    public partial class Ru : Material{


        static string RandomString(int length, Random random) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789     ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        Paragraph CreateParagraph(int numChars, Random random) {
            return new Paragraph() { RandomString(numChars, random) };
        }

        IEnumerable<Paragraph> CreateParagraphs(int count) {
            Random random = new Random(0);
            return Enumerable.Range(0, count).Select(i => CreateParagraph(50, random));
        }


        public override Group? Content => new() {

            new Image(new HttpRequest("https://upload.wikimedia.org/wikipedia/commons/4/49/Koala_climbing_tree.jpg")),

            H1($"H1"),
            "Abc",
            "Abc",
            

            /*H1("H1"),*/
            /*new Modifier {
                FontSize = new((element) => element.Sibling<SliderJs>("Slider").Value + 2),
                Children = { $"Modifier test" }
            },*/

            //CreateParagraphs(100),

            {
                "Slider",
                new Slider {
                    Min = 10,
                    Max = 50
                }
            },

            /*$"A B C D {4}",
            new Space(),

            $"Bold: {new InlineModifier { FontStyle = new FontStyle { Weight = FontWeight.Bold }, Children = { "Text" } }}",
            $"Если   понадобится компонент,которого нет среди стандартных.",
            $"Можно создать компонент прям в проекте вашего сайта."*/
        };
    }
}
