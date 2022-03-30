using UnityEngine;
using TMPro;

public class ChatCell : MonoBehaviour
{
	public ChatType ChatType { private set; get; }

	public void Setup(ChatType type, Color color, string textData)
	{
		TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

		ChatType = type;
		text.color = color;
		text.text = textData;
	}
}
