(function() {
    var MessageFilterViewModel = function(opts) {
        opts = opts || {};

        this.text = ko.observable(opts.text || "");
        this.type = opts.type || "like";
        this.focused = ko.observable(false);
    };

    _.extend(MessageFilterViewModel.prototype, {
        toServerModel: function() {
            return {
                text: this.text(),
                type: this.type
            };
        }
    });

    app.MessageFilterViewModel = MessageFilterViewModel;
})();