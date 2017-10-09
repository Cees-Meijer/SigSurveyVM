using System;
using System.IO;

namespace AD2CPData
{
    public class AD2CP_Header
    {
        public byte Sync;
        public byte HeaderSize;
        public byte ID;
        public byte Family;
        public UInt16 DataSize;
        public UInt16 DataChecksum;
        public UInt16 HeaderChecksum;

        public void Read(BinaryReader b)
        {
            Sync = b.ReadByte();
            HeaderSize = b.ReadByte();
            ID = b.ReadByte();
            Family = b.ReadByte();
            DataSize = b.ReadUInt16();
            DataChecksum = b.ReadUInt16();
            HeaderChecksum = b.ReadUInt16();
        }
    }

    /// <summary>
    /// AD2CP Bottomtrack data, ID = 0x17
    /// </summary>
    public class AD2CP_BottomTrack
    {
        public const int sizeOf = 78;
        public bool throwSizeError = true;
        public byte version;
        public byte offsetOfData;
        public UInt16 headconfig;
        public UInt32 serialNumber;
        public byte year;
        public byte month;
        public byte day;
        public byte hour;
        public byte minute;
        public byte seconds;
        public UInt16 microSeconds100;
        public UInt16 soundSpeed;
        public Int16 temperature;
        public UInt32 pressure;
        public UInt16 heading;
        public Int16 pitch;
        public Int16 roll;
        public UInt16 beams_cy_cells;
        public UInt16 cellSize;
        public UInt16 blanking;
        public UInt16 velocityRange;
        public UInt16 battery;
        public Int16[] magnHxHyHz;
        public Int16[] accl3D;
        public UInt32 ambiguityVelocity;
        public UInt16 DataSetDescription;
        public UInt16 transmitEnergy;
        public sbyte velocityScaling;
        public sbyte powerLevel;
        public Int16 magnTemperature;
        public Int16 rtcTemperature;
        public UInt32 error;
        public UInt32 status;
        public UInt32 ensembleCounter;
        public int dataCells;
        public int dataCoord;
        public int dataBeams;
        public bool pressureSensor;
        public bool temperatureSensor;
        public bool compass;
        public bool tiltSensor;
        public bool velPresent;  // Velocity
        public bool distPresent; // Distance
        public bool fomPresent;     // Figure of Merit

        public int[] velocityData;
        public int[] distanceData;
        public int[] fomData;       // Figure of Merit

        public AD2CP_BottomTrack()
        {
            magnHxHyHz = new Int16[3];
            accl3D = new Int16[3];
        }

        public void Read(BinaryReader b)
        {
            version = b.ReadByte();
            offsetOfData = b.ReadByte();
            headconfig = b.ReadUInt16();
            serialNumber = b.ReadUInt32();
            year = b.ReadByte();
            month = b.ReadByte();
            day = b.ReadByte();
            hour = b.ReadByte();
            minute = b.ReadByte();
            seconds = b.ReadByte();
            microSeconds100 = b.ReadUInt16();
            soundSpeed = b.ReadUInt16();
            temperature = b.ReadInt16();
            pressure = b.ReadUInt32();
            heading = b.ReadUInt16();
            pitch = b.ReadInt16();
            roll = b.ReadInt16();
            beams_cy_cells = b.ReadUInt16();
            cellSize = b.ReadUInt16();
            blanking = b.ReadUInt16();
            velocityRange = b.ReadUInt16();
            battery = b.ReadUInt16();
            for (int i = 0; i < 3; i++)
            {
                magnHxHyHz[i] = b.ReadInt16();
            }
            for (int i = 0; i < 3; i++)
            {
                accl3D[i] = b.ReadInt16();
            }
            ambiguityVelocity = b.ReadUInt32();
            DataSetDescription = b.ReadUInt16();
            transmitEnergy = b.ReadUInt16();
            velocityScaling = b.ReadSByte();
            powerLevel = b.ReadSByte();
            magnTemperature = b.ReadInt16();
            rtcTemperature = b.ReadInt16();
            error = b.ReadUInt32();
            status = b.ReadUInt32();
            ensembleCounter = b.ReadUInt32();

            dataCells = beams_cy_cells & 0x3FF;
            dataCoord = (beams_cy_cells & 0xC00) >> 10;
            dataBeams = (beams_cy_cells & 0xF000) >> 12;
            pressureSensor = (headconfig & 0x1) != 0;
            temperatureSensor = (headconfig & 0x2) != 0;
            compass = (headconfig & 0x4) != 0;
            tiltSensor = (headconfig & 0x8) != 0;
            velPresent = (headconfig & 0x20) != 0;
            distPresent = (headconfig & 0x100) != 0;
            fomPresent = (headconfig & 0x200) != 0;

            if (velPresent)
            {
                    for (int i = 0; i < dataBeams; i++)
                    {
                        velocityData[i] = b.ReadInt32();
                    }
            }

            if (distPresent)
            {
                for (int i = 0; i < dataBeams; i++)
                {
                    distanceData[i] = b.ReadInt32();
                }
            }

            if (fomPresent)
            {
                for (int i = 0; i < dataBeams; i++)
                {
                    fomData[i] = b.ReadInt16();
                }
            }

        }



