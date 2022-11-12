
#ifndef ELEMENTREADERCALLBACK_H
#define ELEMENTREADERCALLBACK_H

#include <BLEDevice.h>
#include "element.hpp"

// callback  para envendos das características
class ElementReaderCallback : public BLECharacteristicCallbacks
{
    Element *topElement;
    Element *bottomElement;

public:
    ElementReaderCallback(Element *topElement, Element *bottomElement);
    void onRead(BLECharacteristic *pCharacteristic);
};

#endif