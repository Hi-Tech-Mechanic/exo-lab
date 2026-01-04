namespace ExoLab
{
    using Exception;
    using ExoLab.Data;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    internal class BodyPartsController : MonoBehaviour
    {
        enum BodyParts
        {
            Head,
            LeftArm,
            RightArm,
            LeftLeg,
            RightLeg,
            Thorax, // Грудная клетка
            Stomach, // Живот
        }

        [Header(nameof(BodyParts.Head))]
        [SerializeField]
        private Image headImage;
        [SerializeField]
        private SuitComponentItemData headData;

        [Header(nameof(BodyParts.LeftArm))]
        [SerializeField]
        private Image leftArmImage;
        [SerializeField]
        private SuitComponentItemData leftArmData;

        [Header(nameof(BodyParts.Thorax))]
        [SerializeField]
        private Image thoraxImage;
        [SerializeField]
        private SuitComponentItemData thoraxData;

        [Header(nameof(BodyParts.Stomach))]
        [SerializeField]
        private Image stomachmage;
        [SerializeField]
        private SuitComponentItemData stomachData;

        [Header(nameof(BodyParts.RightArm))]
        [SerializeField]
        private Image rightArmImage;
        [SerializeField]
        private SuitComponentItemData rightArmData;

        [Header(nameof(BodyParts.LeftLeg))]
        [SerializeField]
        private Image leftLegImage;
        [SerializeField]
        private SuitComponentItemData leftLegData;

        [Header(nameof(BodyParts.RightLeg))]
        [SerializeField]
        private Image rightLegImage;
        [SerializeField]
        private SuitComponentItemData rightLegData;

        private List<BodyPartModel> bodyParts = new();

        private List<BodyPartView> bodyPartsView = new();

        //private void Awake()
        //{
        //    this.Initialize();
        //}

        //private void Update()
        //{
        //    var test = this.bodyPartsView.First();
        //    var testPart = bodyParts.First();
        //    testPart.TakeDamage(0.1);
        //}

        private void Initialize()
        {
            var bodyParts = new List<BodyPartModel>()
            {
                new BodyPartModel(headData),
                //new BodyPartModel(leftArmData), // todo вернуть
                //new BodyPartModel(rightArmData),
                //new BodyPartModel(leftLegData),
                //new BodyPartModel(rightLegData),
                //new BodyPartModel(stomachData),
                //new BodyPartModel(thoraxData),
            };

            this.bodyParts.AddRange(bodyParts);

            foreach (var part in bodyParts)
            {
                if (part.data.Equals(headData))
                    this.bodyPartsView.Add(new BodyPartView(headImage, part));
            }
        }
    }
}
