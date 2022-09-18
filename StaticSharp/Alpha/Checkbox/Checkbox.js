
function Checkbox(element) {

    element.Reactive = {
        Value: false,
        ValueActual: () => element.Value
    }

    function input() {
        return element.children[0]
    }

    function getValueActual() {
        return !!(input().checked)
    }

    new Reaction(() => {

        //console.log("this.checked", this.checked)
        element.ValueActual = getValueActual()

        input().oninput = function () {   
            //console.log("input().oninput", this.checked, element)
            element.ValueActual = getValueActual()
        }
    })

    new Reaction(() => {
        //console.log("Reaction", getValueActual(), "=>", element.Value, element)
        let newValue = element.Value
        input().checked = newValue
        element.ValueActual = newValue
    })


}
