using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager1 : MonoBehaviour
{
    [Header("����, ��Ÿ")]
    public Button btnBackTitle;
    public Text txtQuizScript;
    public int nQuizTotalCount;
    public int nQuizNowStep;
    public bool isQuizLoad;
    public Text txtStage;
    public Text Timer;
    public Text RemainTime;

    [Header("Img ����")]
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

    //[Header("��� ���� - 1")]
    //public GameObject objAnswerType1Set;
    //public Button btnO;
    //public Button btnX;
    //public bool isChooseO;
    //public bool isChooseX;

    [Header("��� ���� - 2")]
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

    //[Header("��� ���� - 3")]
    //public GameObject objAnswerType3Set;
    //public InputField ipfAnswer;
    //public Button btnAnswerIpf;
    //public bool isSubmitIpf;

    [Header("����˾�â")]
    public GameObject objResultPopupSet;
    public GameObject objCorrectAnswer;
    public GameObject objWrongAnswer;
    public Button btnResultConfirm;

    [Header("����")]
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

    public ArrayList RandomWeight = new ArrayList(); // ����ġ �迭

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

        Debug.Log("�ʱ�ȭ ����");
    }

    // Start is called before the first frame update
    void Start()
    {
        ButtonClick = GetComponent<AudioSource>();

        for (int i = 0; i < 14; i++) // ���ڴ� nQuizTotalCount
        {
            RandomWeight.Add(i); // ó���� ������ ��ȣ 8���� �־��ش�.
        }

        //for (int i = 0; i < 14; i++)
        //{
        //    Debug.Log("RandomWeight : " + RandomWeight[i]); // Ȯ�ο�
        //}

        gameDataManager = GameObject.Find("GameDataManager_DontDestroy").GetComponent<GameDataManager>();

        this.ImgHP = GameObject.Find("ImgHP");

        btnBackTitle.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Title");
            Debug.Log("Title �� �ε�");
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
                Debug.Log("���� �� Title �� �ε�");
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



        Debug.Log("�� ���� �� : " + gameDataManager.data_Quiz.Count);

        // ���� �� ����
        nQuizTotalCount = gameDataManager.data_Quiz.Count;
        RandomMax = nQuizTotalCount;
        Debug.Log("RandomMax : " + RandomMax);

        nQuizNowStep = 0;
        RandomPickControl = 0;

        txtTotalResult.text = "";
        nTotalCorrectAnswerCount = 0;

        TimeValue = 10; // ���⼭ ���ѽð� �ʱ�ȭ

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

        TimeValue = 10; // ���� �ѱ涧���� �ð� �ʱ�ȭ

        if (nQuizNowStep >= nQuizTotalCount) // ������ ���ϸ�
        {

            //����� ��Ÿ�������� ġȯ
            nTotalCorrectAnswerCountToInt = nTotalCorrectAnswerCount.ToString();
            nQuizTotalCountToInt = nQuizTotalCount.ToString();
            float a = float.Parse(nTotalCorrectAnswerCountToInt);
            float b = float.Parse(nQuizTotalCountToInt);

            // �̰� �� ���ʸ� �ȉ�?? ����
            //DisplayRemainTime(RemainTimeOfStage);
            //Time.TimeScale=0;
            //DisplayRemainTime(TimeValue);
            txtAnswerDivTotal.text = "����� : " + (a / b) * 100 + "%";
            txtTotalResult.text = nTotalCorrectAnswerCount.ToString() + "/" + nQuizTotalCount.ToString() + " ����";

            objTotalResult.SetActive(true);
            objResultPopupSet.SetActive(true);

            //for (int k = 0; k < RandomMax; k++)
            //{
            //    Debug.Log("Last Check : " + RandomWeight[k]); // Ȯ�ο�
            //}
        }

        // cnt = ���
        if (cnt > 2)
        {
            objResultPopupSet.SetActive(true);
            objWrongAnswer.SetActive(true);

            //SceneManager.LoadScene("Title");
            Debug.Log("Game End");

            for (int k = 0; k < RandomMax; k++)
            {
                Debug.Log("Last Check : " + RandomWeight[k]); // Ȯ�ο�
            }
        }
    }

    void DisplayTimer(float timeToDisplay)
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        Timer.text = string.Format("���� �ð� : {0:00}:{1:00}", minutes, seconds);
    }

    void DisplayRemainTime(float timeToDisplay) // �̰� �� �״��� �߿� x ���߿�
    {

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        RemainTime.text = string.Format("���� �ð� : {0:00}:{1:00}", minutes, seconds);
    }

    void Update()
    {
        if (TimeValue > 0)
        {
            TimeValue -= Time.deltaTime; // ���ѽð� = ���� �ð� - �귯���� �ð�
            //RemainTimeOfStage -= Time.deltaTime;
        }

        else  //���ѽð� �ѱ�� ������� �� ��������
        {
            cnt += 1;
            Debug.Log("cnt : " + cnt);
            DecreaseHP();

            RandomWeight.Add(RandomPickControl);
            RandomMax += 1;

            NextQuizLoad();
        }

        DisplayTimer(TimeValue); // Display Time�Լ��� ���ڷ� timevalue �ֱ�.

        if (nQuizNowStep >= nQuizTotalCount)
        {
            NextQuizLoad();
        }

        if (nQuizNowStep < nQuizTotalCount)
        {

            if (isQuizLoad == false) // ���� ���� ��
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

                Debug.Log("������ ������ȣ-1 : " + RandomPick);
                Debug.Log("���ʹ�����ȣ : " + QuestionNum_1);
                Debug.Log("�����ʹ�����ȣ : " + QuestionNum_2);

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

                // ���� ����
                // txtQuizScript.text = gameDataManager.data_Quiz[RandomPick]["����"].ToString();

                // �Ʒ��� ������ ��Ҹ� ������ �̾ƿ�
                txtQuizScript.text = gameDataManager.data_Quiz[RandomPick]["���ʹ���"].ToString().Split('/')[QuestionNum_1] +
                    gameDataManager.data_Quiz[RandomPick]["����1"].ToString() + gameDataManager.data_Quiz[RandomPick]["�����ʹ���"].ToString().Split('/')[QuestionNum_2] +
                    gameDataManager.data_Quiz[RandomPick]["����2"].ToString();

                txtStage.text = "Now Stage : " + nQuizNowStep.ToString() + "/" + nQuizTotalCount.ToString();
                //switch (gameDataManager.data_Quiz[nQuizNowStep]["Ÿ��"].ToString())
                //{

                //case "2": // Ÿ��2 - ������ ����
                objAnswerType2Set.SetActive(true);
                //break;
                //}

                // �������� �̱� ����
                RandomPickControl = RandomPick;

                isQuizLoad = true;

            }
            else  // ���� ���� ��
            {
                if (RandomPickControl <= 9) // ���� 1���� ��,
                {

                    if (isChoose1 || isChoose2 || isChoose3 || isChoose4)
                    {
                        //objResultPopupSet.SetActive(true); // �̰� �˾�â �߰����ִ°�
                        Debug.Log("checkPoint1////////////////////////");
                        if ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString() == "1" && isChoose1 == true) ||
                            (gameDataManager.data_Quiz[RandomPickControl]["����"].ToString() == "2" && isChoose2 == true) ||
                            (gameDataManager.data_Quiz[RandomPickControl]["����"].ToString() == "3" && isChoose3 == true) ||
                            (gameDataManager.data_Quiz[RandomPickControl]["����"].ToString() == "4" && isChoose4 == true))
                        {
                            nTotalCorrectAnswerCount += 1;
                            objCorrectAnswer.SetActive(true);
                            isCorrect = true;

                            if (isCorrect)
                            {
                                //nTotalCorrectAnswerCount += 1;
                                isCorrect = false;
                            }

                            txtTotalResult.text = nTotalCorrectAnswerCount.ToString() + "/" + nQuizTotalCount.ToString() + " ����";
                            if (nQuizNowStep >= nQuizTotalCount)
                            {
                                SceneManager.LoadScene("Main_1");
                                Debug.Log("���� �� Title �� �ε�");
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

                else //���� 2���� ��
                {

                    // ����
                    if ((isChoose1 && isChoose2) || (isChoose1 && isChoose3) || (isChoose1 && isChoose4) ||
                        (isChoose2 && isChoose3) || (isChoose2 && isChoose4) || (isChoose3 && isChoose4))
                    {
                        if (((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[1] == "1" && isChoose1 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[0] == "3" && isChoose3 == true))) ||
                            ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[1] == "1" && isChoose1 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[0] == "4" && isChoose4 == true))) ||
                            ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[1] == "2" && isChoose2 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[0] == "3" && isChoose3 == true))) ||
                            ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[1] == "2" && isChoose2 == true) && ((gameDataManager.data_Quiz[RandomPickControl]["����"].ToString().Split('/')[0] == "4" && isChoose4 == true))))
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

                            txtTotalResult.text = nTotalCorrectAnswerCount.ToString() + "/" + nQuizTotalCount.ToString() + " ����";
                            if (nQuizNowStep >= nQuizTotalCount)
                            {
                                SceneManager.LoadScene("Main_1");
                                Debug.Log("���� �� Title �� �ε�");
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