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