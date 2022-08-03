using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager1 : MonoBehaviour
{
    [Header("공용, 기타")]
    public Button btnBackTitle;
    public Text txtQuizScript;
    public int nQuizTotalCount;
    public int nQuizNowStep;
    public bool isQuizLoad;
    public Text txtStage;
    public Text Timer;
    public Text RemainTime;

    [Header("Img 삽입")]
    public Image img_0_0;
    public Image img_0_1;
    public Image img_1_0;
    public Image img_1_1;
    public Image img_2_0;
    public Image img_2_1;
    public Image img_3_0;
    public Image img_3_1;

    public GameObject GOimg_0_0;
    public GameObject GOimg_0_1;
    public GameObject GOimg_1_0;
    public GameObject GOimg_1_1;
    public GameObject GOimg_2_0;
    public GameObject GOimg_2_1;
    public GameObject GOimg_3_0;
    public GameObject GOimg_3_1;

    //[Header("답안 형식 - 1")]
    //public GameObject objAnswerType1Set;
    //public Button btnO;
    //public Button btnX;
    //public bool isChooseO;
    //public bool isChooseX;

    [Header("답안 형식 - 2")]
    public GameObject objAnswerType2Set;
    public Button btnRightUp;
    public Button btnRightDown;
    public Button btnLeftUp;
    public Button btnLeftDown;
    public Text txtAnswer1;
    public Text txtAnswer2;
    public Text txtAnswer3;
    public Text txtAnswer4;
    public bool isChoose1;
    public bool isChoose2;
    public bool isChoose3;
    public bool isChoose4;

    //[Header("답안 형식 - 3")]
    //public GameObject objAnswerType3Set;
    //public InputField ipfAnswer;
    //public Button btnAnswerIpf;
    //public bool isSubmitIpf;

    [Header("결과팝업창")]
    public GameObject objResultPopupSet;
    public GameObject objCorrectAnswer;
    public GameObject objWrongAnswer;
    public Button btnResultConfirm;

    [Header("사운드")]
    AudioSource ButtonClick;

    GameDataManager gameDataManager;

    public bool isCorrect;
    public GameObject objTotalResult;
    public Text txtTotalResult;
    public Text txtAnswerDivTotal;
    public int nTotalCorrectAnswerCount;

    public int cnt;
    public bool cnt_check;
    public bool Question_Check = true;
    public bool TwoQuestion_Check = true;
    public int TwoQuestion_CheckLock = 0;

    public int RandomPick;
    public int RandomPick_;
    public int RandomPickControl;
    public int RandomMax;
    public int QuestionNum_1;
    public int QuestionNum_2;
    public int ImgLeft;
    public int ImgRight;

    public string nTotalCorrectAnswerCountToInt;
    public string nQuizTotalCountToInt;

    public float TimeValue;
    public float RemainTimeOfStage;
    public bool RemainTimeLock = true;

    public ArrayList RandomWeight = new ArrayList(); // 가중치 배열

    GameObject ImgHP;

    public void DecreaseHP()
    {
        ImgHP.GetComponent<Image>().fillAmount -= 0.33f;
    }

    public void InitAll()
    {
        txtQuizScript.text = "";
        txtStage.text = "";
        isQuizLoad = false;

        //objAnswerType1Set.SetActive(false);
        objAnswerType2Set.SetActive(false);
        //objAnswerType3Set.SetActive(false);

        GOimg_0_0.SetActive(false);
        GOimg_1_0.SetActive(false);
        GOimg_2_0.SetActive(false);
        GOimg_3_0.SetActive(false);

        GOimg_0_1.SetActive(false);
        GOimg_1_1.SetActive(false);
        GOimg_2_1.SetActive(false);
        GOimg_3_1.SetActive(false);

        Debug.Log("Check InitAll_____________");

        //isChooseO = false;
        //isChooseX = false;

        isChoose1 = false;
        isChoose2 = false;
        isChoose3 = false;
        isChoose4 = false;

        //isSubmitIpf = false;

        //ipfAnswer.text = "";

        isCorrect = false;

        objResultPopupSet.SetActive(false);
        objCorrectAnswer.SetActive(false);
        objWrongAnswer.SetActive(false);
        objTotalResult.SetActive(false);

        cnt_check = false;
        Question_Check = true;

        TwoQuestion_CheckLock += 1;
        if (TwoQuestion_CheckLock == 2)
        {
            TwoQuestion_Check = true;
            TwoQuestion_CheckLock = 0;
        }

        Debug.Log("초기화 진행");
    }

    // Start is called before the first frame update
    void Start()
    {
        ButtonClick = GetComponent<AudioSource>();

        for (int i = 0; i < 14; i++) // 숫자는 nQuizTotalCount
        {
            RandomWeight.Add(i); // 처음에 문제의 번호 8개를 넣어준다.
        }

        //for (int i = 0; i < 14; i++)
        //{
        //    Debug.Log("RandomWeight : " + RandomWeight[i]); // 확인용
        //}

        gameDataManager = GameObject.Find("GameDataManager_DontDestroy").GetComponent<GameDataManager>();

        this.ImgHP = GameObject.Find("ImgHP");

        btnBackTitle.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
            Debug.Log("Title 씬 로드");
        }); // Back to Menu

        btnRightUp.onClick.AddListener(() =>
        {
            ButtonClick.Play();
            isChoose1 = true;
        });
        btnRightDown.onClick.AddListener(() =>
        {
            ButtonClick.Play();
            isChoose2 = true;
        });
        btnLeftUp.onClick.AddListener(() =>
        {
            ButtonClick.Play();
            isChoose3 = true;
        });
        btnLeftDown.onClick.AddListener(() =>
        {
            ButtonClick.Play();
            isChoose4 = true;
        });

        btnResultConfirm.onClick.AddListener(() =>
        {
            // If isCorrect, isCorrect Count++;
            if (isCorrect)
            {
                nTotalCorrectAnswerCount += 1;
                isCorrect = false;
            }

            // If Game End, Go to Menu
            if (nQuizNowStep >= nQuizTotalCount)
            {
                SceneManager.LoadScene("Main_1");
                Debug.Log("게임 끝 Title 씬 로드");
            }

            if (cnt > 2)
            {
                SceneManager.LoadScene("Title");
            }

            else
            {
                NextQuizLoad();
            }
        });



        InitAll();



        Debug.Log("총 퀴즈 수 : " + gameDataManager.data_Quiz.Count);

        // 퀴즈 총 갯수
        nQuizTotalCount = gameDataManager.data_Quiz.Count;
        RandomMax = nQuizTotalCount;
        Debug.Log("RandomMax : " + RandomMax);

        nQuizNowStep = 0;
        RandomPickControl = 0;

        txtTotalResult.text = "";
        nTotalCorrectAnswerCount = 0;

        TimeValue = 10; // 여기서 제한시간 초기화

        if (RemainTimeLock)
        {
            RemainTimeOfStage = 80;
            RemainTimeLock = false;
        }
    }

    public void NextQuizLoad()
    {
        InitAll();
        Debug.Log("Check NextQuiz////////////////////////////");
        nQuizNowStep += 1;

        TimeValue = 10; // 문제 넘길때마다 시간 초기화

        if (nQuizNowStep >= nQuizTotalCount) // 문제를 다하면
        {

            //정답률 나타내기위한 치환
            nTotalCorrectAnswerCountToInt = nTotalCorrectAnswerCount.ToString();
            nQuizTotalCountToInt = nQuizTotalCount.ToString();
            float a = float.Parse(nTotalCorrectAnswerCountToInt);
            float b = float.Parse(nQuizTotalCountToInt);

            // 이거 왜 한쪽만 안됌?? 뭐지
            //DisplayRemainTime(RemainTimeOfStage);
            //Time.TimeScale=0;
            //DisplayRemainTime(TimeValue);
            txtAnswerDivTotal.text = "정답률 : " + (a / b) * 100 + "%";
            txtTotalResult.text = nTotalCorrectAnswerCount.ToString() + "/" + nQuizTotalCount.ToString() + " 정답";

            objTotalResult.SetActive(true);
            objResultPopupSet.SetActive(true);

            //for (int k = 0; k < RandomMax; k++)
            //{
            //    Debug.Log("Last Check : " + RandomWeight[k]); // 확인용
            //}
        }

        // cnt = 목숨
        if (cnt > 2)
        {
            objResultPopupSet.SetActive(true);
            objWrongAnswer.SetActive(true);

            //SceneManager.LoadScene("Title");
            Debug.Log("Game End");

            for (int k = 0; k < RandomMax; k++)
            {
                Debug.Log("Last Check : " + RandomWeight[k]); // 확인용
            }
        }
    }

    void DisplayTimer(float timeToDisplay)
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        Timer.text = string.Format("남은 시간 : {0:00}:{1:00}", minutes, seconds);
    }

    void DisplayRemainTime(float timeToDisplay) // 이건 뭐 그다지 중요 x 나중에
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        RemainTime.text = string.Format("남은 시간 : {0:00}:{1:00}", minutes, seconds);
    }

    void Update()
    {
        if (TimeValue > 0)
        {
            TimeValue -= Time.deltaTime; // 제한시간 = 제한 시간 - 흘러가는 시간
            //RemainTimeOfStage -= Time.deltaTime;
        }

        else  //제한시간 넘기면 목숨차감 및 다음문제
        {
            cnt += 1;
            Debug.Log("cnt : " + cnt);
            DecreaseHP();

            RandomWeight.Add(RandomPickControl);
            RandomMax += 1;

            NextQuizLoad();
        }

        DisplayTimer(TimeValue); // Display Time함수의 인자로 timevalue 넣기.

        if (nQuizNowStep >= nQuizTotalCount)
        {
            NextQuizLoad();
        }

        if (nQuizNowStep < nQuizTotalCount)
        {

            if (isQuizLoad == false) // 퀴즈 셋팅 전
            {

                RandomPick_ = Random.Range(0, RandomMax);
                RandomPick = (int)RandomWeight[RandomPick_];
                // RandomPick_ = 0~13++
                // RandomPick = 0~13

                QuestionNum_1 = Random.Range(0, 4);

                while (Question_Check)
                {
                    QuestionNum_2 = Random.Range(0, 4);
                    if (QuestionNum_1 != QuestionNum_2) Question_Check = false;
                }

                Debug.Log("제출할 문제번호-1 : " + RandomPick);
                Debug.Log("왼쪽문제번호 : " + QuestionNum_1);
                Debug.Log("오른쪽문제번호 : " + QuestionNum_2);

                if (QuestionNum_1 == 0)
                {
                    GOimg_0_0.SetActive(true);
                    GOimg_1_0.SetActive(false);
                    GOimg_2_0.SetActive(false);
                    GOimg_3_0.SetActive(false);
                    Debug.Log("LeftImage==0  ///////////////////////");
                }

                if (QuestionNum_1 == 1)
                {
                    GOimg_0_0.SetActive(false);
                    GOimg_1_0.SetActive(true);
                    GOimg_2_0.SetActive(false);
                    GOimg_3_0.SetActive(false);
                    Debug.Log("LeftImage==1  ///////////////////////");
                }

                if (QuestionNum_1 == 2)
                {
                    GOimg_0_0.SetActive(false);
                    GOimg_1_0.SetActive(false);
                    GOimg_2_0.SetActive(true);
                    GOimg_3_0.SetActive(false);
                    Debug.Log("LeftImage==2  ///////////////////////");
                }

                if (QuestionNum_1 == 3)
                {
                    GOimg_0_0.SetActive(false);
                    GOimg_1_0.SetActive(false);
                    GOimg_2_0.SetActive(false);
                    GOimg_3_0.SetActive(true);
                    Debug.Log("LeftImage==3  ///////////////////////");
                }

                if (QuestionNum_2 == 0)
                {
                    GOimg_0_1.SetActive(true);
                    GOimg_1_1.SetActive(false);
                    GOimg_2_1.SetActive(false);
                    GOimg_3_1.SetActive(false);
                    Debug.Log("RightImage==0  ///////////////////////");
                }

                if (QuestionNum_2 == 1)
                {
                    GOimg_0_1.SetActive(false);
                    GOimg_1_1.SetActive(true);
                    GOimg_2_1.SetActive(false);
                    GOimg_3_1.SetActive(false);
                    Debug.Log("RightImage==1  ///////////////////////");
                }

                if (QuestionNum_2 == 2)
                {
                    GOimg_0_1.SetActive(false);
                    GOimg_1_1.SetActive(false);
                    GOimg_2_1.SetActive(true);
                    GOimg_3_1.SetActive(false);
                    Debug.Log("RightImage==2  ///////////////////////");
                }

                if (QuestionNum_2 == 3)
                {
                    GOimg_0_1.SetActive(false);
                    GOimg_1_1.SetActive(false);
                    GOimg_2_1.SetActive(false);
                    GOimg_3_1.SetActive(true);
                    Debug.Log("RightImage==3  ///////////////////////");
                }

                // 기존 문제
                // txtQuizScript.text = gameDataManager.data_Quiz[RandomPick]["문제"].ToString();

                // 아래는 문제를 요소를 랜덤을 뽑아옴
                txtQuizScript.text = gameDataManager.data_Quiz[RandomPick]["왼쪽문제"].ToString().Split('/')[QuestionNum_1] +
                    gameDataManager.data_Quiz[RandomPick]["보기1"].ToString() + gameDataManager.data_Quiz[RandomPick]["오른쪽문제"].ToString().Split('/')[QuestionNum_2] +
                    gameDataManager.data_Quiz[RandomPick]["보기2"].ToString();

                txtStage.text = "Now Stage : " + nQuizNowStep.ToString() + "/" + nQuizTotalCount.ToString();
                //switch (gameDataManager.data_Quiz[nQuizNowStep]["타입"].ToString())
                //{

                //case "2": // 타입2 - 객관식 퀴즈
                objAnswerType2Set.SetActive(true);
                //break;
                //}

                // 랜덤문제 뽑기 구현
                RandomPickControl = RandomPick;

                isQuizLoad = true;

            }
            else  // 퀴즈 셋팅 후
            {
                if (RandomPickControl <= 9) // 답이 1개일 때,
                {

                    if (isChoose1 || isChoose2 || isChoose3 || isChoose4)
                    {
                        //objResultPopupSet.SetActive(true); // 이게 팝업창 뜨게해주는거
                        Debug.Log("checkPoint1////////////////////////");
                        if ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString() == "1" && isChoose1 == true) ||
                            (gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString() == "2" && isChoose2 == true) ||
                            (gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString() == "3" && isChoose3 == true) ||
                            (gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString() == "4" && isChoose4 == true))
                        {
                            nTotalCorrectAnswerCount += 1;
                            objCorrectAnswer.SetActive(true);
                            isCorrect = true;

                            if (isCorrect)
                            {
                                //nTotalCorrectAnswerCount += 1;
                                isCorrect = false;
                            }

                            txtTotalResult.text = nTotalCorrectAnswerCount.ToString() + "/" + nQuizTotalCount.ToString() + " 정답";
                            if (nQuizNowStep >= nQuizTotalCount)
                            {
                                SceneManager.LoadScene("Main_1");
                                Debug.Log("게임 끝 Title 씬 로드");
                            }

                            NextQuizLoad();
                        }
                        else
                        {

                            //objWrongAnswer.SetActive(true);

                            if (cnt_check == false)
                            {
                                Debug.Log("checkPoint2////////////////////////");
                                cnt += 1;
                                Debug.Log("cnt : " + cnt);
                                DecreaseHP();
                                cnt_check = true;

                                RandomWeight.Add(RandomPickControl);
                                RandomMax += 1;
                            }

                            NextQuizLoad();
                        }
                    }
                }

                else //답이 2개일 때
                {

                    // 정답
                    if ((isChoose1 && isChoose2) || (isChoose1 && isChoose3) || (isChoose1 && isChoose4) ||
                        (isChoose2 && isChoose3) || (isChoose2 && isChoose4) || (isChoose3 && isChoose4))
                    {
                        if (((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[1] == "1" && isChoose1 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[0] == "3" && isChoose3 == true))) ||
                            ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[1] == "1" && isChoose1 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[0] == "4" && isChoose4 == true))) ||
                            ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[1] == "2" && isChoose2 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[0] == "3" && isChoose3 == true))) ||
                            ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[1] == "2" && isChoose2 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["정답"].ToString().Split('/')[0] == "4" && isChoose4 == true))))
                        {
                            Debug.Log("checkPoint3////////////////////////");

                            nTotalCorrectAnswerCount += 1;
                            objCorrectAnswer.SetActive(true);
                            isCorrect = true;

                            if (isCorrect)
                            {
                                //nTotalCorrectAnswerCount += 1;
                                isCorrect = false;
                            }

                            txtTotalResult.text = nTotalCorrectAnswerCount.ToString() + "/" + nQuizTotalCount.ToString() + " 정답";
                            if (nQuizNowStep >= nQuizTotalCount)
                            {
                                SceneManager.LoadScene("Main_1");
                                Debug.Log("게임 끝 Title 씬 로드");
                            }

                            NextQuizLoad();
                        }

                        else
                        {

                            if (!cnt_check)
                            {
                                Debug.Log("checkPoint4////////////////////////");
                                cnt += 1;
                                Debug.Log("cnt : " + cnt);
                                DecreaseHP();
                                cnt_check = true;

                                RandomWeight.Add(RandomPickControl);
                                RandomMax += 1;
                            }
                            if (TwoQuestion_Check)
                            {
                                NextQuizLoad();
                                TwoQuestion_Check = false;
                            }

                        }
                    }
                }

            }
        }

    }
}