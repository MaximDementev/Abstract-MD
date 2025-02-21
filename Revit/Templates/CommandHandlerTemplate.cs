namespace Abstract.Revit.EventHandlerTemplate
{
    public class Command_EventHandler : IExternalEventHandler
    {
        #region Private Fields
        private Document doc;
        private UIDocument uidoc;

        private ExternalEvent _externalEvent;
        #endregion

        #region Constructor
        public void Initialize()
        {
            _externalEvent = ExternalEvent.Create(this);
        }

        public void Raise(DATA) //этот метод должен запускаться в форме. Он может принимать любые данные
        {
            this.Data = DATA;
            _externalEvent.Raise(); //этот метод запускает выполнение операции
        }
        #endregion

        //------------------ Methods ----------------------------------


        public void Execute(UIApplication app) //выполнение операции
        {
            doc = app.ActiveUIDocument.Document;
            uidoc = app.ActiveUIDocument as UIDocument;            

            //ЛОГИКА ОПЕРАЦИИ
        }

        public string GetName()
        {
            return "CommantName";
        }

    }
}
