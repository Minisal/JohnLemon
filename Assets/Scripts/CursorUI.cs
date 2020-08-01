using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUI : MonoBehaviour
{
    public Texture2D[] cursorTextures;
    public enum CursorColor {NONE, WHITE, RED};
    public float timeInterval = 1f;

    float m_TimeInterval;
    CursorColor m_Cursor = CursorColor.NONE;

    private void OnGUI()
    {
        // showCursor
        float x = Input.mousePosition.x - (cursorTextures[0].width / 10);
        float y = Input.mousePosition.y - (cursorTextures[0].height / 10);
        float width = cursorTextures[0].width/5;
        float height = cursorTextures[0].height/5;
        Rect cursorRect = new Rect(x, y, width, height);

        switch (m_Cursor)
        {
            case CursorColor.WHITE:
                GUI.DrawTexture(cursorRect, cursorTextures[0]);
                break;
            case CursorColor.RED:
                GUI.DrawTexture(cursorRect, cursorTextures[1]);
                break;
        }
    }

    private void Update()
    {
        if(m_Cursor==CursorColor.RED)
        {
            if(m_TimeInterval>0)
            {
                m_TimeInterval -= Time.deltaTime;
            }
            else
            {
                m_Cursor = CursorColor.WHITE;
                m_TimeInterval = timeInterval;
            }
        }
    }

    public void Aim()
    {
        m_Cursor = CursorColor.WHITE;
    }

    public void Fire()
    {
        m_Cursor = CursorColor.RED;
        m_TimeInterval = timeInterval;
    }
}
