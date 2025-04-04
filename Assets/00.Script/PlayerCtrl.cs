#pragma warning disable IDE0051
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    private Animator anim;
    private new Transform transform;
    private Vector3 moveDir;

    private PlayerInput playerInput;
    private InputActionMap mainActionMap;
    private InputAction moveAction;
    private InputAction attackAtcion;

    private void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();

        playerInput = GetComponent<PlayerInput>();
        
        //ActionMap 추출
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");

        //Move, Attack추출
        moveAction = mainActionMap.FindAction("Move");
        attackAtcion = mainActionMap.FindAction("Attack");

        //Move 액션의 performed 이벤트 연결
        moveAction.performed += ctx => {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //Warrior_Run 애니메이션 실행
            anim.SetFloat("Movement", dir.magnitude);
        };

        //Move 액션의 canceled 이벤트 연결
        moveAction.canceled += ctx => {
            moveDir = Vector3.zero;
            //Warrior_Run애니메이션 정지
            anim.SetFloat("Movement", 0f);
        };

        //Attack액션의 performed 이벤트 연결
        attackAtcion.performed += ctx =>
        {
            Debug.Log("Attack by c# event");
            anim.SetTrigger("Attack");
        };
    }

    private void Update()
    {
        if (moveDir != Vector3.zero)
        {
            //진행 방향으로 회전
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전한 후 전진 방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);

        }
    }

#region SendMessage
    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();

        //2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);

        //Warrior_Run 애니메이션 실행
        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = ({dir.x}, {dir.y})");
    }

    void OnAttack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Attack");
    }
#endregion


#region  UNITY_EVENTS
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();

        //2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);

        //Warrior_Run애니메이션 실행
        anim.SetFloat("Movement", dir.magnitude);

    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase = {ctx.phase}");

        if(ctx.performed)
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");
        }
    }
#endregion
}
