function loadMessages(user){
    axios
        .get('/Messages/GetMessages', {
          params: {
            user: 'user'
          }
        })
        .then(function (response) {
          var messages = response.data;
          console.log(messages);
        });

}