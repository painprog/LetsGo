// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const showItem_events = 20, whenClickBtn_events = 5, showItem_organizers = 20, whenClickBtn_organizers = 5;

$('#all-events').btnLoadmore({
    showItem: showItem_events, //default 6
    whenClickBtn: whenClickBtn_events, //default 3
    textBtn: 'Больше мероприятий',
    classBtn: 'btn btn-danger btn-loadmore btn-loadmore-events'
});

$('#all-organizers').btnLoadmore({
    showItem: showItem_organizers, //default 6
    whenClickBtn: whenClickBtn_organizers, //default 3
    textBtn: 'Показать еще',
    classBtn: 'btn btn-danger btn-loadmore btn-loadmore-organizers'
});


$('#events-link').on('click', () => {
    $('#all-events').show();
    $('#all-organizers').hide();
    $('#dropdown').show();
    $('#dd').show();
    $('#options').hide();

    if ($('#all-events').children().length > showItem_events) {
        $('.btn-loadmore-events').show();
    }
    $('.btn-loadmore-organizers').hide();
});

$('#organizers-link').on('click', () => {
    $('#all-events').hide();
    $('#all-organizers').show();
    $('#dropdown').hide();
    $('#dd').hide();
    $('#options').show();

    if ($('#all-organizers').children().length > showItem_events)
        $('.btn-loadmore-organizers').show();
    $('.btn-loadmore-events').hide();
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
