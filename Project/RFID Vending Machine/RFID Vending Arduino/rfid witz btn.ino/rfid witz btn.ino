#include <SPI.h>
#include <MFRC522.h>

#define RFID_RTS_PIN   A0
#define RFID_SDA_PIN   10

#define BTN1_PIN 2
#define BTN2_PIN 3

MFRC522 mfrc522(RFID_SDA_PIN,RFID_RTS_PIN);

String content = "";
boolean cardFlg = false;

void setup() 
{
  // put your setup code here, to run once:
  Serial.begin(9600);
//  Serial.print("RFID reader is setup \n");
  pinMode(BTN1_PIN, INPUT);
  pinMode(BTN2_PIN, INPUT);

  SPI.begin();
  mfrc522.PCD_Init();
}

void loop() 
{
 //  if(Serial.available()){
  // put your main code here, to run repeatedly:
  if(mfrc522.PICC_IsNewCardPresent() &&
     mfrc522.PICC_ReadCardSerial() &&
     cardFlg == false)
     {
        byte *id = mfrc522.uid.uidByte;
        byte idSize = mfrc522.uid.size;
        
        MFRC522::PICC_Type piccType = mfrc522.PICC_GetType(mfrc522.uid.sak);

        Serial.print(":");
        for(byte i = 0; i < idSize; i ++)
        {
            Serial.print(mfrc522.uid.uidByte[i], HEX);
            content.concat(String(mfrc522.uid.uidByte[i], HEX));
        }
        Serial.println(":");
        content.toUpperCase();
        cardFlg = true;
     }
     else if(cardFlg)
     {
        int switch1_Status = digitalRead(BTN1_PIN);
        int switch2_Status = digitalRead(BTN2_PIN);

        if( switch1_Status == 1)
        {
           Serial.println(":1:");
           cardFlg = false;
           mfrc522.PICC_HaltA();  // 讓卡片進入停止模式
        }
        else if( switch2_Status == 1)
        {
          Serial.println(":2:");
          cardFlg = false;
           mfrc522.PICC_HaltA();  // 讓卡片進入停止模式
        }
        else
        {
			// Do nothing
        }
     
     } 
}
