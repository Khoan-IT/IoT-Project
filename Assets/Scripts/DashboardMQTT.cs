using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace Dashboard
{
    public class Status_Data
    {
        public string temp {get; set;}
        public string humidity {get; set;}
    }

    public class Led_Data
    {
        public string device = "LED";
        public string status = "OFF";
    }

    public class Pump_Data
    {
        public string device = "PUMP";
        public string status = "OFF";
    }

    public class DashboardMQTT : M2MqttUnityClient
    {
        public bool check = false;
        public Text broker_input;
        public Text username_input;
        public Text password_input;
        public List<string> topics = new List<string>();

        public void setClientSatus(){
            if(this.client != null){
                this.check = true;
            }
        }

        public void setMQTT(){
            if (broker_input.text == ""){
                this.brokerAddress = "mqttserver.tk";
            }else{
                this.brokerAddress = broker_input.text;
            }

            if(username_input.text == ""){
                this.mqttUserName = "bkiot";
            }else{
                this.mqttUserName = username_input.text;
            }

            if(password_input.text == ""){
                this.mqttPassword = "12345678";
            }else{
                this.mqttPassword = password_input.text;
            }
            
            this.Connect();
        }
 
        [SerializeField]
        public Status_Data _status_data;
        public Led_Data _led_data;
        public Pump_Data _pump_data;

        [SerializeField]
        public Text errormsg;


        
        public void publishLedStatus(){
            string msg ="";
            msg = _led_data.status=="ON" ? "{\"device\":\"LED\",\"status\":\"OFF\"}" : "{\"device\":\"LED\",\"status\":\"ON\"}";
            _led_data.status = _led_data.status=="ON"?"OFF":"ON";
            client.Publish(topics[1], System.Text.Encoding.UTF8.GetBytes(msg),MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Publish LED Status: " + _led_data.status);
        }

        public void publishPumpStatus(){
            string msg = "";
            msg = _pump_data.status=="ON" ? "{\"device\":\"PUMP\",\"status\":\"OFF\"}" : "{\"device\":\"PUMP\",\"status\":\"ON\"}";
            _pump_data.status = _pump_data.status=="ON"?"OFF":"ON";
            client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes(msg),MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            Debug.Log("Publish PUMP Status: " + _pump_data.status);
        }


        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
        }

        protected override void OnConnectionSuccess()
        {
            Debug.Log("CONNECTION SUCCESS!");
            GetComponent<DashboardManager>().SwitchLayer();
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            OnConnectionSuccess();
        }

        protected override void SubscribeTopics()
        {

            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log(errorMessage);
            errormsg.text = errorMessage;
        }


        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }

        protected override void Start()
        {

            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            if (topic == topics[0])
                ProcessMessageStatus(msg);
            else if (topic == topics[1])
                ProcessMessageLed(msg);
            else if (topic == topics[2])
                ProcessMessagePump(msg);
        }


        private void ProcessMessageLed(string msg){
            _led_data = JsonConvert.DeserializeObject<Led_Data>(msg);
            GetComponent<DashboardManager>().Update_Led_Status(_led_data.status);
        }
        private void ProcessMessageStatus(string msg){
            _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            GetComponent<DashboardManager>().Update_Status(_status_data);
        }

        private void ProcessMessagePump(string msg)
        {
            _pump_data = JsonConvert.DeserializeObject<Pump_Data>(msg);
            GetComponent<DashboardManager>().Update_Pump_Status(_pump_data.status);
        }
        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {

        }

        public void UpdateConfig()
        {
           
        }

        public void UpdateControl()
        {

        }
    }
}
