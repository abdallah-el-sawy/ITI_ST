// Rule: employee name must not contain digits
$.validator.addMethod("nonumbers", function (value, element) {
    return this.optional(element) || !/\d/.test(value);
});

$.validator.unobtrusive.adapters.add("nonumbers", function (options) {
    options.rules["nonumbers"] = true;
    options.messages["nonumbers"] = options.message;
});

// Rule: salary > threshold requires age >= minimum age
$.validator.addMethod("minimumageforhighsalary", function (value, element, params) {
    var salaryField = $("[name='" + params.salaryfield + "']");
    var salary = parseFloat(salaryField.val());
    var age = parseFloat(value);
    var threshold = parseFloat(params.threshold);
    var minAge = parseFloat(params.minage);

    if (isNaN(salary) || isNaN(age)) {
        return true; // let Range/Required rules handle missing values
    }
    return !(salary > threshold && age < minAge);
});

$.validator.unobtrusive.adapters.add(
    "minimumageforhighsalary",
    ["salaryfield", "threshold", "minage"],
    function (options) {
        options.rules["minimumageforhighsalary"] = {
            salaryfield: options.params.salaryfield,
            threshold: options.params.threshold,
            minage: options.params.minage
        };
        options.messages["minimumageforhighsalary"] = options.message;
    }
);

// Re-check Age whenever Salary changes, and vice versa, so the cross-field rule updates live
$(document).ready(function () {
    $("[name='Salary']").on("input", function () {
        $("[name='Age']").valid();
    });
    $("[name='Age']").on("input", function () {
        $("[name='Age']").valid();
    });
});