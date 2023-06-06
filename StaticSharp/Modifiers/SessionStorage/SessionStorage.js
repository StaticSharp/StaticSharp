StaticSharpClass("StaticSharp.SessionStorage", (modifier, element) => {
    StaticSharp.Modifier(modifier, element)

    modifier.Reactive = {
        StoredValue: undefined,
        ValueToStore: undefined,
    }

    new Reaction(() => {
        sessionStorage.setItem(modifier.name, modifier.ValueToStore)
    })

})