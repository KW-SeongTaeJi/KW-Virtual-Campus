using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ChatType { Normal = 0, Party, Guild, Whisper, System, Count }
public class ChatController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField  inputField;           // 대화 입력창

    [SerializeField]
    private GameObject      textChatPrefab;       // 대화를 출력하는 Text UI 프리팹
    [SerializeField]
    private Transform       parentContent;        // 대화가 출력되는 ScrollView의 Content

    [SerializeField]
    private Sprite[]        spriteChatInputType;  // 대화 입력 속성 버튼에 적용할 이미지 에셋
    [SerializeField]
    private Image           imageChatInputType;   // 대화 입력 속성 버튼의 이미지
    [SerializeField]
    private TextMeshProUGUI textInput;            // 대화 입력 속성에 따라 대화 입력창에 작성되는 텍스트 색상 변경

    private ChatType        currentInputType;     // 현재 대화 입력 속성(Normal, Party, Guild)
    private Color           currentTextColor;     // 대화 입력 속성에 따라 입력/출력 텍스트 색상 설정

    private List<ChatCell>  chatList;             // 대화창에 출력되는 모든 대화를 보관하는 리스트
    private ChatType        currentViewType;      // 현재 대화 보기 속성 (Normal, Party, Guild, Whisper, System)

    private string          lastChatData = "";    // 마지막에 작성한 대화내용
    private string          lastWhisperID = "";   // 마지막에 귓속말을 보낸 대상

    private string          ID = "22soft Jerry";  // 본인 아이디(임시)
    private string          friendID = "Noname"; // 친구 아이디(임시)

    public void Awake()
    {
        chatList         = new List<ChatCell>();

        currentInputType = ChatType.Normal;
        currentTextColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        //대화 입력창이 포커스 되어있지 않을 때 Enter키를 누르면
        if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            //대화 입력창의 포커스를 활성화
            inputField.ActivateInputField();
        }

        //대화 입력창이 포커스 되어있을 때 Tab키를 누르면
        if(Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused==true)
        {
            //현재 대화 입력 속성 설정
            SetCurrentInputType();
        }
    }

    /// <summary>
    /// InputField 입력 종료 후 Enter키를 이용해 Focus를 비활성화할 때 호출
    /// </summary>
    public void OnEndEditEventMethod()
    {
        // Enter키를 누르면 대화 입력창에 입력된 내용을 대화창에 출력
        if(Input.GetKeyDown(KeyCode.Return))
        {
            UpdateChat();
        }
    }

    /// <summary>
    /// Enter키 or 버튼을 눌러 InputField에 작성된 내용을 대화창에 출력
    /// </summary>
    public void UpdateChat()
    {
        // InputField가 비어있으면 종료
        if (inputField.text.Equals("")) return;

        /* // 대화내용 출력을 위해 Text UI 생성(textChatPrefab을 복제생성해서 parentContent의 자식으로 배치)
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell   cell  = clone.GetComponent<ChatCell>();

        // 대화 입력창에 있는 내용을 대화창에 출력(ID:내용)
        // clone.GetComponent<TextMeshProUGUI>().text = $"{ID} : {inputField.text}";
        cell.Setup(currentInputType, currentTextColor, $"{ID} : {inputField.text}");
        // 대화 입력창에 있는 내용을 초기화
        inputField.text = "";
        // 대화창에 출력한 대화를 리스트에 저장
        chatList.Add(cell); */

        // 대화 입력창에 입력된 내용을 대화창에 출력 (명령어가 포함되어 있을 때 명령어 처리)
        UpdateChatWithCommand(inputField.text);
    }

    private Color ChatTypeToColor(ChatType type)
    {
        // 대화 속성에 따라 색상 값 반환 (일반, 파티, 길드, 귓말, 시스템)
        Color[] colors = new Color[(int)ChatType.Count] {Color.white, Color.blue, Color.green, Color.magenta, Color.yellow };
        return colors[(int)type];
    }
    
    public void SetCurrentInputType()
    {
        //현재 대화 속성을 한 단계씩 변화(귓말, 시스템은 입력 속성에 없기 때문에 제외)
        currentInputType = (int)currentInputType < (int)ChatType.Count - 3 ? currentInputType + 1 : 0;
        //현재 대화 속성에 따라 버튼 이미지 변경
        imageChatInputType.sprite = spriteChatInputType[(int)currentInputType];
        //현재 대화 속성에 따라 출력되는 텍스트 색상 설정
        currentTextColor = ChatTypeToColor(currentInputType);
        //대화 입력창의 텍스트 색상 변경(Normal일 때 입력창은 하얀색 대신 검은색 출력)
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newType)
    {
        // Button UI의 OnClick 이벤트에 열거형은 매개변수로 처리가 안되기 때문에 int로 받아온다.
        currentViewType = (ChatType)newType;

        // 현재 대화 보기 속성이 일반이면
        if(currentViewType == ChatType.Normal)
        {
            //모든 대화 목록 활성화
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        //현재 대화 보기 속성이 일반이 아니면
        else
        {
            for(int i=0; i<chatList.Count; ++i)
            {
                // 현재 대화 보기 속성에 해당하는 대화만 활성화하고, 나머지는 비활성화
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }    

    private void PrintChatData(ChatType type, Color color, string text)
    {
        // 대화내용 출력을 위해 Text UI 생성 (textChatPrefab을 복제생성해서 parentContent의 자식으로 배치)
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell cell = clone.GetComponent<ChatCell>();

        // 대화 입력창에 있는 내용을 대화창에 출력 (ID : 내용)
        cell.Setup(type, color, $"{ID} : {text}");
        // 대화 입력창에 있는 내용 초기화
        inputField.text = "";
        // 대화창에 출력한 대화를 리스트에 저장
        chatList.Add(cell);
    }

    public void UpdateChatWithCommand(string chat)
    {
        // '/'로 시작하지 않기 때문에 명령어 처리 없이 대화내용 출력
        if(!chat.StartsWith("/re"))
        {
            // 현재 대화 내용을 lastChatData에 저장(/re로 마지막 대화 내용을 출력할 때 사용)
            lastChatData = chat;
            // 대화창에 대화 내용 출력 (현재 입력 속성, 색상, 대화 내용)
            PrintChatData(currentInputType, currentTextColor, lastChatData);
            return;
        }

        /// '/'로 시작할 때는 명령어로 인식하고 명령어 처리 추가

        // 마지막에 작성한 내용 다시 출력
        if (chat.StartsWith("/re"))
        {
            // 마지막 대화 내용이 없으면 종료
            if(lastChatData.Equals(""))
            {
                inputField.text = "";
                return;
            }

            // 마지막 대화 내용에 명령어가 포함되어 있을 수도 있기 때문에
            // 마지막 대화 내용을 매개변수로 다시 UpdataChatWithCommand() 메소드 호출
            UpdateChatWithCommand(lastChatData);
        }
        // 귓말
        else if(chat.StartsWith("/w"))
        {
            // 현재 대화 내용을 lastChatData에 저장
            lastChatData = chat;

            // ' '(공백)을 기준으로 문자열을 3개로 분할
            // 명령어(/w), 귓말 대상, 내용
            string[] whisper = chat.Split(' ', (char)3);

            // 모든 유저의 아이디를 검색해 동일한 아이디가 있는지 검사 후
            // 대상이 있으면 귓말을 보내고, 대상이 없으면 시스템 메세지 출력
            // (현재는 임시로 friendID 하나만 선언했기 때문에 하나만 비교)
            if(whisper[1]==friendID)
            {
                // 귓말 수신자 정보 저장 (/r로 마지막 귓말 대상에게 귓말 전송할 때 사용)
                lastWhisperID = whisper[1];
                // 귓말 입력 속성으로 귓말 수신자 정보와 내용 출력
                // 명령어를 작성할 때만 보내기 때문에 현재 입력 속성, 색상 변수를 따로 설정하지 않고 보냄
                PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {whisper[1]}] {whisper[2]}");
            }
            else
            {
                // 시스템 입력 속성으로 귓말을 보내려는 대상이 없다고 출력
                PrintChatData(ChatType.System, ChatTypeToColor(ChatType.System), $"Do not find [{whisper[1]}]");
            }

        }
        // 마지막에 귓말을 보낸 대상에게 다시 귓말 보내기
        else if(chat.StartsWith("/r"))
        {
            // 마지막 귓말 대상이 없으면 종료
            if(lastWhisperID.Equals(""))
            {
                inputField.text = "";
                return;
            }

            // 현재 대화 내용을 lastChatData에 저장
            lastChatData = chat;

            // ' '(공백)을 기준으로 문자열을 2개로 분할
            // 명령어(/r), 내용
            string[] whisper = chat.Split(' ', (char)2);

            // 귓말 입력 속성으로 귓말 수신자 정보와 내용 출력
            PrintChatData(ChatType.Whisper, ChatTypeToColor(ChatType.Whisper), $"[to {lastWhisperID}] {whisper[1]}");
        }
    }
}
