using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public static class TransformExtensions
	{
		public static Transform[] GetAllChildren(this Transform transform)
		{
			var childCount = transform.childCount;
			var children = new Transform[childCount];
			for (var i = 0; i < childCount; i++)
			{
				children[i] = transform.GetChild(i);
			}

			return children;
		}

		public static Bounds CalculateLocalRendererBoundsWithChildren(this Transform transform, List<string> nameIgnore = null)
		{
			Quaternion currentRotation = transform.rotation;
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			Bounds bounds = new Bounds(transform.position, Vector3.zero);
			foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
			{
				if (!(renderer is LineRenderer) && !(renderer is ParticleSystemRenderer) && renderer.enabled && (nameIgnore == null || !nameIgnore.Contains(renderer.name)))
					bounds.Encapsulate(renderer.bounds);
			}
			// Vector3 localCenter = bounds.center - transform.position;
			// bounds.center = localCenter;
			transform.rotation = currentRotation;
			return bounds;
		}

		/// <summary>
		/// Finds the game object with the specified tag which is closest
		/// to the provided transform.
		/// </summary>
		/// <param name="transform">The transform to use as the point for closeness.</param>
		/// <param name="tag">The tag name to search for.</param>
		/// <returns></returns>
		public static GameObject FindClosestGameObjectWithTag(this Transform transform, string tag)
		{
			GameObject closest = null;

			GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
			float distance = Mathf.Infinity;
			foreach (GameObject go in gos)
			{
				Vector3 diff = go.transform.position - transform.position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < distance)
				{
					closest = go;
					distance = curDistance;
				}
			}

			return closest;
		}

		/// <summary>
		/// Sets the position of a Transform based on the info stored in a TransformPositionerInfo
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="transformPositionerInfo"></param>
		public static void SetPositioning(this Transform transform, TransformPositionerInfo transformPositionerInfo)
		{
			if (transformPositionerInfo.copyRotation)
			{
				transform.rotation = transformPositionerInfo.transform.rotation;
			}

			if (transformPositionerInfo.copyPosition)
			{
				transform.position = transformPositionerInfo.transform.position;
			}

			if (transformPositionerInfo.parent != null)
			{
				transform.parent = transformPositionerInfo.parent;
			}

			if (transformPositionerInfo.copyScale)
			{
				transform.localScale = transformPositionerInfo.transform.localScale;
			}
		}

		/// <summary>
		/// Sets the layer for the provided transform and its children recursively,
		/// after a short delay.
		/// Used in cases where NGUI is setting the layer when a game object is spawned.
		/// </summary>
		/// <param name="transform">The transform to set the layer for.</param>
		/// <param name="layerName">The name of the layer to set them to.</param>
		/// <returns></returns>
		public static IEnumerator SetLayerDelayed(this Transform transform, string layerName)
		{
			yield return new WaitForSeconds(0.01f);
			transform.SetLayer(LayerMask.NameToLayer(layerName));
		}

		/// <summary>
		/// Sets the layer for the provided transform and its children recursively.
		/// </summary>
		/// <param name="transform">The transform to set the layer for.</param>
		/// <param name="layerName">The name of the layer to set them to.</param>
		/// <returns></returns>
		public static void SetLayer(this Transform transform, string layerName)
		{
			int layer = LayerMask.NameToLayer(layerName);
			SetLayer(transform, layer);
		}

		/// <summary>
		/// Sets the layer for the provided transform and its children recursively.
		/// </summary>
		/// <param name="transform">The transform to set the layer for.</param>
		/// <param name="layer">The layer to set them to.</param>
		/// <returns></returns>
		public static void SetLayer(this Transform transform, int layer)
		{
			if (transform != null)
			{
				transform.gameObject.layer = layer;
				foreach (Transform t in transform)
				{
					SetLayer(t, layer);
				}
			}
		}

		/// <summary>
		/// Returns angle between -1, 1 (-1 = not looking at target, 1 = Looking at target)
		/// </summary>
		/// <param name="trans"></param>
		/// <param name="target"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static bool LookingAtTarget(this Transform trans, Vector3 target, float angle = 0.99f)
		{
			target.y = trans.position.y;
			Vector3 forward = trans.forward;
			Vector3 targetDir = (target - trans.position).normalized;

			if (Vector3.Dot(forward, targetDir) >= angle)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Removes Y Rotation
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="target"></param>
		/// <param name="rotSpeed"></param>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static bool SmoothLookAt(this Transform transform, Vector3 target, float rotSpeed = 5f, float angle = 0.99f)
		{
			//Quaternion targetRot = Quaternion.LookRotation(target - transform.position, Vector3.up);
			//transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

			//Vector3 targetDir = target - transform.position;
			//targetDir.y = 0;
			//transform.rotation = Quaternion.RotateTowards(transform.rotation,
			//     Quaternion.LookRotation(targetDir),
			//     rotSpeed * Time.deltaTime);
			var targetPos = target;
			targetPos.y = transform.position.y;

			if ((targetPos - transform.position) == Vector3.zero)
			{
				return true;
			}

			var targetDir = Quaternion.LookRotation(targetPos - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, rotSpeed * Time.deltaTime);

			return transform.LookingAtTarget(target, angle);
		}

		public static void RotateTowards(this Transform transform, Quaternion target, float rotSpeed = 100f)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation,
				 target,
				 rotSpeed * Time.deltaTime);
		}

		public static void LookDirection(this Transform transform, Vector3 direction)
		{
			direction.Normalize();
			direction.y = 0;
			transform.rotation = Quaternion.LookRotation(direction);
		}

		public static void Slerp(this Transform transform, Vector3 target, float rotationSpeed = 5f)
		{
			transform.rotation = Quaternion.Slerp(
			  transform.rotation,
			  Quaternion.LookRotation(target),
			   Time.deltaTime * rotationSpeed);
		}

		public static void DestroyChildren(this Transform parent)
		{
			while (parent.childCount > 0)
			{
				Transform child = parent.GetChild(0);
				child.parent = null;
				GameObject.Destroy(child.gameObject);
			}
		}

		public static void DestroyAllChildren(this Transform parent)
		{
			// Create a separate list to prevent issues when deleting children while iterating the collection
			foreach (Transform child in parent.GetAllChildren())
			{
				child.SetParent(null);
				if (Application.isPlaying)
					GameObject.Destroy(child.gameObject);
				else
					GameObject.DestroyImmediate(child.gameObject);
			}
		}

		public static float RemainingDistance(this CharacterController agent, ref Vector3[] results, ref int currentNode, int nodeCount)
		{
			float value = (Vector3.Distance(agent.transform.position, results[currentNode]));
			for (int i = currentNode; i < nodeCount - 1; i++)
			{
				value += Vector3.Distance(results[i], results[i + 1]);
			}

			return value;
		}

        public static IEnumerable<Transform> GetChildren(this Transform transform)
        {
            var childCount = transform.childCount;
            for (var i = 0; i < childCount; i++)
            {
                yield return transform.GetChild(i);
            }
        }

        public static void UngroupChildren(this Transform transform)
        {
            foreach (var child in transform.GetChildren())
            {
                child.SetParent(null);
            }
        }

        public static void DestroyEmptyChildren(this Transform transform)
        {
            var childCount = transform.childCount;
            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(i);
                child.DestroyEmptyChildren();

                if (child.childCount == 0 && child.GetComponents<Component>().Length < 2)
                {
                    child.gameObject.EditorSafeDestroy();
                }
            }
        }

        public static void EditorSafeDestroy(this Object target)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(target);
            else
