(function () {
    var dateUtility = app.dateUtility;

    var LogsFilterViewModel = function () {
        this.from = ko.observable(dateUtility.now());
        this.to = ko.observable(dateUtility.addHours(this.from(), -2));

        this.message = ko.observable();
        this.level = ko.observable();
        this.role = ko.observable('');

        this._filterKeys = _.keys(this);

    };

    _.extend(LogsFilterViewModel.prototype, {
        onChange: function(callback, context) {
            _.each(this._filterKeys, function(key) {
                this[key].subscribe(callback, context);
            }, this);
        },

        toServerModel: function() {
            return  _.reduce(this._filterKeys, function(res, key) {

                res[key] = this[key]();
                return res;
            }, {}, this);
        }
    });

    app.LogsFilterViewModel = LogsFilterViewModel;
})();