#ifndef COMMUNICATIONCALLBACK_H
#define COMMUNICATIONCALLBACK_H

#include <BLEDevice.h>
#include "oven.hpp"

// callback  para envendos das características
class CommunicationCallbacks : public BLECharacteristicCallbacks
{
    Oven *oven;

public:
    CommunicationCallbacks(Oven *oven);

    void onWrite(BLECharacteristic *characteristic);
};

#endif