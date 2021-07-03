using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
                moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection = new Vector3(0, moveDirection.y, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= moveSpeed;
            moveDirection.z *= moveSpeed;
        }

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
