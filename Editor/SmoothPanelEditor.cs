using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Extelen.UI;

namespace Extelen.Editor { 

	[CustomEditor(typeof(SmoothPanel))]
	public class SmoothPanelEditor : UnityEditor.Editor {
		
		//Set Params
		private SerializedProperty m_rectTransform = null;
		private SerializedProperty m_canvasGroup = null;

		private SerializedProperty m_activePosition = null;
		private SerializedProperty m_unactivePosition = null;

		private SerializedProperty m_unscaledDeltaTime = null;
		private SerializedProperty m_inTime = null;
		private SerializedProperty m_outTime = null;
		private SerializedProperty m_animationCurve = null;

		private SerializedProperty m_inCalls = null;
		private SerializedProperty m_outCalls = null;

		//Methods
		private void OnEnable() {

			m_rectTransform = serializedObject.FindProperty("m_rectTransform");
			m_canvasGroup = serializedObject.FindProperty("m_canvasGroup");

			m_activePosition = serializedObject.FindProperty("m_activePosition");
			m_unactivePosition = serializedObject.FindProperty("m_unactivePosition");

			m_unscaledDeltaTime = serializedObject.FindProperty("m_unscaledDeltaTime");
			m_inTime = serializedObject.FindProperty("m_inTime");
			m_outTime = serializedObject.FindProperty("m_outTime");
			m_animationCurve = serializedObject.FindProperty("m_animationCurve");

			m_inCalls = serializedObject.FindProperty("m_inCalls");
			m_outCalls = serializedObject.FindProperty("m_outCalls");
			}

		
		public override void OnInspectorGUI() {
			
			serializedObject.Update();
			
			EditorGUILayout.PropertyField(m_rectTransform);
			EditorGUILayout.PropertyField(m_canvasGroup);

			EditorGUILayout.PropertyField(m_activePosition);
			EditorGUILayout.PropertyField(m_unactivePosition);

			EditorGUILayout.PropertyField(m_unscaledDeltaTime);
			EditorGUILayout.PropertyField(m_inTime);
			EditorGUILayout.PropertyField(m_outTime);
			EditorGUILayout.PropertyField(m_animationCurve);

			EditorGUILayout.PropertyField(m_inCalls);
			EditorGUILayout.PropertyField(m_outCalls);

			serializedObject.ApplyModifiedProperties();
			}
		}
	}