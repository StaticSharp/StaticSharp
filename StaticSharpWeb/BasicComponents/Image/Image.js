
function ImageInitialization(element) {

    BlockInitialization(element)



    element.Reactive = {        

        Aspect: element.dataset.width / element.dataset.height,

        Width: () => element.LayoutWidth || (element.Height * element.Aspect) || element.dataset.width,

        Height: () => element.LayoutHeight || (element.Width / element.Aspect) || element.dataset.height,



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

    let img = element.children[0]





    img.style.width = "100%"
    img.style.height = "100%"



    

}