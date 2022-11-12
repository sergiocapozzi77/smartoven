#include "elementcontroller.hpp"

#include "settings.hpp"

ElementController::ElementController(OvenStatus *status, Element *element)
{
    this->element = element;
    this->ovenStatus = status;

    this->pid = new PID(&PidInput, &PidOutput, &PidSetpoint, Kp, Ki, Kd, DIRECT);
    this->pid->SetSampleTime(settings.sampleTime);
    this->pid->SetMode(AUTOMATIC);
    this->pid->SetOutputLimits(0, 100);
}

void ElementController::CheckTemperature()
{
    if (this->ovenStatus->status == 0)
    {
        Serial.print("No check because oven status is off");
        this->element->SwitchOff();
        return;
    }

    double currentTemperature = this->element->GetTemperature();
    double desiredTemperature = this->element->GetDesiredTemperature();

    this->PidSetpoint = desiredTemperature;
    this->PidInput = currentTemperature;
    this->pid->Compute();
    // Serial.print("Pid Setpoint: ");
    // Serial.print(this->PidSetpoint);
    // Serial.print(" Pid Input: ");
    // Serial.print(this->PidInput);
    // Serial.print(" Pid Output: ");
    // Serial.print(this->PidOutput);
    // Serial.println();

    this->element->SetPower(PidOutput);
    /*
        Serial.print(this->element->GetName() + " current temperature: ");
        Serial.println(currentTemperature);
        Serial.print(this->element->GetName() + " desired temperature: ");
        Serial.println(desiredTemperature);
    */
}