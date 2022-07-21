using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Helpers
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Screen fade variables")] [SerializeField]
        private Image _fadeScreen;

        [SerializeField] private bool _transitionToggle;
        [Min(0.001f)] [SerializeField] private float _transitionSpeed = 1f;

        [Header("Loading Screen UI")] [SerializeField]
        private Slider _loadingBar;

        [SerializeField] private Canvas _loadScreenCanvas;

        [Header("GameObjects to be disabled while scene loading")] [SerializeField]
        private GameObject[] _objectsToDisable;

        private bool _isSceneLoading = false;
        private float _loadWaitTime = 1f; //loading screen will wait this long when scene has loaded before showing it

        public static SceneLoader Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void RestartLevel()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        public void LoadScene(string scene)
        {
            Time.timeScale = 1f;
            
            if (_transitionToggle)
                StartCoroutine(PlayTransition(scene));
            else
                TransitionScene(scene);
        }

        public void LoadScene(string scene, bool transitionToggle)
        {
            if (transitionToggle)
                StartCoroutine(PlayTransition(scene));
            else
                TransitionScene(scene);
        }

        private IEnumerator PlayTransition(string scene)
        {
            float alpha = 0;

            while (alpha < 1)
            {
                alpha += _transitionSpeed * Time.unscaledDeltaTime;
                var newColor = new Color(0, 0, 0, alpha);
                _fadeScreen.color = newColor;
                yield return null;
            }

            TransitionScene(scene);
        }

        private void TransitionScene(string scene)
        {
            if (!_isSceneLoading)
            {
                _isSceneLoading = true;
                StartCoroutine(AsyncLoad(scene));
                AkSoundEngine.PostEvent("stop_puzzle_time_running_out", gameObject);
            } else
            {
                Debug.Log("Tried to load new scene but a scene load is already in progress.");
            }
        }

        private IEnumerator AsyncLoad(string scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
            asyncLoad.allowSceneActivation = false;

            try
            {
                ControllerManager[] playerControllers = FindObjectsOfType<ControllerManager>();
                foreach (ControllerManager controller in playerControllers)
                    controller.gameObject.SetActive(false);
            }
            catch (NullReferenceException e)
            {
            }

            foreach (GameObject gameObject in _objectsToDisable)
                gameObject.SetActive(false);

            _loadScreenCanvas.gameObject.SetActive(true);

            while (asyncLoad.progress < 0.9f)
            {
                Debug.Log("Progress: " + asyncLoad.progress + " | Bar: " + _loadingBar.value);
                _loadingBar.value = asyncLoad.progress + 0.01f;
                yield return null;
            }

            _loadingBar.value = 1;
            yield return new WaitForSecondsRealtime(_loadWaitTime);

            asyncLoad.allowSceneActivation = true;
        }
    }
}