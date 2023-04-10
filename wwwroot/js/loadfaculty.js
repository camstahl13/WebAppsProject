$.getJSON("/Internal/Requirements/GetAdvisees", function (data) {
    console.log(data);
    for (let advisee of data.advisees) {
        $("#greet").append("<h1>Hello " + data.advisor + "</h1>");
        $("#students").append("<a href=\"/Student/Ape?student=" + advisee + "\">" + advisee + "</a>");
    }
});