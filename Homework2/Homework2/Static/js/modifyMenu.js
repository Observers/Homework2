$(document).ready(function () {
    $('select').formSelect();
});

$('#selectLinkType').change(function () {
    if ($(this).val() == "2") {
        $('#selectSubMenu').prop("disabled", false);
    }
    else {
        $('#selectSubMenu').prop("disabled", true);
    }
    $('select').formSelect();
})