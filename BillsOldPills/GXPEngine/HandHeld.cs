using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class HandHeld
    {
        private SerialPort hhPort;
        private char[] buffer = new char[100];
        private int offset = 0;
        private MovingAverage movingAverage = null;

        public float Tilt { get; set; }
        public char Direction { get; set; }
        public Boolean Button { get; set; }

        public Boolean Connected { get; set; }
        //------------------------------------------------------------------------------------------------------------
        //                                                  Constructor
        //------------------------------------------------------------------------------------------------------------
        public HandHeld()
        {
            try
            {
                hhPort = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
                hhPort.DtrEnable = true;
                hhPort.Encoding = Encoding.GetEncoding(28591);
                hhPort.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
                hhPort.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceived);
                hhPort.Open();
                movingAverage = new MovingAverage(8);
                Connected = true;
            }
            catch
            {
                Connected = false;
            }

        }
        //------------------------------------------------------------------------------------------------------------
        //                                                  ErrorReceived
        //------------------------------------------------------------------------------------------------------------
        private void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //------------------------------------------------------------------------------------------------------------
        //                                                  DataReceived
        //------------------------------------------------------------------------------------------------------------
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int size = (char)hhPort.Read(buffer, offset, 100 - offset);
                offset += size;
                if (offset > 6)
                {
                    if ((buffer[0] != (char)0xde) || (buffer[1] != (char)0xad)) { offset = 0; }

                    else if (offset >= 8)
                    {
                        //Get tilt from data
                        short tilt = (short)(((short)buffer[2]) << 8);
                        tilt += (short)buffer[3];
                        //Translate to angle
                        if (tilt < 0)
                        {
                            if (tilt > -20) tilt = 0;
                            else if (tilt < -220) tilt = -202;
                            else tilt += 20;
                        }
                        else
                        {
                            if (tilt < 20) tilt = 0;
                            else if (tilt > 220) tilt = 202;
                            else tilt -= 20;
                        }

                        //Tilt = ((float)tilt / 200) * 90;
                        movingAverage.ComputeAverage(((float)tilt / (float)200) * -90);
                        Tilt = movingAverage.Average;

                        //Get direction
                        short direction = (short)(((short)buffer[4]) << 8);
                        direction += (short)buffer[5];
                        //Translate to L l r R
                        if (direction > 973) Direction = 'L';
                        else if (direction > 518) Direction = 'l';
                        else if (direction > 510) Direction = 'n';
                        else if (direction > 50) Direction = 'r';
                        else Direction = 'R';

                        //Get button state
                        Button = (buffer[6] == 1 ? true : false);

                        //Reset buffer
                        offset = 0;
                    }
                }
            }
            catch (TimeoutException) { }
        }
    }
}
