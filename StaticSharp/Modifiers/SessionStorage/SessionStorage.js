function SessionStorage(element) {
    Modifier.call(this, element)
    this.isSessionStorage = true

    let modifier = this

    modifier.Reactive = {
        StoredValue: undefined,
        ValueToStore: undefined,
    }

    new Reaction(() => {
        sessionStorage.setItem(modifier.name, modifier.ValueToStore)
    })


}