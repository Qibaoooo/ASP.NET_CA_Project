$(document).ready(function () {

    /* ------------------------ */
    /* START of gallery page js */
    $("#search").on("keyup", function (e) {

        var searchTerm = $(this).val().toLowerCase();

        refreshCards(searchTerm);

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
                console.log(response.redirectController);
                window.location.href = `/${response.redirectController}/Index`;
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
        e.preventDefault();

        var userResponse = window.confirm("Removing item, are you sure?");
        if (userResponse) {

            let itemId = $(this).attr("data-itemId");
            $.ajax({
                type: "POST",
                url: "/Cart/RemoveOrder",
                data: { itemId: itemId },
                success: function (response) {
                    console.log("Item removed successfully!");
                    location.reload();
                },
                error: function (xhr, status, error) {
                    console.error("Error removing item from cart:", error);
                }
            })
        }
    })

    $(".btn-remove-all-cart").click(function (e) {
        e.preventDefault();
        var userResponse = window.confirm("Clearing cart, are you sure?");
        if (userResponse) {
            $.ajax({
                type: "GET",
                url: "/Cart/RemoveAllOrder",
                success: function (response) {
                    console.log("Item removed successfully!");
                    location.reload();
                },
                error: function (xhr, status, error) {
                    console.error("Error removing item from cart:", error);
                }
            })
        }
    })

    //Not sure if it really works, need to try different situaltions
    $(".orderCountInput").on("blur", function (event) {
        var itemId = event.target.id;
        var newCount = event.target.value;

        //Though we set a limit in the input tag, that will not work when user enter number manually,
        //it only works when user uses the arrows to change count, so I think it is necessary here
        if (newCount > 999 || newCount < 0) {
            alert("The quantity should be a number between 0 and 999!");
            location.reload();
        }
        else {
            newCount = Math.round(newCount);//prevent invaild input
            if (newCount == 0) {
                var userResponse = window.confirm("Quantity is set 0, item will be removed. Are you sure?");
                if (userResponse) {
                    $.ajax({
                        type: "POST",
                        url: "/Cart/RemoveOrder",
                        data: { itemId: itemId },
                        success: function (response) {
                            console.log("Item removed successfully!");
                            location.reload();
                        },
                        error: function (xhr, status, error) {
                            console.error("Error removing item from cart:", error);
                        }
                    });
                }
                else {
                    location.reload();
                }
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "/Cart/ChangeItemCount",
                    data: { itemId: itemId, newCount: newCount },
                    success: function (response) {
                        console.log("Item count changed successfully!");
                        location.reload();
                    },
                    error: function (xhr, status, error) {
                        console.error("Error changing item count:", error);
                    }
                });
            }
        }
    });
    /*  END of cart page js  */
    /* --------------------- */
});
