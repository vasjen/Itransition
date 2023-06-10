"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();


var connectionId = "";

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
   
    
        var row = ' <div class="avatar me-3"> '
        + ' <img src="../assets/img/kal-visuals-square.jpg" alt="kal" class="border-radius-lg shadow"> '
        + '</div>'
        + '<div class="d-flex align-items-start flex-column justify-content-center">' 
        + '<h6 class="mb-0 text-sm">' + user + '</h6>'
        + '<p class="mb-0 text-xs">' + msg + '</p>'
        + '</div>'
        + '<a class="btn btn-link pe-3 ps-0 mb-0 ms-auto w-25 w-md-auto" onclick="toggleReply(this)">Reply</a>';
      
        li.innerHTML = row;
        li.classList.value = "list-group-item border-0 d-flex align-items-center px-0 mb-2";
  
    document.getElementById("messagesList").prepend(li);
});

connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (id) {
        connectionId = id;
        console.log('connectionId: ' + connectionId)
        updateConnectionId();
    });
}).catch(function (err) {
    return console.error(err.toString());
});


function updateConnectionId(){
    var user = document.getElementById("userName").innerText;
  axios.post('/User/CheckAndUpdateUser' ,{
          user: user,
          connectionId: connectionId
   })
  .then(function (response) {
    console.log(response.data);
  })
  .catch(function (error) {
    console.error(error);
  });
};
function sendReply(){
  var sender = document.getElementById("userName").innerText;
  var receiver = document.getElementById('messageSender').innerText;
 var message = document.getElementById("validationTextarea").value;
  if(connection.state != 'Connected'){
    connection.start().then( function (){
      connection.invoke("SendPersonal", sender, receiver, message)
      .catch(function (err) {
        return console.error(err.toString());
      })});
  }
  else

  connection.invoke("SendPersonal", sender, receiver, message)
  .catch(function (err) {
      return console.error(err.toString());
  }
  );

};
function sendMessage()
{
    var sender = document.getElementById('userName').innerText;
    var receiver = document.getElementById('receiver').value;
    var message = document.getElementById("TextArea").value;
    if(connection.state != 'Connected'){
      connection.start().then( function (){
      connection.invoke("SendPersonal", sender, receiver, message)
      .catch(function (err) {
          return console.error(err.toString());
      })}
    );
}
else
{
  connection.invoke("SendPersonal", sender, receiver, message)
      .catch(function (err) {
          return console.error(err.toString());
      });
}
}
