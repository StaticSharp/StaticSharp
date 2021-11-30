function Downloadable(element, primaryName, data) {
    let parent = element.parentElement;

    element.updateWidth = function() {
        let left = parent.anchors.textLeft;
        let right = parent.anchors.textRight;
        element.style.marginLeft = left + "px";
        element.style.width = right - left + "px";

        //element.style.backgroundColor = "red";
    }
    parent.onAnchorsChanged.push(element.updateWidth);

    element.className = "Downloadable";
    /*var data = {
        "Latest stable 11.1.0": {
            "Linux": "AntilatencyService_11.1.0_Android_armeabi- v7a.apk",
            "Android": "AntilatencyService_11.1.0_Android_armeabi- v7a.apk",
            "WinRT": {
                x64: "AntilatencyService_11.1.0_WinRT_x64.zip",
                x86: "AntilatencyService_11.1.0_WinRT_x86.zip",
            }
        },
        "11.0.0-rc.1": {
            "Android": "AntilatencyService_11.1.0_Android_armeabi- v7a.apk",
            "WinRT": "AntilatencyService_11.1.0_WinRT_x64.zip"
        },
        "10.1.4": {
            "Android": "AntilatencyService_11.1.0_Android_armeabi- v7a.apk",
            "WinRT": "AntilatencyService_11.1.0_WinRT_x64.zip"
        },
        "AntilatencyService_0": "AntilatencyService_0.zip"

    }*/
    var selected = []

    var selectedIndex = 0;
    //var options = ["Latest stable 11.1.0", "11.0.0-rc.1", "10.1.4"]




    //var selectors = element.appendChild(document.createElement("div"));
    //selectors.className = "Selectors";







    let BuildSelector = (x, depth) => {
        if (typeof(x) != "object")
            return x;

        let select = element.appendChild(document.createElement("select"));
        select.className = "Select";
        select.addEventListener('change', e => {
            selected[depth] = select.value;
            Build();
        });

        Object.keys(x).forEach(option =>
            select.appendChild(new Option(option, option, false, selected[depth] == option))
        );

        selected[depth] = select.value;
        return BuildSelector(x[select.value], depth + 1)
    }
    var Build = function() {
            element.textContent = '';

            var title = element.appendChild(document.createElement("div"));
            title.innerText = primaryName;
            title.className = "Title";

            var selectorsResult = BuildSelector(data, 0);

            var e = selectorsResult.split("|");
            var href = "Downloadable/" + e[0];
            var size = e[1];
            var name = href.split("/").pop()

            var button = element.appendChild(document.createElement("a"));
            button.className = "Button";
            button.innerText = "Download";
            button.href = href;
            button.title = name + " " + size;
            button.download = "";
        }
        /*var BuildSelectors = function () {
            console.log("BuildSelectors");
            selectors.textContent = '';
            BuildSelector(data, 0);
        }*/

    Build();


}