namespace ExoLab.Interaction
{
    using ExoLab.Data;
    using System;
    using UnityEngine;
    using ExoLab.Assembly;
    using System.Collections.Generic;

    [RequireComponent(typeof(Animator))]
    public class InteractiveIKController : MonoBehaviour
    {
        /// <summary>
        /// Для блокировки вращения в <see cref="ItemInspect"/>
        /// </summary>
        public static Action<bool> OnItemInspectRotationBlock;

        [Tooltip("Слой хэндлов")]
        [SerializeField] private LayerMask bodyPartLayer;

        [Tooltip("Слой самого персонажа (для проверки столкновений)")]
        [SerializeField] private LayerMask selfCollisionLayer;

        [Tooltip("Скорость плавного следования за курсором")]
        [SerializeField] private float smoothSpeed = 8f;

        [SerializeField] private float blendOutTime = 0.3f;
        [SerializeField] private float blendInTime = 0.2f;

        [Header("Настройки ограничений")]
        public LimbLimits armLimits = new LimbLimits
        {
            upperTwist = new Vector2(-90, 90),
            upperSwingY = new Vector2(-45, 45),
            upperSwingZ = new Vector2(-60, 80),
            lowerBend = new Vector2(0, 160),
            lowerTwist = new Vector2(0, 0)
        };

        public LimbLimits legLimits = new LimbLimits
        {
            upperTwist = new Vector2(-30, 30),
            upperSwingY = new Vector2(-30, 30),
            upperSwingZ = new Vector2(-120, 45),
            lowerBend = new Vector2(0, 150),
            lowerTwist = new Vector2(-10, 10)
        };

        private Camera mainCamera;
        private Animator animator;
        private BodyPartHandle currentTarget;
        private Transform ikTarget;
        private float currentIKWeight = 0F;

        private bool isHolding = false;

        private void Start()
        {
            this.mainCamera = Caches.Instance.Assembly.AssemblyCamera;
            animator = GetComponent<Animator>();
            CreateIKTarget();
        }

        private void Update()
        {
            HandleInput();

            // Плавное изменение веса IK
            float targetWeight = isHolding ? 1f : 0f;
            float blendTime = isHolding ? blendInTime : blendOutTime;
            currentIKWeight = Mathf.Lerp(currentIKWeight, targetWeight, Time.deltaTime / blendTime);

            // Если захват активен — двигаем цель за курсором
            if (isHolding && currentTarget != null)
            {
                Plane plane = new Plane(-mainCamera.transform.forward, currentTarget.bone.position);
                if (plane.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out float distance))
                {
                    Vector3 desiredPos = mainCamera.ScreenPointToRay(Input.mousePosition).GetPoint(distance);
                    ikTarget.position = Vector3.Lerp(ikTarget.position, desiredPos, Time.deltaTime * smoothSpeed);
                }
            }
        }
        private void LateUpdate()
        {
            // Опционально: скрыть цель, когда не используется
            if (ikTarget.gameObject.activeSelf != isHolding)
            {
                ikTarget.gameObject.SetActive(isHolding);
            }

            if (currentTarget == null)
                return;

            // Применяем ограничения только если захвачена рука или нога
            if (IsHandBone(currentTarget.bone))
            {
                ApplyLimbLimits(
                    HumanBodyBones.RightShoulder, HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm,
                    HumanBodyBones.LeftShoulder, HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm,
                    armLimits
                );
            }
            else if (IsFootBone(currentTarget.bone))
            {
                ApplyLimbLimits(
                    HumanBodyBones.Hips, HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg,
                    HumanBodyBones.Hips, HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg,
                    legLimits
                );
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (currentTarget == null || currentIKWeight <= 0f) return;

            var goal = GetIKGoalForBone(currentTarget.bone);
            if (goal != null)
            {
                animator.SetIKPositionWeight((AvatarIKGoal)goal, currentIKWeight);
                animator.SetIKRotationWeight((AvatarIKGoal)goal, currentIKWeight);
                animator.SetIKPosition((AvatarIKGoal)goal, ikTarget.position);
                animator.SetIKRotation((AvatarIKGoal)goal, ikTarget.rotation);
            }
        }

        private void CreateIKTarget()
        {
            GameObject go = new GameObject("IK Target");
            ikTarget = go.transform;
            ikTarget.hideFlags = HideFlags.HideInHierarchy; // можно убрать для отладки
        }

        private bool IsHandBone(Transform bone)
        {
            return bone == animator.GetBoneTransform(HumanBodyBones.LeftHand) ||
                   bone == animator.GetBoneTransform(HumanBodyBones.RightHand);
        }

        private bool IsFootBone(Transform bone)
        {
            return bone == animator.GetBoneTransform(HumanBodyBones.LeftFoot) ||
                   bone == animator.GetBoneTransform(HumanBodyBones.RightFoot);
        }

        private void ApplyLimbLimits(
            HumanBodyBones rootLeft, HumanBodyBones upperLeft, HumanBodyBones lowerLeft,
            HumanBodyBones rootRight, HumanBodyBones upperRight, HumanBodyBones lowerRight,
            LimbLimits limits)
        {
            ApplyOneSide(rootLeft, upperLeft, lowerLeft, limits);
            ApplyOneSide(rootRight, upperRight, lowerRight, limits);
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, bodyPartLayer))
                {
                    currentTarget = hit.collider.GetComponent<BodyPartHandle>();
                    if (currentTarget != null)
                    {
                        // Важно: ставим цель в текущую позу кости → нет рывка!
                        ikTarget.position = currentTarget.bone.position;
                        isHolding = true;
                        OnItemInspectRotationBlock?.Invoke(true);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isHolding = false;
                OnItemInspectRotationBlock?.Invoke(false);
            }
        }

