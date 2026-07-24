using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroFade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textToUse;
    [SerializeField] private TextMeshProUGUI extraText;
    private int textIndex;
    private int maxIndex;

    private void Start()
    {
        foreach (var t in textToUse)
        {
            t.gameObject.SetActive(false);
        }
        extraText.gameObject.SetActive(false);
        
        textIndex = 0;
        maxIndex = textToUse.Length - 1;
        StartCoroutine(IntroFadeFunc());
    }

    private IEnumerator IntroFadeFunc()
    {
        while (textIndex <= maxIndex)
        {
            var text = textToUse[textIndex];
            text.gameObject.SetActive(true);

            yield return FadeInText(2f, text);
            if (textIndex == 2)
            {
                yield return new WaitForSeconds(1f);
                extraText.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(3f);
            yield return FadeOutText(1f, text);
            if (textIndex == 2)
            {
                extraText.gameObject.SetActive(false);
            }

            textIndex++;
        }

        SceneManager.LoadScene(2);
    }

    private IEnumerator FadeInText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }

    private IEnumerator FadeOutText(float timeSpeed, TextMeshProUGUI text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }
}
