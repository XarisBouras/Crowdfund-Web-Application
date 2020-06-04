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

let rewardAmountButton = $('.js-reward-amount-button');


rewardAmountButton.on('click', (event) => {
    alert('click');
    let clickedElement = $(event.currentTarget);
    let rewardAmount = clickedElement.parent().parent().find('.js-button-amount').text();    
    let amountToSet = clickedElement.parent().parent().find('.js-amount');
    amountToSet.val(rewardAmount);
});


let button = $('.js-backit');

button.on('click', (event) => {
    let clickedElement = $(event.currentTarget);
    let amount = clickedElement.parent().parent().find('.js-amount').val();  
    let rewardPackageId = clickedElement.parent().parent().find('.js-reward').val();
    let projectId = clickedElement.parent().parent().find('.js-project').val();

   

    alert('click');
    
    let data = {
        "Amount": parseInt(amount),
        "RewardPackageId": parseInt(rewardPackageId),
        "ProjectId": parseInt(projectId)
    };

    alert(JSON.stringify(data));

    $.ajax({
        type: 'POST',
        url: `/projects`,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        alert(data);
             
    }).fail(failureResponse => {
        
    }); 
});                    

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
   
    alert(JSON.stringify(userData));

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/edit/${userId}`,
        contentType: 'application/json',
        data: JSON.stringify(userData)
    }).done(data => {
        alert(data);
        userSuccessAlert.show();
    }).fail(failureResponse => {
        userFailAlert.show();
    });
});

//-------------Create Project Form------------------------//

let createProjectSuccessAlert = $('.js-create-project-success-alert');
createProjectSuccessAlert.hide();

let createProjectFailAlert = $('.js-create-project-fail-alert');
createProjectFailAlert.hide();

let createProjectButton = $('.js-create-project-button');

createProjectButton.on('click', () => {
    let createProjectTitle = $('.js-create-project-title').val();
    let createProjectDescription = $('.js-create-project-description').val();
    let createProjectMainImage = $('.js-create-project-image').val();
    let createProjectDueTo = $('.js-create-project-dueto').val();
    let createProjectGoal = $('.js-create-project-goal').val();
    let createProjectCategory = $('.js-create-project-category').val();

    alert('click');
    createProjectSuccessAlert.hide();
    createProjectFailAlert.hide();

    let projectData = {
        "Title": createProjectTitle,
        "Description": createProjectDescription,
        "MainImageUrl": createProjectMainImage,
        "DueTo": createProjectDueTo,
        "Goal": parseInt(createProjectGoal),
        "CategoryId": parseInt(createProjectCategory)
    };

    alert(JSON.stringify(projectData));

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/project/create`,
        contentType: 'application/json',
        data: JSON.stringify(projectData)
    }).done(data => {
        alert(data);        
        createProjectSuccessAlert.show();
    }).fail(failureResponse => {
        createProjectFailAlert.show();
    });
});

