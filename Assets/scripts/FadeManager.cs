using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;  // 싱글톤
    public Image fadeImage; // 검은 화면
    public float fadeDuration = 1.0f; // 페이드 시간
    private Transform fadePanelTransform; // FadePanel Transform 캐싱
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0); // 초기에는 완전 투명
    }

    private void AssignFadePanel()
    {
        GameObject fadePanelObject = GameObject.Find("FadePanel");

        if (fadePanelObject == null)
        {
            Debug.LogError("FadePanel을 찾을 수 없습니다! Hierarchy에서 이름을 확인하세요.");
            return;
        }

        fadePanelTransform = fadePanelObject.transform;
        fadeImage = fadePanelTransform.GetComponent<Image>();

        if (fadeImage == null)
        {
            Debug.LogError("FadePanel에 Image 컴포넌트가 없습니다!");
        }
    }

    public void StartFade(System.Action onFadeComplete)
    {
        StartCoroutine(FadeInOut(onFadeComplete));
    }

    private IEnumerator FadeInOut(System.Action onFadeComplete)
    {
        if (fadePanelTransform == null)
        {
            Debug.LogError("fadePanelTransform이 할당되지 않았습니다! AssignFadePanel()을 실행하세요.");
            AssignFadePanel();
            if (fadePanelTransform == null)
            {
                yield break;
            }
        }

        // 1️⃣ `FadePanel`을 UI 최상단으로 이동
        fadePanelTransform.SetAsLastSibling();
        fadePanelTransform.GetComponent<Canvas>().sortingOrder = 999; // 강제로 Sorting Order 조정
        Canvas.ForceUpdateCanvases(); // UI 강제 업데이트
        Debug.Log("FadePanel이 UI 최상단으로 이동");

        // 2️⃣ 페이드인 (화면 검게)
        yield return StartCoroutine(Fade(0f, 1f));

        // 3️⃣ 일정 시간 대기 (1초)
        yield return new WaitForSeconds(1f);

        // 4️⃣ 날짜 변경 함수 실행
        onFadeComplete?.Invoke();

        // 5️⃣ 페이드아웃 (화면 다시 밝게)
        yield return StartCoroutine(Fade(1f, 0f));

        // 6️⃣ `FadePanel`을 다시 UI 뒤로 이동 (버튼 클릭 가능하게 함)
        fadePanelTransform.SetAsFirstSibling();
        fadePanelTransform.GetComponent<Canvas>().sortingOrder = 0; // 다시 원래 Sorting Order로 복구
        Canvas.ForceUpdateCanvases(); // UI 강제 업데이트
        Debug.Log("FadePanel이 UI 뒤로 이동");
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
    }
}
