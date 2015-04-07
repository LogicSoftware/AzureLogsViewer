(function() {
    var LogsViewModel = function () {
        this.filter = new app.LogsFilterViewModel();
        this.logs = ko.observable([]);

        this.filter.onChange(this._loadData, this);
        this._loadData();
    };

    _.extend(LogsViewModel.prototype, {
        _loadData: function() {
            var data = this.filter.toServerModel();
            app.ajax.postJson("/Home/GetLogs", data)
               .done(_.bind(this._setData, this));
        },
        _setData: function(result) {
            this.logs(result);
        }
    });

    app.LogsViewModel = LogsViewModel;
})();