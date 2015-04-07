(function() {

    var counter = 0;

    ko.components.register('datetimepicker', {
        viewModel: {
            createViewModel: function (params, componentInfo) {
                var vm = {
                    value: params.value,
                    label: params.label,
                    id: "ko_datepicker_" + counter++
            };
                var $el = $(componentInfo.element).find('.input-group.date');
                $el.datetimepicker();
                if (vm.value())
                    $el.data("DateTimePicker").date(vm.value());

                $el.on("dp.change", function (e) {
                    vm.value(e.date && e.date.toDate());
                });

                return vm;
            },
        },
        template: "<div class='form-group'>\
                        <label data-bind='attr: { for: id },text: label'>Name</label>\
                  </div>\
                  <div class='form-group'>\
                        <div class='input-group date'>\
                        <input type='text' class='form-control' data-bind='attr: { id: id }' />\
                        <span class='input-group-addon'>\
                            <span class='glyphicon glyphicon-calendar'></span>\
                        </span>\
                    </div>\
                </div>",
        synchronous: true
    });
})();