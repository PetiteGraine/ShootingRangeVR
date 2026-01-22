using UnityEngine;

public class XRHandIK : MonoBehaviour
{
    public Animator animator;

    [Header("Targets du XR Origin")]
    [SerializeField] private Transform _headTarget;
    [SerializeField] private Transform _leftHandTarget;
    [SerializeField] private Transform _rightHandTarget;
    [SerializeField] private Transform _xrOrigin;

    [Header("Réglages IK")]
    [Range(0, 1)] public float handWeight = 1f;
    [Range(0, 1)] public float bodyWeight = 0.3f;
    [Range(0, 1)] public float headWeight = 0.6f;

    [Header("Ajustement Corps")]
    public float bodyOffset = 0.15f; // Distance pour reculer le corps de la caméra

    private Quaternion leftHandRotationOffset = Quaternion.Euler(0, 0, 90f);
    private Quaternion rightHandRotationOffset = Quaternion.Euler(0, 0, -90f);

    void Update()
    {
        if (_headTarget != null && _xrOrigin != null)
        {
            // 1. Calcul de la direction du regard (Y uniquement)
            Vector3 forward = _headTarget.forward;
            forward.y = 0;
            forward.Normalize();

            // 2. Positionnement du corps : On part de la tête et on recule
            // Cela évite que la caméra soit "dans" le cou ou le torse
            Vector3 targetPosition = _headTarget.position - (forward * bodyOffset);
            targetPosition.y = _xrOrigin.position.y;

            transform.position = targetPosition;

            // 3. Rotation du corps pour faire face au regard
            if (forward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(forward);
            }
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // --- TORSO & HEAD AIM ---
        animator.SetLookAtWeight(1f, bodyWeight, headWeight, 0f, 0.5f);
        if (_headTarget != null)
        {
            // On vise un point loin devant pour orienter le buste
            animator.SetLookAtPosition(_headTarget.position + _headTarget.forward * 5f);
        }

        // --- MAINS ---
        ApplyHandIK(AvatarIKGoal.LeftHand, _leftHandTarget, leftHandRotationOffset);
        ApplyHandIK(AvatarIKGoal.RightHand, _rightHandTarget, rightHandRotationOffset);
    }

    void ApplyHandIK(AvatarIKGoal goal, Transform target, Quaternion offset)
    {
        if (target != null)
        {
            animator.SetIKPositionWeight(goal, handWeight);
            animator.SetIKRotationWeight(goal, handWeight);
            animator.SetIKPosition(goal, target.position);
            animator.SetIKRotation(goal, target.rotation * offset);
        }
    }
}