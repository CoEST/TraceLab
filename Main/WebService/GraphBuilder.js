Raphael.fn.connection = function (obj1, obj2, line, bg) {
    if (obj1.line && obj1.from && obj1.to) {
        line = obj1;
        obj1 = line.from;
        obj2 = line.to;
    }
    var bb1 = obj1.getBBox(),
        bb2 = obj2.getBBox(),
        p = [{ x: bb1.x + bb1.width / 2, y: bb1.y - 1 },
        { x: bb1.x + bb1.width / 2, y: bb1.y + bb1.height + 1 },
        { x: bb1.x - 1, y: bb1.y + bb1.height / 2 },
        { x: bb1.x + bb1.width + 1, y: bb1.y + bb1.height / 2 },
        { x: bb2.x + bb2.width / 2, y: bb2.y - 1 },
        { x: bb2.x + bb2.width / 2, y: bb2.y + bb2.height + 1 },
        { x: bb2.x - 1, y: bb2.y + bb2.height / 2 },
        { x: bb2.x + bb2.width + 1, y: bb2.y + bb2.height / 2 }],
        d = {}, dis = [];
    for (var i = 0; i < 4; i++) {
        for (var j = 4; j < 8; j++) {
            var dx = Math.abs(p[i].x - p[j].x),
                dy = Math.abs(p[i].y - p[j].y);
            if ((i == j - 4) || (((i != 3 && j != 6) || p[i].x < p[j].x) && ((i != 2 && j != 7) || p[i].x > p[j].x) && ((i != 0 && j != 5) || p[i].y > p[j].y) && ((i != 1 && j != 4) || p[i].y < p[j].y))) {
                dis.push(dx + dy);
                d[dis[dis.length - 1]] = [i, j];
            }
        }
    }
    if (dis.length == 0) {
        var res = [0, 4];
    } else {
        res = d[Math.min.apply(Math, dis)];
    }
    var x1 = p[res[0]].x,
        y1 = p[res[0]].y,
        x4 = p[res[1]].x,
        y4 = p[res[1]].y;
    dx = Math.max(Math.abs(x1 - x4) / 2, 10);
    dy = Math.max(Math.abs(y1 - y4) / 2, 10);
    var x2 = [x1, x1, x1 - dx, x1 + dx][res[0]].toFixed(3),
        y2 = [y1 - dy, y1 + dy, y1, y1][res[0]].toFixed(3),
        x3 = [0, 0, 0, 0, x4, x4, x4 - dx, x4 + dx][res[1]].toFixed(3),
        y3 = [0, 0, 0, 0, y1 + dy, y1 - dy, y4, y4][res[1]].toFixed(3);
    var path = ["M", x1.toFixed(3), y1.toFixed(3), "C", x2, y2, x3, y3, x4.toFixed(3), y4.toFixed(3)].join(",");
    if (line && line.line) {
        line.bg && line.bg.attr({ path: path });
        line.line.attr({ path: path });
    } else {
        var color = typeof line == "string" ? line : "#000";
        return {
            bg: bg && bg.split && this.path(path).attr({ stroke: bg.split("|")[0], fill: "none", "stroke-width": bg.split("|")[1] || 3 }),
            line: this.path(path).attr({ stroke: color, fill: "none" }),
            from: obj1,
            to: obj2
        };
    }
};



var shapes = [];
var connections = [];
var text = [];
var ID = [];
var elementIDindex = [];
var indexSelected = -1;
var rpaper;

var selectNode = function () {
    for (var i = shapes.length; i--;)
    {
        if (shapes[i].id == this.id)
        {
            //set the value for the combo box to i will trigger backend function ComponentLabelText
            document.getElementById('ComponentLabelText').selectedIndex = i;
            document.getElementById('loadSelectedComponent').click();
            
        }
    }
}

var dragger = function () {
    this.ox = this.type == "rect" ? this.attr("x") : this.attr("cx");
    this.oy = this.type == "rect" ? this.attr("y") : this.attr("cy");
    this.animate({ "fill-opacity": .2 }, 500);
}
var move = function (dx, dy) {
    var att = this.type == "rect" ? { x: this.ox + dx, y: this.oy + dy } : { cx: this.ox + dx, cy: this.oy + dy };
    this.attr(att);
    for (var i = connections.length; i--;) {
        rpaper.connection(connections[i]);
    }
    for (var i = shapes.length; i--;)
    {
        if (this.id == shapes[i].id)
        {
            var attText = { x: this.attr("x")+(this.attr("width")/2), y: this.attr("y")+(this.attr("height")/2) };
            text[i].attr(attText);
            indexSelected = i;
        }
    }
}
up = function () {


    this.animate({ "fill-opacity": 0 }, 500);
    for (var i = shapes.length; i--;) {
        if (this.id == shapes[i].id) {
            indexSelected = i;
        }
    }


    var ob = { x: this.attr("x"), y: this.attr("y"), nodeID: ID[indexSelected] };
    var data = JSON.stringify(ob);
    jQuery.ajax({
        type: "POST", dataType: "json", contentType: "application/json; charset=utf-8",
        url: "TraceLab_UI.aspx/MoveNode", data: data,
        success: function (retValue)
        {
         //   alert("boop");
        },
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            $('#post').html(msg);
        }    });
}
function LoadExperiment() {

    if (indexSelected != -1)
    {
        rpaper.clear();
    }

    rpaper = new Raphael("holder", 1000, 800),
        shapes = [],
        connections = [],
        text = [],
        ID = [];
    rpaper.clear();
    rpaper.rect(0, 0, 1000, 800, 10).attr({ fill: "#eee", stroke: "none" });
    selectedIndex = 0;

};
function addNode(x, y, w, l, Text, Identifier) {
    shapes.push(rpaper.rect(x, y, w, l));
    ID.push(Identifier);
    text.push(rpaper.text(x + w / 2, y + l / 2, Text));

    elementIDindex.push(shapes.id);
    
}
function addLink(targetID, sourceID) {
    var i = 0;
    var j = 0;
    while (i < ID.length) {
        if (ID[i] == targetID) {
            break;
        }
        else {
            i++;
        }
    }
    while (j < ID.length) {
        if (ID[j] == sourceID) {
            break;
        }
        else {
            j++;
        }
    }

    connections.push(rpaper.connection(shapes[i], shapes[j], "#000"));

}

function resetNodeHandlers() {

    for (var i = 0, ii = shapes.length; i < ii; i++) {
        shapes[i].attr({ "stroke-width": 2, cursor: "move" });
        shapes[i].drag(move, dragger, up);
        shapes[i].dblclick(selectNode);
    }
}
