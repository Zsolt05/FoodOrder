var defaultUrl = "https://localhost:6200/api/";

async function postData(url = "", data = {}, needAuth = true) {
    // Default options are marked with *
    const response = await fetch(defaultUrl + url, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "same-origin", // include, *same-origin, omit
        headers: {
            "Content-Type": "application/json",
            Authorization: "bearer " + (needAuth ? JSON.parse(localStorage.getItem("data")).token : null),
        },
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: testJSON(data) ? data : JSON.stringify(data), // body data type must match "Content-Type" header
    });
    if (response.status === 401 || response.status === 403) {
        logout();
    }
    try {
        return await response.json(); // parses JSON response into native JavaScript objects
    } catch (error) {
        console.error("Error parsing JSON:", error);
        return {}; // or handle the error as per your requirement
    }
}

async function getData(url = "", needAuth = true) {
    // Default options are marked with *
    const response = await fetch(defaultUrl + url, {
        method: "GET", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "same-origin", // include, *same-origin, omit
        headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + (needAuth ? JSON.parse(localStorage.getItem("data")).token : null),
        },
        redirect: "follow", // manual, *follow, error
        referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
    });
    if (response.status === 401 || response.status === 403) {
        logout();
    }
    return response.json(); // parses JSON response into native JavaScript objects
}

function testJSON(text) {
    if (typeof text !== "string") {
        return false;
    }
    try {
        JSON.parse(text);
        return true;
    } catch (error) {
        return false;
    }
}

function logout() {
    localStorage.clear();
    window.location.href = "login.html";
}
