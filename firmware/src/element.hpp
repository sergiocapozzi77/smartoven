#ifndef ELEMENT_H
#define ELEMENT_H

#include "temperaturecontroller.hpp"
//#include <RBDdimmer.h>
#include "dimmer.hpp"

class Element
{
    TemperatureController *temperatureController;
    String name;
    bool hasDesiredTemperatureReached;
    double desiredTemperature;
    int status;
    double maxPower;
    // dimmerLamp *dimmer;
    Dimmer *dimmer;

public:
    Element(TemperatureController *temperatureController, int pwm_pin, int zc_pin, ELEMENT_POSITION element_position, String name);

    String GetName(void) { return this->name; };
    double GetTemperature();
    int GetStatus(void);
    double GetDesiredTemperature();
    void SetDesiredTemperature(double desiredTemperature);

    void SetPower(double pow);
    double GetPower(void);
    void SwitchOn();
    void SwitchOff();
    void SetMaxPower(double pow);
    double GetMaxPower();
};

#endif