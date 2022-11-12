#ifndef RELAY_H
#define RELAY_H

class Relay
{
  int rel_pins[5];

public:
  Relay(int rel1_pin, int rel2_pin);
  void SetStatus(int pin, int status);
};

#endif

/*
const byte INTERRUPT_PIN = 2;
const byte TRIAC_PIN = 9;

volatile int onCount = 0;
volatile int offCount = 0;
volatile bool state = false;
volatile int counter = 0;

int nextOnCount = 0;
int nextOffCount = 0;

void zeroCrossing() {
  counter++;
  if (counter >= (state ? onCount : offCount)) {
    counter = 0;
    if (!state ? onCount : offCount) {
      state = !state;
    }
  }
}

void setup() {
  Serial.begin(115200);
  pinMode(TRIAC_PIN, OUTPUT);
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), zeroCrossing, RISING);
}

void loop() {
  digitalWrite(TRIAC_PIN, state);

  if (Serial.available()) {
    nextOnCount = Serial.parseInt();
    nextOffCount = Serial.parseInt();
    Serial.find("\n");
    Serial.print(nextOnCount);
    Serial.print(':');
    Serial.println(nextOffCount);
    onCount = nextOnCount;
    offCount = nextOffCount;
    if (nextOnCount == 0) {
      onCount = 0;
      offCount = 0;
      state = false;
    }
  }
}
*/