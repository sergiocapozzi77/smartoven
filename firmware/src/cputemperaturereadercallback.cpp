
#include "cputemperaturereadercallback.hpp"
#include "Arduino.h"

CpuTemperatureReaderCallback::CpuTemperatureReaderCallback(CpuTemperatureController *topCpuTemperatureController, CpuTemperatureController *bottomCpuTemperatureController)
{
    this->topCpuTemperatureController = topCpuTemperatureController;
    this->bottomCpuTemperatureController = bottomCpuTemperatureController;
}

void CpuTemperatureReaderCallback::onRead(BLECharacteristic *pCharacteristic)
{
    //Serial.println("* Reading cpu temperature");
    char str[100];
    snprintf(str, 100, "[%.2f, %.2f]", this->topCpuTemperatureController->GetTemperature(), this->bottomCpuTemperatureController->GetTemperature());
    pCharacteristic->setValue(str);
}