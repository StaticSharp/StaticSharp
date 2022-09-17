
function Checkbox(element) {
    console.log("Checkbox", element.id)
    element.Reactive = {
        Value: () => First(element.ValueInput, false),
        ValueInput: () => element.Value
    }

    function input() {
        return element.children[0]
    }

    function getValueInput() {
        return !!(input().checked)
    }

    new Reaction(() => {

        //console.log("this.checked", this.checked)
        element.ValueInput = getValueInput()

        input().oninput = function () {   
            //console.log("input().oninput", this.checked, element)
            element.ValueInput = getValueInput()
        }
    })

    new Reaction(() => {
        //console.log("Reaction", getValueInput(), "=>", element.Value, element)
        let newValue = element.Value
        input().checked = newValue
        element.ValueInput = newValue
    })


}
