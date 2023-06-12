
let connection = new signalR.HubConnectionBuilder()
.withUrl("/playroom")
.build();
var currentRoom = '';
connection.on("UpdateGameState", function (game) {
  updateGameField(game);
});
function updateGameField(game) {
  for (var i = 0; i < game.board.fields.length; i++) {
    var fieldIndex = i + 1;
    var field = document.getElementById('field-' + fieldIndex);
    if (game.board.fields[i].fieldValue === 1){
      field.textContent = 'X';
    }
    if (game.board.fields[i].fieldValue === 2){
      field.textContent = 'O';
    }

  }
}
connection.start().then(function () {
  currentRoom = document.getElementById('FirstPlayerName').innerText + '_room' + document.getElementById('gameId').innerText;
          console.log("Соединение установлено.");
          getRooms();
      
          connection.invoke("JoinRoom", currentRoom)
          .then( function (resposne){
            console.log("Success! => Joint to: " + currentRoom);
            connection.invoke("GetCurrentGameState", document.getElementById('gameId').innerText).then(function (game) {
              updateGameField(game);
            }).catch(function (error) {
              console.log('Error: ' + error);
            });
          })
          .catch(function (err) {
              return console.error(err.toString());
          });
      });

      function sendMoveToHub(move) {
        connection.invoke("SendMove", currentRoom, move)        
        .catch(function (err) {
            return console.error(err.toString());
        });
      }
      function getRooms() {
        connection.invoke("GetRooms").then(function (rooms) {
            console.log("Список комнат: " + rooms.join(", "));
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }