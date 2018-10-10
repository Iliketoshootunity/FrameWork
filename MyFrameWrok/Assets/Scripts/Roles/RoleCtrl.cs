using UnityEngine;
using System.Collections;

public class RoleCtrl : MonoBehaviour
{


    private Vector3 target;
    [SerializeField]
    private float speed = 10;
    private float rotateSpeed;
    private Quaternion targetRotate;
    private CharacterController cc;
    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item")))
            {
                BoxCtrl boxCtrl = hitinfo.collider.GetComponent<BoxCtrl>();
                boxCtrl.OnHit();
                Debug.Log(hitinfo.collider.name);
            }
        }

        #region 移动
        
        if (cc == null) return;

        if (Input.GetMouseButton(0) || Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            if (Physics.Raycast(ray, out hitinfo, (int)LayerMask.GetMask("Ground")))
            {
                target = hitinfo.point;
                rotateSpeed = 0;

            }

        }

        if (!cc.isGrounded)
        {
            cc.Move(Vector3.up * -1000);
        }
        if (target != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                Vector3 dirction = (target - transform.position).normalized * speed * Time.deltaTime;

                if (rotateSpeed <= 1)
                {
                    rotateSpeed += 5f;
                    targetRotate = Quaternion.LookRotation(dirction);
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, Time.deltaTime * rotateSpeed);
                cc.Move(dirction);

            }


        }

        #endregion
        CameraAutoFollow();
    }

    /// <summary>
    /// 摄像机自动跟随
    /// </summary>
    private void CameraAutoFollow()
    {
        if (CameraCtrl.Instance == null) return;
        CameraCtrl.Instance.transform.position = gameObject.transform.position;
        CameraCtrl.Instance.AutoLookAt(gameObject.transform.position);
        if(Input.GetKey(KeyCode.A))
        {
            CameraCtrl.Instance.SetCameraRotate(0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CameraCtrl.Instance.SetCameraRotate(1);
        }
        if(Input.GetKey(KeyCode.W))
        {
            CameraCtrl.Instance.SetCameraUpAndDown(0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            CameraCtrl.Instance.SetCameraUpAndDown(1);
        }
        if (Input.GetKey(KeyCode.I))
        {
            CameraCtrl.Instance.SetCameraZoom(0);
        }
        if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            CameraCtrl.Instance.SetCameraZoom(0);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            CameraCtrl.Instance.SetCameraZoom(1);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("主角碰撞了");
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("主角碰撞中");
    }
    void OnCollisionExit(Collision collision)
    {
        Debug.Log("主角碰撞离开");
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("主角触发了");
    }

    void OnTriggerStay(Collider collider)
    {
        Debug.Log("主角触发中");
    }
    void OnTriggerExit(Collider collider)
    {
        Debug.Log("主角触发离开");
    }
}
