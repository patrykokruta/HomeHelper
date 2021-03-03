const connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/brokerhub")
    .build();

$(document).ready(async function () {
    connection.onclose(async () => {
        await start();
    });
    await start();



    $('.loading-animation').hide();
    $('#pair-button').click(async function () {
        $(this).toggleClass('toggled');
        if ($(this).hasClass('toggled')) {
            toggleEnableButton();
            await PermitZigbee();
        } else {
            toggleEnableButton();
            await ForbidZigbee();
        }

    });

    enableSelect2();
    hideOptions();
    displayOptions();
    loadDeviceModels();

    $("#device-types").change(function () {
        $("#device-models").val(null);
        $("#device-models").html(null);
        loadDeviceModels();
    });
    $("#protocol-types").change(function () {
        $("#device-models").val(null);
        $("#device-models").html(null);
        $("#mqtt-devices").val(null);
        $("#zigbee-devices").val(null);
        loadDeviceModels();
        hideOptions();
        loadNotConfirmedMqttDevices();
        loadNotConfirmedZigbeeDevices();
        displayOptions();
    });

    $("#create-device").submit(function (event) {
        event.preventDefault();
        const formData = new FormData(this);
        fetch('Add', {
            method: 'post',
            body: formData
        }).then(async function (response) {
            if (formData.get('Protocol') != 1) {
                await connection.invoke("SubscribeClientTopics", formData.get('ConnectedDeviceId'));
            }
        });
        window.location = '/Admin/Device/Add';
    });

    connection.on("DeviceConnected", async (id) => {
        console.log('Device connected');
        toastr.info('Device connected.', id);
        toggleEnableButton();
        await ForbidZigbee();
    });
});


async function PermitZigbee() {
    try {
        await connection.invoke("PermitJoinZigbeeDevices");
    } catch (err) {
        console.error(err);
    }
}
async function ForbidZigbee() {
    try {
        await connection.invoke("ForbidJoinZigbeeDevices");
    } catch (err) {
        console.error(err);
    }
}
function toggleEnableButton() {
    $('.enable-text').toggle();
    $('.loading-animation').toggle();
}

function hideOptions() {
    $("#http-options").addClass("hidden");
    $("#mqtt-options").addClass("hidden");
    $("#zigbee-options").addClass("hidden");
}

function enableSelect2() {
    $("#device-models").select2();
    $("#device-types").select2();
    $("#protocol-types").select2();
    $("#sensor-rooms").select2();
    $("#mqtt-devices").select2();
    $("#zigbee-devices").select2();
}




function displayOptions() {

    if ($("#protocol-types option:selected").text() == "Http") {
        $("#http-options").removeClass("hidden");
    }
    else if ($("#protocol-types option:selected").text() == "Mqtt") {
        $("#mqtt-options").removeClass("hidden");
        loadNotConfirmedMqttDevices();
    }
    else {
        $("#zigbee-options").removeClass("hidden");
    }
}
function loadDeviceModels() {
    $("#device-models").select2({
        ajax: {
            url: '/Admin/Device/GetFilteredDevices',
            type: 'get',
            dataType: 'json',
            contentType: "application/json",
            data: {
                protocolType: $("#protocol-types option:selected").text(),
                deviceType: $("#device-types option:selected").text()
            },
            processResults: function (data) {
                return {
                    results: data
                };
            }
        }
    });
}
function loadNotConfirmedMqttDevices() {
    $("#mqtt-devices").select2({
        ajax: {
            url: '/Admin/MqttDevice/GetNotConfirmed',
            type: 'get',
            dataType: 'json',
            contentType: "application/json",
            processResults: function (data) {
                return {
                    results: data
                };
            }
        }
    });
}
function loadNotConfirmedZigbeeDevices() {
    $("#zigbee-devices").select2({
        ajax: {
            url: '/Admin/ZigbeeDevice/GetNotConfirmed',
            type: 'get',
            dataType: 'json',
            contentType: "application/json",
            processResults: function (data) {
                return {
                    results: data
                };
            }
        }
    });
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





