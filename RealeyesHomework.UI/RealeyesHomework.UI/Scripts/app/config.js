require.config({
    baseUrl: "/Scripts/app",
    paths: {
        jquery: "../jquery-2.1.1",
        jqueryValidate: "../jquery.validate",
        jqueryValidateUnobtrusive: "../jquery.validate.unobtrusive",
        bootstrap: "../bootstrap",
        ko: "/Scripts/knockout-3.4.2"
    },
    shim: {
        jqueryValidate: ["jquery"],
        jqueryValidateUnobtrusive: ["jquery", "jqueryValidate"]
    }
});