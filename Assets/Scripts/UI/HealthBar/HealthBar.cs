using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BillBoard))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true;
    [SerializeField] bool shouldShowHealthNumbers = true;
    [SerializeField] Image redFillImg,yellowFillImg;
    [SerializeField] BillBoard billBoard;
    public GameObject goHeadBar;
    public TextMeshProUGUI dameTxt,headshotTxt;
    public bool isResetDame;

    void Start()
    {
        billBoard.enabled = isBillboarded;
    }

    public void UpdateHealth(float damage,float fillAmount,bool isHeadShot)
    {
        redFillImg.fillAmount = fillAmount;
        dameTxt.gameObject.SetActive(true);
        dameTxt.color = Color.red;
        dameTxt.transform.localPosition = Vector3.zero;
        dameTxt.DOKill();
        if (isResetDame)
        {
            dameTxt.text = (damage+int.Parse(dameTxt.text)).ToString();
        }
        else
        {
            isResetDame = true;
            dameTxt.text = damage.ToString();
        }
        dameTxt.fontSize = Mathf.Clamp(damage * 0.1f, 15, 40);
        StopAllCoroutines();
        Debug.LogWarning(fillAmount);
        if (fillAmount <= 0)
        {
            goHeadBar.gameObject.SetActive(false);
            transform.parent = transform.root;
            headshotTxt.gameObject.SetActive(isHeadShot);
        }
        else
        {
            StartCoroutine(DelayFillAmountYellowBar(fillAmount));
        }
        StartCoroutine(DelayActiveDameTxt());

    }

    IEnumerator DelayActiveDameTxt()
    {
        yield return new WaitForSeconds(1.5f);
        isResetDame = false;
        dameTxt.transform.DOLocalMove(Vector3.up * 20, 0.35f);
        dameTxt.DOFade(0, 0.35f);
      
       
    }
    private IEnumerator DelayFillAmountYellowBar(float fillAmount)
    {
        yield return null;
        yellowFillImg.DOFillAmount(fillAmount, 0.35f);

    }
}