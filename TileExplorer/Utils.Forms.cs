using P3tr0viCh.Utils;
using P3tr0viCh.Utils.Extensions;
using P3tr0viCh.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TileExplorer.Interfaces;
using TileExplorer.Properties;

namespace TileExplorer
{
    internal static partial class Utils
    {
        public static class Forms
        {
            public static List<T> GetChildForms<T>(ChildFormType type)
            {
                var forms = new List<T>();

                foreach (var frm in Application.OpenForms)
                {
                    if (frm is IChildForm childFrm && frm is T childFrmT && type.HasFlag(childFrm.FormType))
                    {
                        forms.Add(childFrmT);
                    }
                }

                return forms;
            }

            public static T GetChildForm<T>(ChildFormType type)
            {
                var forms = GetChildForms<T>(type);

                return forms.Count == 1 ? forms.First() : default;
            }

            public static void ChildFormsListItemsChange(ChildFormType type, IEnumerable<IBaseId> values)
            {
                GetChildForms<IFrmUpdateDataList>(type).ForEach(frm => frm.ListItemsChange(values));
            }

            public static void ChildFormsListItemsDelete(ChildFormType type, IEnumerable<IBaseId> values)
            {
                GetChildForms<IFrmUpdateDataList>(type).ForEach(frm => frm.ListItemsDelete(values));
            }

            public static T FindForm<T>(ChildFormType formType, Func<T, bool> predicate) where T : Form
            {
                return GetChildForms<T>(formType).Where(predicate).FirstOrDefault();
            }

            public static void TextBoxWrongValue(TextBox textBox, string error)
            {
                textBox.Focus();
                textBox.SelectAll();

                Msg.Error(error);
            }

            public static bool TextBoxIsWrongValue(Func<bool> check, TextBox textBox, string error)
            {
                if (check())
                {
                    TextBoxWrongValue(textBox, error);

                    return true;
                }

                return false;
            }

            public static bool TextBoxIsWrongValue(Func<bool> check, TextBox textBox, string error, object arg0)
            {
                return TextBoxIsWrongValue(check, textBox, string.Format(error, arg0));
            }

            public static bool TextBoxIsEmpty(TextBox textBox, string error)
            {
                return TextBoxIsWrongValue(() => textBox.IsEmpty(), textBox, error);
            }

            public static bool TextBoxIsWrongFloat(TextBox textBox)
            {
                if (textBox.IsEmpty())
                {
                    return true;
                }

                if (Misc.FloatCheck(textBox.Text))
                {
                    return true;
                }

                TextBoxWrongValue(textBox, Resources.ErrorNeedDigit);

                return false;
            }
        }
    }
}