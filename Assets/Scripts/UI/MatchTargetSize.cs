using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MatchTargetSize : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private RectTransform selfRect;
    private RectTransform targetRect;
    private Vector2 originSelfSize;
    private Vector2 originTargetSize;

    [SerializeField] private float multiplyX = 1;
    [SerializeField] private float multiplyY = 1;
    [SerializeField] private float height;
    [SerializeField] private float width;

    [SerializeField] private bool matchWidth;
    [SerializeField] private bool matchHeight;

    private void Update()
    {
        if (target != null)
        {
            if (selfRect == null)
            {
                selfRect = GetComponent<RectTransform>();
                originSelfSize = selfRect.sizeDelta;
            }

            targetRect = target.GetComponent<RectTransform>();
            originTargetSize = targetRect.sizeDelta;

            var selfX = width * multiplyX;
            var selfY = height * multiplyY;
            var targetX = originTargetSize.x * multiplyX;
            var targetY = originTargetSize.y * multiplyY;

            if (matchWidth && matchHeight)
            {
                selfRect.sizeDelta = new Vector2(targetX, targetY);
            }
            else if (matchWidth)
            {
                selfRect.sizeDelta = new Vector2(targetX, selfY);
            }
            else if (matchHeight)
            {
                selfRect.sizeDelta = new Vector2(selfX, targetY);
            }
        }
    }
}