        private float ClampAngle(float angle, Vector2 range)
        {
            angle = Normalize360(angle);
            if (range.x <= range.y)
            {
                return Mathf.Clamp(angle, range.x, range.y);
            }
            else
            {
                // Диапазон через 0 (редко, но на всякий случай)
                if (angle >= range.x || angle <= range.y)
                    return angle;
                return range.x - (range.x - angle) % (360 - (range.x - range.y));
            }
        }

        private float Normalize360(float angle)
        {
            while (angle < 0) angle += 360;
            while (angle >= 360) angle -= 360;
            return angle;
        }

        private void ApplyOneSide(HumanBodyBones rootBone, HumanBodyBones upperBone, HumanBodyBones lowerBone, LimbLimits limits)
        {
            Transform upper = animator.GetBoneTransform(upperBone);
            Transform lower = animator.GetBoneTransform(lowerBone);

            if (upper != null)
            {
                Vector3 eulers = upper.localRotation.eulerAngles;
                eulers.x = ClampAngle(eulers.x, limits.upperTwist);
                eulers.y = ClampAngle(eulers.y, limits.upperSwingY);
                eulers.z = ClampAngle(eulers.z, limits.upperSwingZ);
                upper.localRotation = Quaternion.Euler(eulers);
            }

            if (lower != null)
            {
                Vector3 eulers = lower.localRotation.eulerAngles;
                eulers.x = ClampAngle(eulers.x, limits.lowerBend);
                eulers.y = ClampAngle(eulers.y, limits.lowerTwist); // обычно 0
                eulers.z = 0; // фиксируем, чтобы не было лишнего вращения
                lower.localRotation = Quaternion.Euler(eulers);
            }
        }

        private AvatarIKGoal? GetIKGoalForBone(Transform bone)
        {
            var leftHand = new List<HumanBodyBones>
            {
                HumanBodyBones.LeftUpperArm,
                HumanBodyBones.LeftLowerArm,
                HumanBodyBones.LeftHand
            };
            var rightHand = new List<HumanBodyBones>
            {
                HumanBodyBones.RightUpperArm,
                HumanBodyBones.RightLowerArm,
                HumanBodyBones.RightHand
            };
            var leftFoot = new List<HumanBodyBones>
            {
                HumanBodyBones.LeftFoot,
                HumanBodyBones.LeftUpperLeg,
                HumanBodyBones.LeftLowerLeg
            };
            var rightFoot = new List<HumanBodyBones>
            {
                HumanBodyBones.RightFoot,
                HumanBodyBones.RightUpperLeg,
                HumanBodyBones.RightLowerLeg
            };

            Dictionary<AvatarIKGoal, List<HumanBodyBones>> bodyParts = new()
            {
                { AvatarIKGoal.LeftHand, leftHand },
                { AvatarIKGoal.RightHand, rightHand },
                { AvatarIKGoal.LeftFoot, leftFoot },
                { AvatarIKGoal.RightFoot, rightFoot }
            };

            foreach (var keyValuePair in bodyParts)
            {
                foreach (var boneType in keyValuePair.Value)
                {
                    if (bone == animator.GetBoneTransform(boneType))
                        return keyValuePair.Key;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Ограничения (в градусах, локальные оси)
    /// </summary>
    [Serializable]
    public struct LimbLimits
    {
        [Header("Плечо / Бедро")]
        [Tooltip("ось X (вокруг продольной оси)")]
        public Vector2 upperTwist;
        [Tooltip("ось Y (влево-вправо)")]
        public Vector2 upperSwingY;
        [Tooltip("ось Z (вперёд-назад)")]
        public Vector2 upperSwingZ;

        [Header("Локоть / Колено")]
        [Tooltip("основное сгибание (локоть: X, колено: X)")]
        public Vector2 lowerBend;  
        [Tooltip("вращение вокруг продольной оси (обычно 0)")]
        public Vector2 lowerTwist;
    }
}
