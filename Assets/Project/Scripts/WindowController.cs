using UnityEngine;
using System.Collections;

public class WindowController : MonoBehaviour
{
    public GameObject Rwindow;
    public GameObject Lwindow;
    private Vector3 Rrot = new Vector3(0, -120, 0);
    private Vector3 Lrot = new Vector3(0, 120, 0);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenWindow();
        }
    }

    public void OpenWindow(){
        StartCoroutine(WindowAnimation());
    }

    private IEnumerator WindowAnimation()
    {
        float duration = 1f;
        float elapsedTime = 0f;
        
        // 시작 각도 (0도)
        Quaternion RstartQuat = Quaternion.Euler(0, 0, 0);
        Quaternion LstartQuat = Quaternion.Euler(0, 0, 0);
        
        // 목표 각도
        Quaternion RtargetQuat = Quaternion.Euler(Rrot);
        Quaternion LtargetQuat = Quaternion.Euler(Lrot);
        
        // 창문 열기 애니메이션
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            Rwindow.transform.rotation = Quaternion.Slerp(RstartQuat, RtargetQuat, t);
            Lwindow.transform.rotation = Quaternion.Slerp(LstartQuat, LtargetQuat, t);
            
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        
        // 창문 닫기 애니메이션
        elapsedTime = 0f;
        Quaternion RcurrentQuat = Rwindow.transform.rotation;
        Quaternion LcurrentQuat = Lwindow.transform.rotation;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            
            Rwindow.transform.rotation = Quaternion.Slerp(RcurrentQuat, RstartQuat, t);
            Lwindow.transform.rotation = Quaternion.Slerp(LcurrentQuat, LstartQuat, t);
            
            yield return null;
        }
    }
}
