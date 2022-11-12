#include "dimmer.hpp"
#include <Arduino.h>

Dimmer *dimmers[5];
double power[5];
int on_cycles[5];     // the cycles the traic should be on
int current_cycle[5]; // the current cycle
int counter[5];
int triac_pins[5];
const int max_cycles = 20;

void ManageDimmer(ELEMENT_POSITION element_position)
{
  if (counter[element_position] > max_cycles)
  {
    counter[element_position] = 0;
  }

  if (counter[element_position] < on_cycles[element_position])
  {
    digitalWrite(triac_pins[element_position], HIGH);
  }
  else
  {
    digitalWrite(triac_pins[element_position], LOW);
  }

  counter[element_position]++;
}

void ZeroCrTop(void)
{
  ManageDimmer(ELEMENT_TOP);
}

void ZeroCrBottom(void)
{
  ManageDimmer(ELEMENT_BOTTOM);
}

Dimmer::Dimmer(int triac_pin, int interrupt_pin, ELEMENT_POSITION element_position)
{
  this->triac_pin = triac_pin;
  this->interrupt_pin = interrupt_pin;
  this->element_position = element_position;

  power[element_position] = 0;
  on_cycles[element_position] = 0;
  current_cycle[element_position] = 0;
  counter[element_position] = 0;
  triac_pins[element_position] = triac_pin;

  pinMode(triac_pin, OUTPUT);
  pinMode(interrupt_pin, INPUT_PULLUP);

  dimmers[element_position] = this;
  Serial.print("Added dimmer: ");
  Serial.println(element_position);

  if (element_position == ELEMENT_TOP)
  {
    attachInterrupt(interrupt_pin, ZeroCrTop, RISING);
  }
  else
  {
    attachInterrupt(interrupt_pin, ZeroCrBottom, RISING);
  }
}

void Dimmer::setPower(double pow) // pow is 0 to 100
{
  power[element_position] = pow;
  on_cycles[element_position] = pow / 5.0;

  //  Serial.print("Set On Cycles to : ");
  //  Serial.println(on_cycles[element_position]);
}

double Dimmer::getPower()
{
  return power[element_position];
}
