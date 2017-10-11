define(["ko"],
    function (ko) {
        "use strict";
        var availableCurrencies = ko.observableArray(["HUF", "EUR"]);
        var selectedSource = ko.observable();
        var selectedTarget = ko.observable();

        var showData = function () {
            console.log("show data for " + selectedSource() + " and " + selectedTarget());
        }

        return {
            availableCurrencies: availableCurrencies,
            showData: showData,
            selectedSource: selectedSource,
            selectedTarget: selectedTarget
        };
    }
);