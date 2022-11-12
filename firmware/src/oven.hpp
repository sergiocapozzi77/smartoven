#ifndef OVEN_H
#define OVEN_H

#include "elementcontroller.hpp"
#include "temperaturecontroller.hpp"
#include "cputemperaturecontroller.hpp"
#include "element.hpp"
#include "blemanager.hpp"
#include "ovenstatus.hpp"
#include "relay.hpp"

class Oven
{
    BLEManager *bleManager;

    OvenStatus ovenStatus;
    TemperatureController *topTemperatureController;
    TemperatureController *bottomTemperatureController;
    ElementController *topElementController;
    ElementController *bottomElementController;

public:
    Relay *relay;
    Element *topElement;
    Element *bottomElement;
    CpuTemperatureController *topCpuTemperatureController;
    CpuTemperatureController *bottomCpuTemperatureController;

    Oven();

    void Create();
    void DoCheck();

    void SwitchOn();
    void SwitchOff();
    OvenStatus *GetStatus();
    void SetTopDesiredTemperature(double temperature);
    void SetBottomDesiredTemperature(double temperature);
    void SetTopPower(double power);
    void SetBottomPower(double power);
    void SetTopMaxPower(double power);
    void SetBottomMaxPower(double power);
};

#endif