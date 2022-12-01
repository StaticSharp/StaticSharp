
function MaterialShadow(element) {
    Hierarchical(element)

    new Reaction(() => {
        element.Parent.style.boxShadow = "0 10px 20px rgba(0,0,0,0.19), 0 6px 6px rgba(0,0,0,0.23)"
    })


    console.log("MaterialShadow")
}