#endif
                Object.Destroy(target);
        }

        /// <summary>
        /// Warp agent to a location around the target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="radius"></param>
        public static void WarpToTarget(this CharacterController cc, Transform target, float radius = 0f)
		{
			WarpToTarget(cc, target.position, radius);
		}

		public static bool WouldBeObstructedAtLocation(this CharacterController cc, Vector3 target)
		{
			return !Physics.CheckCapsule(cc.center, cc.center, cc.radius);
		}

		/// <summary>
		/// warp agent to a location around the target
		/// </summary>
		/// <param name="target"></param>
		/// <param name="radius"></param>
		public static void WarpToTarget(this CharacterController cc, Vector3 target, float radius = 0f)
		{
			cc.enabled = false;
			Vector3 tp = target;

			if (radius > 0)
			{
				Vector2 point = UnityEngine.Random.insideUnitCircle * radius;
				tp = new Vector3(target.x + point.x, target.y, target.z + point.y);
			}

			RaycastHit hit;
			Ray ray = new Ray(tp + Vector3.up, Vector3.down);
			int layer = 1 << 0 | 1 << 18 | 1 << 17 | 1 << 16;

			if (Physics.Raycast(ray, out hit, 10f, layer))
			{
				if (hit.point.y >= Terrain.activeTerrain.SampleHeight(hit.point))
					cc.transform.position = hit.point;
				else
				{
					Ray offsetRay = new Ray(hit.point + Vector3.up, Vector3.down);
					if (Physics.Raycast(offsetRay, out hit, 1.5f, layer))
					{
						cc.transform.position = hit.point;
					}
					else
					{
						tp.y = Terrain.activeTerrain.SampleHeight(tp) + 0.15f;
						cc.transform.position = tp;
					}
				}
			}
			else
			{
				if (tp.y >= Terrain.activeTerrain.SampleHeight(hit.point))
					cc.transform.position = tp;
				else
				{
					Ray offsetRay = new Ray(tp + Vector3.up, Vector3.down);
					if (Physics.Raycast(offsetRay, out hit, 1.5f, layer))
					{
						cc.transform.position = hit.point;
					}
					else
					{
						tp.y = Terrain.activeTerrain.SampleHeight(tp) + 0.15f;
						cc.transform.position = tp;
					}
				}
			}

			cc.transform.rotation = Quaternion.Euler(Vector3.up);
			cc.enabled = true;

			//This wont actually work when exiting a vehicle on the bridge....
			/*
            if (Physics.Raycast(ray, out hit, 10f))
            {
                Vector3 hitPoint = hit.point;

                if (hitPoint.y < Terrain.activeTerrain.SampleHeight(target))
                {
                    hitPoint.y = Terrain.activeTerrain.SampleHeight(target) + Terrain.activeTerrain.GetPosition().y + 1f;

                    RaycastHit offsetHit;
                    if (Physics.Raycast(new Ray(hitPoint, Vector3.down), out offsetHit, 1.5f))
                    {
                        hitPoint = offsetHit.point;
                    }

                    //cc.transform.rotation = Quaternion.Euler(Vector3.up);
                }

                cc.transform.position = hitPoint;

                FoxieGames.Debug.DrawLine(tp + Vector3.up, hitPoint, Color.red);
            }
            else
            {
                Vector3 pos = target;

                if (pos.y < Terrain.activeTerrain.SampleHeight(target))
                {
                    pos.y = Terrain.activeTerrain.SampleHeight(target) + Terrain.activeTerrain.GetPosition().y + 0.5f;
                    cc.transform.rotation = Quaternion.Euler(Vector3.up);
                }

                cc.transform.position = pos;

                FoxieGames.Debug.LogError("Failed to Warp with raycast");
            }
            */
		}
	}

