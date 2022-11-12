#ifndef CPUTEMPERATURECONTROLLER_H
#define CPUTEMPERATURECONTROLLER_H

#include <Thermistor.h>
#include <Thermo.h>
#include <NTC_Thermistor.h>

class CpuTemperatureController
{
    int vcc_pin;
    NTC_Thermistor *thermistor;

public:
    CpuTemperatureController(
        const int vcc_pin,
        const int sensor_pin,
        const double referenceResistance,
        const double nominalResistance,
        const double nominalTemperatureCelsius,
        const double bValue,
        const int adcResolution);

    double GetTemperature();
};

#endif