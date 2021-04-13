using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using BatteryMonitorBLE ;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class BateryMonitorBLE

{
    public static void GetBatteryStatus(ref double[] status)
    {
        PowerStatus pw = SystemInformation.PowerStatus;
        
        status = new double[5];
        status[0] = pw.BatteryLifePercent * 100;
        status[1] = (double)pw.PowerLineStatus;
        status[2] = pw.BatteryFullLifetime;
        status[3] = (double)pw.BatteryChargeStatus;
        status[4] = pw.BatteryLifeRemaining;
    }
    /// <summary>
    /// 
    /// </summary>
    public static void Main()
    {

        // string str = "123456";
        /*
        var regex = new Regex(@"^-?[0-9][0-9,\.]+$");
        Console.WriteLine(Regex.Match("anything 876.8 anything", @"\d+\.*\d*").Value);
        */

        /*
        
        Console.WriteLine("data received = " + data);
        */
        double [] result;
        result = new double [5];
        GetBatteryStatus(ref result);

        
        // Establish connection to HC-06
        BluetoothModeule hc06 = new BluetoothModeule("COM7", 9600);
        //string data = "hi ";
       
        
        
        if (result[0]< 80) hc06.sendCommand("1", 1000);
        else if (result[0] >= 80) hc06.sendCommand("0", 1000);
        if (result[1] == 1)
        {
            MessageBox.Show("Charging Now ... ", "Smart Charging Status");
            hc06.createBatteryReport();
            //hc06.updateBatteryReport();
        }
        else MessageBox.Show("Please Connect Charger Now ..", "Smart Charging Status");
        


    }
    

}


/*
        STATE state = STATE.IDLE;

        switch(state)
        {
            case STATE.IDLE:
                if (result[0] < 80) state = STATE.CHARGING;
                else
                {
                    state = STATE.STABLE;
                    hc06.sendCommand("0",1000);
                }
                break;

            case STATE.AC_LINE_NOT_CONNECTED:
                //             ( msg , title)                            
                MessageBox.Show("Please Connect Charger Now ..", "Smart Charger Monitor");
                hc06.sendCommand("1", 1000);
                if (result[1] == 1) state = STATE.STABLE;
                else state = STATE.AC_LINE_NOT_CONNECTED;
                break;

            case STATE.CHARGING:
                if (result[0] >= 80) hc06.sendCommand("0", 1000);
                else hc06.sendCommand("1", 1000);
                if (result[1] == 1) state = STATE.STABLE;
                else state = STATE.AC_LINE_NOT_CONNECTED;
                break;

            case STATE.STABLE:
                //hc06.createBatteryReport();
                //hc06.updateBatteryReport();
                if ((result[0] > 30) && (result[0] <= 80)) 
                    state = STATE.STABLE;
                else state = STATE.CHARGING;
                break;
            default:break;
        }
*/
