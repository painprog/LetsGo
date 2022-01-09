$(document).ready(function () {
    $('#locations').select2({
        placeholder: "Выберите место проведения",
        allowClear: true,
        theme: "bootstrap-5"
    });
});

let startpicker = flatpickr('#eventStart', {
    altInput: false,
    enableTime: true,
    dateFormat: "Y-m-d H:i",
    minDate: "today",
    "locale": "ru",
    onClose: function (selectedDates, dateStr, instance) {
        endpicker.set('minDate', dateStr);
    },
});

let endpicker = flatpickr('#eventEnd', {
    altInput: false,
    enableTime: true,
    dateFormat: "Y-m-d H:i",
    minDate: "today",
    "locale": "ru",
    onClose: function (selectedDates, dateStr, instance) {
        startpicker.set('maxDate', dateStr);
    }
});