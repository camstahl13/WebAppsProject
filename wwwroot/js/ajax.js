//import {plan} from './ajax.js';

// Map course_id to bool (fulfilled or not fulfilled)
var req_fulfilled = {};

// Map course_id to li element(s) in requirements accordion
var req_elements = {};

function populateCatalog(courses) {
    let $table = $("#coursefinder > tbody");
    for (let courseID in courses) {
        let course = courses[courseID];
        let courseTableEntry = "<tr>";
        for (let field of [course.name, course.id, course.description, course.credits]) {
            courseTableEntry += `<td>${field}</td>`;
        }
        courseTableEntry += "</tr>";
        $table.append(courseTableEntry);
    }
}

function termCmp(term1, term2) {
    if (term1 == term2) {
        return 0;
    } else if (term1 == "Spring") {
        return -1; // term2 is greater
    } else if (term1 == "Fall") {
        return 1; // term2 is less
    } else {
        return term2 == "Spring" ? 1 : -1; // -1 if term2 is less
    }
}

function displayPlan(years, currYear, currTerm) {
    let $planArea = $("#aca-plan");

    document.addEventListener("dragover", function (event) {
        event.preventDefault();
    });

    let sortedYears = Object.keys(years);
    sortedYears.sort((a,b) => a-b);
    for (let year of sortedYears) {
        let sortedTerms = Object.keys(years[year]);
        sortedTerms.sort(termCmp);
        for (let termName of sortedTerms) {
            let term = years[year][termName];
            let scheduled = (year < currYear) || (year == currYear && termCmp(termName, currTerm) <= 0);


            let termHtml = $("<li>");
            termHtml.addClass(scheduled ? "scheduled" : "unscheduled");
            let p1 = $("<p>");
            p1.addClass("sem");
            p1.append(termName + " " + year);
            let p2 = $("<p>");
            p2.addClass("hours text-secondary");
            p2.append("Hours: " + term.credits);
            let d = $("<div>");
            d.addClass("sem-courses");
            let u = $("<ul>");
            u.addClass("course-list");
            d.append(u);

            termHtml.append(p1).append(p2).append(d);

            for (let course of term.courses) {
                let li = $("<li>");
                li.attr("draggable", true);
                li.append(course.id + " " + course.title);
                if (!scheduled) {
                    li.on("dragstart", (e) => {
                        e.originalEvent.dataTransfer.setData("text/plain", e.target.innerText);
                    });
                    li.on("dragend", (e) => {
                        if (e.target.firstChild.parentElement.parentElement.parentElement.parentElement.className != "unscheduled") {
                            e.originalEvent.target.remove();
                        }
                    });
                }
                u.append(li);
            }

            $planArea.append(termHtml);

            /*
            let termHtml = `<li class=${scheduled ? "scheduled" : "unscheduled"}>
                                 <p class='sem'>${termName} ${year}</p>
                                 <p class="hours text-secondary">Hours: ${term.credits}</p>
                                 <div class="sem-courses">
                                 <ul class="course-list">`;
            for (let course of term.courses) {
                termHtml += `<li draggable="true">${course.id} ${course.title}</li>`;
            }
            termHtml += "</ul></div></li>";
            let termElement = $(termHtml);
            $planArea.append(termElement);
            */

            if (!scheduled) {
                termHtml.on("drop", (e) => {
                    let ll = $("<li>");
                    ll.attr("draggable", true);
                    ll.append(e.originalEvent.dataTransfer.getData("text/plain"));
                    ll.on("dragstart", (e) => {
                        e.originalEvent.dataTransfer.setData("text/plain", e.target.innerText);
                    })
                    ll.on("dragend", (e) => {
                        if (e.target.firstChild.parentElement.parentElement.parentElement.parentElement.className != "unscheduled") {
                            e.originalEvent.target.remove();
                        }
                    })
                    termHtml.children(2).children(0).append(ll);
                });
            }
            if (scheduled) {
                termHtml.on("drop", (e) => {
                    e.preventDefault();
                })
            }
        }
    }
}

function recolorCheck(course_id) {
    for (let item of req_elements[course_id]) {
        item.addClass((course_id in req_fulfilled) ? "planned" : "unplanned");
        item.removeClass((course_id in req_fulfilled) ? "unplanned" : "planned");
    }
}

