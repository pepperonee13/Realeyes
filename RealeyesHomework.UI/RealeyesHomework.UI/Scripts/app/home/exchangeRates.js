define(["ko", "rickshaw"],
    function (ko) {
        "use strict";
        var availableCurrencies = ko.observableArray();
        var isLoaded = ko.observable(false);
        var selectedSource = ko.observable();
        var selectedTarget = ko.observable();

        var status = ko.observable();
        var graph = undefined;
        var legend = undefined;

        var initGraph = function() {
            graph = new Rickshaw.Graph({
                element: document.querySelector("#chart"),
                renderer: 'line',
                series: [
                    {
                        data: [{ x: 0, y: 40 }, { x: 1, y: 49 }],
                        color: 'steelblue',
                        name: "line 1"
                    }, {
                        data: [{ x: 0, y: 20 }, { x: 1, y: 24 }],
                        color: 'lightblue',
                        name: "line 2"
                    }
                ]
            });

            legend = new Rickshaw.Graph.Legend({
                graph: graph,
                element: document.querySelector('#bootstrap-overrides-legend')
            });

            var highlighter = new Rickshaw.Graph.Behavior.Series.Highlight({
                graph: graph,
                legend: legend
            });
        };

        var loadCurrencies = function () {
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

        var init = function () {
            loadCurrencies();
        };

        var showData = function () {
            console.log("show data for " + selectedSource() + " and " + selectedTarget());
            if (!graph) {
                initGraph();
            }
            graph.render();
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