using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("GUI Control Header")]
    [SerializeField] private GameObject [] panels;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Camera mainCamera;
    



    public int InputNumber { get; set; }


     private void Awake()// singleton Design Pattern
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartButton);
        quitButton.onClick.AddListener(QuitGame);
      
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartButton);
        quitButton.onClick.RemoveListener(QuitGame);
       
    }

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnInputEndEdit);
        
    }
    

   
   
    
    public void StartButton()  // start butonuna basildiginda menuyu kapatip oyunu baslatir
    {
        panels[0].SetActive(false);
        GridManager.Instance.GenerateGrid();
        SetCameraSize();
    }
    
    
    private void OnInputEndEdit(string value)// input field degerini alir
    {
        int intValue;
        if (int.TryParse(value, out intValue))
        {
            if (intValue > 100) 
            {
                intValue = 100;
                inputField.text = "100"; // Güncelleme sonucunu kullanıcıya göster
            }
            InputNumber = intValue;
        }
        else
        {
            Debug.Log("Invalid value!");
        }
    }


    private void SetCameraSize()
    {
        float newSize = 5; // Varsayılan boyut
        if (InputNumber >= 5)
            newSize = 7f;
        if (InputNumber >= 14)
            newSize = 10f;
        if (InputNumber >= 20)
            newSize = 12f;
        if (InputNumber >= 25)
            newSize = 14f;
        if (InputNumber >= 30)
            newSize = 17f;
        if (InputNumber >= 35)
            newSize = 20f;
        if (InputNumber >= 40)
            newSize = 22f;
        if (InputNumber >= 45)
            newSize = 24f;
        if (InputNumber >= 50)
            newSize = 26f;
        if (InputNumber >= 55)
            newSize = 28f;
        if (InputNumber >= 60)
            newSize = 30f;
        if (InputNumber >= 65)
            newSize = 32f;
        if (InputNumber >= 70)
            newSize = 34f;
        if (InputNumber >= 75)
            newSize = 36f;
        if (InputNumber >= 80)
            newSize = 38f;
        if (InputNumber >= 85)
            newSize = 40f;
        if (InputNumber >= 90)
            newSize = 42f;
        if (InputNumber >= 95)
            newSize = 44f;
        if (InputNumber >= 100)
            newSize = 48f;

        mainCamera.orthographicSize = newSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
       
    }

    private static void QuitGame()
    {
        Application.Quit();
    }
    
   
    
}
