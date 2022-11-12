#include <Arduino.h>
#include "oven.hpp"

Oven oven;

int step = 0;

// void setupWifi()
// {
//   Serial.print("Connecting Wifi");
//   WiFi.begin(ssid, pass);
//   while (WiFi.status() != WL_CONNECTED)
//   {
//     delay(500);
//     Serial.println("Connecting to WiFi..");
//   }

//   Serial.println("Connected to the WiFi network");
// }

void setup()
{
  Serial.begin(115200);
  Serial.println();
  Serial.println(F("Smart Oven starting"));

  // setupWifi();

  oven.Create();
  Serial.println(F("Oven created"));
}

void loop()
{
  oven.DoCheck();
  delay(1000);
}