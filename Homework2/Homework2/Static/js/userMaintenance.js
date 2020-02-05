function GetTableValues() {
    //Create an Array to hold the Table values.
    var users = new Array();

    //Reference the Table.
    var table = document.getElementsByName("table");
    console.log(table[0].rows.length);
    //Loop through Table Rows.
    for (var i = 1; i < table[0].rows.length; i++) {
        //Reference the Table Row.
        var row = table[0].rows[i];

        //Copy values from Table Cell to JSON object.
        var user = {};
        user.user = row.cells[1].innerHTML;
        user.role = row.cells[2].innerHTML;
        user.status = row.cells[3].innerHTML;
        user.createDate = row.cells[4].innerHTML;
        user.createUser = row.cells[5].innerHTML;
        user.modifyDate = row.cells[6].innerHTML;
        user.modifyUser = row.cells[7].innerHTML;
        users.push(user);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("JSON").value = JSON.stringify(users);
}