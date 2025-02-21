using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Abstract.Revit.Templates
{
    public class Application : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                   iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";

            string tabName = "TABMANE";
            string panelName = "PANELNAME";
            string ribbonName = "BUTTONNAME";


            try
            {
                application.CreateRibbonTab(tabName); 
            }
            catch { }

            #region 1. SPECIFIC
            {
                RibbonPanel panel = application.GetRibbonPanels(tabName).Where(p => p.Name == panelName).FirstOrDefault();
                if (panel == null)
                {
                    panel = application.CreateRibbonPanel(tabName, panelName);
                }

                panel.AddItem(new PushButtonData(nameof(COMMANDNAME), ribbonName, assemblyLocation, typeof(COMMANDNAME).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "IMAGENAME.png"))
                });
            }
            #endregion


            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
