$(document).ready(
    function () {
        init();
    }
);

function init() {
    M.AutoInit();
    M.updateTextFields();
    $("#sortableTable").tablesorter();
}

// Function to toggle all checkboxes.
function toggle(source) {
    checkboxes = document.getElementsByName('checkbox');
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        checkboxes[i].checked = source.checked;
    }
}