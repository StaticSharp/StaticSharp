


function Outline(element) {
    Modifier.call(this,element)
    this.isOutline = true

    let modifier = this



    modifier.Reactive = {
        Enabled: true,
        Style: "Solid",
        Color: () => element.HierarchyForegroundColor,
        Width: 1,
        Offset: 0        
    }


    new Reaction(() => {
        var last = element.Modifiers.findLast(x => x.isOutline && x.Enabled)
        if (last == modifier) {
            element.style.outlineStyle = modifier.Style.toLowerCase()
            element.style.outlineColor = modifier.Color.toString()
            element.style.outlineWidth = `${modifier.Width}px`
            element.style.outlineOffset = `${modifier.Offset}px`
            /*if (modifier.Sides == 15) {
                
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

                element.style.outlineStyle = styles.join(' ')
                element.style.outlineColor = colors.join(' ')
                element.style.outlineWidth = widths.join(' ')
            }*/
        }
    })
}