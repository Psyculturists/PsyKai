using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;

namespace FoxieGames
{
	[ExecuteAlways]
	public class RadialLayoutGroup : MonoBehaviour
	{
		[SerializeField]
		private bool autoSize = true;
		[SerializeField, OnValueChanged("UpdateOnValueChange")]
		private float radius = 100.0f;
		[SerializeField, EnableIf("autoSize", true), Range(0, 1), OnValueChanged("UpdateOnValueChange")]
		private float fractionOfSpaceToFill = 0.9f;
		[SerializeField, Range(0, 360), OnValueChanged("UpdateOnValueChange")]
		private float startPointDegrees = 0.0f;
		[SerializeField, OnValueChanged("UpdateOnValueChange")]
		private bool autoSpace = true;
		[SerializeField, EnableIf("autoSpace", false), OnValueChanged("UpdateOnValueChange")]
		private float spacingDegrees = 0.0f;
		[SerializeField]
		private bool rotateAroundCentre = false;
		private int previousActiveChildCount = 0;

		private int ActiveChildCount => transform.GetChildren().Where(c => c.gameObject.activeSelf == true).Count();

		private bool isFanTweening = false;
		private bool fanTweenForwards = false;
		[OnValueChanged("UpdateOnValueChange")]
		private float fractionThroughFanningTween = 0.0f;
		public float fanningTime = 0.75f;

		public float RemainingFanningTime
		{
			get
			{
				if(fanTweenForwards)
				{
					return (fanningTime * (1.0f - fractionThroughFanningTween));
				}
				else
				{
					return (fanningTime * fractionThroughFanningTween);
				}
			}
		}

		///<summary>
		/// Gets the distance between each child element of the radial group - direct line, not along circumference
		///</summary>
		private float GetDistanceBetweenElements()
		{
			float angle;
			if(autoSpace)
			{
				int elements = ActiveChildCount;
				// get angle between each dot
				angle = 360.0f / (float)elements;
			}
			else
			{
				angle = spacingDegrees;
			}
			// divide by two to get the bisect angle to get right-angle-triangles from isoceles
			float bisectAngle = angle / 2.0f;
			// use Cos of the angle, multiplied by the radius (hypotenuse) to get the base of the right angle triangle
			float halfLength = radius * Mathf.Sin(Mathf.Deg2Rad * bisectAngle);
			Debug.LogError(halfLength + "  /  " + Mathf.Cos(Mathf.Deg2Rad * bisectAngle) + "  /  " + bisectAngle);
			// multiply by two for full length for distance between dots
			float fullLength = halfLength * 2;

			return Mathf.Abs(fullLength);
		}

		///<summary>
		/// Gets the local position for the child element depending on sibling index
		/// Factors in radius, spacing, and offset
		///</summary>
		private Vector3 GetRelativePosition(int index, bool overrideAutoSpace = false)
		{
			float angle;
			if(autoSpace || overrideAutoSpace)
			{
				angle = 360.0f / (float)ActiveChildCount;
			}
			else
			{
				angle = spacingDegrees;
			}
			Vector3 firstPos = transform.up * radius;
			if(!isFanTweening)
			{
				firstPos = Quaternion.Euler(0, 0, (angle * index) - startPointDegrees) * firstPos;
			}
			else
			{
				firstPos = Quaternion.Euler(0, 0, ((angle * index) - startPointDegrees) * fractionThroughFanningTween) * firstPos;
			}

			return firstPos;
		}

		///<summary>
		/// Gets the desired rotation of an element if it should face centre
		///</summary>
		private Vector3 GetElementRotation(int index)
		{
			float angle;
			if(autoSpace)
			{
				angle = 360.0f / (float)ActiveChildCount;
			}
			else
			{
				angle = spacingDegrees;
			}
			return new Vector3(0, 0, angle * (index));
		}

		///<summary>
		/// Executes when a value is changed to update the layout.
		///</summary>
		private void UpdateOnValueChange()
		{
			float spriteSize = 0;
			if(autoSize)
			{
				// this part doesn't work for anything that isn't X by X dimensions, but could be updated to handle it.
				spriteSize = (2 * fractionOfSpaceToFill * GetDistanceBetweenElements()) / 2.0f;
			}
			Transform[] activeChildren = transform.GetAllChildren().Where(c => c.gameObject.activeSelf).ToArray();
			for(int i = 0; i < ActiveChildCount; i++)
			{
				Transform child = activeChildren[i];


				child.localPosition = GetRelativePosition(i);
				if(autoSize)
				{
					child.GetComponent<RectTransform>().sizeDelta = new Vector2(spriteSize, spriteSize);
				}

				if(rotateAroundCentre)
				{
					child.localRotation = Quaternion.Euler(GetElementRotation(i) - new Vector3(0, 0, startPointDegrees));
					//float rotationalOffset = child.localRotation.eulerAngles.z;
					SetRotationsOfChildren(child);
				}
			}
		}

		///<summary>
		/// Pulls layout elements and applies rotation offset to those that ignore the layout.
		/// If you want an object at eg 90 degrees, add it to an empty parent, rotate parent 90, apply layout element and set ignore on original object
		///</summary>
		private void SetRotationsOfChildren(Transform parent)
		{
			List<LayoutElement> elements = new List<LayoutElement>();
			parent.GetComponentsInChildren<LayoutElement>(elements);
			elements = elements.Where(l => l.ignoreLayout == true).ToList();

			foreach(LayoutElement le in elements)
			{
				le.transform.localRotation = Quaternion.Euler(le.transform.localRotation.eulerAngles.x, le.transform.localRotation.eulerAngles.y, -parent.localEulerAngles.z);
			}
		}

		private bool ValidateActiveChildCount()
		{
			int activeChildren = 0;
			for(int i = 0; i < transform.childCount; i++)
			{
				if(transform.GetChild(i).gameObject.activeSelf)
				{
					activeChildren++;
				}
			}
			if(previousActiveChildCount != activeChildren)
			{
				previousActiveChildCount = activeChildren;
				return false;
			}
			return true;
		}

		public void ExpandIntoAutoSpace()
		{
			SetAutoSpace(true);
			isFanTweening = true;
			fanTweenForwards = true;
			//fractionThroughFanningTween = 0;
			UpdateOnValueChange();
		}
		public void ContractFromAutoSpace()
		{
			isFanTweening = true;
			fanTweenForwards = false;
			//fractionThroughFanningTween = 1;
			UpdateOnValueChange();
		}

		public void SetAutoSpace(bool val)
		{
			autoSpace = val;
		}

		private void Update()
		{
			if(ValidateActiveChildCount())
			{
				UpdateOnValueChange();
			}

			UpdateFanning();
		}

		private void UpdateFanning()
		{
			if(isFanTweening)
			{
				if(fanTweenForwards)
				{
					fractionThroughFanningTween += (1.0f / fanningTime) * Time.deltaTime;
					if(fractionThroughFanningTween >= 1)
					{
						fractionThroughFanningTween = 1;
						isFanTweening = false;
					}
				}
				else
				{
					fractionThroughFanningTween -= (1.0f / fanningTime) * Time.deltaTime;
					if(fractionThroughFanningTween <= 0)
					{
						fractionThroughFanningTween = 0;
						isFanTweening = false;
						autoSpace = false;
					}
				}
			}
		}
	}
}