using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BatteryMonitorBLE
{
    
    class BluetoothModeule
    {
           
        private static int _chargeCounter = 0;
        private static string date = DateTime.Now.ToString();

        private string[] fileContent = { "Battery Report : ",
                                           "-----------------",
                                           "date : " +date,
                                           "Author: Mohammed hemed",
                                           "Number of cahrges = "+_chargeCounter

        };

        private int chargePercentage = 0;
        const int CHARGE_THRESHOLD = 80;
        // path of the file 
        private string _fileName = @"D:\Embedded\Work\BatteryMonitorBLE\batteryReport.txt";

        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        static int parseInteger(int line , string fileName)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            int extractedNumber = int.Parse(arrLine[line]);
            return extractedNumber;
        }
       
        SerialPort serialPort = new SerialPort();
        // constructor : 
        public enum STATE { AC_LINE_NOT_CONNECTED, CHARGING, STABLE }

        public BluetoothModeule(string portName, int baudRate)
        {
            // create object of serial port : 
            serialPort.BaudRate = baudRate;
            serialPort.PortName = portName; // Set in Windows
            serialPort.Open();
        }

           
        public void updateBatteryReport()
        {
            var chargePercentage = 0;

            _chargeCounter = parseInteger(4, _fileName);
            if (chargePercentage == CHARGE_THRESHOLD) 

            lineChanger("Number of cahrges = " +  ++_chargeCounter, _fileName, 5);

        }
        public void createBatteryReport()
        {
            //create log file if not exist
            string fileName = @"D:\Embedded\Work\BatteryMonitorBLE\batteryReport.txt";
            //string fileName = "\\D:\\Embedded\\Work\\BatteryMonitorBLE\\" + "Log -" + DateTime.Now.ToString("dd-MM-yyyy - HH.mm.ss tt") + ".txt";

            var logFile = new StreamWriter($"{fileName}", true);
            logFile.WriteLine("DATE : " + DateTime.Now.ToString("dd-MM-yyyy - HH.mm.ss tt"));
            try
            {
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        for (byte i = 0; i < 5; i++)
                        {
                            sw.WriteLine(fileContent[i]);    
                        }
                    }
                
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

        }

        
        public void sendCommand(string command ,int timeInSecond)
        {

            
            // check if bluetooth module port is open :
            if (serialPort.IsOpen)
            {
                
                serialPort.Write(command);
                Thread.Sleep(timeInSecond);
            }
            
        }

        public void ReceiveData(ref string dataReceived )
        {
                          
                if (serialPort.IsOpen)
                {   
                    if (serialPort.BytesToRead > 0)
                       dataReceived = serialPort.ReadLine();
                    
                }
          
        }
        

    }
}
