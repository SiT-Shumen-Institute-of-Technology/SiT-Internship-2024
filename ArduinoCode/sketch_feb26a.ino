  #include <HttpClient.h>
  #include <b64.h>

  #include <WiFi.h>
  #include <WiFiAP.h>
  #include <WiFiClient.h>
  #include <WiFiClientSecure.h>
  #include <WiFiGeneric.h>
  #include <WiFiMulti.h>
  #include <WiFiSTA.h>
  #include <WiFiScan.h>
  #include <WiFiServer.h>
  #include <WiFiType.h>
  #include <WiFiUdp.h>

  #include <SPI.h>
  #include <MFRC522.h>

  #define SS_PIN 5   // SDA
  #define RST_PIN 22   // RST

  MFRC522 mfrc522(SS_PIN, RST_PIN); 

  const char* ssid = "SiT_Academy-Tenda";
  const char* password = "telepoletaj5";

  const char serverAddress[] = "192.168.2.107";  
  IPAddress serverAddressTemp(192,168,2,107);

  const int serverPort = 5001;  
  const char kPath[] = "/api/cardreaderapi";
  const char postPath[] = "/api/cardreaderapi/apipost";


  WiFiClientSecure wifiClient;

  void setup() {
      Serial.begin(9600);
      initWiFi();
      wifiClient.setInsecure();
      delay(1000);
      Serial.println("");
      Serial.println("");
      delay(1000);
      GetRFIDCards();
      delay(1000);
      SPI.begin();
      mfrc522.PCD_Init();
      Serial.println("Scan an RFID tag...");
  }

  void loop() {
      // Check if a new card is present
      //mfrc522.PCD_Reset();

      if (!mfrc522.PICC_IsNewCardPresent() || !mfrc522.PICC_ReadCardSerial()) {
          return;
      }

      String code = "";

      // Print UID
      Serial.print("Card UID: ");
      for (byte i = 0; i < mfrc522.uid.size; i++) {
          code += String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " ");
          code += String(mfrc522.uid.uidByte[i], HEX);
      }
      Serial.println();
      Serial.println("Sending code: " + code);
      code.replace(" ", "");
      PostRFIDCode(code);
      // Stop communication with this tag
      mfrc522.PICC_HaltA();
      //mfrc522.PCD_StopCrypto1();

      delay(1000);
  }

  void initWiFi() {
    WiFi.mode(WIFI_STA);
    WiFi.begin(ssid, password);
    Serial.print("Connecting to WiFi ..");
    while (WiFi.status() != WL_CONNECTED) {
      Serial.print('.');
      delay(1000);
    }

    Serial.println("Connection established! ESP32 IP:");
    Serial.println(WiFi.localIP());
  }

  void PostRFIDCode(String code)
  {
    Serial.println("CONNECTING TO SERVER...");

    if(wifiClient.connect(serverAddress, serverPort))
    {
        Serial.println("SUCCESSFULLY CONNECTED TO SERVER");
        Serial.println("POSTING RFID CODE " + code);
        
        String url = String(postPath) + "?code=" + code;

        wifiClient.print(
        String("POST ") + url + " HTTP/1.1\r\n" +
        "Host: " + String(serverAddress) + ":" + String(serverPort) + "\r\n" +
        "Content-Length: "+ String(code.length()) + "\r\n" +
        "Connection: close\r\n\r\n");

        Serial.println("POSTED [" + code + "] TO SERVER");

        Serial.println(" ");

        String response = wifiClient.readString();
        Serial.println("Server response: ");
        Serial.println(String(response));
        delay(1000);
    }
    else
    {
      Serial.println("failed to connect to server");
    }

    wifiClient.stop();
    
  }

  void GetRFIDCards()
  {
    if(WiFi.status() == WL_CONNECTED)
    {
      int err =0;

      HttpClient http(wifiClient);

      err = http.get(serverAddress, serverPort, "/api/cardreaderapi");
    if (err == 0)
    {
      Serial.println("startedRequest ok");

      err = http.responseStatusCode();
      if (err >= 0)
      {
        Serial.print("Got status code: ");
        Serial.println(err);

        err = http.skipResponseHeaders();
        if (err >= 0)
        {
          int bodyLen = http.contentLength();
          Serial.print("Content length is: ");
          Serial.println(bodyLen);
          Serial.println();
          Serial.println("Body returned follows:");
        
          // Now we've got to the body, so we can print it out
          unsigned long timeoutStart = millis();
          char c;
          // Whilst we haven't timed out & haven't reached the end of the body
          while (http.connected() || http.available() )
          {
              if (http.available())
              {
                  c = http.read();
                  // Print out this character
                  Serial.print(c);
                
                  bodyLen--;
                  // We read something, reset the timeout counter
                  timeoutStart = millis();
              }
              else
              {
                  // We haven't got any data, so let's pause to allow some to
                  // arrive
                  delay(1000);
              }
          }
        }
        else
        {
          Serial.print("Failed to skip response headers: ");
          Serial.println(err);
        }
      }
      else
      {    
        Serial.print("Getting response failed: ");
        Serial.println(err);
      }
    }
    else
    {
      Serial.print("Connect failed: ");
      Serial.println(err);
    }
    http.stop();

    delay(1000);
  }
  }

