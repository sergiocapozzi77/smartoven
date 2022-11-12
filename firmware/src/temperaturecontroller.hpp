#ifndef TEMPERATURECONTROLLER_H
#define TEMPERATURECONTROLLER_H
#include <max6675.h>

class TemperatureController
{
    int thermoDO;
    int thermoCS;
    int thermoCLK;

    MAX6675 *thermocouple;

public:
    TemperatureController(
        int thermoDO,
        int thermoCS,
        int thermoCLK);

    double GetTemperature();
};

#endif