        private void Allocate(int beams)
            {
                if ((velocityData == null) || (velocityData.Length != beams))
                {
                    velocityData = new int[beams];
                    distanceData = new int[beams];
                    fomData = new int[beams];
                }

            }
        }

    /// <summary>
    /// AD2CP Burst / Average data, ID = 0x15
    /// </summary>
    public class AD2CP_DataFormat3
    {
        public const int sizeOf = 76;
        public bool throwSizeError = true;
        public byte version;
        public byte offsetOfData;
        public UInt16 headconfig;
        public UInt32 serialNumber;
        public byte year;
        public byte month;
        public byte day;
        public byte hour;
        public byte minute;
        public byte seconds;
        public UInt16 microSeconds100;
        public UInt16 soundSpeed;
        public Int16 temperature;
        public UInt32 pressure;
        public UInt16 heading;
        public Int16 pitch;
        public Int16 roll;
        public UInt16 beams_cy_cells;
        public UInt16 cellSize;
        public UInt16 blanking;
        public byte nominalCorrelation;
        public byte pressTemp;
        public UInt16 battery;
        public Int16[] magnHxHyHz;
        public Int16[] accl3D;
        public UInt16 ambVelocity;
        public UInt16 DataSetDescription;
        public UInt16 transmitEnergy;
        public sbyte velocityScaling;
        public sbyte powerLevel;
        public Int16 magnTemperature;
        public Int16 rtcTemperature;
        public UInt32 error;
        public UInt32 status;
        public UInt32 ensembleCounter;
        public short[][] velocityData;
        public short[][] amplitudeData;
        public short[][] correlationData;
        public int dataCells;
        public int dataCoord;
        public int dataBeams;
        public bool pressureSensor;
        public bool temperatureSensor;
        public bool compass;
        public bool tiltSensor;
        public bool velPresent;
        public bool ampPresent;
        public bool corrPresent;

        public AD2CP_DataFormat3()
        {
            magnHxHyHz = new Int16[3];
            accl3D = new Int16[3];
        }

