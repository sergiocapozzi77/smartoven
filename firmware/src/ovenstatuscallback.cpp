
#include "ovenstatuscallback.hpp"
#include "Arduino.h"

OvenStatusCallback::OvenStatusCallback(OvenStatus *status)
{
    this->ovenStatus = status;
}

void OvenStatusCallback::onRead(BLECharacteristic *pCharacteristic)
{
 //   Serial.println("* Reading oven status");
    char str[100];
    snprintf(str, 100, "{status:%d}", this->ovenStatus->status);
    pCharacteristic->setValue(str);
}