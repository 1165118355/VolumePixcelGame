using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceWidget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void setHidden(bool isHidden)
    {
        if (isHidden)
            hide();
        else
            show();
    }
    public bool isHidden() { return m_IsHidden; }

    public virtual void show()
    {
        m_IsHidden = false;
        gameObject.SetActive(true);
    }
    public virtual void hide()
    {
        m_IsHidden = true;
        gameObject.SetActive(false);
    }

    private bool m_IsHidden = true;
}
