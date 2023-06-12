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

const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKV1QgZm9yIFRpY1Rha1RvZSIsImp0aSI6IjA5NTg5OTYyLTg2MDAtNDExNi05M2ZjLWNlNjVmOGI1OGU5OCIsImlhdCI6IjYvMTIvMjAyMyA5OjQ5OjUzIEFNIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI5ZmExZDUwYS1mOTM5LTRjY2UtYmIwOS1lYThjNTA4Njc5MWYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidmFzdCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RtYWlsQGdtYWlsLmNvbSIsImV4cCI6MTY4NjU5MzM5MywiaXNzIjoiVGljVGFrVG9lIiwiYXVkIjoiVGljVGFrVG9lIn0.0wy-g6hDx5my_wZbIsAHwj3bHDQBTJwHbAD5Jwvr3M0 ';
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


