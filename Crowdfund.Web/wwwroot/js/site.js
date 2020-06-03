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


//-------------Update User Profile--------------//

let userSuccessAlert = $('.js-user-profile-success-alert');
userSuccessAlert.hide();

let userFailAlert = $('.js-user-profile-fail-alert');
userFailAlert.hide();

let saveUserProfileButton = $('.js-user-profile-save');

saveUserProfileButton.on('click', () => {
    let userFirstName = $('.js-firstName').val();
    let userLastName = $('.js-lastName').val();
    let userAddress = $('.js-address').val();
    let userEmail = $('.js-email').val();
    let userId = $('.js-user-id').val();

    alert('click');
    userSuccessAlert.hide();
    userFailAlert.hide();

    if (userEmail == '') {
        userFailAlert.show();
        return;
    }

    let userData = {
        "Email": userEmail,
        "FirstName": userFirstName,
        "LastName": userLastName,
        "Address": userAddress
    };
    debugger;
    alert(JSON.stringify(userData));
    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/edit/${userId}`,
        contentType: 'application/json',
        data: JSON.stringify(userData)
    }).done(data => {
        alert(data);
       // successAlert.html('User options saved successfully.');
        userSuccessAlert.show();
    }).fail(failureResponse => {
        userFailAlert.show();
    });
});