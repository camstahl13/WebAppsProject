$.getJSON("/Internal/Requirements/GetAdvisees", function (data) {
    console.log(data);
    $("#greet").append("<h1>Hello " + data.advisor + "</h1>");
    for (let advisee of data.advisees) {
        $("#advisees").append("<form action=\"/Student/Ape\" method=\"get\">"
            + "<input type=\"hidden\" name=\"student\" value=\"" + advisee + "\">"
            + "<button type=\"submit\">Access " + advisee + "\'s Plans</button></form>");
    }
});