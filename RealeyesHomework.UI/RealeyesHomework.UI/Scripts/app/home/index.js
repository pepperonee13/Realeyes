require(["ko", "home/exchangeRates"], function (ko, exchangeRates) {
    ko.applyBindings(exchangeRates, $("#home")[0]);
    exchangeRates.init();
});