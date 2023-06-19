StaticSharpClass("StaticSharp.SessionStorage", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        StoredValue: undefined,
        ValueToStore: undefined,
    }


    window.addEventListener("beforeunload", function (e) {
        console.log(modifier.name, modifier.ValueToStore)
        sessionStorage.setItem(modifier.name, modifier.ValueToStore)
    }, false);


    /*new Reaction(() => {
        sessionStorage.setItem(modifier.name, modifier.ValueToStore)
    })*/

})
