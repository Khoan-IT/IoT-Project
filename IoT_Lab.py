print("IoT Gateway")
import paho.mqtt.client as mqttclient
import time
import json
import serial.tools.list_ports




def get_port():
    ports = list(serial.tools.list_ports.comports())
    for p in ports:
        if 'BBC micro:bit' in str(p):
            return str(p).split('-')[0].strip()
    return ''

# bcc_port = get_port()
print(get_port())

BROKER_ADDRESS = "demo.thingsboard.io"
PORT = 1883
mess = ""

#TODO: Add your token and your comport
#Please check the comport in the device manager
THINGS_BOARD_ACCESS_TOKEN = "tTcr6kT44CNqxmmCYyfb"

# if len(bbc_port) > 0:
ser = serial.Serial(port=get_port(), baudrate=115200)

light = 0
temp = 0
def processData(data):
    global light
    global temp
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    print(splitData)
    # TODO: Add your source code to publish data to the server
    # Example demo hercules: !1:TEMP:35## !2:LIGHT:33##
    if splitData[1] == "TEMP":
        splitData[1] = "temperature"
    elif splitData[1] == "LIGHT":
        splitData[1] = "light"
    collect_data = {splitData[1]: int(splitData[2])}
    # print(collect_data)
    client.publish("v1/devices/me/telemetry", json.dumps(collect_data), 1)
    #TODO: Add your source code to publish data to the server

def readSerial():
    bytesToRead = ser.inWaiting()
    if (bytesToRead > 0):
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF-8")
        while ("#" in mess) and ("!" in mess):
            start = mess.find("!")
            end = mess.find("#")
            processData(mess[start:end + 1])
            if (end == len(mess)):
                mess = ""
            else:
                mess = mess[end+1:]


def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")

def recv_message(client, userdata, message):
    print("Received: ", message.payload.decode("utf-8"))
    temp_data = {'value': True}
    cmd = 0 
    #TODO: Update the cmd to control 2 devices
    try:
        jsonobj = json.loads(message.payload)
        if jsonobj['method'] == "setLED":
            temp_data['value'] = jsonobj['params']
            client.publish('v1/devices/me/Button_Led', json.dumps(temp_data), 1)
            if(temp_data['value'] == True):
                cmd =1
            else:
                cmd = 0    
        if jsonobj['method'] == "setFAN":
            temp_data['value'] = jsonobj['params']
            client.publish('v1/devices/me/Button_fan', json.dumps(temp_data), 1)
            if(temp_data['value'] == True):
                cmd = 3
            else:
                cmd = 2    
    except:
        pass

    if len(get_port()) > 0:
        ser.write((str(cmd) + "#").encode())

def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Thingsboard connected successfully!!")
        client.subscribe("v1/devices/me/rpc/request/+")
    else:
        print("Connection is failed")


client = mqttclient.Client("Gateway_Thingsboard")
client.username_pw_set(THINGS_BOARD_ACCESS_TOKEN)

client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()

client.on_subscribe = subscribed
client.on_message = recv_message


while True:

    if len(get_port()) >  0:
        readSerial()

    time.sleep(1)