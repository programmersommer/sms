using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// добавим
using System.IO.Ports;


namespace sendSMS
{
    class Program
    {

        static SerialPort port;

        static void Main(string[] args)
        {
            port = new SerialPort();

            Console.WriteLine("Отправка сообщения СМС");

            OpenPort();
            bool result;
            result = sendSMS("Hello phone!", "+375123456789");

            if (result == true)
            {
                Console.WriteLine("Сообщение отправлено успешно");
            }
            else
            {
                Console.WriteLine("Произошла ошибка при отправке");
            }
            Console.ReadLine();

            port.Close();

        }

        private static bool sendSMS(string textsms, string telnumber)
        {
            if (!port.IsOpen) return false;

            try
            {
                System.Threading.Thread.Sleep(500);
                port.WriteLine("AT\r\n"); // означает "Внимание!" для модема 
                System.Threading.Thread.Sleep(500);

                port.Write("AT+CMGF=1\r\n"); // устанавливается текстовый режим для отправки сообщений
                System.Threading.Thread.Sleep(500);
            }
            catch
            {
                return false;
            }

            try
            {
                port.Write("AT+CMGS=\"" + telnumber + "\"" + "\r\n"); // задаем номер мобильного телефона получаталя смс
                System.Threading.Thread.Sleep(500);

                port.Write(textsms + char.ConvertFromUtf32(26) + "\r\n"); // отправляем текст смс
                System.Threading.Thread.Sleep(500);
            }
            catch
            {
                return false;
            }

            try
            {
                string recievedData;
                recievedData = port.ReadExisting();

                if (recievedData.Contains("ERROR"))
                {
                    return false;
                }

            }
            catch { }

            return true;
        }

        private static void OpenPort()
        {

            port.BaudRate = 2400; // еще варианты 4800, 9600, 28800 или 56000
            port.DataBits = 7; // еще варианты 8, 9

            port.StopBits = StopBits.One; // еще варианты StopBits.Two StopBits.None или StopBits.OnePointFive         
            port.Parity = Parity.Odd; // еще варианты Parity.Even Parity.Mark Parity.None или Parity.Space

            port.ReadTimeout = 500; // еще варианты 1000, 2500 или 5000 (больше уже не стоит)
            port.WriteTimeout = 500; // еще варианты 1000, 2500 или 5000 (больше уже не стоит)

            //port.Handshake = Handshake.RequestToSend;
            //port.DtrEnable = true;
            //port.RtsEnable = true;
            //port.NewLine = Environment.NewLine;

            port.Encoding = Encoding.GetEncoding("windows-1251");

            port.PortName = "COM5";

            // незамысловатая конструкция для открытия порта
            if (port.IsOpen)
                port.Close();
            try
            {
                port.Open();
            }
            catch { }

        }



    }
}
