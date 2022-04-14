using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Dashboard
{
    public class DashboardManager : MonoBehaviour
    {   

        [SerializeField]
        private Text temp;

        [SerializeField]
        private Text humidity;
        
        [SerializeField]
        private CanvasGroup _canvasLayer2;

        [SerializeField]
        private CanvasGroup _canvasLayer1;

        [SerializeField]
        private Button _btn_config;

        [SerializeField]
        RectTransform uiHandleRectTransformLed;

        [SerializeField]
        RectTransform uiHandleRectTransformPump;
        
        

        private Tween twenFade;

        public void Update_Led_Status(string status){
            if (status == "ON") {
                uiHandleRectTransformLed.anchoredPosition = new Vector2(58f, 0.0f);
            } else {
                uiHandleRectTransformLed.anchoredPosition = new Vector2(-58f, 0.0f);
            }
        }
        public void Update_Pump_Status(string status){
            if (status == "ON"){
                uiHandleRectTransformPump.anchoredPosition = new Vector2(58f, 0.0f);
            } else 
            {
                uiHandleRectTransformPump.anchoredPosition = new Vector2(-58f, 0.0f);
            }
        }

        public void Update_Status(Status_Data _status_data)
        {
            temp.text = _status_data.temp + " °C";
            humidity.text = _status_data.humidity + " %";
        }

        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }

            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }

        public void Disable_Config_Btn()
        {
            _btn_config.interactable = false;
        }

        IEnumerator _IESwitchLayer()
        {
                if (_canvasLayer1.interactable == true)
                {
                    FadeOut(_canvasLayer1, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    FadeIn(_canvasLayer2, 0.25f);
                }
                else
                {
                    FadeOut(_canvasLayer2, 0.25f);
                    yield return new WaitForSeconds(0.5f);
                    FadeIn(_canvasLayer1, 0.25f);
                }
        }
        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }

    }
}

