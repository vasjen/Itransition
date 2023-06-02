$(document).ready(function () {
    var tableBody = $('#tableBody');
    var isLoading = false;
    var pageNumber = 0;
    var seed = parseInt($('#seed').val());
    var rangeInput = document.getElementById('customRange1');
    var textInput = document.getElementById('textInput');
    var region = $('#region').val();
    var currentData = [];
    var lastIndex = 0;
    loadTable(20);

    rangeInput.addEventListener('input', function() {
      textInput.value = rangeInput.value;
    });
    
    
    $(window).scroll(function () {
      if ($(window).scrollTop() + $(window).height() >= $(document).height()) {
        loadTable(10);
      }
    });
  
    $('#region, #textInput, #customRange1, #seed').on('change', function () {
      resetTable();
      loadTable(20);
    });
  
    $('#randomSeed').on('click', function () {
      var randomSeed = Math.floor(Math.random() * 1000);
      $('#seed').val(randomSeed);
      resetTable();
      loadTable(20);
    });
    $('#getCSV').on('click', function()
    {
      createCsv();
    });
    function resetTable() {
      tableBody.empty();
      pageNumber = 0;
      lastIndex = 0;
      
    }
  
    function loadTable(usersCount) {
      if (isLoading) {
        return;
      }
      
      isLoading = true;
      seed = parseInt($('#seed').val());
      var region = $('#region').val();
      var errorCount = $('#textInput').val();
      axios
        .get('/FakeData/GenerateData', {
          params: {
            region: region,
            errorCount: errorCount,
            seed: seed + pageNumber,
            count: usersCount
          }
        })
        .then(function (response) {
          var users = response.data;
          
          if (users.length > 0) {
            $.each(users, function (index, user) {
              currentData.push(user);
              lastIndex++;
             // var counter = (pageNumber) * currentData.length + index + 1;
              var row =
                '<tr>' +
                '<td>' +
                '<div class="d-flex px-2 py-1">' +
                '<div>' +
                '<p class="text-xs text-secondary mb-0">' + lastIndex + '</p>' +
                '</div>' +
                '</div>' +
                '</td>' +
                '<td>' +
                '<p class="text-xs font-weight-bold mb-0">' + user.id + '</p>' +
                '</td>' +
                '<td>' +
                '<div class="d-flex flex-column justify-content-center">' +
                '<p class="text-xs font-weight-bold mb-0">' + user.name + '</p>' +
                '</div>' +
                '</td>' +
                '<td class="align-middle">' +
                '<p class="text-xs font-weight-bold mb-0">' + user.address + '</p>' +
                '</td>' +
                '<td class="align-middle">' +
                '<p class="text-xs font-weight-bold mb-0">' + user.phone + '</p>' +
                '</td>' +
                '</tr>';
              tableBody.append(row);
            });
  
            pageNumber++;
            
          }
  
          isLoading = false;
          
        });
    }
    function createCsv(){
      console.log('Call from createCSV function');
      var url = '/FakeData/CreateCsv';
      fetch(url, {
        method: 'POST',
        headers:{'content-type':'application/json'},
        body: JSON.stringify(currentData)
    })
    .then(response => response.blob())
          .then(blob => {
            let file = window.URL.createObjectURL(blob);
            window.location.assign(file);
  })
    .catch(error => {
        console.error(error);
    });
    }
  });