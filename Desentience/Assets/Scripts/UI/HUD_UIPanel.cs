using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://www.youtube.com/watch?v=hjnJnz77OVU

public class HUD_UIPanel : Base_UIPanel
{

    public Image image;
    public override void UpdateBehavior()
    {
        base.UpdateBehavior();
        image.rectTransform.Rotate(Vector3.forward * Time.deltaTime);
    }
}