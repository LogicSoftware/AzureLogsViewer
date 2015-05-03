(function() {
    var LogsViewModel = function () {
        this.filter = new app.LogsFilterViewModel();
        this.logs = ko.observable([]);

        this._loadData = _.debounce(_.bind(this._loadDataImpl, this), 10);
        this.filter.subscribe(this._loadData, this);
        this.loading = ko.observable(false);
        this._loadData();
    };

    _.extend(LogsViewModel.prototype, {
        _loadDataImpl: function() {
            var data = this.filter.toServerModel();
            if (this._request)
                this._request.abort();

            this.loading(true);
            this._request = app.ajax.postJson("Home/GetLogs", data)
               .done(_.bind(this._setData, this));
        },
        _setData: function (result) {
            this._request = null;
            this.loading(false);

            this.logs(result);
        }
    });

    app.LogsViewModel = LogsViewModel;
})();