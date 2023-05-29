


function Border(element) {
    Modifier.call(this,element)
    this.isBorder = true

    let modifier = this



    modifier.Reactive = {
        Enabled: true,
        Sides: 15,
        Style: "Solid",
        Color: () => element.HierarchyForegroundColor,
        Width: 1,
        
    }

    modifier.getBorder = function () {
        let result = Array(4)
        for (let i = 0; i < 4; i++) {

        }
    }

    new Reaction(() => {

        var coModifiers = element.Modifiers.filter(x => x.isBorder && x.Enabled).reverse()
        if (coModifiers[0] == modifier) {

            if (modifier.Sides == 15) {
                element.style.borderStyle = modifier.Style.toLowerCase()
                element.style.borderColor = modifier.Color.toString()
                element.style.borderWidth = `${modifier.Width}px`
            } else {
                let sides = 15

                let styles = Array(4).fill("none");
                let colors = Array(4).fill("black");
                let widths = Array(4).fill("1px");
                for (let m of coModifiers) {
                    let sidesToSet = m.Sides && sides

                    for (let i = 0; i < 4; i++) {
                        if (sidesToSet & (1 << i)) {
                            styles[i] = m.Style.toLowerCase()
                            colors[i] = m.Color.toString()
                            widths[i] = `${m.Width}px`
                        }
                    }

                    sides = sides & ~m.Sides
                    if (sides == 0)
                        break

                }

                element.style.borderStyle = styles.join(' ')
                element.style.borderColor = colors.join(' ')
                element.style.borderWidth = widths.join(' ')
            }
        }
    })
}