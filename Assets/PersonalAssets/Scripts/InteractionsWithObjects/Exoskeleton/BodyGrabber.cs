//namespace ExoLab.Interaction
//{
//    using ExoLab.Data;
//    using UnityEngine;

//    public class BodyGrabber : MonoBehaviour
//    {
//        public Transform handTarget; // Пустой объект, который будет следовать за курсором
//        public float weight = 1f;    // Сила влияния IK

//        private Animator animator;
//        private Camera assemblyCamera;

//        void Start()
//        {
//            animator = GetComponent<Animator>();
//            assemblyCamera = Caches.Instance.AssemblyCamera;

//            // Включаем IK в Animator
//            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
//            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
//        }

//        void OnAnimatorIK(int layerIndex)
//        {
//            if (handTarget != null)
//            {
//                animator.SetIKPosition(AvatarIKGoal.RightHand, handTarget.position);
//                animator.SetIKRotation(AvatarIKGoal.RightHand, handTarget.rotation);
//            }
//        }

//        void Update()
//        {
//            // Пример: перемещение цели за курсором мыши (на плоскости перед камерой)
//            if (Input.GetMouseButton(0))
//            {
//                Ray ray = assemblyCamera.ScreenPointToRay(Input.mousePosition);
//                // Можно использовать фиксированную глубину или Raycast по плоскости
//                float distance = 5f; // или результат Raycast
//                handTarget.position = ray.GetPoint(distance);
//            }
//        }
//    }
//}
