using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{
    //Cuanto tiempo real equivale a un dia en juego, 3 minutos equivale a una hora
    private const float REAL_SECONDS_PER_INGAME_DAY = 4320f;
    //Manecillas del reloj
    private Transform hourHandTransform;
    private Transform minuteHandTransform;
    //Valor entre 0 y 1, representa el avance del día(0=00:00,1=24:00)
    private float day;
    private TextMeshProUGUI timeText;
    private void Awake()
    {
        hourHandTransform = transform.Find("HourHand");
        minuteHandTransform = transform.Find("MinuteHand");
        timeText = transform.Find("TimeText").GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        //Aumenta el progreso del día usando el tiempo real transcurrido
        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;
        float dayNormalized = day % 1f;

        float rotationDegreesPerDay = 360f;

        //Rotación de la manecilla de la hora
        hourHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized*rotationDegreesPerDay);

        float hourPerDay = 24f;
        //Rotacion de la manecilla del minuto
        minuteHandTransform.eulerAngles=new Vector3(0, 0, -dayNormalized*rotationDegreesPerDay*hourPerDay);

        //Calculo para la hora del texto
        float minutesPerHour = 60f;
        string hourString = Mathf.Floor(dayNormalized * hourPerDay).ToString("00");
        string minuteString = Mathf.Floor(((dayNormalized * hourPerDay) % 1f) * minutesPerHour).ToString("00");

        timeText.text = hourString+":"+minuteString;
    }
}
