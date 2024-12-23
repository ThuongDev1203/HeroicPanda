﻿using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static AdsControl;

public class HomeManager : MonoBehaviour
{
    public GameObject levelSelector, homePanel, loadingPanel, settingPanel, outLifePanel, moreCoinPanel, frontLife1, frontLife2, frontLife3;

    public Image loadingMask, musicImg, soundImg, virImg;

    public Sprite onBtn, offBtn;

    public static HomeManager _instance;

    public Text coinText, lockLevelText;

    private void Awake()
    {
        _instance = this;

        if (PlayerPrefs.GetInt("StartGame") == 0)
        {
            PlayerPrefs.SetInt("StartGame", 1);
            PlayerPrefs.SetInt("Coin", 200);
            PlayerPrefs.SetInt("Life", 3);
            PlayerPrefs.SetInt("Sound", 1);
            PlayerPrefs.SetInt("Music", 1);
            PlayerPrefs.SetInt("Vir", 1);
            PlayerPrefs.SetInt("LockLevel", 1);
        }


    }

    // Start is called before the first frame update
    void Start()
    {
        lockLevelText.text = "LEVEL " + PlayerPrefs.GetInt("LockLevel");
        UpdateCoinText();
        UpdateSetting();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowLevelPanel()
    {
        if (PlayerPrefs.GetInt("Life") > 0)
            levelSelector.SetActive(true);
        else
            ShowOutOfLife();
    }

    public void ShowSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.GetComponent<Animator>().SetTrigger("Close");
    }

    public void ShowOutOfLife()
    {
        outLifePanel.SetActive(true);
        int _life = PlayerPrefs.GetInt("Life");
        UpdateLive(_life);
    }

    public void HideOutOfLife()
    {
        outLifePanel.GetComponent<Animator>().SetTrigger("Close");
    }


    public void ShowMoreCoin()
    {
        moreCoinPanel.SetActive(true);
    }

    public void HideMoreCoin()
    {
        moreCoinPanel.GetComponent<Animator>().SetTrigger("Close");
    }

    public void HideLevelPanel()
    {
        levelSelector.GetComponent<Animator>().SetTrigger("Close");
    }

    public void LoadLevel(int _level)
    {
        PlayerPrefs.SetInt("CurrentLevel", _level);
        HideLevelPanel();
        homePanel.SetActive(false);
        StartCoroutine(Fading());

    }

    IEnumerator Fading()
    {
        yield return new WaitForSeconds(0.5f);
        loadingPanel.SetActive(true);
        SceneManager.LoadScene("MainGame");

        //yield return new WaitForSeconds(0.2f);
        for (float i = 1; i >= 0; i -= Time.deltaTime * 0.35f)
        {
            // set color with i as alpha
            loadingMask.color = new Color(0, 0, 0, i);
            yield return null;
        }
        loadingPanel.SetActive(false);
    }

    IEnumerator FadingReplay()
    {
        //yield return new WaitForSeconds(5.0f);
        loadingPanel.SetActive(true);
        SceneManager.LoadScene("MainGame");

        //yield return new WaitForSeconds(0.2f);
        for (float i = 1; i >= 0; i -= Time.deltaTime * 0.35f)
        {
            // set color with i as alpha
            loadingMask.color = new Color(0, 0, 0, i);
            yield return null;
        }
        loadingPanel.SetActive(false);
    }

    IEnumerator FadeImage(bool fadeAway, Image img)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                img.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

    void UpdateCoinText()
    {
        coinText.text = PlayerPrefs.GetInt("Coin") + "";
    }

    public void MoreLive()
    {
        int _coin = PlayerPrefs.GetInt("Coin");
        int _life = PlayerPrefs.GetInt("Life");
        if (_coin >= 500 && _life < 3)
        {
            _coin -= 500;
            PlayerPrefs.SetInt("Coin", _coin);
            UpdateCoinText();
            _life++;
            PlayerPrefs.SetInt("Life", _life);
            UpdateLive(_life);
        }
        else if (_coin < 500)
            ShowMoreCoin();
    }

