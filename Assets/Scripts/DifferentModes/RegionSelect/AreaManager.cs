using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class AreaManager : MonoBehaviour
{
    [Header("UI References")]
    public TMPro.TMP_InputField verticalInput;
    public TMPro.TMP_InputField horizontalInput;

    [Header("Scene References")]
    public GameObject fencePrefab;       // Префаб сегмента забора (длиной 1 единица)
    public Transform fenceParent;        // Родительский объект для всех сегментов
    public Camera mainCamera;

    private const int MULTIPLIER = 3;

    private List<GameObject> spawnedFences = new List<GameObject>();

    private void Start()
    {
        SetupValidation();
        UpdateArea();
    }

    private void SetupValidation()
    {
        if (verticalInput == null || horizontalInput == null)
            Debug.LogError("Не назначены UI-компоненты!");

        if (fencePrefab == null || fenceParent == null || mainCamera == null)
            Debug.LogError("Не назначены сценные объекты!");
    }

    public void UpdateArea()
    {
        if (!ValidateInput(out int width, out int depth)) return;

        ClearExistingFences();
        SpawnNewFence(width * MULTIPLIER, depth * MULTIPLIER);
        AdjustCamera(width * MULTIPLIER, depth * MULTIPLIER);
    }

    private bool ValidateInput(out int width, out int depth)
    {
        width = depth = 0;

        if (!float.TryParse(verticalInput.text, out float depthVal) ||
            !float.TryParse(horizontalInput.text, out float widthVal))
        {
            Debug.LogError("Ошибка ввода: введите числовые значения");
            return false;
        }

        // Округление до целых с ограничением минимума
        depth = Mathf.Max(1, Mathf.RoundToInt(depthVal));
        width = Mathf.Max(1, Mathf.RoundToInt(widthVal));

        // Обновляем поля ввода округленными значениями
        verticalInput.text = depth.ToString();
        horizontalInput.text = width.ToString();

        return true;
    }

    private void ClearExistingFences()
    {
        foreach (GameObject fence in spawnedFences)
        {
            Destroy(fence);
        }
        spawnedFences.Clear();
    }

    private void SpawnNewFence(int width, int depth)
    {
        Vector3 parentPosition = fenceParent.position;

        // Верхняя стена (Z+)
        for (int i = 0; i < width; i++)
        {
            float localX = -width / 2.0f + 0.5f + i;
            Vector3 localPos = new Vector3(localX, 0, depth / 2.0f);
            SpawnFenceSegment(localPos, Quaternion.identity);
        }

        // Нижняя стена (Z-)
        for (int i = 0; i < width; i++)
        {
            float localX = -width / 2.0f + 0.5f + i;
            Vector3 localPos = new Vector3(localX, 0, -depth / 2.0f);
            SpawnFenceSegment(localPos, Quaternion.identity);
        }

        // Левая стена (X-)
        for (int i = 0; i < depth; i++)
        {
            float localZ = -depth / 2.0f + 0.5f + i;
            Vector3 localPos = new Vector3(-width / 2.0f, 0, localZ);
            SpawnFenceSegment(localPos, Quaternion.Euler(0, 90, 0));
        }

        // Правая стена (X+)
        for (int i = 0; i < depth; i++)
        {
            float localZ = -depth / 2.0f + 0.5f + i;
            Vector3 localPos = new Vector3(width / 2.0f, 0, localZ);
            SpawnFenceSegment(localPos, Quaternion.Euler(0, 90, 0));
        }
    }

    private void SpawnFenceSegment(Vector3 localPosition, Quaternion rotation)
    {
        // Создаем объект в локальной позиции родителя
        GameObject segment = Instantiate(fencePrefab, fenceParent);
        segment.transform.localPosition = localPosition;
        segment.transform.localRotation = rotation;
        spawnedFences.Add(segment);
    }

    private void AdjustCamera(int width, int depth)
    {
        if (!mainCamera.orthographic)
        {
            Debug.LogWarning("Рекомендуется использовать ортографическую камеру");
            return;
        }

        // Рассчитываем размер камеры с отступом 20%
        float aspect = (float)Screen.width / Screen.height;
        float sizeBasedOnWidth = (width * 0.6f) / aspect; // 0.6 = 1.2 / 2 (20% отступ)
        float sizeBasedOnHeight = depth * 0.6f;

        mainCamera.orthographicSize = Mathf.Max(sizeBasedOnWidth, sizeBasedOnHeight);

        // Центрируем камеру на позиции родителя
        Vector3 cameraPosition = fenceParent.position;
        cameraPosition.y = mainCamera.transform.position.y; // Сохраняем высоту камеры
        mainCamera.transform.position = cameraPosition;
    }
}