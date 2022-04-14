import random
import paho.mqtt.client as mqttclient
import time
import json
import subprocess as sp
import re
import requests
# import serial.tools.list_ports

USER_NAME = "bkiot"
PASSWORD = "12345678"
BROKER_ADDRESS = "mqttserver.tk"
PORT = 1883
TOPIC = ["/bkiot/1913832/status", "/bkiot/1913832/led", "/bkiot/1913832/pump"]



def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")

def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))

def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        for topic in TOPIC:
            client.subscribe(topic)
            print("Subcribed to: ", topic)
    else:
        print("Connection is failed")

#  connect to server mqttserver
client = mqttclient.Client()
client.on_connect = connected
client.on_message = recv_message
client.on_subscribe = subscribed
client.username_pw_set(USER_NAME, PASSWORD)
client.connect(BROKER_ADDRESS, PORT)
client.loop_start()

while True:
    temp = random.randint(0, 100)
    humidity = random.randint(0, 100)
    data = {"temp": temp, "humidity": humidity}
    client.publish(TOPIC[0], json.dumps(data))
    led = "ON" if random.randint(0,1)==1 else "OFF"
    data_led = {"device": "LED", "status": led}
    client.publish(TOPIC[1], json.dumps(data_led))
    pump = "ON" if random.randint(0,1)==1 else "OFF"
    data_pump = {"device": "PUMP", "status": pump}
    client.publish(TOPIC[2], json.dumps(data_pump))
    time.sleep(5)
