define(["ko"],
    function (ko) {
        "use strict";
        var availableCurrencies = ko.observableArray();
        var isLoaded = ko.observable(false);
        var selectedSource = ko.observable();
        var selectedTarget = ko.observable();

        var status = ko.observable();

        var init = function () {
            status("Loading currencies...");
            $.ajax({
                url: "/api/exchange/getcurrencies",
                type: "GET",
                contentType: "application/json"
            })
                .done(function (response) {
                    console.log(response);
                    availableCurrencies(response);
                    isLoaded(true);
                    status("");
                });
        };

        var showData = function () {
            console.log("show data for " + selectedSource() + " and " + selectedTarget());
        };

        return {
            availableCurrencies: availableCurrencies,
            showData: showData,
            selectedSource: selectedSource,
            selectedTarget: selectedTarget,
            status: status,
            init: init,
            isLoaded: isLoaded
        };
    }
);