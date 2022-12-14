using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public bool start;
    public float fadeDamp;
    public string fadeScene;
    public float alpha;
    public Color fadeColor;
    public bool isFadeIn;
    private Image bg;
    private float lastTime;
    private CanvasGroup myCanvas;

    private bool startedLoading;

    //Set callback
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    //Remove callback
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void InitiateFader()
    {
        DontDestroyOnLoad(gameObject);

        //Getting the visual elements
        if (transform.GetComponent<CanvasGroup>())
            myCanvas = transform.GetComponent<CanvasGroup>();

        if (transform.GetComponentInChildren<Image>())
        {
            bg = transform.GetComponent<Image>();
            bg.color = fadeColor;
        }

        //Checking and starting the coroutine
        if (myCanvas && bg)
        {
            myCanvas.alpha = 0.0f;
            StartCoroutine(FadeIt());
        }
        else
        {
            Debug.LogWarning("Something is missing please reimport the package.");
        }
    }

    private IEnumerator FadeIt()
    {
        while (!start)
            //waiting to start
            yield return null;
        lastTime = Time.time;
        var coDelta = lastTime;
        var hasFadedIn = false;

        while (!hasFadedIn)
        {
            coDelta = Time.time - lastTime;
            if (!isFadeIn)
            {
                //Fade in
                alpha = newAlpha(coDelta, 1, alpha);
                if (alpha == 1 && !startedLoading)
                {
                    startedLoading = true;
                    SceneManager.LoadScene(fadeScene);
                }
            }
            else
            {
                //Fade out
                alpha = newAlpha(coDelta, 0, alpha);
                if (alpha == 0) hasFadedIn = true;
            }

            lastTime = Time.time;
            myCanvas.alpha = alpha;
            yield return null;
        }

        Initiate.DoneFading();

        Debug.Log("Your scene has been loaded , and fading in has just ended");

        Destroy(gameObject);

        yield return null;
    }


    private float newAlpha(float delta, int to, float currAlpha)
    {
        switch (to)
        {
            case 0:
                currAlpha -= fadeDamp * delta;
                if (currAlpha <= 0)
                    currAlpha = 0;

                break;
            case 1:
                currAlpha += fadeDamp * delta;
                if (currAlpha >= 1)
                    currAlpha = 1;

                break;
        }

        return currAlpha;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIt());
        //We can now fade in
        isFadeIn = true;
    }
}