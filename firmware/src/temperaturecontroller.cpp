#include "temperaturecontroller.hpp"

TemperatureController::TemperatureController(
    int thermoDO,
    int thermoCS,
    int thermoCLK)
{
    this->thermocouple = new MAX6675(thermoCLK, thermoCS, thermoDO);
}

double TemperatureController::GetTemperature()
{
    float val = this->thermocouple->readCelsius();
    if (isnan(val))
    {
        return 0;
    }

    return val;
}