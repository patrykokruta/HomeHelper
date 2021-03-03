const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/brokerhub")
    .build();
$(document).ready(async function () {

    connection.onclose(async () => {
        await start();
    });
    await start();

    let tempContainers = document.querySelectorAll('div.temperature-container');
    tempContainers.forEach(function (tempContainer) {
        let sensorId = tempContainer.firstElementChild;
        let protocolType = sensorId.querySelector('div.protocol-type');
        console.log(protocolType);

        let tempValue = sensorId.querySelector('div.temperature-value');
        fetch('/Admin/TemperatureSensor/Temperature?id=' + sensorId.id).
            then(response => response.json().
                then(data => {
                    console.log(response.status);
                    if (response.status != 200 || data == null) {
                        return;
                    }
                    else {
                        tempValue.innerHTML = data.temperature + ' °C';
                    }
                }));

        let batteryValue = sensorId.querySelector('.battery-value');
        fetch('/Admin/TemperatureSensor/Battery?id=' + sensorId.id).
            then(response => response.json().
                then(data => {
                    console.log(data.battery);
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
    connection.on("Temperature", (id, payload) => {
        console.log(payload);
        tempContainers.forEach(function (tempContainer) {
            let sensorId = tempContainer.firstElementChild;
            if (sensorId.id == id) {
                let tempValue = sensorId.querySelector('div.temperature-value');
                tempValue.innerHTML = payload.temperature + ' °C';
            }
        });
    });
    connection.on("Battery", (id, payload) => {
        tempContainers.forEach(function (tempContainer) {
            let sensorId = tempContainer.firstElementChild;
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
        tempContainers.forEach(function (tempContainer) {
            let sensorId = tempContainer.firstElementChild.id;
            let signalIcon = tempContainer.querySelector('.signal-icon');
            let signalAnimation = tempContainer.querySelector('.signal-animation');
            if (id == sensorId) {
                console.log(payload.linkquality);
                let signalStrength = tempContainer.querySelector('.signal-strength');
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