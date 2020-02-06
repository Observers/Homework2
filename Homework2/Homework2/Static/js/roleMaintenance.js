// Function to convert table into JSON.
function GetTableValues() {
    //Create an Array to hold the Table values.
    var roles = new Array();

    //Reference the Table.
    var table = document.getElementsByName("table");
    console.log(table[0].rows.length);
    //Loop through Table Rows.
    for (var i = 1; i < table[0].rows.length; i++) {
        //Reference the Table Row.
        var row = table[0].rows[i];

        //Copy values from Table Cell to JSON object.
        var role = {};
        role.role = row.cells[1].innerHTML;
        role.roleDescription = row.cells[2].innerHTML;
        role.status = row.cells[3].innerHTML;
        role.createDate = row.cells[4].innerHTML;
        role.createUser = row.cells[5].innerHTML;
        role.modifyDate = row.cells[6].innerHTML;
        role.modifyUser = row.cells[7].innerHTML;
        roles.push(role);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("JSON").value = JSON.stringify(roles);
}