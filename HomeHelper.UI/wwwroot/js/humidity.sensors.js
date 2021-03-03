const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/brokerhub")
    .build();
$(document).ready(async function () {

    connection.onclose(async () => {
        await start();
    });
    await start();

    let humContainers = document.querySelectorAll('div.humidity-container');
    humContainers.forEach(function (humContainer) {
        let sensorId = humContainer.firstElementChild;
        let protocolType = sensorId.querySelector('div.protocol-type');
        console.log(protocolType);

        let humValue = sensorId.querySelector('div.humidity-value');
        fetch('/Admin/HumiditySensor/Humidity?id=' + sensorId.id).
            then(response => response.json().
                then(data => {
                    console.log(response.status);
                    if (response.status != 200 || data == null) {
                        humValue.innerHTML = 'No data available';
                    }
                    else {
                        humValue.innerHTML = data.humidity + '%';
                    }
                }));

        let batteryValue = sensorId.querySelector('.battery-value');
        fetch('/Admin/HumiditySensor/Battery?id=' + sensorId.id).
            then(response => response.json().
                then(data => {
                    console.log(response.status);
                    if (response.status != 200 || data == null) {
                        return;
                    }
                    else {
                        batteryValue.innerHTML = data.battery + '%';
                        let batteryIcon = sensorId.querySelector('.battery-icon');
                        let batteryAnim = sensorId.querySelector('.battery-animation');
                        if (batteryIcon.classList.contains('hidden')) {
                            batteryIcon.classList.remove('hidden');
                        }
                        if (!batteryAnim.classList.contains('hidden')) {
                            batteryAnim.classList.add('hidden');
                        }
                    }
                }));

        //let availability = sensorId.querySelector('div.availability');
        //fetch('/Admin/TemperatureSensor/Battery?id=' + sensorId.id).
        //    then(response => {
        //        console.log(response.text());
        //        if (response.json() == null) {
        //            availability.innerHTML = 'No data available';
        //        }
        //        else {
        //            if (response.text() == "true") {
        //                availability.innerHTML = 'online';
        //            }
        //            else if (response.text() == "false") {
        //                availability.innerHTML = 'offline';
        //            }
        //        }
        //    });

    });
    connection.on("Humidity", (id, payload) => {
        console.log(payload);
        humContainers.forEach(function (humContainer) {
            let sensorId = humContainer.firstElementChild;
            if (sensorId.id == id) {
                let humValue = sensorId.querySelector('div.humidity-value');
                humValue.innerHTML = payload.humidity + ' %';
            }
        });
    });
    connection.on("Battery", (id, payload) => {
        humContainers.forEach(function (humContainer) {
            let sensorId = humContainer.firstElementChild;
            if (sensorId.id == id) {
                let batteryValue = sensorId.querySelector('.battery-value');
                batteryValue.innerHTML = payload.battery + '%';
                let batteryIcon = sensorId.querySelector('.battery-icon');
                let batteryAnim = sensorId.querySelector('.battery-animation');
                if (batteryIcon.classList.contains('hidden')) {
                    batteryIcon.classList.remove('hidden');
                }
                if (!batteryAnim.classList.contains('hidden')) {
                    batteryAnim.classList.add('hidden');
                }
            }
        });
    });
    connection.on("Linkquality", (id, payload) => {
        console.log(payload);
        humContainers.forEach(function (humContainer) {
            let sensorId = humContainer.firstElementChild.id;
            let signalIcon = humContainer.querySelector('.signal-icon');
            let signalAnimation = humContainer.querySelector('.signal-animation');
            if (id == sensorId) {
                console.log(payload.linkquality);
                let signalStrength = humContainer.querySelector('.signal-strength');
                signalStrength.innerHTML = payload.linkquality;


                if (signalIcon.classList.contains('hidden')) {
                    signalIcon.classList.remove('hidden');
                }
                if (!signalAnimation.classList.contains('hidden')) {
                    signalAnimation.classList.add('hidden');
                }

            }
        });
    });
    //connection.on("DeviceDisconnected", (id, payload) => {
    //    tempContainers.forEach(function (tempContainer) {
    //        let sensorId = tempContainer.firstElementChild;
    //        if (sensorId.id == id) {
    //            let availability = sensorId.querySelector('div.availability');
    //            availability.innerHTML = payload.text();
    //        }
    //    });
    //});
    //connection.on("DeviceConnected", (id, payload) => {
    //    tempContainers.forEach(function (tempContainer) {
    //        let sensorId = tempContainer.firstElementChild;
    //        if (sensorId.id == id) {
    //            let availability = sensorId.querySelector('div.availability');
    //            availability.innerHTML = payload.text();
    //        }
    //    });
    //});
});
async function start() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};