const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/brokerhub")
    .build();
$(document).ready(async function () {

    connection.onclose(async () => {
        await start();
    });
    await start();

    let contactContainers = document.querySelectorAll('div.contact-container');
    contactContainers.forEach(function (contactContainer) {
        let sensorId = contactContainer.firstElementChild;
        let protocolType = sensorId.querySelector('div.protocol-type');


        let contactValue = sensorId.querySelector('div.contact-value');

        fetch('/Admin/ContactSensor/IsContact?id=' + sensorId.id).
            then(response => response.json().
                then(data => {
                    console.log(response.status);
                    if (response.status != 200 || data == null) {
                        return;
                    }
                    else {
                        if (data.contact == true) {
                            contact.innerHTML = 'Closed'
                        }
                        else if (data.contact == false) {
                            contactValue.innerHTML = 'Opened'
                        }
                    }
                }));

        let batteryValue = sensorId.querySelector('.battery-value');
        fetch('/Admin/TemperatureSensor/Battery?id=' + sensorId.id).
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
    connection.on("Contact", (id, payload) => {
        console.log(payload);
        contactContainers.forEach(function (contactContainer) {
            let sensorId = contactContainer.firstElementChild;
            if (sensorId.id == id) {
                let contactValue = sensorId.querySelector('div.contact-value');
                if (payload.contact == true) {
                    contactValue.innerHTML = 'Closed';
                }
                else if (payload.contact == false) {
                    contactValue.innerHTML = 'Opened';
                }
            }
        });
    });
    connection.on("Battery", (id, payload) => {
        contactContainers.forEach(function (contactContainer) {
            let sensorId = contactContainer.firstElementChild;
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
        contactContainers.forEach(function (contactContainer) {
            let sensorId = contactContainer.firstElementChild.id;
            let signalIcon = contactContainer.querySelector('.signal-icon');
            let signalAnimation = contactContainer.querySelector('.signal-animation');
            if (id == sensorId) {
                console.log(payload.linkquality);
                let signalStrength = contactContainer.querySelector('.signal-strength');
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