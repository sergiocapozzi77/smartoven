#define NO_GLOBAL_BLYNK

#include <Arduino.h>
#include "oven.hpp"

/* Fill-in information from Blynk Device Info here */
#define BLYNK_TEMPLATE_ID "TMPL5MF04nUUK"
#define BLYNK_TEMPLATE_NAME "SmartOven"
#define BLYNK_AUTH_TOKEN "7iBnqofxQLr-PccYmZk8GHO8S5bzWG_I"

/* Comment this out to disable prints and save space */
#define BLYNK_PRINT Serial

#include <WiFi.h>
#include <WiFiClient.h>
#include <BlynkSimpleEsp32.h>

// Your WiFi credentials.
// Set password to "" for open networks.
char ssid[] = "TP-Link_8724";
char pass[] = "40950211";

BlynkTimer timer;
Oven oven;

int step = 0;

// This function is called every time the Virtual Pin 0 state changes
BLYNK_WRITE(V0)
{
  // Set incoming value from pin V0 to a variable
  int value = param.asInt();

  // Update state
  Blynk.virtualWrite(V1, value);
}

// This function is called every time the device is connected to the Blynk.Cloud
BLYNK_CONNECTED()
{
  // Change Web Link Button message to "Congratulations!"
  Blynk.setProperty(V3, "offImageUrl", "https://static-image.nyc3.cdn.digitaloceanspaces.com/general/fte/congratulations.png");
  Blynk.setProperty(V3, "onImageUrl", "https://static-image.nyc3.cdn.digitaloceanspaces.com/general/fte/congratulations_pressed.png");
  Blynk.setProperty(V3, "url", "https://docs.blynk.io/en/getting-started/what-do-i-need-to-blynk/how-quickstart-device-was-made");
}

// This function sends Arduino's uptime every second to Virtual Pin 2.
void myTimerEvent()
{
  // You can send any value at any time.
  // Please don't send more that 10 values per second.
  Blynk.virtualWrite(V2, millis() / 1000);
}

// This function sends Arduino's uptime every second to Virtual Pin 2.
void ovenTimer()
{
  oven.DoCheck();
}

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
  Blynk.begin(BLYNK_AUTH_TOKEN, ssid, pass);
  // You can also specify server:
  // Blynk.begin(BLYNK_AUTH_TOKEN, ssid, pass, "blynk.cloud", 80);
  // Blynk.begin(BLYNK_AUTH_TOKEN, ssid, pass, IPAddress(192,168,1,100), 8080);

  // Setup a function to be called every second
  timer.setInterval(1000L, myTimerEvent);
  timer.setInterval(1000L, ovenTimer);
}

void loop()
{
  Blynk.run();
  timer.run();
}