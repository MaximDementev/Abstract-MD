#region Открываемое окно только поверх родительского

// Импортируем функции WinAPI
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using System;

[DllImport("user32.dll")]
private static extern IntPtr GetActiveWindow();

[DllImport("user32.dll", SetLastError = true)]
private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

[DllImport("user32.dll", SetLastError = true)]
private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

private const int GWL_HWNDPARENT = -8;

public void ShowMyForm(UIApplication uiapp)
{
    // Получаем дескриптор окна Revit
    IntPtr revitHandle = GetActiveWindow();

    _newForm = new NewForm();
    _newForm.Show();


    // Указываем Revit как родительское окно
    IntPtr formHandle = _newDoorTypeForm.Handle;
    SetWindowLongPtr(formHandle, GWL_HWNDPARENT, revitHandle);
}

#endregion