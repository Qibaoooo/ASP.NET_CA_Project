//let inputFields = document.querySelectorAll('input[type="number"]');

let inputFields = document.getElementsByClassName("count");

//inputFields.forEach(function (inputField)
for (var i = 0; i < inputFields.length; i++) {
    inputFields[i].addEventListener('blur', function (event) {
        var inputId = event.target.id;
        var userInput = event.target.value;

        if (userInput == 0) {
            let xhr = new XMLHttpRequest();

            xhr.open("GET", ("/Cart/RemoveItem?itemId="+inputId));
            xhr.onreadystatechange = function () {
                if (xhr.readystate == 4 && xhr.status == 200) {
                    location.reload();
                }
            }
            xhr.send();
            location.reload();
        }
        else {
            var data = {
                inputId: inputId,
                userInput: userInput
            };
            var jsondata = JSON.stringify(data);

            let xhr = new XMLHttpRequest();

            xhr.open("POST", "/Cart/ChangeItemCount");
            xhr.setRequestHeader("Content-Type", "application/json");
            /* xhr.onreadystatechange = function () {
                 if (xhr.readystate == 4 && xhr.status == 201) {
                     //let itemInfo = JSON.parse(xhr.responseText);
                     location.reload();
                 }
             }*/
            xhr.send(jsondata);
        }
    })
}
//});