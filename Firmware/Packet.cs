﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hardware;

namespace Firmware
{
    public class Packet
    {
        public byte cmd;
        public int length;
        public ushort checksum;
        public byte[] data;

        public Packet(byte cmd, byte[] data)
        {
            this.cmd = cmd;
            this.data = data;
            this.length = data.Length;
            FindChecksum(cmd, data, length);
        }

        public byte [] GetHeader()
        {
            byte[] header = {cmd, (byte) length, BitConverter.GetBytes(checksum)[0], BitConverter.GetBytes(checksum)[1]};

            return header;
        }

        public void FindChecksum(byte cmd, byte[] data, int length)
        {
            foreach (byte element in data)
            {
                checksum += element;
            }

            checksum += cmd;
            checksum += (byte) length;
        }

        public static Packet LaserOn(bool onOff)
        {
            return new Packet((byte) Cmds.LASER, BitConverter.GetBytes(onOff));
        }

        public static Packet MoveGalvos(double x, double y)
        {
            x = x * 0.025; //converts coordinate to voltage
            y = y * 0.025; //converts coordinate to voltage
            byte xVolt = (byte) x;
            byte yVolt = (byte) y;
            byte[] volts = { xVolt, yVolt };
            return new Packet((byte) Cmds.GALVOS, volts);
        }

        public static Packet MoveZ(double prevZ, double newZ)
        {
            double z = newZ - prevZ;
            byte zVal = (byte)z;
            byte[] zcor = { zVal };
            return new Packet((byte)Cmds.ZCOR, zcor);
        }

        public Packet Reset()
        {
            byte[] top = { (byte)0 };
            return new Packet((byte)Cmds.RESET, top);
        }


        public enum Cmds
        {
            LASER = 0,
            GALVOS = 1,
            ZCOR = 2,
            RESET = 3
        }
    }
}
