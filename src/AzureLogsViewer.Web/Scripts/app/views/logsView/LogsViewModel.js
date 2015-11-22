(function() {
    var LogsViewModel = function (options) {
        this.dataUrl = options.dataUrl;
        this.filter = new app.LogsFilterViewModel();
        this.logs = ko.observable([]);
        this.forceIdFilter = options.id;

        this._loadData = _.debounce(_.bind(this._loadDataImpl, this), 10);
        this.filter.subscribe(this._loadData, this);
        this.loading = ko.observable(false);
        this._loadData();
    };

    _.extend(LogsViewModel.prototype, {
        _loadDataImpl: function() {
            var data = this.filter.toServerModel();

            //if id of item was specified then load only it first time independent of filter state.
            if (this.forceIdFilter) {
                data = {
                    id: this.forceIdFilter
                };
                this.forceIdFilter = null;
            }

            if (this._request)
                this._request.abort();

            this.loading(true);
            this._request = app.ajax.postJson(this.dataUrl, data)
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