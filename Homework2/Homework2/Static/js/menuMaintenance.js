function GetTableValues() {
    //Create an Array to hold the Table values.
    var menus = new Array();

    //Reference the Table.
    var table = document.getElementsByName("table");
    console.log(table[0].rows.length);
    //Loop through Table Rows.
    for (var i = 1; i < table[0].rows.length; i++) {
        //Reference the Table Row.
        var row = table[0].rows[i];

        //Copy values from Table Cell to JSON object.
        var menu = {};
        menu.menuNo = row.cells[1].innerHTML;
        menu.level = row.cells[2].innerHTML;
        menu.title = row.cells[3].innerHTML;
        menu.linkType = row.cells[4].innerHTML;
        menu.linkUrl = row.cells[5].innerHTML;
        menu.status = row.cells[6].innerHTML;
        menus.push(menu);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("JSON").value = JSON.stringify(menus);
}