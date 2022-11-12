#include "otacallback.hpp"
#include "Arduino.h"
#include <Update.h>

OtaCallbacks::OtaCallbacks(BLECharacteristic *characteristicOtaFinish)
{
    this->characteristicOtaFinish = characteristicOtaFinish;

    updateFlag = false;
    readyFlag = false;
    bytesReceived = 0;
    timesWritten = 0;
}

void OtaCallbacks::onWrite(BLECharacteristic *pCharacteristic)
{
    std::string rxValue = pCharacteristic->getValue();
    // verifica se existe dados (tamanho maior que zero)
    if (rxValue.length() > 0)
    {
        String command = String(rxValue.c_str());
        if (rxValue == "@END")
        {
            if (Update.end(true))
            { // true to set the size to the current progress
                Serial.printf("] Update Success: \nRebooting...\n");
                this->characteristicOtaFinish->setValue("SU");
                this->characteristicOtaFinish->notify();
                delay(5000);
                ESP.restart();
            }
            else
            {
                Update.printError(Serial);
                this->characteristicOtaFinish->setValue("ERE");
                this->characteristicOtaFinish->notify();
            }
        }
    }

    uint8_t *rxData = pCharacteristic->getData();
    size_t dataLength = pCharacteristic->getLength();

    if (!updateFlag)
    { // If it's the first packet of OTA since bootup, begin OTA
        Serial.println("Update OTA [");
        if (!Update.begin())
        { // start with max available size
            Serial.print("Update begin error: ");
            Update.printError(Serial);
            this->characteristicOtaFinish->setValue("ERB");
            this->characteristicOtaFinish->notify();
        }
        updateFlag = true;
    }

    Serial.print(".");
    if (dataLength > 0)
    {
        if (Update.write(rxData, dataLength) != dataLength)
        {
            Update.printError(Serial);
            this->characteristicOtaFinish->setValue("ERW");
            this->characteristicOtaFinish->notify();
        }
    }
}
