// Function to convert table into JSON.
function GetTableValues() {
    //Create an Array to hold the Table values.
    var menus = new Array();

    //Reference the Table.
    var table = document.getElementsByName("table");
    console.log(table[0].rows.length);
    //Loop through Table Rows.
    for (var i = 0; i < table[0].rows.length; i++) {
        //Reference the Table Row.
        var row = table[0].rows[i];

        //Copy values from Table Cell to JSON object.
        var menu = {};
        menu.menuNo = row.cells[1].innerText;
        menu.level = row.cells[2].innerText;
        menu.title = row.cells[3].innerText;
        menu.linkType = row.cells[4].innerText;
        menu.linkUrl = row.cells[5].innerText;
        menu.status = row.cells[6].innerText;
        menus.push(menu);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("JSON").value = JSON.stringify(menus);
}