
#ifndef OVENSTATUSCALLBACK_H
#define OVENSTATUSCALLBACK_H

#include <BLEDevice.h>
#include "ovenstatus.hpp"

// callback  para envendos das características
class OvenStatusCallback : public BLECharacteristicCallbacks
{
    OvenStatus *ovenStatus;

public:
    OvenStatusCallback(OvenStatus *status);
    void onRead(BLECharacteristic *pCharacteristic);
};

#endif