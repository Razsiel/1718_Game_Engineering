using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Lib.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class MonologueManager : MonoBehaviour {

	public Text NpcNameText;
	public Text SentenceText;

    private CanvasGroup _canvasGroup;

	private Queue<string> _sentences;

    void Awake()
    {
        EventManager.InitializeMonologue += Initialize;
        EventManager.MonologueStart += StartDialogue;
        EventManager.MonologueEnded += EndDialogue;
    }

	void Initialize () {
		_sentences = new Queue<string>();
	    _canvasGroup = GetComponent<CanvasGroup>();
	    _canvasGroup.alpha = 0;
	}

	public void StartDialogue (Monologue monologue)
	{
        // Open dialogue panel
	    ToggleVisibility();
	    _canvasGroup.blocksRaycasts = true;

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
		foreach (char letter in sentence.ToCharArray())
		{
		    SentenceText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
        // Close dialogue panel
	    ToggleVisibility();
	    _canvasGroup.blocksRaycasts = false;
        EventManager.OnMonologueEnded();

	}

    void ToggleVisibility()
    {
        _canvasGroup.alpha = MathHelper.Mod(_canvasGroup.alpha + 1f, 2f);
    }
}
