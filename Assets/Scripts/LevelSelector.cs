using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Transform[] levelItemLst = new Transform[20];
    [HideInInspector]
    public int currentPage = 0, totalPage;

    public Text pageText;

    public Sprite unlockLevel, lockLevel;

    private void Awake()
    {
        currentPage = PlayerPrefs.GetInt("LockLevel", 1) / 20;
    }

    void Start()
    {
        for (int i = 0; i < GetChildByName("Holder").transform.childCount; i++)
            levelItemLst[i] = GetChildByName("Holder").transform.GetChild(i);
        ShowLevelItemInfo();
    }

    GameObject GetChildByName(string _name)
    {
        GameObject _child = null;
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        if (ts == null)
            return _child;
        foreach (Transform t in ts)
        {
            if (t != null && t.gameObject != null)
            {
                if (t.gameObject.name == _name)
                    _child = t.gameObject;
            }
        }

        return _child;
    }

    public void ShowLevelItemInfo()
    {
        pageText.text = "PAGE " + (currentPage + 1).ToString();
        int _lockLevel = PlayerPrefs.GetInt("LockLevel", 1);

        for (int i = 0; i < levelItemLst.Length; i++)
        {
            int levelNumber = i + 1 + levelItemLst.Length * currentPage;

            // Ẩn số màn chơi nếu màn bị khóa
            Text levelText = levelItemLst[i].Find("LevelText").GetComponent<Text>();
            levelText.text = "";

            if (levelNumber <= _lockLevel)
            {
                levelText.text = levelNumber.ToString();
                levelItemLst[i].Find("Panel").GetComponent<Image>().sprite = unlockLevel;
            }
            else
            {
                levelItemLst[i].Find("Panel").GetComponent<Image>().sprite = lockLevel;
            }

            // Xóa các listener trước khi thêm để tránh bị trùng lặp
            Button levelButton = levelItemLst[i].GetComponent<Button>();
            levelButton.onClick.RemoveAllListeners();

            if (levelNumber <= _lockLevel)
            {
                levelButton.onClick.AddListener(() => LoadLevel(levelNumber));
                levelButton.interactable = true;
            }
            else
            {
                levelButton.interactable = false;
            }
        }
    }

    public void LoadLevel(int _level)
    {
        if (_level <= PlayerPrefs.GetInt("LockLevel", 1))
        {
            // Chuyển tới màn chơi tương ứng
            HomeManager._instance.LoadLevel(_level);
        }
    }

    public void NextPage()
    {
        if (currentPage < totalPage - 1)
        {
            currentPage++;
            ShowLevelItemInfo();
        }
    }

    public void PrePage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowLevelItemInfo();
        }
    }
}

