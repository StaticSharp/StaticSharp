
var Block = {}

Block.After = function () {
    console.log("block")
}


var Image = Block
Image.Initialization = function (element) {
    
}






function ImageInitialization(element) {

    BlockInitialization(element)

    Image.After()

    //console.log(E)



    element.Reactive = {        

        Aspect: element.dataset.width / element.dataset.height,

        InternalWidth: () => First(element.Height * element.Aspect, element.dataset.width),
        InternalHeight: () => First(element.Width / element.Aspect, element.dataset.height),
        //Width: () => element.LayoutWidth || (element.Height * element.Aspect) || element.dataset.width,

        //Height: () => element.LayoutHeight || (element.Width / element.Aspect) || element.dataset.height,



    }


    
}



function ImageBefore(element) {
   

    BlockBefore(element)


    WidthToStyle(element)
    HeightToStyle(element)



}


function getPixel(img, x, y) {
    var canvas = document.createElement('canvas');
    var context = canvas.getContext('2d');
    //context.drawImage(img, 0, 0);
    return context.getImageData(x, y, 1, 1).data;
}


function ImageAfter(element) {
    BlockAfter(element)

    let content = element.children[0]





    content.style.width = "100%"
    content.style.height = "100%"



    

}