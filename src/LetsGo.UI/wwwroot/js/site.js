// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$('#all-events').btnLoadmore({
    showItem: 20, //default 6
    whenClickBtn: 5, //default 3
    textBtn: 'Больше мероприятий',
    classBtn: 'btn btn-danger btn-loadmore'
});

$('#all-organizers').btnLoadmore({
    showItem: 20, //default 6
    whenClickBtn: 5, //default 3
    textBtn: 'Показать еще',
    classBtn: 'btn btn-danger btn-loadmore'
});

function filter() {
    filterEvents();
    filterUsers();
}

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

function filterUsers() {
    var input, filter, cards, cardContainer, i, organaizerName, organaizerEmail, organaizerPhone;
    input = document.getElementById("searchbox-input");
    filter = input.value.toUpperCase();
    cardContainer = document.getElementById("all-organizers");
    cards = cardContainer.getElementsByClassName("card");
    for (i = 0; i < cards.length; i++) {
        organaizerName = cards[i].querySelector("#organizer-name").innerText;
        organaizerPhone = cards[i].querySelector("#organizer-phone").innerText;
        organaizerEmail = cards[i].querySelector("#organizer-email").innerText;

        if (organaizerName.toUpperCase().indexOf(filter) > -1
            || organaizerEmail.toUpperCase().indexOf(filter) > -1
            || organaizerPhone.toUpperCase().indexOf(filter) > -1) {
            cards[i].style.display = "";
        } else {
            cards[i].style.display = "none";
        }
    }
}
// Write your JavaScript code.
