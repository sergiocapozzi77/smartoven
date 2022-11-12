
#ifndef CPUTEMPERATUREREADERCALLBACK_H
#define CPUTEMPERATUREREADERCALLBACK_H

#include <BLEDevice.h>
#include "cputemperaturecontroller.hpp"

// callback  para envendos das características
class CpuTemperatureReaderCallback : public BLECharacteristicCallbacks
{
    CpuTemperatureController *topCpuTemperatureController;
    CpuTemperatureController *bottomCpuTemperatureController;

public:
    CpuTemperatureReaderCallback(CpuTemperatureController *topCpuTemperatureController, CpuTemperatureController *bottomCpuTemperatureController);
    void onRead(BLECharacteristic *pCharacteristic);
};

#endif