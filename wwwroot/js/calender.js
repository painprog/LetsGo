let calendar = document.querySelector('.calendar')

let month_names = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь']

isLeapYear = (year) => {
    return (year % 4 === 0 && year % 100 !== 0 && year % 400 !== 0) || (year % 100 === 0 && year % 400 === 0)
}

getFebDays = (year) => {
    return isLeapYear(year) ? 29 : 28
}

generateCalendar = (month, year) => {
    let calendar_days = calendar.querySelector('.calendar-days')
    let calendar_header_year = calendar.querySelector('#year')

    let days_of_month = [31, getFebDays(year), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]

    calendar_days.innerHTML = ''

    let currDate = new Date()
    if (!month) month = currDate.getMonth()
    if (!year) year = currDate.getFullYear()

    let curr_month = `${month_names[month]}`
    month_picker.innerHTML = curr_month
    calendar_header_year.innerHTML = year

    // get first day of month

    let first_day = new Date(year, month, 1)

    for (let i = 0; i <= days_of_month[month] + first_day.getDay() - 1; i++) {
        let day = document.createElement('div')
        if (i >= first_day.getDay()) {
            day.classList.add('calendar-day-hover')
            day.innerHTML = i - first_day.getDay() + 1
            if (i - first_day.getDay() + 1 === currDate.getDate() && year === currDate.getFullYear() && month === currDate.getMonth()) {
                day.style.fontWeight = 800; //today 
            }
        }
        calendar_days.appendChild(day)
    }
}

let month_list = calendar.querySelector('.month-list')

month_names.forEach((e, index) => {
    let month = document.createElement('div')
    month.innerHTML = `<div data-month="${index}">${e}</div>`
    month.querySelector('div').onclick = () => {
        month_list.classList.remove('show')
        curr_month.value = index
        generateCalendar(index, curr_year.value)
    }
    month_list.appendChild(month)
})

let month_picker = calendar.querySelector('#month-picker')

month_picker.onclick = () => {
    month_list.classList.add('show')
    $('.choose-date-btn').text('Выберите дату');
    $('.choose-date-btn').addClass('btn-secondary disabled');
}

let currDate = new Date()

let curr_month = { value: currDate.getMonth() }
let curr_year = { value: currDate.getFullYear() }

generateCalendar(curr_month.value, curr_year.value)

document.querySelector('#prev-year').onclick = () => {
    --curr_year.value
    generateCalendar(curr_month.value, curr_year.value)
}

document.querySelector('#next-year').onclick = () => {
    ++curr_year.value
    generateCalendar(curr_month.value, curr_year.value)
}

$('.calendar[data-handledropdownclose="true"]').on("click.bs.dropdown", function (e) {
    if ($(this).parent().hasClass("show")) {
        var target = $(e.target);
        if (!(target.hasClass("CloseDropDown") || target.parents(".CloseDropDown").length)) {
            e.stopPropagation();
        }
    }
});

$('.choose-date-btn').on("click", function () {
    if (!$(this).hasClass('disabled')) {
        let day = $('.curr-date').text(), year = $('#year').text(), month = $('#month-picker').text();
        console.log(day, month, year);

        window.location.href = 'Event/AfishaOn?year=' + year + '&month=' + month + '&day=' + day;
    } 
})
$('.calendar-body').delegate('.calendar-days .calendar-day-hover', 'click', function () {
    $('.calendar-days').find('.curr-date').removeClass('curr-date');
    $(this).addClass('curr-date');
    let day = $(this).text(), year = $('#year').text(), month = $('#month-picker').text();
    console.log(day + 'day' + month + 'month' + year + 'year');

    $.ajax({
        type: "GET",
        url: "/Event/GetEventsQty",
        data: { "year": year, "month": month, "day": day }
    }).done(function (data) {
        if (data >= 1) {
            $('.choose-date-btn').removeClass('btn-secondary disabled');
            $('.choose-date-btn').addClass('btn-dark');
            if (data == 1) {
                $('.choose-date-btn').text('Показать ' + data + ' событие');
            }
            else {
                $('.choose-date-btn').text('Показать ' + data + ' событий');
            }
        }
        else {
            if (!$('.choose-date-btn').hasClass('disabled')) {
                $('.choose-date-btn').addClass('btn-secondary disabled');
            }
            $('.choose-date-btn').text('Событий нет');
        }
    });
});

$('.dropbtn').click(function () {
    $('html, body').animate({
        scrollTop: $(".dropbtn").offset().top
    }, 600);
});

$('.calendar-days').delegate('.calendar-day-hover', 'click', function () {
    console.log('ыыыы');
});