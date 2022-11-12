#include "communicationcallback.hpp"
#include "Arduino.h"

CommunicationCallbacks::CommunicationCallbacks(Oven *oven)
{
    this->oven = oven;
}

void CommunicationCallbacks::onWrite(BLECharacteristic *characteristic)
{
    // retorna ponteiro para o registrador contendo o valor atual da caracteristica
    std::string rxValue = characteristic->getValue();
    // verifica se existe dados (tamanho maior que zero)
    if (rxValue.length() > 0)
    {
        String command = String(rxValue.c_str());
        Serial.println("*********");
        Serial.print("Received Value: ");
        Serial.println(command);

        // Do stuff based on the command received
        if (command == "ON")
        {
            Serial.print("Turning Oven On");
            this->oven->SwitchOn();
        }

        if (command == "OFF")
        {
            Serial.print("Turning Oven Off");
            this->oven->SwitchOff();
        }

        if (command.startsWith("TTO"))
        {
            String sTemp = command.substring(4, command.length() - 1);
            Serial.print("Setting top desired temperature: ");
            Serial.println(sTemp);
            double temperature = atof(sTemp.c_str());
            this->oven->SetTopDesiredTemperature(temperature);
        }

        if (command.startsWith("TBO"))
        {
            String sTemp = command.substring(4, command.length() - 1);
            Serial.print("Setting bottom desired temperature: ");
            Serial.println(sTemp);
            double temperature = atof(sTemp.c_str());
            this->oven->SetBottomDesiredTemperature(temperature);
        }

        if (command.startsWith("PWT"))
        {
            String sPow = command.substring(4, command.length() - 1);
            Serial.print("Setting top power: ");
            Serial.println(sPow);
            double power = atof(sPow.c_str());
            this->oven->SetTopPower(power);
        }

        if (command.startsWith("PWB"))
        {
            String sPow = command.substring(4, command.length() - 1);
            Serial.print("Setting bottom power: ");
            Serial.println(sPow);
            double power = atof(sPow.c_str());
            this->oven->SetBottomPower(power);
        }

        if (command.startsWith("REL1ON"))
        {
            Serial.print("REL1ON");
            this->oven->relay->SetStatus(0, 1);
        }
        if (command.startsWith("REL1OFF"))
        {
            Serial.print("REL1OFF");
            this->oven->relay->SetStatus(0, 0);
        }

        if (command.startsWith("REL2ON"))
        {
            Serial.print("REL2ON");
            this->oven->relay->SetStatus(1, 1);
        }
        if (command.startsWith("REL2OFF"))
        {
            Serial.print("REL2OFF");
            this->oven->relay->SetStatus(1, 0);
        }
        if (command.startsWith("BMX"))
        {
            String sPow = command.substring(4, command.length() - 1);
            Serial.print("Setting bottom max power: ");
            Serial.println(sPow);
            double power = atof(sPow.c_str());
            this->oven->SetBottomMaxPower(power);
        }
        if (command.startsWith("TMX"))
        {
            String sPow = command.substring(4, command.length() - 1);
            Serial.print("Setting top max power: ");
            Serial.println(sPow);
            double power = atof(sPow.c_str());
            this->oven->SetTopMaxPower(power);
        }

        Serial.println();
        Serial.println("*********");
    }
}