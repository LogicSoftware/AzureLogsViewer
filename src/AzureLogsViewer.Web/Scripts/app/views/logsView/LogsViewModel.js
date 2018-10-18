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

        this.affectedUsersMsg = ko.observable(null);
        this.isAffectedUsersVisible = ko.pureComputed(function() {
            return this.affectedUsersMsg() !== null;
        }, this);
        this.toggleAffectedUsersBtnTitle = ko.pureComputed(function() {
            return this.isAffectedUsersVisible() ? "Hide affected users" : "Show afffected users";
        }, this);
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
        },

        toggleAffectedUsersMsg: function () {

            if (this.isAffectedUsersVisible()) {
                this.affectedUsersMsg(null);
                return;
            }

            var result = {};
            _.each(this.logs(), function(log) {
                var match = /UserId =(\d*), Login =.*\n?\s*AccountId=(\d*),/gi.exec(log.message);
                if (match) {

                    var userId = match[1];
                    var accountId = match[2];
                    var key = accountId + "_" + userId;

                    if (!result[key]) {
                        result[key] = {
                            userId: userId,
                            accountId: accountId
                        };
                    }
                }
            });

            var users = _.chain(result)
                        .values()
                        .map(function(x) {
                            return "( u.AccountId = " + x.accountId + " AND u.UserId = " + x.userId + ")";
                        })
                        .value();

            var msg = users.join(" OR <br />");
            if (!msg) {
                msg = "1 = 0";
            }
            this.affectedUsersMsg(msg);

        }
    });

    app.LogsViewModel = LogsViewModel;
})();