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
    };

    EditViewModel.prototype.toServerModel = function() {
        return _.mapObject(this, function (value) {
            return ko.utils.peekObservable(value);
        });
    };

    app.SlackIntegrationViewModel = SlackIntegrationViewModel;
})();