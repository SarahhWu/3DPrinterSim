﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hardware;

namespace Firmware
{

    public class FirmwareController
    {
        PrinterControl printer;
        bool fDone = false;
        bool fInitialized = false;

        public FirmwareController(PrinterControl printer)
        {
            this.printer = printer;
        }

        // Handle incoming commands from the serial link
        void Process()
        {
            // Todo - receive incoming commands from the serial link and act on those commands by calling the low-level hardwarwe APIs, etc.
            while (!fDone)
            {
            }
        }

        public void Start()
        {
            fInitialized = true;

            Process(); // this is a blocking call
        }

        public void Stop()
        {
            fDone = true;
        }

        public void WaitForInit()
        {
            while (!fInitialized)
                Thread.Sleep(100);
        }

        //Since the build plate starts at a random point every time, this function moves it all the way up until
        //the limit switch is pressed. It then steps down 39780 (calculated from datasheet) which will put it at the home position.
        public void setBuildPlateHome()
        {
            while (printer.LimitSwitchPressed() != true)
            {
                printer.StepStepper(PrinterControl.StepperDir.STEP_UP);
            }
            Console.WriteLine("Build plate is at top and will now move to bottom");

            for (int i = 0; i < 39780; i++)
            {
                printer.StepStepper(PrinterControl.StepperDir.STEP_DOWN);
            }
        }

        public void turnLaserOn()
        {
            printer.SetLaser(true);
        }

        public void turnLaserOff()
        {
            printer.SetLaser(false);
        }
    }
}
