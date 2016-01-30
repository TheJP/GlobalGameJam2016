using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlameItemIndicator : MonoBehaviour
{
    private Targets target;
    public Targets Target
    {
        get { return target; }
        set
        {
            target = value;
            GetComponent<Image>().color = value.ToColor();
        }
    }
}
