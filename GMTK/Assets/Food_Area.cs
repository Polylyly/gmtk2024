using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Food_Area : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject steak, winCanva, WinText, JamText, JamTextScore, steakText, SteakTextScore, finalTextScore, steakOverCookBonusText,
        OverCookedBonusScoreText, finaltext, ratingText, ratingScoreText;

    [Header("text ")]
    public TextMeshProUGUI FinalTextScore_TMP, bonusScore_TMP, ratingScoreText_TMP;

    [Header("Layour mask ")]
    public LayerMask jam;
    public LayerMask staek;

    public bool jam_On = false, steak_on = false;
    public int score, steakScore;


    private HashSet<GameObject> objectsInside = new HashSet<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            ratingScoreText_TMP.text = "ULTRA COOKED";

    }

    private void Start()
    {
        score = 40;
        steak = GameObject.FindGameObjectWithTag("steak");

        steak.gameObject.SetActive(false);
        winCanva.gameObject.SetActive(false);
        WinText.gameObject.SetActive(false);
        JamText.gameObject.SetActive(false);
        JamTextScore.gameObject.SetActive(false);
        steakText.gameObject.SetActive(false);
        SteakTextScore.gameObject.SetActive(false);
        finalTextScore.gameObject.SetActive(false);
        steakOverCookBonusText.gameObject.SetActive(false);
        OverCookedBonusScoreText.gameObject.SetActive(false);
        finaltext.gameObject.SetActive(false);
        ratingText.gameObject.SetActive(false);
        ratingScoreText.gameObject.SetActive(false);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(waiter());

        if (((1 << other.gameObject.layer) & jam) != 0)
        {

            jam_On = true;
            Debug.Log("jam ready ");
        }

        if (((1 << other.gameObject.layer) & staek) != 0)
        {

            steak_on = true;
            Debug.Log("steak ready");
        }
    }

    private void OnTriggerExit(Collider other)
    {
  
        if (((1 << other.gameObject.layer) & jam) != 0)
        {

            jam_On = false;
        }

        if (((1 << other.gameObject.layer) & staek) != 0)
        {

            steak_on = false;
        }
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(3);

        if (jam_On == true && steak_on == true)
        {
            winCanva.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            WinText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            JamText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            JamTextScore.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            steakText.gameObject.SetActive(true);                                     //meat score

            yield return new WaitForSeconds(1);
            SteakTextScore.gameObject.SetActive(true);

            if (steak.GetComponent<meatCook>().cookTime >= 20)
            {
                steakScore = steak.GetComponent<meatCook>().cookTime;
                yield return new WaitForSeconds(1);
                steakOverCookBonusText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1);

                bonusScore_TMP.text = steak.GetComponent<meatCook>().cookTime.ToString();
                OverCookedBonusScoreText.gameObject.SetActive(true);

                yield return new WaitForSeconds(1);

            }
            
            score += steakScore;
            FinalTextScore_TMP.text = score.ToString();

            finaltext.gameObject.SetActive(true);
            finalTextScore.gameObject.SetActive(true);

            yield return new WaitForSeconds(1);
            ratingText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            rating();
           
        }
            
    }

    void rating()
    {
        if (score >= 40 && score <= 49)
            ratingScoreText_TMP.text = "F";
        if (score >= 100 && score <= 199)
            ratingScoreText_TMP.text = "S";
        if (score >= 200 && score <= 299)
            ratingScoreText_TMP.text = "SS";
        if (score >= 300 && score <= 399)
            ratingScoreText_TMP.text = "SSS";
        if (score >= 400 & score <= 499)
            ratingScoreText_TMP.text = "SSS";
        if (score >= 500)
            ratingScoreText_TMP.text = "ULTRA COOKED";
        if (score >= 80 && score <= 99)
            ratingScoreText_TMP.text = "A";
        if (score >= 50 && score <= 59 )
            ratingScoreText_TMP.text = "D";
        if (score >= 60 && score <= 69)
            ratingScoreText_TMP.text = "C";
        if (score >= 70 && score <79)
            ratingScoreText_TMP.text = "B";
        ratingScoreText.gameObject.SetActive(true);
    }

}
