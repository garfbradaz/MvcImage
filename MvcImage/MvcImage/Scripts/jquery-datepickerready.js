if (!Modernizr.inputtypes.date) {
    $(function () {
        $(".datefield").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd MM yy',
            yearRange: '-100'
        });
    });
}