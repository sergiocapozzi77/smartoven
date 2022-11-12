#ifndef SERVERCALLBACKS_H
#define SERVERCALLBACKS_H

#include <BLEServer.h>

// callback para receber os eventos de conexão de dispositivos
class ServerCallbacks : public BLEServerCallbacks
{
    bool deviceConnected;

public:
    ServerCallbacks();
    void onConnect(BLEServer *pServer);
    void onDisconnect(BLEServer *pServer);

    bool IsDeviceConnected();
};

#endif