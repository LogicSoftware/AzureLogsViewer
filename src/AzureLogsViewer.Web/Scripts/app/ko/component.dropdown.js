(function() {

    ko.components.register('actionsbtn', {
        viewModel: {
            createViewModel: function (params, componentInfo) {
                var vm = {
                    items: params.items,
                    title: params.title || ""
                };
                var $el = $(componentInfo.element).find('.dropdown-menu');
                $el.on("click", "a", function (e) {
                    e.preventDefault();
                    var item = ko.dataFor(this);

                    if (!item)
                        throw new Error("Cant find item for element");

                    if (!item.action)
                        throw new Error("No action for item");

                    item.action();
                });

                return vm;
            },
        },
        template: '<div class="dropdown">\
                    <a href="#" data-toggle="dropdown" class="btn btn-default btn-sm dropdown-toggle"><span data-bind="text:title"></span><b class="caret"></b></a>\
                    <ul class="dropdown-menu" data-bind="foreach:items">\
                        <li><a href="#" data-bind="text:text, attr"></a></li>\
                    </ul>\
                </div>',
        synchronous: true
    });
})();