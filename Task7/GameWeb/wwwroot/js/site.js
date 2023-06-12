$(document).ready(function () {
    $("#userName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/User/Find',
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: { "search": request.term },                
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    response($.map(data, function (item) {
                        return {
                            value: item
                        }
                    }))
                },
                error: function (xhr, textStatus, error) {
                    alert(xhr.statusText);
                    alert(textStatus);
                    alert(error);
                },
                failure: function (response) {
                    alert("failure " + response.responseText);
                }
            });
        },
        select: function (i) { 
            
            $("#userName").attr("value",i.item.value);
            
        },
        minLength: 3
    });
   
});

const token = 'jwtToken';
    const config = {
    headers: { 
        Authorization: `Bearer ${token}`}
    };

function acceptInvite(inviteId){

    axios.put('/Invite/'+inviteId+'/Accept',
    {},
    config)
.then(
    function (response){
        console.log('Start game with id: ' + response.data);
        window.location('/Game/'+ response);
    }
)
}
function declineInvite(inviteId){
    console.log(inviteId)
   
        axios.delete('/Invite/'+inviteId+'/Decline',
        {},
        config)
    .then(
        function (response){
            console.log(response.data);
            location.reload();
        }
    )
}


