const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/brokerhub")
    .build();

$(document).ready(async function () {

    connection.onclose(async () => {
        await start();
    });
    await start();

    let switchContainers = document.querySelectorAll('div.switch-container');

    switchContainers.forEach(async function (switchContainer) {

        let switchId = switchContainer.firstElementChild.id;
        let protocolType = switchContainer.querySelector('div.protocol-type');
        let switchIcon = switchContainer.querySelector('.switch-icon');
        let switchAnimation = switchContainer.querySelector('.switch-animation');
        let signalIcon = switchContainer.querySelector('.signal-icon');
        let signalAnimation = switchContainer.querySelector('.signal-animation');

        if (protocolType.innerHTML == 'Http') {
            fetch('/Admin/Switch/State?id=' + switchId).
                then(function (response) {
                    response.json().then(data => {

                        console.log(data.state);
                        if (response.status == 200) {

                            switchIcon.classList.remove('hidden');
                            switchAnimation.classList.add('hidden');
                            if (data.state == "on") {
                                switchIcon.classList.add('power-on');
                                switchIcon.classList.remove('power-off');
                            }
                            else if (data.state == "off") {
                                switchIcon.classList.add('power-off');
                                switchIcon.classList.remove('power-on');
                            }
                        }

                    });
                    let signalStrength = switchContainer.querySelector('.signal-strength');
                    fetch('/Admin/Switch/Linkquality?id=' + switchId).
                        then(response => response.json().
                            then(data => {
                                signalStrength.innerHTML = data.linkquality;
                                signalIcon.classList.remove('hidden');
                                signalAnimation.classList.add('hidden');


                            }));
                });
            


        }
        else {
            fetch('/Admin/Switch/MqttMessage?id=' + switchId + '&publishType=' + 'CheckState', {
                method: 'get',
            }).
                then(response => {
                    response.json().
                        then(async data => {
                            if (response.status == 200) {
                                await connection.invoke("PublishAsync", data.topic, data.payload);
                            }
                        })
                });
        }
    });

    connection.on("State", (id, payload) => {
        console.log(payload);
        switchContainers.forEach(function (switchContainer) {
            let switchId = switchContainer.firstElementChild.id;
            let switchIcon = switchContainer.querySelector('.switch-icon');
            let switchAnimation = switchContainer.querySelector('.switch-animation');
            if (id == switchId) {
                console.log(payload.state)
                if (payload.state == "ON") {
                    switchIcon.classList.add('power-on');
                    switchIcon.classList.remove('power-off');
                }
                else if (payload.state == "OFF") {
                    switchIcon.classList.add('power-off');
                    switchIcon.classList.remove('power-on');
                }
                switchIcon.classList.remove('hidden');
                switchAnimation.classList.add('hidden');
            }
        });
    });
    connection.on("Linkquality", (id, payload) => {
        console.log(payload);
        switchContainers.forEach(function (switchContainer) {
            let switchId = switchContainer.firstElementChild.id;
            let signalIcon = switchContainer.querySelector('.signal-icon');
            let signalAnimation = switchContainer.querySelector('.signal-animation');
            if (id == switchId) {
                console.log(payload.linkquality);
                let signalStrength = switchContainer.querySelector('.signal-strength');
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
});

function httpToggle(id) {
    let switchContainer = document.getElementById(id);
    let switchIcon = switchContainer.querySelector('.switch-icon');
    if (switchIcon.classList.contains('power-on')) {
        fetch('/Admin/Switch/TurnOff?id=' + id, {
            method: 'post'
        }).
            then(response => {
                if (response.status == 200) {
                    switchIcon.classList.remove('power-on');
                    switchIcon.classList.add('power-off');
                }
            });
    }
    else if (switchIcon.classList.contains('power-off')) {
        let switchContainer = document.getElementById(id);
        let switchIcon = switchContainer.querySelector('.switch-icon');
        if (switchIcon.classList.contains('power-off')) {
            fetch('/Admin/Switch/TurnOn?id=' + id, {
                method: 'post'
            }).
                then(response => {
                    if (response.status == 200) {
                        switchIcon.classList.remove('power-off');
                        switchIcon.classList.add('power-on');
                    }
                });
        }
    }
}
function mqttToggle(id) {
    console.log('jak');
    let switchContainer = document.getElementById(id);
    let switchIcon = switchContainer.querySelector('.switch-icon');
    if (switchIcon.classList.contains('power-on')) {
        fetch('/Admin/Switch/MqttMessage?id=' + id + '&publishType=' + 'TurnOff', {
            method: 'get',
        }).
            then(response => {

                response.json().then(async data => {
                    if (response.status == 200) {
                        await connection.invoke("PublishAsync", data.topic, data.payload);
                    }
                })
            });
    }
    else if (switchIcon.classList.contains('power-off')) {
        let switchContainer = document.getElementById(id);
        let switchIcon = switchContainer.querySelector('.switch-icon');
        
          
            fetch('/Admin/Switch/MqttMessage?id=' + id + '&publishType=' + 'TurnOn', {
                method: 'get',
            }).
                then(response => {
                    response.json().
                        then(async data => {
                            if (response.status == 200) {
                                await connection.invoke("PublishAsync", data.topic, data.payload);
                            }
                        })

                });
        
    }
}
async function start() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};