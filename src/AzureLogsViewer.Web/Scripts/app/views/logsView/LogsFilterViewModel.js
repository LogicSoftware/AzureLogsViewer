(function () {
    var dateUtility = app.dateUtility;

    var LogsFilterViewModel = function () {
        this.from = ko.observable(dateUtility.addHours(dateUtility.utcNow(), -2));
        this.to = ko.observable(dateUtility.utcNow());

        this.message = ko.observable();
        this.level = ko.observable();
        this.role = ko.observable('');
        
        this._filterKeys = _.chain(_.keys(this))
                            .value();

        this.messageFilters = ko.observableArray();

        this.addMessageItems = [
            { text: "Add LIKE ", action: _.bind(this.addLikeMessage, this) },
            { text: "Add NOT LIKE ", action: _.bind(this.addNotLikeMessage, this) }
        ];

        this.removeMessage = _.bind(this._removeMessage, this);

        this._serverModel = ko.computed(function() {
            var result = _.reduce(this._filterKeys, function (res, key) {

                res[key] = this[key]();
                return res;
            }, {}, this);

            result.messageFilters = _.map(this.messageFilters(), function(item) {
                return item.toServerModel();
            });

            return result;
        }, this);

        //TODO: don't run change notification if new message filter with empty text was added
        this._serverModel.subscribe(this._onChange, this);
    };

    _.extend(LogsFilterViewModel.prototype, {
        addLikeMessage: function() {
            this.messageFilters.push(new app.MessageFilterViewModel({type: "like"}));
            _.last(this.messageFilters()).focused(true);
        },

        addNotLikeMessage: function () {
            this.messageFilters.push(new app.MessageFilterViewModel({ type:"notlike" }));
            _.last(this.messageFilters()).focused(true);
        },

        _removeMessage: function(messageFilter) {
            this.messageFilters.remove(messageFilter);
        },

        subscribe: function (callback, context) {
            this._onChangeCallback = _.bind(callback, context);
        },

        _onChange: function() {
            if (this._onChangeCallback)
                this._onChangeCallback();
        },

        toServerModel: function() {
            return this._serverModel();
        }
    });

    app.LogsFilterViewModel = LogsFilterViewModel;
})();