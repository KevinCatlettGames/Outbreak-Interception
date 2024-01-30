using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TextWriting : MonoBehaviour
{ 
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;

    AudioSource audioSource;
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
     textComponent.text = string.Empty;
     Invoke("StartDialogue", 8f);
    // StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
     //if (Input.GetMouseButtonDown(0))
     //{
     //    if (textComponent.text == lines[index])
     //    {
     //        NextLine();
     //    }
     //    else
     //    {
     //       StopAllCoroutines();
     //       textComponent.text = lines[index];
     //    }
     //}
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(textSpeed);
        }
        yield return new WaitForSeconds(.5f);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            //textComponent.text = string.Empty;
            textComponent.text += "\n";
            StartCoroutine(TypeLine());
        }
        else
        {
           // gameObject.SetActive(false);
        }
    }
}
