$(document).ready(function () {
    var table = document.getElementsByName("table")
    var x = table[0].rows.length
    if (x > 1) {
        $('#download').prop('disabled', false)
    }
})

// Function to convert table into JSON.
function GetTableValues() {
    //Create an Array to hold the Table values.
    var roles = new Array();

    //Reference the Table.
    var table = document.getElementsByName("table");
    //Loop through Table Rows.
    for (var i = 0; i < table[0].rows.length; i++) {
        //Reference the Table Row.
        var row = table[0].rows[i];

        //Copy values from Table Cell to JSON object.
        var role = {};
        role.role = row.cells[1].innerText;
        role.roleDescription = row.cells[2].innerText;
        role.status = row.cells[3].innerText;
        role.createDate = row.cells[4].innerText;
        role.createUser = row.cells[5].innerText;
        role.modifyDate = row.cells[6].innerText;
        role.modifyUser = row.cells[7].innerText;
        roles.push(role);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("JSON").value = JSON.stringify(roles);
}