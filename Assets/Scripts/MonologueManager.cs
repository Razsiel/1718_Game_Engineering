using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour {

	public Text NpcNameText;
	public Text SentenceText;
    
    private RectTransform _rectTransform;
    Vector3 HidePosition = new Vector3(-1920.0f, -1079.7f, 0.0f);

    private Queue<string> _sentences;

    void Awake()
    {
        _sentences = new Queue<string>();
        _rectTransform = GetComponent<RectTransform>();
        EventManager.MonologueStart += StartDialogue;
    }

	public void StartDialogue (Monologue monologue)
	{
        // Open dialogue panel
	    ShowMonologue();

        NpcNameText.text = monologue.name;

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
        EventManager.OnMonologueEnded();
	}

    void HideMonologue()
    {
        _rectTransform.DOLocalMove(HidePosition, 1f);
    }

    void ShowMonologue()
    {
        Vector3 pos = HidePosition;
        pos.y += 700f;
        _rectTransform.DOLocalMove(pos, 1f);
    }
}
