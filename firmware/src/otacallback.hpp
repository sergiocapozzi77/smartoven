#ifndef OTACALLBACK_H
#define OTACALLBACK_H

#include <BLEDevice.h>

// callback  para envendos das características
class OtaCallbacks : public BLECharacteristicCallbacks
{
    bool updateFlag;
    bool readyFlag;
    int bytesReceived;
    int timesWritten;
    BLECharacteristic *characteristicOtaFinish;

public:
    OtaCallbacks(BLECharacteristic *characteristicOtaFinish);

    void onWrite(BLECharacteristic *characteristic);
};

#endif