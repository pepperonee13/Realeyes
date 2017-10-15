define(["ko", "rickshaw"],
    function (ko) {
        "use strict";
        var availableCurrencies = ko.observableArray();
        var isLoaded = ko.observable(false);
        var selectedSource = ko.observable();
        var selectedTarget = ko.observable();

        var status = ko.observable();
        var graph = undefined;

        var seriesData = [];
        var createGraph = function () {
            graph = new Rickshaw.Graph({
                element: document.querySelector("#chart"),
                renderer: 'line',
                series: [
                    {
                        data: seriesData,
                        color: 'steelblue'
                    }
                ],
                min: "auto"
            });

            new Rickshaw.Graph.Axis.Y({
                graph: graph
            });

            var hoverDetail = new Rickshaw.Graph.HoverDetail({
                graph: graph,
                xFormatter: function (timestamp) {
                    return new Date(timestamp).toLocaleDateString();
                }
            });

            graph.render();
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

        var loadExchangeData = function (onLoaded) {
            $.ajax({
                url: "/api/exchange/getexchangerate?fromCurrency=" + selectedSource() + "&toCurrency=" + selectedTarget(),
                type: "GET",
                contentType: "application/json"
            })
                .done(function (response) {
                    console.log(response);
                    seriesData.length = 0;
                    $.each(response,
                        function (i, item) {
                            seriesData.push({
                                x: Number(i) * 1000,
                                y: item
                            });
                        });
                    onLoaded();
                });
        };

        var updateGraph = function () {
            if (!graph) {
                createGraph();
            }
            graph.series[0].name = selectedTarget();
            graph.update();
        };

        var showData = function () {
            console.log("loading exchange rates for " + selectedSource() + " and " + selectedTarget());
            loadExchangeData(updateGraph);
        };

        var init = function () {
            status("Loading Exchange data...");
            $.ajax({
                url: "api/exchange/loaddata",
                type: "POST",
                contentType: "application/json"
            }).done(function (result) {
                console.log(result);
                loadCurrencies();
            });
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