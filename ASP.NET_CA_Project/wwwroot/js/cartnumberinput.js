//let inputFields = document.querySelectorAll('input[type="number"]');
window.onload = function () {
    let inputFields = document.getElementsByClassName("count");

    //inputFields.forEach(function (inputField)
    for (var i = 0; i < inputFields.length; i++) {
        inputFields[i].addEventListener('blur', function (event) {
            var inputId = event.target.id;
            var userInput = event.target.value;

            if (userInput == 0) {
                let xhr = new XMLHttpRequest();

                xhr.open("GET", ("/Cart/RemoveItem?itemId=" + inputId));
                xhr.onreadystatechange = function () {
                    console.log(xhr.readystate,xhr.status);
                    if (xhr.status == 200) {
                        location.reload();
                    }
                }
                xhr.send();
            }
            else {
                let xhr = new XMLHttpRequest();

                xhr.open("POST", `/Cart/ChangeItemCount?inputId=${inputId}&userInput=${userInput}`);
                xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
                xhr.onreadystatechange = function () {
                     if (xhr.status == 200) {
                         location.reload();
                     }
                 }
                xhr.send();
            }
        })
    }
}
//});