using UnityEngine;

public class TrailFaide : MonoBehaviour
{
    private Color _color;
    private float _speed = 45;
    private LineRenderer _lineRenderer;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _color = _lineRenderer.startColor;
        _color.a = 0;
        _lineRenderer.startColor = _lineRenderer.endColor = _color;
    }
    void Update()
    {
        _color.a = Mathf.Lerp(_color.a,0, Time.deltaTime * _speed);
        _lineRenderer.startColor = _lineRenderer.endColor = _color;
    }
    public void SetAlpha(int value)
    {
        _color.a = value;
    }
}
