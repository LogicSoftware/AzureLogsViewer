(function () {
    // overriding Date.toJSON to serialize local time (original implementation serializes UTC time)
    // we could use replacer parameter of JSON.stringify but unfortunately it calls toJSON first
    Date.prototype.toJSON = function () {
        if (!isFinite(this.valueOf())) {
            return null;
        }

        // trimming the time part away because dateParser/date select returns dates with time
        return new Date(Date.UTC(this.getFullYear(), this.getMonth(), this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds(), this.getMilliseconds())).toISOString();
    };

    isoDateRegex = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(?:\.(\d*))?Z?$/;
    jsonParse = function (json) {
        if (!json) {
            return null;
        }

        function reviver(key, value) {
            if (typeof value === 'string') {
                var date = Date.fromISOString(value);
                if (date) {
                    // treating parsed date as local client date
                    return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds(), date.getUTCMilliseconds());
                }
            }
            return value;
        }

        return JSON.parse(json, reviver);
    };

    app.ajax = {
        postJson: function (url, data, callback) {
            // shift arguments if data argument was omitted
            if (jQuery.isFunction(data)) {
                callback = data;
                data = undefined;
            }

            var options = {
                url: url,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: (data && typeof data !== 'string') ? JSON.stringify(data) : data, // serializing data with own serializer
                dataType: 'json',
                converters: { "text json": jsonParse }, // overriding standard jQuery json parser
                success: callback
            };

            return jQuery.ajax(options);
        }
    };
})();