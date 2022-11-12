#ifndef BLEMANAGER_H
#define BLEMANAGER_H
#include <BLEServer.h>

#include "servercallbacks.hpp"

// See the following for generating UUIDs:
// https://www.uuidgenerator.net/
#define SERVICE_UUID "a18ba38e-c3d7-4df8-a6c2-906dfd6a605b"                 // UART service UUID
#define CHARACTERISTIC_UUID_COMMANDS "fec39133-ef77-4bd0-bc1d-d7e5c1290bf8" // commands
#define CHARACTERISTIC_UUID_NOTIFY "bae88bba-bd3f-4ef7-ad9b-d2585a42dab0"
#define CHARACTERISTIC_UUID_TEMPERATURES "a3068be8-1b05-11ed-861d-0242ac120002"     // element temperatures
#define CHARACTERISTIC_UUID_CPU_TEMPERATURES "c0fd6f7c-1b05-11ed-861d-0242ac120002" // cpu temperatures
#define CHARACTERISTIC_UUID_OVENSTATUS "5f1a489b-7fb6-49ac-b9a2-5807befe2c3a"       // oven status

#define OTA_SERVICE_UUID "ef19c991-0aaa-40c0-82cd-3602722a9cdb"            // OTA service UUID
#define CHARACTERISTIC_UUID_OTA "1bfab929-ec5c-4d21-8f06-494105887f68"     // OTA update
#define CHARACTERISTIC_UUID_VERSION "790dbac1-ee08-4dbf-8b4a-6914ed3fe6bf" // device firmware version
#define CHARACTERISTIC_UUID_OTA_FINISH "e4caefcc-fd51-42e8-8960-41fa20bb10a4"     // OTA update

extern uint32_t FIRMWARE_VERSION; // device firmware version

class Oven;

class BLEManager
{
    ServerCallbacks *serverCallbaks;
    Oven *oven;

    void CreateCommunicationCharactersitic(BLEService *service);
    void CreateReadTemperatureCharacteristic(BLEService *service);
    void CreateReadCpuTemperatureCharacteristic(BLEService *service);
    void CreateOvenStatusCharacteristic(BLEService *service);
    void CreateOTACharacteristic(BLEService *service);

public:
    BLEManager(Oven *oven);
    void SetupBLE();
    void SendNotification();
    bool IsDeviceConnected();
};

#endif