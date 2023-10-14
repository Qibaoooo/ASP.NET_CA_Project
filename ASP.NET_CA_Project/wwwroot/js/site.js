$(document).ready(function () {

    /* ------------------------ */
    /* START of gallery page js */
    $("#search").keydown(function (e) {
        if (e.keyCode === 13) {
            // Check if Enter key was pressed
            e.preventDefault(); // Prevent the form from submitting
            var searchTerm = $(this).val().toLowerCase();

            refreshCards(searchTerm);
        }
    });

    function refreshCards(searchTerm) {
        $("#item-cards")
            .children()
            .each(function (index, element) {
                // for each item card, we check if
                // its text and title contains the search term.
                // if not, we hide it.

                var text = $(this).find(".card-text").text().toLowerCase();
                var title = $(this).find(".card-title").text().toLowerCase();
                $(this).show();

                if (searchTerm === "") {
                    return;
                }

                if (!(text.includes(searchTerm) || title.includes(searchTerm))) {
                    $(this).hide();
                }
            });
    }

    $(".btn-add-to-cart").click(function (e) {
        e.preventDefault(); // Prevent the default link behavior

        let itemId = $(this).attr("data-itemId");

        $.ajax({
            type: "POST",
            url: "/Gallery/AddToCart",
            data: { itemId: itemId },
            success: function (response) {
                console.log("Item added to cart successfully!");
                $("#cart-badge").html(parseInt($("#cart-badge").html(), 10) + 1);
                $("#cart-badge").css("visibility", "visible");
            },
            error: function (xhr, status, error) {
                console.error("Error adding item to cart:", error);
            },
        });
    });
    /* END of gallery page js */
    /* ---------------------- */


    /* ---------------------- */
    /* START of login page js */
    $("#LoginForm").submit(function (e) {
        e.preventDefault();

        let username = $("#username").val();
        let password = $("#password").val();
        let mergeCart = $('#mergeCartCheck')[0].checked;

        callLoginAction(username, password, mergeCart);
    });

    function callLoginAction(username, password, mergeCart) {
        $.ajax({
            type: "POST",
            url: "/Login/UserLogin",
            data: {
                userName: username,
                password: password,
                mergeCart: mergeCart
            },
            success: function (response) {
                console.log("Login success!");
                window.location.href = "/Gallery/Index";
            },
            error: function (data) {
                console.log(data.responseText);

                $("#loginToast").children(".toast-body").text(data.responseText);
                var loginToast = new bootstrap.Toast($("#loginToast"));
                loginToast.show();
            },
        });
    }

    /* END of login page js */
    /* -------------------- */


    /* --------------------- */
    /* START of cart page js */
    $(".btn-remove-cart").click(function (e) {
        let itemId = $(this).attr("data-itemId");

        $.ajax({
            type: "POST",
            url: "/Cart/RemoveOrder",
            data: { itemId: itemId },
            success: function (response) {
                console.log("Item removed successfully!");
                window.location.href = "/Cart/Index";
            },
            error: function (xhr, status, error) {
                console.error("Error removing item from cart:", error);
            },
        });
    });

    let inputFields = document.getElementsByClassName("orderCountInput");

    // TODO: rewrite below function to use AJAX
    for (var i = 0; i < inputFields.length; i++) {
        inputFields[i].addEventListener("blur", function (event) {
            var itemId = event.target.id;
            var newCount = event.target.value;

            if (newCount == 0) {
                let xhr = new XMLHttpRequest();

                xhr.open("POST", "/Cart/RemoveOrder?itemId=" + itemId);
                xhr.onreadystatechange = function () {
                    console.log(xhr.readystate, xhr.status);
                    if (xhr.status == 200) {
                        location.reload();
                    }
                };
                xhr.send();
            } else {
                let xhr = new XMLHttpRequest();

                xhr.open(
                    "POST",
                    `/Cart/ChangeItemCount?itemId=${itemId}&newCount=${newCount}`
                );
                xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
                xhr.onreadystatechange = function () {
                    if (xhr.status == 200) {
                        location.reload();
                    }
                };
                xhr.send();
            }
        });
    }
    /* END of cart page js */
    /* ------------------- */


});
