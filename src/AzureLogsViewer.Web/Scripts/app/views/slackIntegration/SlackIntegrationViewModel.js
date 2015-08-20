(function() {
    var SlackIntegrationViewModel = function(options) {
        this.options = options;
        this.infos = ko.observableArray();
        this.loading = ko.observable(true);
        this.editForm = ko.observable();

        this.isListVisible = ko.pureComputed(function() {
            return !this.editForm() && !this.loading();
        }, this);

        this.isEditFormVisible = ko.pureComputed(function() {
            return !!this.editForm();
        }, this);
    };

    _.extend(SlackIntegrationViewModel.prototype, {
        init: function() {
            app.ajax.postJson(this.options.listUrl)
                .done(_.bind(this._setInfos, this));
        },

        create: function() {
            this.editForm(new EditViewModel({
                messagePattern: "{Date}-{Level}-{Role}-{Message}",
                enabled: true
            }));
        },

        edit: function(info) {
            this.editForm(new EditViewModel(info));
        },

        saveEdit: function () {
            if (this.loading())
                return;

            this.loading(true);
            app.ajax.postJson(this.options.saveUrl, this.editForm().toServerModel())
                .done(_.bind(this._onSaved, this));
        },

        _onSaved: function (result) {
            this.editForm(null);

            var oldInfo = _.find(this.infos(), function (x) { return x.id === result.id; });
            if (oldInfo) {
                this.infos.replace(oldInfo, result);
            } else {
                this.infos.push(result);
            }

            this.loading(false);

        },

        cancelEdit: function () {
            if (this.loading())
                return;

            this.editForm(null);
        },

        _setInfos: function(infos) {
            this.infos.push.apply(this.infos, infos);
            this.loading(false);
        }
    });

    var EditViewModel = function (data) {
        data = data || {};

        this.id = data.id || 0;
        this.enabled = ko.observable(data.enabled);
        _.each(["webHookUrl", "chanel", "messagePattern"], function(key) {
            this[key] = ko.observable(data[key] || "");
        }, this);

        this.filter = new FilterViewModel(data.filter || {});
    };

    EditViewModel.prototype.toServerModel = function() {
        return _.mapObject(this, function (value) {
            if (value && value.toServerModel)
                return value.toServerModel();

            return ko.utils.peekObservable(value);
        });
    };


    var FilterViewModel = function (data) {

        this.level = ko.observable(data.level || '');
        this.role = ko.observable(data.role || '');

        this.messageFilters = ko.observableArray(_.map(data.messageFilters || [], function(x) {
            return new app.MessageFilterViewModel(x);
        }));

        this.addMessageItems = [
            { text: "Add LIKE ", action: _.bind(this.addLikeMessage, this) },
            { text: "Add NOT LIKE ", action: _.bind(this.addNotLikeMessage, this) }
        ];

        this.removeMessage = _.bind(this._removeMessage, this);
    };

    _.extend(FilterViewModel.prototype, {
        addLikeMessage: function () {
            this.messageFilters.push(new app.MessageFilterViewModel({ type: "like" }));
            _.last(this.messageFilters()).focused(true);
        },

        addNotLikeMessage: function () {
            this.messageFilters.push(new app.MessageFilterViewModel({ type: "notlike" }));
            _.last(this.messageFilters()).focused(true);
        },

        _removeMessage: function (messageFilter) {
            this.messageFilters.remove(messageFilter);
        },

        toServerModel: function () {
            var result = {
                level: this.level(),
                role: this.role()
            };

            result.messageFilters = _.map(this.messageFilters(), function (item) {
                return item.toServerModel();
            });

            return result;
        }
    });

    app.SlackIntegrationViewModel = SlackIntegrationViewModel;
})();