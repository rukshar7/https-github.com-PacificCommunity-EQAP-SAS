function deleteCountry(roleId) {

    var confirmMessageBox = confirm('Are you sure you wish to delete this record ?');
    if (confirmMessageBox) {
        window.location = "/Admin/DeleteCountry/" + roleId;
    }
    else {
        return false;
    }
}

function editCountry(countryName,active,countryid) {

    $("#Id").val(countryid);
    $("#Name").val(countryName.trim());
    if (active == 'true') {
        $("#Active").prop('checked', true);
    }
    else
    {
        $("#Active").prop('checked', false);
    }
}

function editSchool(SchoolName, countryid,schoolId,province,authorityBody) {

    $("#Id").val(schoolId);
    $("#Name").val(SchoolName.trim());
    $("#CountryId").val(countryid);
    $("#AuthorityBody").val(authorityBody.trim());
    $("#Province").val(province.trim());
}

function editQualification(qualificationName, qualificationid, acronym) {

    $("#Id").val(qualificationid);
    $("#Name").val(qualificationName.trim());
    $("#Acronym").val(acronym.trim());
}

function editCourse(courseName, courseid,active ,shortName) {

    $("#Id").val(courseid);
    $("#Name").val(courseName.trim());
    $("#code").val(shortName.trim());
    if (active == 'true') {
        $("#Active").prop('checked', true);
    }
    else {
        $("#Active").prop('checked', false);
    }
}

function editClassLevel(description, active, classLevelId) {

    $("#Id").val(classLevelId);
    $("#Description").val(description.trim());
    if (active == 'true') {
        $("#Active").prop('checked', true);
    }
    else {
        $("#Active").prop('checked', false);
    }
}
function editLSA(lSAName, active, lSAid) {

    $("#Id").val(lSAid);
    $("#Name").val(lSAName.trim());
    if (active == 'true') {
        $("#Active").prop('checked', true);
    }
    else {
        $("#Active").prop('checked', false);
    }
}