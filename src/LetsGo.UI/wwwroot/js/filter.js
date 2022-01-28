$(document).ready(function () {
    var dates = $('#SelectedDates');
    if (dates.val() === '') {
        $('#filter-date-input-all').prop("checked", true);
    }
    else {
        dates.val().split(',').forEach(function (elem) {
            if (!(parseInt(elem) || 0)) {
                $('#filter-date-input-' + elem).prop('checked', true);
            }
            else {
                $('#filter-date-input-month').prop('checked', true);
            }
        })
    }

    var categories = $('#SelectedCategories');
    categories.val().split(',').forEach(function (elem) {
        var category = $('#category-' + elem);
        if (category.length) {
            category.prop('checked', true);
        }
        else {
            category = $('#subcategory-' + elem);
            category.prop('checked', true);
            category.closest('.subcategories').children().show();
        }
    })
})

function OpenSubcategories(id) {
    var subcategories = $('.subcategories-' + id);
    if (subcategories.is(':visible')) {
        $('.icon-up-' + id).hide();
        $('.icon-down-' + id).show();
        subcategories.hide();
    }
    else {
        $('.icon-down-' + id).hide();
        $('.icon-up-' + id).show();
        subcategories.animate({ opacity: "show" }, "fast");
    }
}

function PressCategoryInput(id) {
    if ($('#category-' + id).is(":checked") && !$('.subcategories-' + id).is(':visible')) {
        $('.subcategories-' + id).animate({ opacity: "show" }, "fast");
        $('.icon-down-' + id).hide();
        $('.icon-up-' + id).show();
    }
}

function PressDateInput() {
    if (
        ($('#filter-date-input-today').is(':checked') || $('#filter-date-input-tomorrow').is(':checked') ||
            $('#filter-date-input-weekend').is(':checked') || $('#filter-date-input-month').is(':checked'))
    ) { $('#filter-date-input-all').prop('checked', false); }
    else { $('#filter-date-input-all').prop('checked', true); }
}

$('#form').on('submit', function () {
    var dates = [];
    var categories = [];
    $('.filter-date-checkbox input:checked').each(function () {
        dates.push($(this).val());
    })
    $('.filter-categories input:checked').each(function () {
        categories.push($(this).val());
    })
    $('#SelectedDates').val(dates.join());
    $('#SelectedCategories').val(categories.join());
})