/// <summary>
/// From https://forum.unity.com/threads/quickly-retrieving-the-immediate-children-of-a-gameobject.39451/#post-3554195
/// </summary>
public static class UnityEngineEx
{
	public static T GetComponentInDirectChildren<T>(this Component parent) where T : Component
	{
		return parent.GetComponentInDirectChildren<T>(false);
	}

	public static T GetComponentInDirectChildren<T>(this Component parent, bool includeInactive) where T : Component
	{
		foreach (Transform transform in parent.transform)
		{
			if (includeInactive || transform.gameObject.activeInHierarchy)
			{
				T component = transform.GetComponent<T>();
				if (component != null)
				{
					return component;
				}
			}
		}
		return null;
	}

	public static T[] GetComponentsInDirectChildren<T>(this Component parent) where T : Component
	{
		return parent.GetComponentsInDirectChildren<T>(false);
	}

	public static T[] GetComponentsInDirectChildren<T>(this Component parent, bool includeInactive) where T : Component
	{
		List<T> tmpList = new List<T>();
		foreach (Transform transform in parent.transform)
		{
			if (includeInactive || transform.gameObject.activeInHierarchy)
			{
				tmpList.AddRange(transform.GetComponents<T>());
			}
		}
		return tmpList.ToArray();
	}

	public static T GetComponentInSiblings<T>(this Component sibling) where T : Component
	{
		return sibling.GetComponentInSiblings<T>(false);
	}

	public static T GetComponentInSiblings<T>(this Component sibling, bool includeInactive) where T : Component
	{
		Transform parent = sibling.transform.parent;
		if (parent == null) return null;
		foreach (Transform transform in parent)
		{
			if (includeInactive || transform.gameObject.activeInHierarchy)
			{
				if (transform != sibling)
				{
					T component = transform.GetComponent<T>();
					if (component != null)
					{
						return component;
					}
				}
			}
		}
		return null;
	}

	public static T[] GetComponentsInSiblings<T>(this Component sibling) where T : Component
	{
		return sibling.GetComponentsInSiblings<T>(false);
	}

	public static T[] GetComponentsInSiblings<T>(this Component sibling, bool includeInactive) where T : Component
	{
		Transform parent = sibling.transform.parent;
		if (parent == null) return null;
		List<T> tmpList = new List<T>();
		foreach (Transform transform in parent)
		{
			if (includeInactive || transform.gameObject.activeInHierarchy)
			{
				if (transform != sibling)
				{
					tmpList.AddRange(transform.GetComponents<T>());
				}
			}
		}
		return tmpList.ToArray();
	}

	public static T GetComponentInDirectParent<T>(this Component child) where T : Component
	{
		Transform parent = child.transform.parent;
		if (parent == null) return null;
		return parent.GetComponent<T>();
	}

	public static T[] GetComponentsInDirectParent<T>(this Component child) where T : Component
	{
		Transform parent = child.transform.parent;
		if (parent == null) return null;
		return parent.GetComponents<T>();
	}
}

/// <summary>
/// Stores information to set the position of a transform, based on the position of a transform on the scene and a parent
/// </summary>
[System.Serializable]
public class TransformPositionerInfo
{
	public Transform transform;
	public bool copyPosition = true;
	public bool copyRotation = true;
	public bool copyScale = false;
	public Transform parent;
}