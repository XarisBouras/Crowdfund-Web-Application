﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

let backSuccessAlert = $('.js-back-success-alert');
backSuccessAlert.hide();

let backFailAlert = $('.js-back-fail-alert');
backFailAlert.hide();

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
    var amount = clickedElement.parent().parent().find('.js-amount').val();  
    let rewardPackageId = clickedElement.parent().parent().find('.js-reward').val();
    let projectId = clickedElement.parent().parent().find('.js-project').val();

    alert('click');
    backSuccessAlert.hide();
    backFailAlert.hide();

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
       // document.querySelector('.js-create-project-form').reset();
        $('.js-back-modal').modal('show');
      //  $('.js-user-modal').modal('hide');
      //  backSuccessAlert.show().delay(3000);
      //  backSuccessAlert.fadeOut();
             
    }).fail(failureResponse => {
        backFailAlert.show().delay(3000);
        backFailAlert.fadeOut();
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
   
    userSuccessAlert.hide();
    userFailAlert.hide();    

    let userData = {
        "Email": userEmail,
        "FirstName": userFirstName,
        "LastName": userLastName,
        "Address": userAddress
    };   

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/edit/${userId}`,
        contentType: 'application/json',
        data: JSON.stringify(userData)
    }).done(data => {
        userSuccessAlert.show().delay(3000);
        userSuccessAlert.fadeOut();
    }).fail(failureResponse => {
        userFailAlert.text(failureResponse.responseText);
        userFailAlert.show().delay(3000);
        userFailAlert.fadeOut();
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

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/project/create`,
        contentType: 'application/json',
        data: JSON.stringify(projectData)
    }).done(data => {
        document.querySelector('.js-create-project-form').reset();
        createProjectSuccessAlert.show().delay(3000);
        createProjectSuccessAlert.fadeOut();
    }).fail(failureResponse => {
        createProjectFailAlert.text(failureResponse.responseText);
        createProjectFailAlert.show().delay(3000);
        createProjectFailAlert.fadeOut();
    });
});


//--------------Create Post--------//

$('.js-create-post-success-alert').hide();
$('.js-create-post-fail-alert').hide();

$('.js-createpost').on('click', () => {
    let title = $('.js-title').val();
    let projectId = $('.js-project').val();
    let description = $('.js-description').val();
    
    let data = {
        "Title": title,
        "ProjectId": parseInt(projectId),
        "Text": description
    };

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/post/project/${projectId}`,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        document.querySelector('.js-create-post-form').reset();
        $('.js-create-post-success-alert').show().delay(3000);
        $('.js-create-post-success-alert').fadeOut();
    }).fail(failureResponse => {
        $('.js-create-post-fail-alert').text(failureResponse.responseText);
        $('.js-create-post-fail-alert').show().delay(3000);
        $('.js-create-post-fail-alert').fadeOut();
    });
});


//-------Create Reward Package----------//

$('.js-create-reward-success-alert').hide();
$('.js-create-reward-fail-alert').hide();

$('.js-createrewardpackage').on('click', () => {
    let title = $('.js-title').val();
    let amount = $('.js-amount').val();
    let quantity = $('.js-quantity').val();
    let description = $('.js-description').val();
    let projectId = $('.js-projectId').val();

    let data = {
        "Title" : title,
        "ProjectId": parseInt(projectId),
        "Description": description,
        "MinAmount": parseInt(amount),
        "Quantity" : quantity ? parseInt(quantity) : null
    };

    $.ajax({
        type: 'POST',
        url: `/Dashboard/User/reward/project/${projectId}`,
        contentType: 'application/json',
        data: JSON.stringify(data)
    }).done(data => {
        document.querySelector('.js-create-reward-form').reset();
        $('.js-create-reward-success-alert').show().delay(3000);
        $('.js-create-reward-success-alert').fadeOut();
    }).fail(failureResponse => {
        $('.js-create-reward-fail-alert').text(failureResponse.responseText);
        $('.js-create-reward-fail-alert').show().delay(3000);
        $('.js-create-reward-fail-alert').fadeOut();
    });
});
