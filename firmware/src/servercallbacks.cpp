#include "servercallbacks.hpp"
#include <Arduino.h>
#include <BLEDevice.h>
#include <Update.h>

ServerCallbacks::ServerCallbacks()
{
    this->deviceConnected = false;
}

void ServerCallbacks::onConnect(BLEServer *pServer)
{
    deviceConnected = true;
    Serial.println("Device connected");
}

void ServerCallbacks::onDisconnect(BLEServer *pServer)
{
    deviceConnected = false;
    Serial.println("Device disconnected");
    if (Update.isRunning())
    {
        Update.abort();
    }

    BLEDevice::startAdvertising();
}

bool ServerCallbacks::IsDeviceConnected()
{
    return this->deviceConnected;
}