using UnityEngine;

public class Cube : MonoBehaviour
{
    public float scaleSpeed;
    public float rotateSpeed; 
    public Vector3 rotateAxis;

    readonly float _limitScale = 10;
    void Update()
    {
        transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);

        ScaleUp();
    }

    void ScaleUp()
    {
        // 현재 스케일에 증가값을 더하고, 각 축이 limitScale을 넘지 않도록 제한
        Vector3 newScale = transform.localScale + Vector3.one * (scaleSpeed * Time.deltaTime);

        // 각 축이 _limitScale을 넘지 않도록 제어
        newScale.x = Mathf.Min(newScale.x, _limitScale);
        newScale.y = Mathf.Min(newScale.y, _limitScale);
        newScale.z = Mathf.Min(newScale.z, _limitScale);

        // 계산된 새로운 스케일을 적용
        transform.localScale = newScale;
    }
}
