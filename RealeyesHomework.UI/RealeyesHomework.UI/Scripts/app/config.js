require.config({
    baseUrl: "/Scripts/app",
    paths: {
        jquery: "../jquery-2.1.1",
        jqueryValidate: "../jquery.validate",
        jqueryValidateUnobtrusive: "../jquery.validate.unobtrusive",
        bootstrap: "../bootstrap",
        ko: "../knockout-3.4.2",
        d3: "../d3.v3",
        rickshaw: "../rickshaw"
    },
    shim: {
        jqueryValidate: ["jquery"],
        jqueryValidateUnobtrusive: ["jquery", "jqueryValidate"],
        d3: { exports: "d3" },
        rickshaw: { exports: "Rickshaw", deps: ["d3"] }
    }
});