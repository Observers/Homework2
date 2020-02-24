$(document).ready(function () {
    var table = document.getElementsByName("table")
    console.log(x)
    if (x > 1) {
        $('#download').prop('disabled', false)
    }
})

// Function to convert table into JSON.
function GetTableValues() {
    //Create an Array to hold the Table values.
    var users = new Array();

    //Reference the Table.
    var table = document.getElementsByName("table");
    //Loop through Table Rows.
    for (var i = 0; i < table[0].rows.length; i++) {
        //Reference the Table Row.
        var row = table[0].rows[i];

        //Copy values from Table Cell to JSON object.
        var user = {};
        user.user = row.cells[1].innerText;
        user.role = row.cells[2].innerText;
        user.status = row.cells[3].innerText;
        user.createDate = row.cells[4].innerText;
        user.createUser = row.cells[5].innerText;
        user.modifyDate = row.cells[6].innerText;
        user.modifyUser = row.cells[7].innerText;
        users.push(user);
    }

    //Convert the JSON object to string and assign to Hidden Field.
    document.getElementById("JSON").value = JSON.stringify(users);
}