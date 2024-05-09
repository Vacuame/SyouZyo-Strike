using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MyScene
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class AsyncSceneLoader : MonoBehaviour
    {
        //public LoaderAnim _loaderAnim;
        [SerializeField] private Slider slider;
        public GameObject ContiuneText;
        [SerializeField] private Image _bg;
        private bool loaded;
        private CanvasGroup canvasGroup;
        private float fadeTime;
        private bool needPressAnyKey;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void DoLoad(string sceneName, float fadeTime, bool needPressAnyKey)
        {
            this.fadeTime = fadeTime;
            this.needPressAnyKey = needPressAnyKey;

            loaded = false;
            gameObject.SetActive(true);
            slider.gameObject.SetActive(true);
            ContiuneText.SetActive(false);
            canvasGroup.alpha = 1;
            slider.value = 0;

            StartCoroutine(Loading(sceneName));
        }
        IEnumerator Loading(string sceneName)
        {
            AsyncOperation asy = SceneManager.LoadSceneAsync(sceneName);
            while (asy.isDone == false)
            {
                slider.value = asy.progress;
                yield return null;
            }

            slider.gameObject.SetActive(false);
            ContiuneText.SetActive(true);
            loaded = true;
        }

        private void Update()
        {
            if (loaded && 
                (!needPressAnyKey||(needPressAnyKey && Input.anyKeyDown)))
            {
                loaded = false;
                StartCoroutine(Fading(fadeTime));
            }
        }

        private IEnumerator Fading(float maxTime)
        {
            float timer = maxTime;
            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, timer / maxTime);
                yield return null;
            }
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

    }
}
