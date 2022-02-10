using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;

namespace Zapis_parametra
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            IList<Reference> selectedElementRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Выберите элемент");
            var wallList = new List<Element>();
            foreach (var selectedElement in selectedElementRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Wall)
                {
                    Wall oWall = (Wall)element;
                    wallList.Add(oWall);
                }
            }
            foreach (Wall wall in wallList)
            {
                Parameter dlinaParametr = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                double znachenieParametra = (dlinaParametr.AsDouble() * 1.1);
                using (Transaction ts = new Transaction(doc, "Set parameter"))
                {
                    ts.Start();
                    Parameter typeCommentParameter = wall.LookupParameter("Длина с запасом");
                    typeCommentParameter.Set(znachenieParametra);
                    ts.Commit();
                }
            }
            return Result.Succeeded;
        }
    }
}
