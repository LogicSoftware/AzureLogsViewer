(function(){
	ko.bindingHandlers.number = {
	    init: function(element, valueAccessor) {
	        var targetValue = valueAccessor();
	        var value = targetValue();

	        var $el = $(element);
            
	        var numberComputed = ko.computed({
	            read: function() {
	                return value;
	            },
	            write: function (newValue) {
	                value = newValue;

	                if (newValue && isNaN(parseInt(newValue, 10))) {
	                    $el.closest('.form-group').addClass("has-error");
                        return;
	                }

	                $el.closest('.form-group').removeClass("has-error");
	                targetValue(newValue ? parseInt(newValue, 10) : null);
	            },
	            disposeWhenNodeIsRemoved: element
	        });

	        var valueBindingInfo = {
	            value: numberComputed
	        };

	        ko.applyBindingsToNode(element, valueBindingInfo);
	    }
	}
})();