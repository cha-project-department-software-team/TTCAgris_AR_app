using System.Collections;
using UnityEngine;

public class TimeOut_Function : MonoBehaviour
{
  public float timeoutDuration = 60f;  // Timeout duration in seconds
  private float timeSinceLastInteraction; // Time since last interaction
  private Coroutine timerCoroutine;
  [SerializeField]
  private float timeShowToast = 5;
  private bool activeLogOut = false;

  void Start()
  {

    ResetTimeout();
  }

  void Update()
  {
    // Check for any user interaction (key press or touch)
    if (Input.touchCount > 0)
    {
      activeLogOut = false;
      ResetTimeout();  // Reset the timer if there is interaction
    }
  }

  // Method to reset the timeout timer
  public void ResetTimeout()
  {
    // Reset the interaction time
    timeSinceLastInteraction = 0f;

    // Stop the existing timer coroutine if it's running
    if (timerCoroutine != null)
    {
      StopCoroutine(timerCoroutine);
    }

    // Start a new timer coroutine
    timerCoroutine = StartCoroutine(TimerRoutine());
  }

  // Method to exit the application
  public void ExitApplication()
  {
    Application.Quit();
  }

  // Coroutine to handle the timeout timer
  IEnumerator TimerRoutine()
  {
    while (true)
    {
      // Wait for 1 second before incrementing the timer
      yield return new WaitForSeconds(1f);

      // Increment the interaction time
      timeSinceLastInteraction += 1f;
      //DebugtimeSinceLastInteraction);

      // Check if the interaction time exceeds the timeout duration
      if (timeSinceLastInteraction >= timeoutDuration)
      {
        activeLogOut = true;

        Show_Toast.Instance.ShowToast("failure", "Phát hiện treo máy lâu! Hãy chạm vào màn hình hoặc ứng dụng sẽ tự thoát");
        yield return new WaitForSeconds(timeShowToast);
        yield return Show_Toast.Instance.Set_Instance_Status_False();
        if (activeLogOut) ExitApplication();  // Exit the application if no interaction
        break;
      }
    }
  }

  // Coroutine to wait for the toast message to be displayed


  private void OnDestroy()
  {
    if (timerCoroutine != null)
    {
      StopCoroutine(timerCoroutine);
    }
  }
}
