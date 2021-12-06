// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


function filterEvents() {
    var input, filter, cards, cardContainer, h5, i, title, location, eventStatus;
    input = document.getElementById("searchbox-input");
    filter = input.value.toUpperCase();
    cardContainer = document.getElementById("all-events");
    cards = cardContainer.getElementsByClassName("card");
    for (i = 0; i < cards.length; i++) {
        title = cards[i].querySelector(".pn-head_title h1");
        location = cards[i].querySelector(".pn-head_place");
        eventStatus = cards[i].querySelector("#event-status");
        if (title.innerText.toUpperCase().indexOf(filter) > -1
            || location.innerText.toUpperCase().indexOf(filter) > -1
            || eventStatus.innerText.toUpperCase().indexOf(filter) > -1) {
            cards[i].style.display = "";
        } else {
            cards[i].style.display = "none";
        }
    }
}

$('#all-events').btnLoadmore({
    showItem: 20, //default 6
    whenClickBtn: 5, //default 3
    textBtn: 'Больше мероприятий',
    classBtn: 'btn btn-danger btn-loadmore'
});
// Write your JavaScript code.
