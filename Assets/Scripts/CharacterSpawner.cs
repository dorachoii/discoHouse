using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject[] characterPrefab;
    public GameObject characterParent;
      private int windowCount = 4;

    private float xPos = -4.5f;
    private float xOffset = 3f;
    private float yPos = 3.7f;
    private float yOffset = 2.5f;
    private float zPos = 0f;

    void Start()
    {
        SpawnCharacter();
    }

    // Update is called once per frame
    void SpawnCharacter()
    {
        for (int i = 0; i < windowCount; i++)
        {
            for (int j = 0; j < windowCount; j++)
            {
                GameObject character = Instantiate(characterPrefab[Random.Range(0, characterPrefab.Length)], new Vector3(xPos + i * xOffset, yPos - j * yOffset, zPos), Quaternion.identity);
                character.transform.SetParent(characterParent.transform, true);
                character.transform.rotation = Quaternion.Euler(0, 180, 0);
                               
                Animator[] animators = character.GetComponentsInChildren<Animator>();
                int randomParam = Random.Range(0, 6);

                switch(randomParam){
                    case 2:
                        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z + 0.5f);
                        break;
                    case 1:
                        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z + 0.2f);
                        break;
                }
                   
                foreach (Animator animator in animators)
                {
                    animator.SetBool(randomParam.ToString(), true);
                }
            }
        }
    }
}
