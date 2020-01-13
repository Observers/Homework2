function toggle(source) {
    checkboxes = document.getElementsByName('checkbox');
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        checkboxes[i].checked = source.checked;
    }
}

function clicked() {
    if (confirm('Do you want to Delete?')) {
        yourformelement.submit();
    } else {
        return false;
    }
}
