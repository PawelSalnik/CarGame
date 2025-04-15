using UnityEngine;
using System.IO.Ports;

public class ArduinoCommunication : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM3", 115200);  // Ustaw odpowiedni port COM

    public CarController carController;  // Referencja do skryptu CarController

    void Start()
    {
        stream.Open();
    }

    void Update()
    {
        if (stream.IsOpen)
        {
            try
            {
                // Read data from Arduino
                string value = stream.ReadLine();
                Debug.Log("Received data from Arduino: " + value); // Debug: Received data from Arduino

                // Split the data into parts
                string[] data = value.Split(' ');

                // Check if the data is sufficient
                if (data.Length >= 3)
                {
                    // Check if it's a button press
                    if (data[0] == "Button" && data[2] == "pressed")
                    {
                        // Check which button is pressed
                        if (data[1] == "1")
                        {
                            // Emulate shift up
                            Debug.Log("Emulate Shift Up");
                            EmulateShift(1); // Emulate shift up
                        }
                        else if (data[1] == "2")
                        {
                            // Emulate shift down
                            Debug.Log("Emulate Shift Down");
                            EmulateShift(-1); // Emulate shift down
                        }
                    }

                    // If it's not a button press, assume it's potentiometer data
                    else if (data.Length >= 2)
                    {
                        // Read the potentiometer value
                        int potValue = int.Parse(data[1]);

                        // Emulate change in Unity direction based on potentiometer value
                        float horizontalInput = Mathf.InverseLerp(0, 1023, potValue) * 2 - 1;
                        EmulateCarMovement(horizontalInput);

                        // Debug: Add information about potentiometer value
                        Debug.Log($"PotValue: {potValue}, HorizontalInput: {horizontalInput}");
                    }
                }
                else
                {
                    // Debug: If the data is incomplete, display a warning
                    Debug.LogWarning("Received incomplete data from Arduino. Expected at least 3 elements, received: " + data.Length);
                }
            }
            catch (System.Exception e)
            {
                // Debug: If an error occurs, display an error message
                Debug.LogError("Error processing data from Arduino: " + e.Message);
            }
        }
    }




    void OnDestroy()
    {
        if (stream.IsOpen)
        {
            stream.Close();
        }
    }

    // Funkcja emulacji zmiany biegu

    void EmulateShift(int direction)
    {
        if (carController != null)
        {
            carController.ChangeGear(direction);
        }
    }

    // Funkcja emulacji ruchu samochodu

    void EmulateCarMovement(float horizontalInput)
    {
        if (carController != null)
        {
            carController.Turn(horizontalInput);
        }
    }
}