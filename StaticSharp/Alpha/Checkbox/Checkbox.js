
function Checkbox(element) {
    console.log("Checkbox", element.id)
    element.Reactive = {
        Value: false,
        InputValue: () => element.InputValue
    }

    function input() {
        return element.children[0]
    }

    function getInputValue() {
        return !!(input().checked)
    }

    new Reaction(() => {


        //console.log("this.checked", this.checked)
        element.InputValue = getInputValue()

        input().oninput = function () {   
            //console.log("input().oninput", this.checked, element)
            element.InputValue = getInputValue()
        }
    })

    new Reaction(() => {
        //console.log("Reaction", getInputValue(), "=>", element.Value, element)
        input().checked = element.Value
        element.InputValue = element.Value
    })


}
