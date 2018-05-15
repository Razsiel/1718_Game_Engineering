using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Extensions;
using DG.Tweening;
using SmartLocalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour {
    public Text NpcNameText;
    public TextMeshProUGUI SentenceText;
    public Text ContinueText;
    public Image NpcImage;
    public Image TutorialPopup;

    public RectTransform RectTransform;

    Vector3 HidePosition = new Vector3(0.0f, -726.0f, 0.0f);
    Vector3 ShowPosition = new Vector3(0.0f, -378.0f, 0.0f);

    private Queue<string> _sentences;

    private Coroutine _currentRoutine;

    void Awake() {
        _sentences = new Queue<string>();
        EventManager.OnMonologueStart += StartDialogue;
        ContinueText.text = LanguageManager.Instance.GetTextValue("MONOLOGUE_CLICK_TO_CONTINUE");
        TutorialPopup.gameObject.SetActive(false);
    }

    void OnDestroy() {
        StopAllCoroutines();
    }

    public void StartDialogue(Monologue monologue) {
        EventManager.OnMonologueStart -= StartDialogue;
        // Open dialogue panel
        ShowMonologue();

        NpcNameText.text = monologue.NpcName;
//	    NpcImage.sprite = monologue.NpcImage;

        _sentences.Clear();

        foreach (string sentence in monologue.Sentences) {
            string sentenceTranslation = LanguageManager.Instance.GetTextValue(sentence);
            _sentences.Enqueue(sentenceTranslation);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (_sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = _sentences.Dequeue();
        if (_currentRoutine != null) {
            StopCoroutine(_currentRoutine);
        }
        _currentRoutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        SentenceText.text = "";

        SentenceText.text = sentence; // No letter for letter print
        yield break;

//        foreach (char letter in sentence)
//		{
//		    SentenceText.text += letter;
//			yield return null;
//		}
    }

    void EndDialogue() {
        StartCoroutine(HideMonologue());
        EventManager.MonologueEnded();
    }

    IEnumerator HideMonologue() {
        RectTransform.DOLocalMove(HidePosition, 1f);
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    void ShowMonologue() {
        RectTransform.DOLocalMove(ShowPosition, 1f);
    }
}