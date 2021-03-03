# HomeHelper
## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Third party tools](#third-party-tools)
* [Project overview](#project-overview)
* [Example application views](#example-application-views)
* [Setup](#setup)

## General info
HomeHelper is an automation system for IoT. It supports three types of communication protocols: `HTTP`, `MQTT` and `Zigbee`. New devices can be added as plugins which can be created via `HomeHelper.SDK` project. Home Helper is intended to work on `Raspberry Pi` with local `MySQL` database.
	
## Technologies
Project is created with:
* .NET Core 3.1
* ASP.NET Core
* Entity Framework Core
* SignalR
* MediatR
* [MQTTNet](https://github.com/chkr1011/MQTTnet)
* MySQL

## Third party tools
* [Zigbee2MQTT](https://www.zigbee2mqtt.io/)
* [Zigbee CC2531 adapter](https://pl.aliexpress.com/af/cc2531.html?d=y&origin=n&SearchText=cc2531&catId=0&initiative_id=SB_20191108075039)

## Project overview

### Adding new devices
To distinguish between devices it's required to set name of new device and choose corresponding room. Depend on protocol type, there are different data that needs to be provided:
* HTTP protocol requires device IP address and Port number
* MQTT protocol requires connecting to the Broker with login and password
* Zigbee protocol requires enabling joining new devices

Add new device form:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109693377-df2df880-7b89-11eb-831d-62e2bfc05929.gif">
</p>

Example of adding Zigbee device ([Sonoff SNZB02 Temperature and Humidity Sensor](https://sonoff.tech/product/smart-home-security/snzb-02)):
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109690251-72652f00-7b86-11eb-864d-ce0291f7cafa.gif">
</p>
After enabling joining, we need to manually pair device with the Broker. When pairing success, there is a prompt with information and new device is available under connected devices selection.

### MQTT and Zigbee communication
`MQTT Broker` and `MQTT Client` are implemented with `MQTTnet` library. `MQTT Broker` is working under port 1883. MQTT devices can subscribe and publish messages to the Broker. `MQTT Client` provides a way to communicate with devices connected to the Broker. Both the application using `MQTT Client`, and devices connected to the Broker can be considered as clients. `MQTT Client` methods such as publish and subscribe can be invoked through `BrokerHub` created with `SignalR`. Communication with Zigbee devices is possible thanks to [Zigbee2MQTT](https://www.zigbee2mqtt.io/) which provides an adapter to communicate with Zigbee via MQTT.   

The following diagram illustrates implementation of MQTT and Zigbee communication inside application:  


<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109624277-5047be80-7b3e-11eb-9156-49a4998b5b99.png">
</p>

### BrokerHub
`BrokerHub` created with `SignalR` enables two way communication with MQTT and Zigbee devices. Javascript client's code can invoke `BrokerHub` methods and `BrokerHub` can invoke Javascript client's method. Example of `BrokerHub` method:
```
public async Task PublishAsync(string topic, string payload)
{
    await _mqttClientService.PublishAsync(topic, payload);
}
```
And the corresponding Javascript code which invokes above method:
```
await connection.invoke("PublishAsync", data.topic, data.payload);
```
Example of Javascript code listening for temperature data:
```
connection.on("Temperature", (id, payload) => {
	tempContainers.forEach(function (tempContainer) {
	    let sensorId = tempContainer.firstElementChild;
	    if (sensorId.id == id) {
		let tempValue = sensorId.querySelector('div.temperature-value');
		tempValue.innerHTML = payload.temperature + ' °C';
	    }
	});
});
```
As the result we obtain temperature updates in real time:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109682298-94f34a00-7b7e-11eb-8074-a69da059e661.gif">
</p>

### Plugins
New types of devices can be created with `HomeHelper.SDK`. All created devices must implement `IDevice` interface:
```
public interface IDevice
{
	string Name { get; }
	ProtocolType Protocol { get; }
	DeviceType Type { get; }
	void ConfigureServices(IServiceCollection services);
}
```
HTTP devices should implement extended interfaces, based on device type e.g: `ISwitch`:
```
public interface ISwitch : IDevice
{
	Task<bool> TurnOn(string ip, int port);
	Task<bool> TurnOff(string ip, int port);
	Task<int?> SignalStrength(string ip, int port);
	Task<bool?> IsOn(string ip, int port);
}
```
MQTT and Zigbee devices should implement `IMqtt` interface:
```
public interface IMqtt : IDevice
{
	public IEnumerable<IMqttSubscribeMessage> SubscribeMessages { get; }
	public IEnumerable<IMqttPublishMessage> PublishMessages { get; }
}
```
`IMqttSubscribeMessage` describes messages published by device and which can be subscribed by the application. `IMqttPublishMessage` describes messages subcribed by device and which can be published by the application to control it.

There can be find some implemented devices inside `HomeHelper.Sonoff`. [Message structures of Zigbee devices can be found here](https://www.zigbee2mqtt.io/information/supported_devices.html).

After creating new device, you need to put the corresponding `.dll` file inside `plugins` folder. During application startup, `DeviceLoader` class loads all `.dll` files and instantiates of all classes which implement `IDevice` interface. This functionality was built with [McMaster.NETCore.Plugins](https://github.com/natemcmaster/DotNetCorePlugins).

### Example application views

#### Application main page:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109705887-93cf1680-7b98-11eb-80b9-044bf4e12c95.JPG">
</p>

#### Application main page mobile view:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109705892-9467ad00-7b98-11eb-9e4e-a2353a41d2af.JPG">
</p>

#### Application login view:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109705893-95004380-7b98-11eb-900e-143394c6051d.JPG">
</p>

#### Application register view with client validation:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109705894-95004380-7b98-11eb-8e53-7440ba78b231.JPG">
</p>

#### List of rooms:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109708138-4e601880-7b9b-11eb-939e-e8b12aa0d2e2.JPG">
</p>

#### Toggle HTTP and Zigbee switches:
<p align="center"> 
<img src="https://user-images.githubusercontent.com/37352041/109708106-443e1a00-7b9b-11eb-8ffd-f1da15c9bf2e.gif">
</p>

## Setup
To run this project, deploy the application to your device (`Raspberry Pi` is preferrable) and update your `MySQL` database with provided migrations. Then you have to plug the `Zigbee adapter` to your device's `usb port`. Once you have it, install `Zigbee2mqtt` and change its configuration based on `application.json` file. When everything is configured, first run the application and next start `Zigbee2mqtt`. Application is now ready to access under port `1884`. If you want your application to be accessed outside of you local network, you need to expose `1884` port outside by `port forwarding` mechanism.
