window.onload = function () {
    let inputFields = document.getElementsByClassName("count");
    //get multiple input boxes, all named count but with different id(actually,id == order.Item.Id)
    var dataBefore = new Array();

    //iterate through inputFields
    for (var i = 0; i < inputFields.length; i++) {
        //add event listener for each input box
        dataBefore[i] = inputFields[i].value;
        inputFields[i].addEventListener('blur', function (event) {
            var inputId = event.target.id;
            var userInput = event.target.value;
            //get user input, as well as itemId
            if (userInput > 999 || userInput < 0) {
                alert("The quantity should be a number between 0 and 999!");
                event.target.value = dataBefore[i];
                location.reload();
            }
            else {

                if (userInput == 0) {
                    //count = 0, remove item

                    var userResponse = window.confirm("Quantity is set 0, are you sure to remove the item?");
                    //alert user about removing

                    if (userResponse) {
                        let xhr = new XMLHttpRequest();

                        xhr.open("GET", ("/Cart/RemoveItem?itemId=" + inputId));
                        //use query string to pass arguments
                        xhr.onreadystatechange = function () {
                            console.log("Item removed");
                            if (xhr.status == 200) {
                                location.reload();
                            }
                        }
                        xhr.send();
                    }
                    else {
                        event.target.value = dataBefore[i];
                        location.reload();
                    }
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
            }
        })
    }
}