function populatePlan(plan, catalog) {
    let years = {};
    for (let courseID in plan.courses) {
	    if (!catalog.courses[courseID]) {
		    continue;
        }

        req_fulfilled[courseID] = true;
 
        let course = plan.courses[courseID];
        if (!(course.year in years)) {
            years[course.year] = {};
        }
        years[course.year][course.term] = years[course.year][course.term] || {credits:0, courses: []};
        if (course.term == "Fall") {
            let next = course.year+1;
            years[next] = years[next] || {};
            years[next]["Spring"] = years[next]["Spring"] || {credits:0, courses: []};
            years[next]["Summer"] = years[next]["Summer"] || {credits:0, courses: []};
        } else {
            let prev = course.year-1;
            years[prev] = years[prev] || {};
            years[prev]["Fall"] = years[prev]["Fall"] || {credits:0, courses: []};
            let otherTerm = course.term == "Spring" ? "Summer" : "Spring";
            years[course.year][otherTerm] = years[course.year][otherTerm] || {credits:0, courses: []};
        }
        years[course.year][course.term].courses.push({id: course.id, title: catalog.courses[course.id].name});
        years[course.year][course.term].credits += catalog.courses[course.id].credits;
    }

    displayPlan(years, plan.currYear, plan.currTerm);
}


function parseCombined() {
    const crss = JSON.parse(this.response);

    if ("nodefault" in crss) {
        return;
    }

    let plan = crss.plan;
    let catalog = crss.catalog;
    console.log(crss);

    $("#studentName").text(plan.student);
    $("#catalogYear").text(plan.catYear);
    $("#major").text(plan.major);
    $("#minor").text(plan.minor || "None");

    getRequirements(catalog);
    populateCatalog(catalog.courses);
    populatePlan(plan, catalog);
}

function getCombined() {
    cataXhr = new XMLHttpRequest();
    cataXhr.onload = parseCombined;
    cataXhr.open("GET", "/Internal/Requirements/GetCombined");
    cataXhr.send();
}

function makeCoursesDraggable() {
    document.addEventListener("dragover", function (event) {
        event.preventDefault();
    });

    for (let item of document.getElementById("accordion").children) {
        for (req of item.children[1].children) {
            req.addEventListener("dragstart", (e) => {
                e.dataTransfer.setData("text/plain", e.target.innerText);
            });
        }
    }

    for (let item of document.getElementById("aca-plan").children) {
        item.addEventListener("drop", (e) => {
            let ll = document.createElement("li");
            ll.appendChild(document.createTextNode(e.dataTransfer.getData("text/plain")));
            item.children[2].children[0].appendChild(ll);
        });
    }
}

function createRequirement(course_id, course_name) {
    if (!(course_id in req_fulfilled)) {
        req_fulfilled[course_id] = false;
    }
    let course = $("<li>");
    if (!(course_id in req_elements)) {
        req_elements[course_id] = [];
    }
    req_elements[course_id].push(course);

    course.addClass(req_fulfilled[course_id] ? "planned" : "unplanned");
    course.attr("draggable", true);
    course.on("dragstart", (e) => {
        console.log(e.target.innerText);
        e.originalEvent.dataTransfer.setData("text/plain", e.target.innerText);
    });
    course.append($("<label>"));
    course.append(`${course_id} ${course_name}`);
    return course;
}

function getRequirements(catalog) {
    reqXhr = new XMLHttpRequest();
    reqXhr.onload = function() {
        let $acc = $("#accordion");
        const rrs = JSON.parse(reqXhr.response);
        if ("nodefault" in rrs) {
            return;
        }

        for (let categoryName in rrs.categories) {
            let category = $("<div>");
            category.addClass("group");
            category.append($("<h2>" + categoryName + "</h2>"));
            let courses = $("<ul>");

            rrs.categories[categoryName].courses.forEach(function (course_id) {
                courses.append(createRequirement(course_id, catalog.courses[course_id].name));
                //let planned = (course_id in plan.courses) ? "planned" : "unplanned";
            });
            category.append(courses);
            $acc.append(category);
        }

        $acc.accordion({
            header: "> div > h2",
            heightStyle: "fill",
        });

        //makeCoursesDraggable();
    }
    reqXhr.open("GET", "/Internal/Requirements/Get");
    reqXhr.send();
}

function getCreatePlanMajors() {
	xhr = new XMLHttpRequest();
	xhr.onload = function() {
		const majmin = JSON.parse(xhr.response);
		for (let maj of majmin[0]) {
			$("#createMajor").append(`<option value="${maj}">${maj}</option>`);
		}
		for (let min of majmin[1]) {
			$("#createMinor").append(`<option value="${min}">${min}</option>`);
		}
		for (let year of majmin[2]) {
			$("#catayear").append(`<option value="${year}">${year}</option>`);
		}
	}
	xhr.open("GET","/Internal/Requirements/GetPlannable");
	xhr.send();
}
