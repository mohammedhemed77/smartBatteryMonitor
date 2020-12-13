#include <SoftwareSerial.h>
#include <avr/sleep.h>    // Sleep Modes
#include <avr/power.h>

#define  TX       PB1
#define  RX       PB0
#define  CHARGER  PB4
#define  LED      PB3   
#define  ON       '0' 
#define  OFF      '1'
SoftwareSerial bluetoothPort (RX,TX) ;
byte inputBuffer = NULL ;

void setup(){
  bluetoothPort.begin(9600);
  // initialize chargerState = OFF - Relay is LOW Enable signal  
  DDRB |= 1<< CHARGER ;
  PORTB |= 1<<CHARGER ; 
  DDRB |= 1<<LED ;
  
  }


void loop(){

  // put attiny85 in power down sleep mode 
  set_sleep_mode(SLEEP_MODE_PWR_DOWN);
  sleep_enable();
  sleep_cpu();
   
  if (bluetoothPort.available()>0)
  inputBuffer = bluetoothPort.read();
  
  if (inputBuffer == ON)  PORTB |= 1<<CHARGER ; 
  if (inputBuffer == OFF)  PORTB &= (~(1<<CHARGER)) ; 
  PORTB ^= 1 << LED ;
  delay(1000);  
  }

 

/*
void goToSleep ()
  {
  set_sleep_mode(SLEEP_MODE_PWR_DOWN);
  ADCSRA = 0;            // turn off ADC
  power_all_disable ();  // power off ADC, Timer 0 and 1, serial interface
  sleep_enable();
  sleep_cpu();                             
  sleep_disable();   
  power_all_enable();    // power everything back on
  }  // end of goToSleep

    */
