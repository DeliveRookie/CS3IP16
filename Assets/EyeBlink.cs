using UnityEngine;
using System.Collections;
/// <summary>
/// ’£—€¿‡
/// </summary>
public class EyeBlink : MonoBehaviour
{
    static float[] Weights = new float[] { 100, 75, 50, 25, 0 };     

    public SkinnedMeshRenderer Face;
    bool m_enable = true;

    bool m_blinking;
    float m_timer;
    int m_index;

    public void Update()
    {
        if (!Face || !Enable)
            return;

        m_timer -= Time.deltaTime;

        if (m_blinking)
        {
            if (m_timer < 0)
            {
                m_timer = 0.05f;                    
                m_index++;

                if (m_index < Weights.Length)
                    SetShape(m_index);              
                else
                {
                    m_blinking = false;             
                    m_timer = Random.Range(3, 3);   
                }
            }
        }
        else
        {
            if (m_timer < 0)
                ToBlink();                         
        }
    }

    void ToBlink()
    {
        m_blinking = true;
        m_timer = 0;
        m_index = -1;
    }

    void SetShape(int index)
    {
        Face.SetBlendShapeWeight(0, Weights[index]);
    }

    public bool Enable
    {
        get { return m_enable; }
        set
        {
            if (m_enable != value)
            {
                m_enable = value;

                if (m_enable)
                    ToBlink();
                else
                    SetShape(Weights.Length - 1);
            }
        }
    }
}
