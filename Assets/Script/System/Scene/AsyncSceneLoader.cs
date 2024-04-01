using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneFramework
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AsyncSceneLoader : MonoBehaviour
    {
        //public LoaderAnim _loaderAnim;
        [SerializeField] private Slider slider;
        public GameObject ContiuneText;
        [SerializeField] private Image _bg;
        [SerializeField] private Text _title;
        [SerializeField] private Text _text;
        private bool loaded;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void DoLoad(string sceneName)
        {
            /*            int randomIndex = Random.Range(0, this._loaderAnim._loaderAnims.Count);
                        LoaderAnim.LoaderAnimStruct _loaderAnim = this._loaderAnim._loaderAnims[randomIndex];
                        _bg.sprite = _loaderAnim.background;
                        _title.text = _loaderAnim.title;
                        _text.text = _loaderAnim.text;*/
            slider.value = 0;
            slider.gameObject.SetActive(true);
            ContiuneText.SetActive(false);

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
            if (loaded && Input.anyKeyDown)
            {
                loaded = false;
                StartCoroutine(Fading(1f));
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
            enabled = false;
        }

    }

}
