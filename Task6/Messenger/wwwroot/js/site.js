$(document).ready(function () {
    $("#receiver").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/User/FindReceivers',
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                data: { "search": request.term },                
                dataType: "json",
                success: function (data) {
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
        select: function (e, i) { 
            
            $("#receiver").attr("value",i.item.value);
            
        },
        minLength: 1
    });
   
});
