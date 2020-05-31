// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



// Write your JavaScript code.

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