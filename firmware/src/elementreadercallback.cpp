
#include "elementreadercallback.hpp"

#include "Arduino.h"

ElementReaderCallback::ElementReaderCallback(Element *topElement, Element *bottomElement)
{
    this->topElement = topElement;
    this->bottomElement = bottomElement;
}

void ElementReaderCallback::onRead(BLECharacteristic *pCharacteristic)
{
    // Serial.println("* Reading temperature");
    char str[250];
    snprintf(str, 250, "{current:[%.2f, %.2f], desired:[%.2f, %.2f], power:[%.2f, %.2f], maxpower:[%.2f, %.2f], status:[%d, %d]}",
             this->topElement->GetTemperature(), this->bottomElement->GetTemperature(),
             this->topElement->GetDesiredTemperature(), this->bottomElement->GetDesiredTemperature(),
             this->topElement->GetPower(), this->bottomElement->GetPower(),
             this->topElement->GetMaxPower(), this->bottomElement->GetMaxPower(),
             this->topElement->GetStatus(), this->bottomElement->GetStatus());

    // Serial.println(str);
    pCharacteristic->setValue(str);
}