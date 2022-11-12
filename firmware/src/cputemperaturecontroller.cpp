#include "cputemperaturecontroller.hpp"

CpuTemperatureController::CpuTemperatureController(
    const int vcc_pin,
    const int sensor_pin,
    const double referenceResistance,
    const double nominalResistance,
    const double nominalTemperatureCelsius,
    const double bValue,
    const int adcResolution)
{
    this->vcc_pin = vcc_pin;

    this->thermistor = new NTC_Thermistor(
        sensor_pin,
        referenceResistance,
        nominalResistance,
        nominalTemperatureCelsius,
        bValue,
        adcResolution);

    pinMode(vcc_pin, OUTPUT);
}

double CpuTemperatureController::GetTemperature()
{
    digitalWrite(this->vcc_pin, HIGH);
    const double temperature = this->thermistor->readCelsius();
    digitalWrite(this->vcc_pin, LOW);

    if (isnan(temperature))
    {
        return 0;
    }

    return temperature;
}