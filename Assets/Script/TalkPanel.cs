using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkPanel : MonoBehaviour
{
    public GameObject Target;
    public Person p;

    private float openRate;
    public bool  show;
    private float textRate;

    private string showString;

	// Use this for initialization
	void Start ()
	{
	    //Target = GameScene.m_Player;
	}
	
	// Update is called once per frame
	void Update ()
	{
        if(Target!=null)
	        transform.localPosition = Target.transform.localPosition + new Vector3(150, 100, 0);

	    transform.localScale = new Vector3(openRate, openRate, 1);

        if (showString!=null)
	    {
	        transform.Find("Text").GetComponent<Text>().text = showString.Substring(0, (int) textRate);
	    }
	}

    void FixedUpdate()
    {
        if (show && openRate < 1.0f)
        {
            openRate += 0.2f;
            openRate = Mathf.Min(openRate, 1.0f);
        }

        if (show && openRate == 1.0f && textRate < showString.Length)
        {
            textRate += 0.2f;
        }

        if (!show && openRate > 0)
        {
            openRate -= 0.2f;
            openRate = Mathf.Max(openRate, 0.0f);
        }
    }

    public void OpenTalkPanel(string text)
    {
        show = true;
        showString = text;
        textRate = 0;
        openRate = 0;
    }

    public void CloseTlkPanel()
    {
        show = false;
    }
}
