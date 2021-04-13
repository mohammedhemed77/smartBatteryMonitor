/* ******************************************************************************* *
 Monitor laptop Battery 220 V charger
 using ATTiny85 avr uC :
 -------------------------
 - POWER DOWN sleep mode , Internal crystal = 8Mhz
 - Wake up when receiving signal from HC06 bluetooth module
 - Using SoftwareSerial library to communicate with blth as UART module isn't 
   embedded in Attiny85 uC
 - Date   : 14 / 12 / 2020 
 - Author : MohammedHemed
 - SmartBatteryMonitor_v1
 * ******************************************************************************* */

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

 


