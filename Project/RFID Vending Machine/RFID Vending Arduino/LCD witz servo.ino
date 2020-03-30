#include <Wire.h> 
#include <LiquidCrystal_I2C.h>
#include <Servo.h>

#define SERVO_PIN 5
#define SERVO_A_PIN 8
#define SERVO_C_PIN 9

Servo doorServo;
// Set the pins on the I2C chip used for LCD connections:
//                    addr, en,rw,rs,d4,d5,d6,d7,bl,blpol
LiquidCrystal_I2C lcd(0x3F, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE);  // Setting LCD I2C address

void setup() {
  Serial.begin(9600);    
  lcd.begin(16, 2);     // initalize LCD display, 16 char in 1 row, there're 2 row

  pinMode(LED_PIN,OUTPUT);
  doorServo.attach(SERVO_PIN);
  lcd.backlight();

  
  lcd.setCursor(0, 0); // Setting LCD to Line 0
  lcd.print("Vending Machine!");
  delay(1000);
  lcd.setCursor(0, 1); // Setting LCD to line 1
  lcd.print("Wating RFID...");
  delay(8000);
}

void loop() {
 if(Serial.available())
 {
    int angel = 0;
    int ind=0;
    char buff[1] = {0};
    while(Serial.available())
    {
        unsigned char c = Serial.read();
        buff[ind] = c;
        if(ind++ >= 1) break;
    }

    Serial.println(buff);
    if(strcmp(buff, "1")==0)  
       servoA();
    else if(strcmp(buff, "2")==0)
        servoC();
    else if(strcmp(buff, "8")==0)
    {
      // Clear LCD msg in moniter
      lcd.clear();
      lcd.setCursor(0, 0); // Setting LCD to Line 0
      lcd.print("Vender Machine!");
      lcd.setCursor(0, 1);  // Setting LCD to line 1
      lcd.print("Wating RFID...");
    }
    else if(strcmp(buff, "9")==0)
    {
      // Clear LCD msg in moniter
      lcd.clear();
      lcd.setCursor(0, 0); // Setting LCD to Line 0
      lcd.print("Vender Machine!");
      lcd.setCursor(0, 1); // Setting LCD to line 1
      lcd.print("Press Button");
    }
   }
}

const int counterclockwise = 1700;
const int clockwise = 1700;
void servoA(){
   doorServo.write(0);                  // open door for vending 
   delay(15);                           // waits for servo to get there 
      
   for(int i=0; i<29; i++){             // change this to adjust +- full revolution
    digitalWrite(SERVO_A_PIN,HIGH);
    delayMicroseconds(clockwise); 
    digitalWrite(SERVO_A_PIN,LOW);
    delay(18.5); // 18.5ms 
   } 
  delay(1000);                       // some time for item to be redeemed 
  doorServo.write(90); delay(15);    //close door for vending 
}

void servoC(){
   doorServo.write(0);                  // open door for vending 
   delay(15);                           // waits for servo to get there 
      
   for(int i=0; i<29; i++){             // change this to adjust +- full revolution
    digitalWrite(SERVO_C_PIN,HIGH);
    delayMicroseconds(clockwise); 
    digitalWrite(SERVO_C_PIN,LOW);
    delay(18.5); // 18.5ms 
   } 
  delay(1000);                       // some time for item to be redeemed 
  doorServo.write(90); delay(15);    //close door for vending 
}
