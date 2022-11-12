#include "element.hpp"
#include "settings.hpp"

Element::Element(TemperatureController *temperatureController, int pwm_pin, int zc_pin, ELEMENT_POSITION element_position, String name)
{
    this->temperatureController = temperatureController;

    this->hasDesiredTemperatureReached = false;
    this->desiredTemperature = 0.0;
    this->status = 0;
    this->name = name;
    this->maxPower = 0;

    this->dimmer = new Dimmer(pwm_pin, zc_pin, element_position);
    // this->dimmer = new dimmerLamp(pwm_pin, zc_pin);
    // this->dimmer->begin(NORMAL_MODE, ON);

    this->dimmer->setPower(0);
}

double Element::GetTemperature()
{
    return this->temperatureController->GetTemperature();
}

void Element::SwitchOn()
{
    this->status = 1;
    Serial.println(this->name + ": switching element ON");
}

void Element::SwitchOff()
{
    if (this->status == 0 && this->dimmer->getPower() == 0)
    {
        return;
    }

    this->dimmer->setPower(0);
    this->status = 0;
    Serial.println(this->name + ": switching element OFF");
}

void Element::SetDesiredTemperature(double desiredTemperature)
{
    if (desiredTemperature > settings.maxTemperature)
    {
        desiredTemperature = settings.maxTemperature;
    }

    this->desiredTemperature = desiredTemperature;
}

double Element::GetDesiredTemperature(void)
{
    return this->desiredTemperature;
}

void Element::SetPower(double pow)
{
    if (pow > 0)
    {
        this->status = 1;
    }
    else
    {
        this->status = 0;
    }

    // Serial.print(this->name);
    // Serial.print(" set power to: ");
    // Serial.println(pow);

    if (this->maxPower > 0 && pow > this->maxPower)
    {
        this->dimmer->setPower(this->maxPower);
    }
    else
    {
        this->dimmer->setPower(pow);
    }
}

void Element::SetMaxPower(double pow)
{
    this->maxPower = pow;
}

int Element::GetStatus(void)
{
    return this->status;
}

double Element::GetPower(void)
{
    return this->dimmer->getPower();
}

double Element::GetMaxPower(void)
{
    return this->maxPower;
}