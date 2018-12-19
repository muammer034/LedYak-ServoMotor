#include <Servo.h>

Servo motor1;
Servo motor2;
int pos=0;
int x,y,z;

void setup() {
 Serial.begin(9600);
 motor1.attach(2); //atadığımız pin
 motor2.attach(3);
 motor1.write(90);// başlangıç konumu
 motor2.write(90);
}

void loop() 
{
  
if(Serial.available())
{
  pos= Serial.read();
    if(pos=='1')
 {   {
     x=pos;
    }
    motor1.write(150);
}
 else if(pos=='3')
 {   {
     x=pos;
    }
    motor1.write(30);
}
else if(pos=='2')
{
  {
     x=pos;
    }
  motor1.write(90);
}
       

}
{
  pos= Serial.read();
    if(pos=='4')
 {   {
     x=pos;
    }
    motor2.write(150);
}
 else if(pos=='6')
 {   {
     x=pos;
    }
    motor2.write(30);
}
else if(pos=='5')
{
  {
     x=pos;
    }
  motor2.write(90);
}
       

}


}
