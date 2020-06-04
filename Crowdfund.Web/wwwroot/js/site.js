// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.
$("#addMedia").click(function () {
    let html = '';
    html += '<div id="inputFormRow">';
    html += '<div class="input-group mb-3">';
    html += '<input type="text" name="url[]" class="form-control m-input" placeholder="Enter URL">';
    html += '<div class="input-group-append">';
    html += '<button id="removeRow" type="button" class="btn btn-danger">Remove</button>';
    html += '</div>';
    html += '</div>';

    $('#newRow').append(html);
});

$(document).on('click', '#removeRow', function () {
    $(this).closest('#inputFormRow').remove();
});

//--------------------Project Page JS-------------------------//
let button = $('.js-backit');

button.on('click', () => {
    let amount = $('.js-amount').val();
    let rewardPackageId = $('.js-reward').val();
    let projectId = $('.js-project').val();

    alert('click');
    
    let data = {
        "Amount": parseFloat(amount),
        "RewardPackageId": parseInt(rewardPackageId),
        "ProjectId": parseInt(projectId)
    };
    alert(JSON.stringify(data));
    $.ajax({
        type: 'POST',
        url: '/Project',
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        alert(data);
        //successAlert.html('Backing with  was created');
        //successAlert.show();
    }).fail(failureResponse => {
        
    });
});                     //  data: JSON.stringify(data)




//--------------------Project Update/Edit JS-------------------------//

let projectUpdateSuccessAlert = $('.js-project-update-success-alert');
projectUpdateSuccessAlert.hide();

let projectUpdateFailAlert = $('.js-project-update-fail-alert');
projectUpdateFailAlert.hide();

let saveProjectUpdateButton = $('.js-project-update-save');

saveProjectUpdateButton.on('click', () => {
    let projectTitle = $('.js-title').val();
    let projectDescription = $('.js-description').val();
    let projectMainImageUrl = $('.js-mainImageUrl').val();
    let projectDueTo = $('.js-dueTo').val();
    let projectGoal = $('.js-goal').val();
    let projectId = $('.js-project-id').val();

    alert('click');
    projectUpdateSuccessAlert.hide();
    projectUpdateFailAlert.hide();

    //if (userEmail == '') {
    //    userFailAlert.show();
    //    return;
    //}

    let projectData = {
        "Title": projectTitle,
        "Description": projectDescription,
        "MainImageUrl": projectMainImageUrl,
        "DueTo": projectDueTo,
        "Goal": parseInt(projectGoal)
    };

    alert(JSON.stringify(projectData));
    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/project/edit/${projectId}`,
        contentType: 'application/json',
        data: JSON.stringify(projectData)
    }).done(data => {
        alert(data);
        projectUpdateSuccessAlert.show();
    }).fail(failureResponse => {
        projectUpdateFailAlert.show();
    });
});