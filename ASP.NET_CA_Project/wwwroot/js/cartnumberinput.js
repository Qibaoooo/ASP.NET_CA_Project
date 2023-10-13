var inputFields = document.querySelectorAll('input[type="number"]');
inputField.forEach(function (inputField) {
    inputField.addEventListener('blur', function (event) {
        var inputId = event.target.id;
        var userInput = event.target.value;

        var data = {
            inputId: inputId,
            userInput: userInput
        };
        var jsondata = JSON.stringify(data);

        let xhr = new XMLHttpRequest();

        xhr.open("POST", "/Cart/ChangeItemCount");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.onreadystatechange = function () {
            if (xhr.readystate == 4 && xhr.status == 201) {
                //let itemInfo = JSON.parse(xhr.responseText);
                location.reload();
            }
        }
        xhr.send(jsondata);
    })
});