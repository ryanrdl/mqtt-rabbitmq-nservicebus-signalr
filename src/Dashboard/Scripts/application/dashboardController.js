﻿app.controller("dashboardController", [
    '$scope', '$http', function ($scope, $http) {

        $scope.ui = {
            temperatureDevices: [],
            humidityDevices: []
        }

        $scope.actions = {
            addNewTemperatureDevice: null,
            removeTemperatureDevice: null, 
            addNewHumidityDevice: null,
            removeHumidityDevice: null 
        };

        $scope.actions.addNewTemperatureDevice = function () {
            $http.post('/api/devices/temperature').success(function (deviceId) {
                //console.log('command sent to add temperature device with id ' + deviceId);
            });
        }

        $scope.actions.removeTemperatureDevice = function (deviceId) {
            $http.delete('/api/devices/temperature?deviceId=' + deviceId).success(function () {
                //console.log('command sent to delete temperature device with id ' + deviceId);
            });
        };

        $scope.actions.addNewHumidityDevice = function () {
            $http.post('/api/devices/humidity').success(function (deviceId) {
                //console.log('command sent to add humidity device with id ' + deviceId);
            });
        } 

        var temperatureChart = new SmoothieChart();
        temperatureChart.streamTo(document.getElementById("temperature"), 500);

        var humidityChart = new SmoothieChart();
        humidityChart.streamTo(document.getElementById("humidity"), 500);


        var hub = $.connection.deviceHub;

        var usedColors = [];
        function getColor() {
            var color = null;
            while (!color) {
                color = '#' + Math.floor(Math.random() * 16777215).toString(16);
                for (var i = 0; i < usedColors.length; i++) {
                    if (usedColors[i] === color) {
                        color = null;
                        break;
                    }
                }
            }
            return color;
        }

        var c = getColor();
        var coapChart = new SmoothieChart();
        coapChart.streamTo(document.getElementById("coap"), 500);
        var t = new TimeSeries();
        var d = {
            id: 1,
            avg: 0,
            above: 0,
            below: 0,
            style: { "color": c },
            timeSeries: t
        };

        coapChart.addTimeSeries(t, { strokeStyle: c, lineWidth: 4 });

        var i = 0;
        setInterval(function() {
            var n = i++ % 5;
            var v = 0;
            if (n == 0) {
                v = 1;
            }

            d.timeSeries.append(new Date().getTime(), v);
        }, 1000);

        function addTemperatureDevice(deviceId) {
            var found = false;
            for (var i = 0; i < $scope.ui.temperatureDevices.length; i++) {
                if ($scope.ui.temperatureDevices[i].id === deviceId) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                var color = getColor();

                var timeSeries = new TimeSeries();
                $scope.ui.temperatureDevices.push({
                    id: deviceId,
                    avg: 0,
                    above: 0,
                    below: 0,
                    style: { "color": color }, 
                    timeSeries: timeSeries
                });

                temperatureChart.addTimeSeries(timeSeries, { strokeStyle: color, lineWidth: 4 });
            }
        }

        function addHumidityDevice(deviceId) {
            var found = false;
            for (var i = 0; i < $scope.ui.humidityDevices.length; i++) {
                if ($scope.ui.humidityDevices[i].id === deviceId) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                var color = getColor();

                var timeSeries = new TimeSeries();
                $scope.ui.humidityDevices.push({
                    id: deviceId,
                    avg: 0,
                    above: 0,
                    below: 0,
                    style: { "color": color }, 
                    timeSeries: timeSeries
                });

                humidityChart.addTimeSeries(timeSeries, { strokeStyle: color, lineWidth: 4 });
            }
        }

        hub.client.addTemperatureDevice = function (deviceId) {
            //console.log('addTemperatureDevice ' + deviceId);
            addTemperatureDevice(deviceId);
        };

        hub.client.removeTemperatureDevice = function (deviceId) {
            //console.log('removeTemperatureDevice ' + deviceId);
            $scope.ui.temperatureDevices = _.filter($scope.ui.temperatureDevices, function (device) {
                return device.id !== deviceId;
            });
        };

        hub.client.addHumidityDevice = function (deviceId) {
            //console.log('addHumidityDevice ' + deviceId);
            addHumidityDevice(deviceId);
        };

        hub.client.removeHumidityDevice = function (deviceId) {
            //console.log('removeHumidityDevice ' + deviceId);
            $scope.ui.humidityDevices = _.filter($scope.ui.humidityDevices, function (device) {
                return device.id !== deviceId;
            });
        };

        hub.client.temperatureChanged = function (message) {
            //console.log('temperatureChanged ' + message.DeviceId);
            addTemperatureDevice(message.DeviceId);
            for (var i = 0; i < $scope.ui.temperatureDevices.length; i++) {
                if ($scope.ui.temperatureDevices[i].id === message.DeviceId) {
                    $scope.ui.temperatureDevices[i].avg = message.Average;
                    $scope.ui.temperatureDevices[i].timeSeries.append(new Date().getTime(), parseFloat(message.Average));
                    break;
                }
            }
        };

        hub.client.temperatureAboveThreshold = function (deviceId) {
            //console.log('temperatureAboveThreshold ' + deviceId);
            addTemperatureDevice(deviceId);
            for (var i = 0; i < $scope.ui.temperatureDevices.length; i++) {
                if ($scope.ui.temperatureDevices[i].id === deviceId) {
                    $scope.ui.temperatureDevices[i].above += 1;
                    break;
                }
            }
        };

        hub.client.temperatureBelowThreshold = function (deviceId) {
            //console.log('temperatureBelowThreshold ' + deviceId);
            addTemperatureDevice(deviceId);
            for (var i = 0; i < $scope.ui.temperatureDevices.length; i++) {
                if ($scope.ui.temperatureDevices[i].id === deviceId) {
                    $scope.ui.temperatureDevices[i].below += 1;
                    break;
                }
            }
        };

        hub.client.humidityChanged = function (message) {
            //console.log('humidityChanged ' + message.DeviceId);
            addHumidityDevice(message.DeviceId);
            for (var i = 0; i < $scope.ui.humidityDevices.length; i++) {
                if ($scope.ui.humidityDevices[i].id === message.DeviceId) {
                    $scope.ui.humidityDevices[i].avg = message.Average;
                    $scope.ui.humidityDevices[i].timeSeries.append(new Date().getTime(), parseFloat(message.Average));
                    break;
                }
            }
        };

        hub.client.humidityAboveThreshold = function (deviceId) {
            //console.log('humidityAboveThreshold ' + deviceId);
            addHumidityDevice(deviceId);
            for (var i = 0; i < $scope.ui.humidityDevices.length; i++) {
                if ($scope.ui.humidityDevices[i].id === deviceId) {
                    $scope.ui.humidityDevices[i].above += 1;
                    break;
                }
            }
        };

        hub.client.humidityBelowThreshold = function (deviceId) {
            //console.log('humidityBelowThreshold ' + deviceId);
            addHumidityDevice(deviceId);
            for (var i = 0; i < $scope.ui.humidityDevices.length; i++) {
                if ($scope.ui.humidityDevices[i].id === deviceId) {
                    $scope.ui.humidityDevices[i].below += 1;
                    break;
                }
            }
        };

        //$.connection.hub.logging = true; 
        $.connection.hub.start();
    }
]);