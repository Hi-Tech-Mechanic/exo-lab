namespace ExoLab
{
    using ExoLab.StructuralСomponents;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

#if UNITY_EDITOR
    [CustomEditor(typeof(AssemblyComponentBase), editorForChildClasses: true)]
    public class AttachmentOptionEditor : Editor
    {
        private int selectedOptionIndex = 0;

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            AssemblyComponentBase saver = (AssemblyComponentBase)this.target;

            if (GUILayout.Button("Save Attachment"))
            {
                saver.SaveAttachmentOptionInGuiEditor();

                // Обязательно вызвать SetDirty, чтобы изменения сохранились в файле
                if (saver.TypedItemData != null)
                {
                    EditorUtility.SetDirty(saver.TypedItemData);
                }
            }

            if (saver.TypedItemData == null || saver.TypedItemData.AttachmentOptions.Count == 0)
                return;

            GUILayout.Space(10);

            if (saver.TypedItemData.AttachmentOptions.Count == 0)
                return;

            var parentNames = saver.TypedItemData.AttachmentOptions.Select(x => x.ParentData.Name).ToArray();
            this.selectedOptionIndex = EditorGUILayout.Popup("Select Attachment", selectedOptionIndex, parentNames);

            if (GUILayout.Button("Load Attachment"))
            {
                var selectedOption = saver.TypedItemData.AttachmentOptions[this.selectedOptionIndex];
                saver.UpdateAttachmentOptions(selectedOption);
                saver.SetAttachmentOptionInCurrentObject(saver.transform.parent.gameObject);
            }
        }
    }
#endif
}
