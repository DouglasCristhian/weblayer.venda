using Android.App;
using Android.OS;
using System;

namespace weblayer.venda.android.exp.Helpers
{
    public class DatePickerHelper : DialogFragment, DatePickerDialog.IOnDateSetListener
    {
        public static readonly string TAG = "X:" + typeof(DatePickerHelper).Name.ToUpper();
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerHelper NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerHelper frag = new DatePickerHelper();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
            return dialog;
        }

        public void OnDateSet(Android.Widget.DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            _dateSelectedHandler(selectedDate);
        }

        //public static DateTime DataCerta()
        //{
        //    DateTime currently = DateTime.Now;
        //    DateTime selectedDate = new DateTime(currently.Year, currently.Month, currently.Day);
        //    //frag._dateSelectedHandler = onDateSelected;
        //    return selectedDate;
        //}
    }
}