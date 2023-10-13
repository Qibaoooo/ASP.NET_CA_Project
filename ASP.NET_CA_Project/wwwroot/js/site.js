// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    $("#search").keydown(function (e) {
        if (e.keyCode === 13) { // Check if Enter key was pressed
            e.preventDefault(); // Prevent the form from submitting
            var searchTerm = $(this).val().toLowerCase();

            refreshCards(searchTerm);
        }
    });


    $("#LoginForm").on("submit", function (e){
        e.preventDefault();
        //Calling login action method
        console.log("Login Clicked");
    }

    function refreshCards(searchTerm) {
        $("#item-cards").children().each(function (index, element) {
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
                $('#cart-badge').html(parseInt($('#cart-badge').html(), 10) + 1)
                $('#cart-badge').css('visibility', 'visible');
            },
            error: function (xhr, status, error) {
                console.error("Error adding item to cart:", error);
            }
        });
    });

});