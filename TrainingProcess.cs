using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingProcess : MonoBehaviour
{
    public Button yourButton; // Ссылка на кнопку
    public CurrencyManager currencyManager; // Ссылка на CurrencyManager
    public float count; // Стоимость для вычета
    public GameObject tagObject; // Объект, тег которого будет проверяться
    public GameObject objectToHide; // Объект, который будет скрыт, если тег "NotSelected"
    public int unitIndex; // Публичный индекс юнита
    public int trainingTime; // Публичное время тренировки
    public string unitName; // Публичное имя юнита
    public OpenCloseUnitChoice openCloseUnitChoice; // Ссылка на OpenCloseUnitChoice
    public List<GameObject> windows; // Новый список окон
    public GameObject expensesObject; // Объект для учета расходов
    public GameObject unitTimers;
    public float expenses; // Расходы

    private Coroutine trainingCoroutine;

    void Update() 
    {
        if (tagObject != null && objectToHide != null)
        {
            if (tagObject.CompareTag("NotSelected"))
            {
                if (objectToHide.activeSelf)
                {
                    StartCoroutine(HideObjectWithDelay(objectToHide, 0.2f)); // Запускаем корутину с задержкой 0.2 секунды
                }
            }
        }
    }

    public void OnButtonClick()
    {
        Debug.Log("OnButtonClick called");
        if (currencyManager == null)
        {
            Debug.LogError("currencyManager is not assigned.");
            return;
        }

        Debug.Log("Current player balance: " + currencyManager.playerBalance);
        Debug.Log("Cost to deduct: " + count);

        if (currencyManager.playerBalance >= count)
        {
            Debug.Log("Sufficient balance. Proceeding to deduct.");

            // Вычесть стоимость из баланса
            currencyManager.AddToBalance(-count);

            // Начать корутину для отложенной деактивации кнопки
            trainingCoroutine = StartCoroutine(DelayedButtonDeactivation());

            Debug.Log("Balance updated. New balance: " + currencyManager.playerBalance);

            // Изменение functionIndex на значение unitIndex в UnitReleaseController
            if (openCloseUnitChoice != null)
            {
                int currentIndex = openCloseUnitChoice.currentIndex;
                if (currentIndex >= 0 && currentIndex < windows.Count)
                {
                    GameObject window = windows[currentIndex];
                    Transform releaseTransform = FindChildByName(window.transform, "Release");
                    if (releaseTransform != null)
                    {
                        UnitReleaseController releaseController = releaseTransform.GetComponent<UnitReleaseController>();
                        if (releaseController != null)
                        {
                            releaseController.functionIndex = unitIndex;
                        }
                    }
                }
            }
            if (expensesObject != null)
            {
                if (openCloseUnitChoice != null)
                {
                    int currentIndex = openCloseUnitChoice.currentIndex;
                    if (currentIndex >= 0 && currentIndex < expensesObject.transform.childCount)
                    {
                        Transform expenseTransform = expensesObject.transform.GetChild(currentIndex);
                        Expenses expensesComponent = expenseTransform.GetComponent<Expenses>();
                        if (expensesComponent != null)
                        {
                            expensesComponent.expensesValue += expenses;
                            Debug.Log("Expenses updated. New expenses value: " + expensesComponent.expensesValue);
                        }
                        else
                        {
                            Debug.LogError("Expenses component not found on the child object.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Invalid index for expensesObject children.");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Not enough balance.");
        }
    }

    IEnumerator DelayedButtonDeactivation()
    {
        Debug.Log("Starting DelayedButtonDeactivation");
        yield return new WaitForSeconds(0.2f);
        yourButton.gameObject.SetActive(false);

        // Процесс тренировки юнита
        ProcessUnitTraining();
    }

    void ProcessUnitTraining()
    {
        if (openCloseUnitChoice == null)
        {
            Debug.LogError("openCloseUnitChoice is not assigned.");
            return;
        }

        int currentIndex = openCloseUnitChoice.currentIndex;
        if (currentIndex < 0 || currentIndex >= windows.Count)
        {
            Debug.LogError("currentIndex is out of range.");
            return;
        }

        GameObject window = windows[currentIndex];

        // Находим TrainingUnit среди всех дочерних объектов
        Transform trainingUnit = FindChildByName(window.transform, "TrainingUnit");
        if (trainingUnit != null)
        {
            Debug.Log("Found TrainingUnit: " + trainingUnit.name);
            TrainingUnitController controller = trainingUnit.GetComponent<TrainingUnitController>();
            if (controller != null)
            {
                Debug.Log("Found TrainingUnitController");
                controller.imageIndex = (short)unitIndex;
                controller.UpdateImage(); // Обновляем изображение сразу
                trainingUnit.tag = "Training";
            }
        }

        // Находим UnitDescriptions среди всех дочерних объектов
        Transform unitDescriptions = FindChildByName(window.transform, "UnitDescriptions");
        if (unitDescriptions != null)
        {
            Debug.Log("Found UnitDescriptions: " + unitDescriptions.name);
            LanguageChanges languageChanges = unitDescriptions.GetComponent<LanguageChanges>();
            if (languageChanges != null)
            {
                Debug.Log("Found LanguageChanges");
                languageChanges.index = (short)unitIndex; // Явное приведение типа
                languageChanges.UpdateText(); // Обновляем текст сразу
            }
        }

        if (unitTimers != null)
            {
                if (openCloseUnitChoice != null)
                {
                    int timerIndex = openCloseUnitChoice.currentIndex;
                    if (timerIndex >= 0 && timerIndex < expensesObject.transform.childCount)
                    {
                        Transform timersTransform = unitTimers.transform.GetChild(currentIndex);
                        CountTime countdown = timersTransform.GetComponent<CountTime>();
                        if (countdown != null)
                        {
                            Debug.Log("Found CountTime");
                            countdown.countdownTime = trainingTime;
                            countdown.StartCountdown(); // Начинаем отсчет времени
                        }
                    }
                }
            }


        trainingCoroutine = StartCoroutine(CompleteTraining(window));
    }

    IEnumerator CompleteTraining(GameObject window)
    {
        yield return new WaitForSeconds(trainingTime);

        // Находим UnitDescriptions среди всех дочерних объектов
        Transform unitDescriptions = FindChildByName(window.transform, "UnitDescriptions");
        if (unitDescriptions != null)
        {
            Debug.Log("Found UnitDescriptions for completion: " + unitDescriptions.name);
            LanguageChanges languageChanges = unitDescriptions.GetComponent<LanguageChanges>();
            if (languageChanges != null)
            {
                Debug.Log("Found LanguageChanges for completion");
                languageChanges.index = 0;
                languageChanges.UpdateText(); // Обновляем текст сразу
            }
        }

        // Находим targetUnit среди всех дочерних объектов
        string cleanedUnitName = unitName.Trim();
        Transform targetUnit = FindChildByName(window.transform, cleanedUnitName);
        if (targetUnit != null)
        {
            Debug.Log("Found targetUnit for completion: " + targetUnit.name);
            Debug.Log("Current tag of targetUnit: " + targetUnit.tag);
            if (targetUnit.tag == "NotHired")
            {
                targetUnit.tag = "Hired";
                Debug.Log("Changed targetUnit tag to Hired");
            }
            else if (targetUnit.tag == "Hired")
            {
                targetUnit.tag = "Hired2";
                Debug.Log("Changed targetUnit tag to Hired2");
            }
        }
        else
        {
            Debug.LogError("targetUnit not found with name: " + cleanedUnitName);
        }

        // Находим TrainingUnit среди всех дочерних объектов
        Transform trainingUnit = FindChildByName(window.transform, "TrainingUnit");
        if (trainingUnit != null)
        {
            Debug.Log("Found TrainingUnit for completion: " + trainingUnit.name);
            TrainingUnitController controller = trainingUnit.GetComponent<TrainingUnitController>();
            if (controller != null)
            {
                Debug.Log("Found TrainingUnitController for completion");
                controller.imageIndex = 0;
                trainingUnit.tag = "NotTraining";
                Debug.Log("Reset TrainingUnit tag to NotTraining and imageIndex to 0");
            }
        }

        
    }

    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            Debug.Log("Checking child: " + child.name); // Логирование имени каждого дочернего объекта
            if (child.name == name)
                return child;
            Transform result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }

    IEnumerator HideObjectWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    public void ChangeTagWithDelay(GameObject obj, string newTag, float delay)
    {
        StartCoroutine(ChangeTagCoroutine(obj, newTag, delay));
    }

    IEnumerator ChangeTagCoroutine(GameObject obj, string newTag, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.tag = newTag;
    }

    public void ResetTrainingProcess()
    {
        Debug.Log("ResetTrainingProcess called");

        // Остановить текущие корутины, если есть
        if (trainingCoroutine != null)
        {
            StopCoroutine(trainingCoroutine);
            trainingCoroutine = null;
        }

        // Восстановить состояние кнопки
        yourButton.gameObject.SetActive(true);

        // Восстановить баланс
        currencyManager.AddToBalance(count);
        Debug.Log("Balance restored. New balance: " + currencyManager.playerBalance);

        // Сбросить состояния объектов
        ResetObjectsState();
    }

    public void ResetObjectsState()
    {
        Debug.Log("ResetObjectsState called");

        // Остановить текущие корутины, если есть
        if (trainingCoroutine != null)
        {
            StopCoroutine(trainingCoroutine);
            trainingCoroutine = null;
        }

        if (openCloseUnitChoice != null)
        {
            int currentIndex = openCloseUnitChoice.currentIndex;
            if (currentIndex >= 0 && currentIndex < windows.Count)
            {
                GameObject window = windows[currentIndex];

                // Находим TrainingUnit среди всех дочерних объектов
                Transform trainingUnit = FindChildByName(window.transform, "TrainingUnit");
                if (trainingUnit != null)
                {
                    TrainingUnitController controller = trainingUnit.GetComponent<TrainingUnitController>();
                    if (controller != null)
                    {
                        controller.imageIndex = 0;
                        trainingUnit.tag = "NotTraining";
                    }
                }

                // Находим UnitDescriptions среди всех дочерних объектов
                Transform unitDescriptions = FindChildByName(window.transform, "UnitDescriptions");
                if (unitDescriptions != null)
                {
                    LanguageChanges languageChanges = unitDescriptions.GetComponent<LanguageChanges>();
                    if (languageChanges != null)
                    {
                        languageChanges.index = 0;
                        languageChanges.UpdateText();
                    }
                }

                // Находим targetUnit среди всех дочерних объектов
                Transform targetUnit = FindChildByName(window.transform, unitName.Trim());
                if (targetUnit != null)
                {
                    if (targetUnit.tag == "Hired" || targetUnit.tag == "Hired2")
                    {
                        targetUnit.tag = "NotHired";
                    }
                }

                if (unitTimers != null)
                    {
                        if (openCloseUnitChoice != null)
                        {
                            int timerIndex = openCloseUnitChoice.currentIndex;
                            if (timerIndex >= 0 && timerIndex < expensesObject.transform.childCount)
                            {
                                Transform timersTransform = unitTimers.transform.GetChild(currentIndex);
                                CountTime countdown = timersTransform.GetComponent<CountTime>();
                                if (countdown != null)
                                {
                                    countdown.StopCountdown();
                                }
                            }
                        }
                    }

                Transform trainingTimeObj = FindChildByName(window.transform, "TrainingTime");
                if (trainingTimeObj != null)
                {
                    trainingTimeObj.gameObject.SetActive(false); // Деактивируем объект TrainingTime
                }

                // Сбросить functionIndex в UnitReleaseController
                Transform releaseTransform = FindChildByName(window.transform, "Release");
                if (releaseTransform != null)
                {
                    UnitReleaseController releaseController = releaseTransform.GetComponent<UnitReleaseController>();
                    if (releaseController != null)
                    {
                        releaseController.functionIndex = 0;
                    }
                }
                if (expensesObject != null)
                {
                    int index = openCloseUnitChoice.currentIndex;
                    if (index >= 0 && index < expensesObject.transform.childCount)
                    {
                        Transform expenseTransform = expensesObject.transform.GetChild(index);
                        Expenses expensesComponent = expenseTransform.GetComponent<Expenses>();
                        if (expensesComponent != null)
                        {
                            expensesComponent.expensesValue -= (expenses / 2);
                        }
                        else
                        {
                            Debug.LogError("Expenses component not found on the child object.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Invalid index for expensesObject children.");
                    }
                }
            }
        }
    }
}   



