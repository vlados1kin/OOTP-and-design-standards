async function getFigures() {
    const svg = document.getElementById("svg");
    while (svg.firstChild) {
        svg.removeChild(svg.firstChild);
    }
    const response = await fetch("/editor", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const figures = await response.json();
        figures.forEach(figure => createElement(figure));
    }
}
async function getTypesOfFigures() {
    const select = document.getElementById("figure");
    const response = await fetch("/editor/types", {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" }
    });
    if (response.ok === true) {
        const types = await response.json();
        types.forEach(type => {
            let option = new Option(type.nameOfFactory, type.typeOfFactory);
            select.appendChild(option);
        });
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
async function removeFigures() {
    const response = await fetch("/editor", {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.status === 204) {
        await getFigures();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}
function createElement(element) {
    const path = document.createElementNS("http://www.w3.org/2000/svg", "path");
    path.setAttribute("d", element.pattern);
    path.setAttribute("fill", "none");
    path.setAttribute("stroke", "black");
    path.setAttribute("stroke-width", "1");
    path.setAttribute("id", element.id);
    document.getElementById("svg").appendChild(path);
    
    createTd(element.id);
}
function createTd(id) {
    const list = document.getElementById("shape-list");
    const td = document.createElement("td");
    td.setAttribute("id", "shape" + id);
    td.setAttribute("class", "shape-item");
    td.innerHTML = "ID фигуры " + id;
    list.appendChild(td);
}
function removeTd(id) {
    document.getElementById("shape" + id).remove();
}
function removeAllTd() {
    const list = document.getElementById("shape-list");
    while (list.firstChild) {
        list.removeChild(list.firstChild);
    }
}
async function uploadFile(file) {
    const formData = new FormData();
    formData.append('file', file);
    const response = await fetch("/editor/file", {
        method: 'POST',
        body: formData
    });
    if (response.ok === true) {
        alert('Плагин успешно загружен');
        location.reload();
    }
}
async function checkSign(figure) {
    const response = await fetch("editor/sign/" + figure, {
        method: "GET",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
    });
    if (response.ok === true) {
        const sign = await response.json();
        alert(sign);
    }
}
async function openFile() {
    const formData = new FormData();
    formData.append('file', file);
}
async function uploadFigures(file) {
    const formData = new FormData();
    formData.append('file', file);
    const response = await fetch("/editor/open", {
        method: 'POST',
        body: formData,
    });
    if (response.ok === true) {
        alert('Фигуры загружены');
        location.reload();
    } else {
        alert(response.statusText);
        location.reload();
    }
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
    const angle = document.getElementById("angle").value * Math.PI / 180;
    const figure = document.getElementById("figure").value;
    
    await createFigure(x, y, bottomSide, leftSide, angle, figure);
});
document.getElementById("update").addEventListener("click", async (event) => {
    const id = parseInt(document.getElementById("id").value, 10);
    let response = await fetch("/editor/" + id, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    const shape = await response.json();
    const x = shape.x;
    const y = shape.y;
    const bottomSide = document.getElementById("bottomSide").value;
    const leftSide = document.getElementById("leftSide").value;
    const angle = document.getElementById("angle").value * Math.PI / 180;
    const figure = document.getElementById("figure").value;

    response = await fetch("/editor/" + id, {
        method: "PUT",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: id,
            X: parseInt(x, 10),
            Y: parseInt(y, 10),
            BottomSide: parseInt(bottomSide, 10),
            LeftSide: parseInt(leftSide, 10),
            Angle: angle,
            TypeOfFactory: figure
        })
    });
    const element = await response.json();
    const path = document.getElementById("" + id);
    path.setAttribute("d", element.pattern);
});
document.getElementById("clear").addEventListener("click", async () => {
    await removeFigures();
    removeAllTd();
});
document.getElementById("delete").addEventListener("click", async () => {
    const id = parseInt(document.getElementById("id").value, 10);
    const response = await fetch("/editor/" + id, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" }
    });
    document.getElementById("" + id).remove();
    removeTd(id);
});
document.getElementById("save").addEventListener("click", async () => {
    const response = await fetch("/editor/save", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    await response.json();
    alert('Фигуры сохранены');
});
document.getElementById("plugin-upload").addEventListener("click", () => {
    document.getElementById("plugin").click();
});
document.getElementById("send").addEventListener("click", async () => {
    const fileInput = document.getElementById('plugin'); 
    const file = fileInput.files[0];
    await uploadFile(file);
});
document.getElementById("sign").addEventListener("click", async () => {
    const figure = document.getElementById("figure").value;
    await checkSign(figure);
});
document.getElementById("open").addEventListener("click", async () => {
    document.getElementById("openFigures").click();
});
document.getElementById("loadFigures").addEventListener("click", async () => {
    const fileInput = document.getElementById('openFigures');
    const file = fileInput.files[0];
    await uploadFigures(file);
});
document.getElementById("settings").addEventListener("click", () => {
    document.querySelector('dialog').show();
});
document.getElementById("close-dialog").addEventListener("click", () => {
    document.querySelector('dialog').close();
});
document.getElementById("send-settings").addEventListener("click", async () => {
    const response = await fetch("editor/settings", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            key: document.getElementById("key").value,
        })
    });
    await response.json();
});
document.getElementById("path-submit").addEventListener("click", async () => {
    const path = document.getElementById("path").value;
    const response = await fetch("editor/convert", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            path: path
        })
    });
    await response.json();
});

getFigures();
getTypesOfFigures();