    public void MoreCoin()
    {
        WatchAds();
    }

    public void UpdateLive(int life)
    {
        switch (life)
        {

            case 0:
                frontLife1.SetActive(false);
                frontLife2.SetActive(false);
                frontLife3.SetActive(false);

                break;

            case 1:
                frontLife1.SetActive(true);
                frontLife2.SetActive(false);
                frontLife3.SetActive(false);



                break;
            case 2:
                frontLife1.SetActive(true);
                frontLife2.SetActive(true);
                frontLife3.SetActive(false);



                break;
            case 3:
                frontLife1.SetActive(true);
                frontLife2.SetActive(true);
                frontLife3.SetActive(true);



                break;
        }


    }

    public void ToggleMusic()
    {
        int _music = PlayerPrefs.GetInt("Music");

        if (_music == 1)
        {
            PlayerPrefs.SetInt("Music", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        UpdateSetting();
    }

    public void ToggleSound()
    {
        int _sound = PlayerPrefs.GetInt("Sound");

        if (_sound == 1)
        {
            PlayerPrefs.SetInt("Sound", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        UpdateSetting();
    }

    public void ToggleVir()
    {
        int _vir = PlayerPrefs.GetInt("Vir");

        if (_vir == 1)
        {
            PlayerPrefs.SetInt("Vir", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Vir", 1);
        }
        UpdateSetting();
    }

    public void UpdateSetting()
    {
        int _music = PlayerPrefs.GetInt("Music");

        int _sound = PlayerPrefs.GetInt("Sound");

        int _vir = PlayerPrefs.GetInt("Vir");


        if (_music == 1)
        {
            SoundManager.Instance.MusicSource.mute = false;
            musicImg.sprite = onBtn;
        }
        else
        {
            SoundManager.Instance.MusicSource.mute = true;
            musicImg.sprite = offBtn;
        }

        if (_sound == 1)
        {
            SoundManager.Instance.EffectsSource.mute = false;
            soundImg.sprite = onBtn;
        }
        else
        {
            SoundManager.Instance.EffectsSource.mute = true;
            soundImg.sprite = offBtn;
        }

        if (_vir == 1)
        {
            //SoundManager.Instance.EffectsSource.mute = false;
            virImg.sprite = onBtn;
        }
        else
        {
            // SoundManager.Instance.EffectsSource.mute = true;
            virImg.sprite = offBtn;
        }
    }

    public void Restore()
    {
        Purchase.Instance.RestorePurchases();
    }

    public void WatchAds()
    {
        if (AdsControl.Instance.currentAdsType == ADS_TYPE.ADMOB)
        {
            if (AdsControl.Instance.rewardedAd != null)
            {
                if (AdsControl.Instance.rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.ShowRewardAd(EarnRW);
                }
            }
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.UNITY)
        {
            ShowRWUnityAds();
        }
        else if (AdsControl.Instance.currentAdsType == ADS_TYPE.MEDIATION)
        {
            if (AdsControl.Instance.rewardedAd.CanShowAd())

                AdsControl.Instance.ShowRewardAd(EarnRW);

            else
                ShowRWUnityAds();
        }
    }

    public void EarnRW(Reward reward)
    {
        //function
        int _coin = PlayerPrefs.GetInt("Coin");
        _coin += 50;
        PlayerPrefs.SetInt("Coin", _coin);
        UpdateCoinText();
    }

    public void ShowRWUnityAds()
    {
        AdsControl.Instance.PlayUnityVideoAd((string ID, UnityAdsShowCompletionState callBackState) =>
        {

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                //function
                int _coin = PlayerPrefs.GetInt("Coin");
                _coin += 50;
                PlayerPrefs.SetInt("Coin", _coin);
                UpdateCoinText();
            }

            if (ID.Equals(AdsControl.Instance.adUnityRWUnitId) && callBackState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                AdsControl.Instance.LoadUnityAd();
            }

        });
    }

}
