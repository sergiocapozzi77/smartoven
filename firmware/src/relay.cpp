#include "relay.hpp"
#include <Arduino.h>

Relay::Relay(int rel1_pin, int rel2_pin)
{
  this->rel_pins[0] = rel1_pin;
  this->rel_pins[1] = rel2_pin;

  pinMode(rel1_pin, OUTPUT);
  pinMode(rel2_pin, OUTPUT);
}

void Relay::SetStatus(int index, int status)
{
  if (status == 0)
  {
    digitalWrite(this->rel_pins[index], LOW);
  }
  else if (status == 1)
  {
    digitalWrite(this->rel_pins[index], HIGH);
  }
}
