const socket = new WebSocket("wss://localhost:6200/api/food/ws");

socket.onopen = (event) => {
    //console.log("WebSocket connection established.");
};

socket.onmessage = (event) => {
    //console.log(event.data)
    alert(event.data);
};

socket.onclose = (event) => {
    if (event.wasClean) {
        //console.log(`WebSocket connection closed cleanly, code=${event.code}, reason=${event.reason}`);
    } else {
        //console.error(`WebSocket connection died`);
    }
};

function sendMessage(message) {
    socket.send(message);
}