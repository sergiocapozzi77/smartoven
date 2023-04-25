#include "oven.hpp"
#include "settings.hpp"
#include <BlynkSimpleEsp32.h>

Oven::Oven()
{
    this->ovenStatus.status = 0;
}

void Oven::Create()
{
    Serial.println(F("Creating cpu controllers "));

    this->topCpuTemperatureController = new CpuTemperatureController(
        settings.topCpuThermoVcc,
        settings.topCpuThermoSensor,
        100000, 7000, 25.5, 3950, 4095);
    this->bottomCpuTemperatureController = new CpuTemperatureController(
        settings.bottomCpuThermoVcc,
        settings.bottomCpuThermoSensor,
        100000, 7000, 25.5, 3950, 4095);

    Serial.println(F("Creating temperature controllers Top"));
    this->topTemperatureController = new TemperatureController(
        settings.topThermoDO,
        settings.topThermoCS,
        settings.topThermoCLK);
    Serial.println(F("Creating temperature controllers Bottom"));
    this->bottomTemperatureController = new TemperatureController(
        settings.bottomThermoDO,
        settings.bottomThermoCS,
        settings.bottomThermoCLK);

    Serial.println(F("Creating elements"));
    this->topElement = new Element(this->topTemperatureController, settings.topElementPwm, settings.topElementZc, ELEMENT_TOP, "Top");
    this->bottomElement = new Element(this->bottomTemperatureController, settings.bottomElementPwm, settings.bottomElementZc, ELEMENT_BOTTOM, "Bottom");

    Serial.println(F("Creating element controllers"));
    this->topElementController = new ElementController(&this->ovenStatus, this->topElement);
    this->bottomElementController = new ElementController(&this->ovenStatus, this->bottomElement);

    this->relay = new Relay(settings.relayPin);

    Serial.println(F("Creating ble manager"));
    this->bleManager = new BLEManager(this);
    this->bleManager->SetupBLE();
}

OvenStatus *Oven::GetStatus()
{
    return &this->ovenStatus;
}

float tt = 1;
void Oven::DoCheck()
{
    tt += 1.5;
    Blynk.virtualWrite(V3, tt);
    // Serial.println("Start checks");

    if (this->ovenStatus.status == 0)
    {
        this->SwitchOff();
        return;
    }

    double topCpuTemperature = this->topCpuTemperatureController->GetTemperature();
    double bottomCpuTemperature = this->bottomCpuTemperatureController->GetTemperature();
    /*Serial.print("Top CPU temperature: ");
    Serial.println(topCpuTemperature);
    Serial.print("Bottom CPU temperature: ");
    Serial.println(bottomCpuTemperature);*/

    if (topCpuTemperature > 60 ||
        bottomCpuTemperature > 60)
    {
        if (this->ovenStatus.status == 1)
        {
            Serial.println("CPU temperature too high. Switching off");
        }

        this->SwitchOff();
        return;
    }

    this->topElementController->CheckTemperature();
    this->bottomElementController->CheckTemperature();
    // Serial.println("End checks");
}

void Oven::SwitchOn()
{
    this->ovenStatus.status = 1;
}

void Oven::SwitchOff()
{
    this->ovenStatus.status = 0;

    this->topElement->SwitchOff();
    this->bottomElement->SwitchOff();
}

void Oven::SetTopDesiredTemperature(double temperature)
{
    this->topElement->SetDesiredTemperature(temperature);
}

void Oven::SetBottomDesiredTemperature(double temperature)
{
    this->bottomElement->SetDesiredTemperature(temperature);
}

void Oven::SetTopPower(double power)
{
    this->topElement->SetPower(power);
}

void Oven::SetBottomPower(double power)
{
    this->bottomElement->SetPower(power);
}

void Oven::SetTopMaxPower(double power)
{
    this->topElement->SetMaxPower(power);
}

void Oven::SetBottomMaxPower(double power)
{
    this->bottomElement->SetMaxPower(power);
}
