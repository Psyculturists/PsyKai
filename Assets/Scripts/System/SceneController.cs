using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Systems/SceneController", fileName = "SceneController")]
public class SceneController : ScriptableObject
{
	public void ReloadCurrentScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}