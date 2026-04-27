using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObject : MonoBehaviour
{
    public static PersistantObject Instance;
    private int levelIndex = 0;

    public int GetLevelIndex() {
        return levelIndex;
    }

    public void SetLevelIndex(int index) {
        levelIndex = index;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
