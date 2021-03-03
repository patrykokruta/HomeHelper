const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/brokerhub")
    .build();
$(document).ready(async function () {

    connection.onclose(async () => {
        await start();
    });
    await start();

    let motionContainers = document.querySelectorAll('div.motion-container');
    motionContainers.forEach(function (motionContainer) {
        let sensorId = motionContainer.firstElementChild;
        let protocolType = sensorId.querySelector('div.protocol-type');


        let motionValue = sensorId.querySelector('div.motion-value');

        fetch('/Admin/MotionSensor/IsMotion?id=' + sensorId.id).
            then(response => response.json().
                then(data => {
                    console.log(response.status);
                    if (response.status != 200 || data == null) {
                        return;
                    }
                    else {
                        if (data.motion == true) {
                            motionValue.innerHTML = 'Motion detected.'
                        }
                        else if (data.motion == false) {
                            motionValue.innerHTML = 'No motion detected.'
                        }
                    }
                }));

        let batteryValue = sensorId.querySelector('.battery-value');
        fetch('/Admin/MotionSensor/Battery?id=' + sensorId.id).
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
    connection.on("Motion", (id, payload) => {
        console.log(payload);
        motionContainers.forEach(function (motionContainer) {
            let sensorId = motionContainer.firstElementChild;
            if (sensorId.id == id) {
                let motionValue = sensorId.querySelector('div.motion-value');
                if (payload.motion == true) {
                    motionValue.innerHTML = 'Motion detected.';
                }
                else if (payload.motion == false) {
                    motionValue.innerHTML = 'No motion detected.';
                }
            }
        });
    });
    connection.on("Battery", (id, payload) => {
        motionContainers.forEach(function (motionContainer) {
            let sensorId = motionContainer.firstElementChild;
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
        motionContainers.forEach(function (motionContainer) {
            let sensorId = motionContainer.firstElementChild.id;
            let signalIcon = motionContainer.querySelector('.signal-icon');
            let signalAnimation = motionContainer.querySelector('.signal-animation');
            if (id == sensorId) {
                console.log(payload.linkquality);
                let signalStrength = motionContainer.querySelector('.signal-strength');
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