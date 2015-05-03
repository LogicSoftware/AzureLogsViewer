(function () {

    // toISOString polyfill for older browsers
    if (!Date.prototype.toISOString) {
        function pad(number) {
            if (number < 10) {
                return '0' + number;
            }
            return number;
        }

        Date.prototype.toISOString = function () {
            return this.getUTCFullYear() +
                '-' + pad(this.getUTCMonth() + 1) +
                '-' + pad(this.getUTCDate()) +
                'T' + pad(this.getUTCHours()) +
                ':' + pad(this.getUTCMinutes()) +
                ':' + pad(this.getUTCSeconds()) +
                '.' + (this.getUTCMilliseconds() / 1000).toFixed(3).slice(2, 5) +
                'Z';
        };
    }

    // IE/FF/Chrome handle ISO date format in Date.parse inconsistently so we need to have our own implementation
    var isoDateRegex = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(?:\.(\d*))?Z?$/;

    Date.fromISOString = function (date) {
        var parts = isoDateRegex.exec(date);
        if (parts) {
            var ms = +((parts[7] || "") + "000").substring(0, 3); // handling 1/10 and 1/100 and trimming extra digits (may come from C# DateTime)

            return new Date(Date.UTC(+parts[1], +parts[2] - 1, +parts[3], +parts[4], +parts[5], +parts[6], ms)); // always UTC
        }

        return null;
    };

    app.dateUtility = {
        now: function() {
            return new Date();
        },

        addHours: function(source, hours) {
            return new Date(source.getTime() + (hours * 60 * 60 * 1000));
        },

        formatDateTime: function(dateTime) {
            return moment(dateTime).format("MMMM Do, HH:mm:ss");
        },

        utcNow: function() {
            var now = this.now();
            return new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(),
                  now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
        }
    };


})();