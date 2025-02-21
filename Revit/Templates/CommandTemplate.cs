using Abstract.Revit.EventHandlerTemplate;
using System;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace Abstract.Revit.Templates
{

    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]

    public class COMMANDNAME : IExternalCommand
    {
        #region Private Fields
        private Document _doc;
        private Form _form; //форма, которую необходимо открывать
        private Command_EventHandler _Command_eventHandler; //обработчик событий - команда, запускаемая формой
        #endregion

        #region Открываемое окно только поверх родительского
        // Импортируем функции WinAPI
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

            _form = new Form (DATA, Command_eventHandler); //создание экземпляра формы. Нужно передать в нее экземпляр обработчика событий
            _form.Show(); //отображение формы


            // Указываем Revit как родительское окно
            IntPtr formHandle = _form.Handle;
            SetWindowLongPtr(formHandle, GWL_HWNDPARENT, revitHandle);
        }

        //------------------------- Methods -----------------------------------------------------

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                _Command_eventHandler = new Command_EventHandler(); //создаем обработчик событий
                _Command_eventHandler.Initialize(); //инициализируем обработчик событий

                ShowMyForm(commandData.Application);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }

            return Result.Succeeded;
        }
    }
}
