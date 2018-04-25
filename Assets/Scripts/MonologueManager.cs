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

    private RectTransform _rectTransform;
    //    Vector3 HidePosition = new Vector3(-1920.0f, -1079.7f, 0.0f);

    Vector3 HidePosition = new Vector3(0.0f, -1404.3f, 0.0f);
    Vector3 ShowPosition = new Vector3(0.0f, -756.3f, 0.0f);

    private Queue<string> _sentences;

    void Awake()
    {
        _sentences = new Queue<string>();
        _rectTransform = GetComponent<RectTransform>();
        EventManager.OnMonologueStart += StartDialogue;
        ContinueText.text = LanguageManager.Instance.GetTextValue("MONOLOGUE_CLICK_TO_CONTINUE");
    }

	public void StartDialogue (Monologue monologue)
	{
        // Open dialogue panel
	    ShowMonologue();

        NpcNameText.text = monologue.NpcName;
	    NpcImage.sprite = monologue.NpcImage;

        _sentences.Clear();

		foreach (string sentence in monologue.Sentences)
		{
			_sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (_sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = _sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		SentenceText.text = "";
		foreach (char letter in sentence)
		{
		    SentenceText.text += letter;
			yield return null;
		}
	}
    
	void EndDialogue()
	{
	    HideMonologue();
        EventManager.MonologueEnded();
	}

    void HideMonologue()
    {
        _rectTransform.DOLocalMove(HidePosition, 1f);
    }

    void ShowMonologue()
    {
        _rectTransform.DOLocalMove(ShowPosition, 1f);
    }
}
