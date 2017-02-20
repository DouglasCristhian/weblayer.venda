using Android.Text;
using Android.Widget;
using Java.Lang;
using Java.Text;
using Java.Util;
using System.Text.RegularExpressions;

namespace weblayer.venda.android.exp.Helpers
{
    public class CurrencyConverterHelper : Java.Lang.Object, ITextWatcher
    {
        private EditText editText;
        private string lastAmount = "";
        private int lastCursorPosition = -1;

        public CurrencyConverterHelper(EditText txt)
        {
            this.editText = txt;
        }

        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
            string value = s.ToString();
            if (!value.Equals(""))
            {
                string cleanString = clearCurrencyToNumber(value);
                string formattedAmount = transformtocurrency(cleanString);
                lastAmount = formattedAmount;
                lastCursorPosition = editText.SelectionStart;
            }
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (!s.ToString().Equals(lastAmount))
            {
                string cleanString = clearCurrencyToNumber(s.ToString());
                try
                {
                    string formattedAmount = transformtocurrency(cleanString);
                    editText.RemoveTextChangedListener(this);
                    editText.Text = formattedAmount;
                    editText.SetSelection(formattedAmount.Length);
                    editText.AddTextChangedListener(this);

                    if (lastCursorPosition != lastAmount.Length && lastCursorPosition != -1)
                    {
                        int lengthDelta = formattedAmount.Length - lastAmount.Length;
                        int newCursorOffset = Java.Lang.Math.Max(0, Java.Lang.Math.Min(formattedAmount.Length, lastCursorPosition + lengthDelta));
                        editText.SetSelection(newCursorOffset);
                    }
                }
                catch (System.Exception e)
                {

                }
            }
            else if (s.ToString() == "")
            {
                return;
            }
        }

        public static string clearCurrencyToNumber(string currencyValue)
        {
            string result = "";

            if (currencyValue == null)
            {
                result = "";
            }
            else
            {
                result = Regex.Replace(currencyValue, "[^0-9]", "");
            }
            return result;
        }

        public static string transformtocurrency(string value)
        {
            double parsed = double.Parse(value);
            string formatted = NumberFormat.GetCurrencyInstance(new Locale("pt", "br")).Format((parsed / 100));
            formatted = Regex.Replace(formatted, "[^(0-9)(.,)]", "");
            return formatted;
        }

        public static bool isCurrencyValue(string currencyValue, bool podeSerZero)
        {
            bool result;

            if (currencyValue == null || currencyValue.Length == 0)
            {
                result = false;
            }
            else
            {
                if (!podeSerZero && currencyValue.Equals("0,00"))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
    }
}