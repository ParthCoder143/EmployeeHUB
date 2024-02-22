let cache = {};
let start = 0, Pagelength = 10
const getData = (url) => {

    return fetch(url, {
        method: 'GET',
        headers: new Headers({
            'Authorization': 'Bearer ' + localStorage.getItem("UserToken")
        })
    }).then(function (response) {
        if (response.status == 401) {
            setTimeout(() => {
                window.location.href = LoginUrl;
            }, 2000);
            return response.json();
        }
        else {
            return response.json();
        }
    });
};
async function RequestMethod(url, data, Method, ContentType) {
    if ((ContentType || "").length == 0)
        ContentType = 'application/json; charset=utf-8';
    if ((Method || "").length == 0)
        Method = 'POST';

    // Default options are marked with *
    const response = await fetch(url, {
        method: Method, // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': ContentType,
            'Authorization': 'Bearer ' + localStorage.getItem("UserToken")
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: data // body data type must match "Content-Type" header
    });
    return response;
}
// Example POST method implementation:
async function postData(url, data, Method, ContentType) {
    const response = await RequestMethod(url, data, Method, ContentType);
    return response.json(); // parses JSON response into native JavaScript objects
}
async function postDataWithImage(url, data, Method) {
    const response = await fetch(url, {
        method: Method, // *GET, POST, PUT, DELETE, etc.
        body: data,
        headers: {
            'Authorization': 'Bearer ' + localStorage.getItem("UserToken")
        }
    });
    return response.json(); // parses JSON response into native JavaScript objects
}
async function FileDownloadData(url, data, Method = "", ContentType = "") {
    const response = await RequestMethod(url, data, Method, ContentType);
    if (response.ok) {
        return response.blob().then(blob => {
            return {
                contentType: response.headers.get("Content-Type"),
                raw: blob,
                FileName: response.headers.get("FileName")
            }
        });
    }
    return null;
}