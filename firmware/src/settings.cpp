#include "settings.hpp"

Settings settings;

Settings::Settings()
{
    // this->histeresys = 5.0;
    this->histeresys = 0.0;
    this->maxTemperature = 500.0;
    this->topElementPwm = 25;
    this->topElementZc = 33;
    this->bottomElementPwm = 32;
    this->bottomElementZc = 35;

    this->topThermoDO = 19;
    this->topThermoCS = 18;
    this->topThermoCLK = 5;
    this->bottomThermoDO = 17;
    this->bottomThermoCS = 16;
    this->bottomThermoCLK = 4;

    this->topCpuThermoVcc = 14;
    this->topCpuThermoSensor = 36;
    this->bottomCpuThermoVcc = 26;
    this->bottomCpuThermoSensor = 34;

    this->relayPin = 21;

    this->sampleTime = 1000;
}
