using QuanLyKho.Models.Entities;
using System.Globalization;

namespace QuanLyKho.Extensions
{
    public static class Helpers
    {
        public static String GetFirstLetterInString(String str)
        {
            string newStr = "";

            // split the string by spaces
            string[] words = str.Split(' ');

            // loop through each word and get the first character
            foreach (string word in words)
            {
                newStr += word[0];
            }

            return newStr;
        }

        public static int GetLastTwoDigitsOfYear(string yearOfBirth)
        {
            int year = int.Parse(yearOfBirth);
            int lastTwoDigits = year % 100;

            return lastTwoDigits;
        }

        public static int GetRandomTwoDigitNumber()
        {
            Random random = new Random();
            int number = random.Next(10, 100);
            return number;
        }

        public static int GetRandomTwoDigitNumberMinusGivenNumber(int givenNumber)
        {
            Random random = new Random();
            int number = random.Next(10, 100);
            while (number == givenNumber) // Đảm bảo số ngẫu nhiên khác số cho trước
            {
                number = random.Next(10, 100);
            }
            int result = number - givenNumber;
            return result;
        }

        public static string PriceToVND(double price)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            string totalString = price.ToString("#,###", cul.NumberFormat);

            return totalString;
        }

        public static string ToCustomDateTimeStr(this DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy - HH\\hmm");
        }

        public static Status ChangeStatus(this Status status)
        {
            if(status is Status.Hide)
                return Status.Show;
            else
                return Status.Hide;
        }
    }
}
