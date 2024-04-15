async function getFigures() {
    const response = await fetch("/editor", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const figures = await response.json();
        figures.forEach(figure => createElement(figure));
    }
}
async function createFigure(x, y, bottomSide, leftSide, angle, figure) {
    const response = await fetch("/editor", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            X: parseInt(x, 10),
            Y: parseInt(y, 10),
            BottomSide: parseInt(bottomSide, 10),
            LeftSide: parseInt(leftSide, 10),
            Angle: parseFloat(angle),
            TypeOfFactory: figure
        })
    });
    if (response.ok === true) {
        const s = await response.json();
        createElement(s);
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
function createElement(element) {
    const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
    path.setAttribute("d", element);
    path.setAttribute("fill", "none");
    path.setAttribute("stroke", "black");
    path.setAttribute("stroke-width", "1");
    document.getElementById("svg").appendChild(path);
}
document.getElementById("figure").addEventListener("change", () => {
    const figure = document.getElementById("figure").value;
    const angle = document.getElementById("angle");
    const leftSide = document.getElementById("leftSide");
    if (leftSide.readOnly)
        leftSide.readOnly = !leftSide.readOnly;
    if (angle.readOnly)
        angle.readOnly = !angle.readOnly;
    switch (figure) {
        case "parallelogram":
            break;
        case "rectangle":
            angle.value = 90;
            angle.readOnly = !angle.readOnly;
            break;
        case "square":
            leftSide.value = "";
            leftSide.readOnly = !leftSide.readOnly;
            angle.value = 90;
            angle.readOnly = !angle.readOnly;
            break;
        case "rhombus":
            leftSide.value = "";
            leftSide.readOnly = !leftSide.readOnly;
            break;
        case "triangle":
            break;
    }
});
document.getElementById("svg").addEventListener("click", async (event) => {
    const x = event.offsetX;
    const y = event.offsetY;
    const bottomSide = document.getElementById("bottomSide").value;
    const leftSide = document.getElementById("leftSide").value;
    let angle = document.getElementById("angle").value;
    angle = angle * Math.PI / 180;
    const figure = document.getElementById("figure").value;
    await createFigure(x, y, bottomSide, leftSide, angle, figure);
});
document.getElementById("clear").addEventListener("click", () => {
    const svg = document.getElementById("svg");
    while (svg.firstChild) svg.removeChild(svg.firstChild);
});

getFigures();