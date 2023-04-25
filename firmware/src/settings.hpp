#ifndef SETTINGS_H
#define SETTINGS_H

class Settings
{
public:
    Settings();

    volatile double histeresys;
    volatile double maxTemperature;

    // dimmers
    int topElementPwm;
    int topElementZc;
    int bottomElementPwm;
    int bottomElementZc;

    // max6675
    int topThermoDO;
    int topThermoCS;
    int topThermoCLK;
    int bottomThermoDO;
    int bottomThermoCS;
    int bottomThermoCLK;

    // cpu
    int topCpuThermoVcc;
    int topCpuThermoSensor;
    int bottomCpuThermoVcc;
    int bottomCpuThermoSensor;

    int sampleTime;

    // relay
    int relayPin;
};

extern Settings settings;

#endif