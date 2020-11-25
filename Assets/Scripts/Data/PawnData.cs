using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct StatsAbility : IDataPiece
    {
        [InspectorLabel("HP", min: 10.0f)] [SerializeField] public float m_HealthPoint;
        [InspectorLabel("MP", min: 10.0f)] [SerializeField] public float m_MagicPoint;
        [InspectorLabel("SP", min: 10.0f)] [SerializeField] public float m_StaminaPoint;

        [InspectorLabel("STR", min: 1.0f)] [SerializeField] public float m_Strength;
        [InspectorLabel("DEX", min: 1.0f)] [SerializeField] public float m_Dexterity;
        [InspectorLabel("AGI", min: 1.0f)] [SerializeField] public float m_Agility;
        [InspectorLabel("VIT", min: 1.0f)] [SerializeField] public float m_Vitality;
        [InspectorLabel("INT", min: 1.0f)] [SerializeField] public float m_Intelligence;
        [InspectorLabel("RES", min: 1.0f)] [SerializeField] public float m_Resistance;

        public float m_MeleeDamage => (m_Strength * 2.0f) + m_Dexterity;

        public void Init() { }
    }

#if UNITY_EDITOR
    public class StatsAbilityEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
#endif

    [Serializable]
    public struct Study : IDataPiece
    {
        [InspectorLabel("Assets/Editor/Icons/Sword.png", min: 1.0f)] [SerializeField] public float m_Sword;
        [InspectorLabel("Assets/Editor/Icons/Dagger.png", min: 1.0f)] [SerializeField] public float m_Dagger;
        [InspectorLabel("Assets/Editor/Icons/Bow.png", min: 1.0f)] [SerializeField] public float m_Bow;
        [InspectorLabel("Assets/Editor/Icons/Hammer.png", min: 1.0f)] [SerializeField] public float m_Hammer;
        [InspectorLabel("Assets/Editor/Icons/Spear.png", min: 1.0f)] [SerializeField] public float m_Spear;
        [InspectorLabel("Assets/Editor/Icons/PoleArm.png", min: 1.0f)] [SerializeField] public float m_PoleArm;
        [InspectorLabel("Assets/Editor/Icons/Fire.png", min: 1.0f)] [SerializeField] public float m_Fire;
        [InspectorLabel("Assets/Editor/Icons/Water.png", min: 1.0f)] [SerializeField] public float m_Water;
        [InspectorLabel("Assets/Editor/Icons/Wind.png", min: 1.0f)] [SerializeField] public float m_Wind;
        [InspectorLabel("Assets/Editor/Icons/Earth.png", min: 1.0f)] [SerializeField] public float m_Earth;
        [InspectorLabel("Assets/Editor/Icons/Cook.png", min: 1.0f)] [SerializeField] public float m_Cook;
        [InspectorLabel("Assets/Editor/Icons/Fishing.png", min: 1.0f)] [SerializeField] public float m_Fishing;
        [InspectorLabel("Assets/Editor/Icons/Agriculture.png", min: 1.0f)] [SerializeField] public float m_Agriculture;
        [InspectorLabel("Assets/Editor/Icons/Sewing.png", min: 1.0f)] [SerializeField] public float m_Sewing;
        [InspectorLabel("Assets/Editor/Icons/Smithing.png", min: 1.0f)] [SerializeField] public float m_Smithing;

        public void Init() { }
    }

    [Serializable]
    public struct TalentAbility : IDataPiece
    {
        [SerializeField] public Study m_Fields;

        public void Init() { }
    }

    [Serializable]
    public struct TrainingStatus : IDataPiece
    {
        [SerializeField] public Study m_Fields;

        public void Init() { }
    }

    [Serializable]
    public struct ExperienceStatus : IDataPiece
    {
        [SerializeField] public Study m_Fields;

        public void Init() { }
    }

    [Serializable]
    public struct AilmentStatus : IDataPiece
    {
        public void Init() { }
    }

    [Serializable]
    public struct Ability : IDataPiece
    {
        [SerializeField] public StatsAbility m_Stats;
        [SerializeField] public TalentAbility m_Talent;
        [SerializeField] public TrainingStatus m_Training;
        [SerializeField] public ExperienceStatus m_Experience;
        [SerializeField] public AilmentStatus m_AilmentStatus;

        public void Init()
        {
            m_Stats = new StatsAbility();
            m_Talent = new TalentAbility();
            m_AilmentStatus = new AilmentStatus();
            m_Training = new TrainingStatus();
            m_Experience = new ExperienceStatus();
        }

        public static Ability Create()
        {
            Ability a = new Ability();
            a.Init();
            return a;
        }
    }

    // place me at first
    public class InspectorLabel : PropertyAttribute
    {
        public string m_Label;
        public float m_MinVal;
        public InspectorLabel(string label, float min = float.MinValue)
        {
            m_Label = label;
            m_MinVal = min;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InspectorLabel))]
    public class StatsAbilityDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InspectorLabel insp = attribute as InspectorLabel;
            if (insp.m_Label.StartsWith("Assets/"))
            {
                label.image = AssetDatabase.LoadAssetAtPath<Texture>(insp.m_Label);
                label.text = "";
            }
            else
            {
                label.text = insp.m_Label;
            }

            property.floatValue = Mathf.Max(property.floatValue, insp.m_MinVal);
            EditorGUI.PropertyField(position, property, label);
        }
    }
#endif

    public enum Team
    {
        None = -1,
        Player = 0,
        Monster,
    }

    [Serializable]
    [CreateAssetMenu(order = 51)]
    public class PawnData : ScriptableObject, IDataPiece
    {
        [SerializeField] public string m_DataLabel = "";
        [SerializeField] public Ability m_Ability;

        public PawnData()
        {
            Init();
        }

        public void Init()
        {
            m_Ability = new Ability();
            m_Ability.Init();
        }
    }
}
