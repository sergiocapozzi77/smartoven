#ifndef ELEMENTCONTROLLER_H
#define ELEMENTCONTROLLER_H

#include "element.hpp"
#include "ovenstatus.hpp"
#include <PID_v1.h>

class ElementController
{
    Element *element;
    OvenStatus *ovenStatus;

    double PidSetpoint, PidInput, PidOutput;
    double Kp = 2, Ki = 5, Kd = 1;
    PID *pid;

public:
    ElementController(OvenStatus *status, Element *element);
    void CheckTemperature();
};

#endif