        public void Read(BinaryReader b)
        {
            version = b.ReadByte();
            offsetOfData = b.ReadByte();
            headconfig = b.ReadUInt16();
            serialNumber = b.ReadUInt32();
            year = b.ReadByte();
            month = b.ReadByte();
            day = b.ReadByte();
            hour = b.ReadByte();
            minute = b.ReadByte();
            seconds = b.ReadByte();
            microSeconds100 = b.ReadUInt16();
            soundSpeed = b.ReadUInt16();
            temperature = b.ReadInt16();
            pressure = b.ReadUInt32();
            heading = b.ReadUInt16();
            pitch = b.ReadInt16();
            roll = b.ReadInt16();
            beams_cy_cells = b.ReadUInt16();
            cellSize = b.ReadUInt16();
            blanking = b.ReadUInt16();
            nominalCorrelation = b.ReadByte();
            pressTemp = b.ReadByte();
            battery = b.ReadUInt16();
            magnHxHyHz[0] = b.ReadInt16();
            magnHxHyHz[1] = b.ReadInt16();
            magnHxHyHz[2] = b.ReadInt16();
            accl3D[0] = b.ReadInt16();
            accl3D[1] = b.ReadInt16();
            accl3D[2] = b.ReadInt16();
            ambVelocity = b.ReadUInt16();
            DataSetDescription = b.ReadUInt16();
            transmitEnergy = b.ReadUInt16();
            velocityScaling = b.ReadSByte();
            powerLevel = b.ReadSByte();
            magnTemperature = b.ReadInt16();
            rtcTemperature = b.ReadInt16();
            error = b.ReadUInt32();
            status = b.ReadUInt32();
            ensembleCounter = b.ReadUInt32();
            dataCells = beams_cy_cells & 0x3FF;
            dataCoord = (beams_cy_cells & 0xC00) >> 10;
            dataBeams = (beams_cy_cells & 0xF000) >> 12;
            Allocate((ushort)dataBeams, (ushort)dataCells);
            pressureSensor = (headconfig & 0x1) != 0;
            temperatureSensor = (headconfig & 0x2) != 0;
            compass = (headconfig & 0x4) != 0;
            tiltSensor = (headconfig & 0x8) != 0;
            velPresent = (headconfig & 0x20) != 0;
            ampPresent = (headconfig & 0x40) != 0;
            corrPresent = (headconfig & 0x80) != 0;
            if (velPresent)
            {
                for (int c = 0; c < dataBeams; c++)
                {
                    for (int i = 0; i < dataCells; i++)
                    {
                        velocityData[c][i] = b.ReadInt16();
                    }
                }
            }

            if (ampPresent)
            {
                for (int c = 0; c < dataBeams; c++)
                {
                    for (int i = 0; i < dataCells; i++)
                    {
                        amplitudeData[c][i] = b.ReadByte();
                    }
                }
            }

            if (corrPresent)
            {
                for (int c = 0; c < dataBeams; c++)
                {
                    for (int i = 0; i < dataCells; i++)
                    {
                        correlationData[c][i] = b.ReadByte();
                    }
                }
            }

        }
        /// Allocate internal memory for the given number of beams / bins
        /// </summary>
        /// <param name="beams">Number of expected beams in the data</param>
        /// <param name="cells">Number of expected cells int the data</param>
        private void Allocate(ushort beams, ushort cells)
        {
            if ((velocityData == null) || (velocityData.Length != beams)
                    || (velocityData[0].Length != cells))
            {
                velocityData = new short[beams][];
                amplitudeData = new short[beams][];
                correlationData = new short[beams][];

                for (int i = 0; i < beams; i++)
                {
                    velocityData[i] = new short[cells];
                    amplitudeData[i] = new short[cells];
                    correlationData[i] = new short[cells];
                }
            }
        }


        /// <summary>
        /// Calculates a wordwise (16-bit) checksum of a byte array.
        /// </summary>
        /// <param name="buff"> The binary input buffer</param>
        /// <param name="n">The number of bytes (not words)</param>
        /// <returns>The checksum value</returns>
        public static UInt16 CheckSum(byte[] buff, int n)
        {
            int i;
            int check_sum = 0xb58c;
            int b = 0;
            for (i = 0, b = 0; i < n / 2; i++, b += 2)
            {
                check_sum += (((int)buff[b] & 0xFF) + ((int)(buff[b + 1] & 0xFF) << 8));
            }
            /* Odd number of bytes */
            if ((n & 1) == 1)
            {
                check_sum += (int)(buff[n - 1] << 8);
            }

            return ((UInt16)(check_sum & 0xFFFF));
        }


        /// <summary>
        /// Calculates the wordwise (16-bit) checksum of a byte array starting at byte "offset".
        /// </summary>
        /// <param name="buff">The binary input buffer</param>
        /// <param name="offset">Position in buffer from which to start checksumming</param>
        /// <param name="n">The number of bytes to process</param>
        /// <returns>The checksum value</returns>
        public static UInt16 CheckSum(byte[] buff, int offset, int n)
        {
            int i;
            int check_sum = (int)0xb58c;
            int b = 0;
            for (i = 0, b = 0; i < n / 2; i++, b += 2)
            {
                check_sum += (((int)buff[b + offset] & 0xFF) + ((int)(buff[b
                        + 1 + offset] & 0xFF) << 8));
            }
            if ((n & 1) == 1)
            {
                check_sum += (int)(buff[n - 1 + offset] << 8);
            }

            return ((UInt16)(check_sum & 0xFFFF));
        }
    }

}
