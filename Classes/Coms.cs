using global::SIMULATOR.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;

namespace SIMULATOR.Classes
{
    public class Variable
    {
        public const int VARIABLE_BUFFER_SIZE = 4;
        public const string InputGroup = "Input", OutputGroup = "Output", VariableGroup = "Variable", StateGroup = "State";
        public const string ReadAccess = "Read", WriteAccess = "Write", ReadAndWriteAccess = "ReadWrite";
        private readonly string[] _allowedVariableTypes = new string[] { "1_bit", "8_bit_unsigned", "32_bit_signed", "32_bit_float" };
        public string Name { set; get; }
        public string Description { set; get; }
        private string _variableType;
        public string VariableType
        {
            set
            {
                if (_allowedVariableTypes.Any(x => x == value))
                    _variableType = value;
                else throw new Exception("Tipo inválido");
            }

            get
            {
                return _variableType;
            }
        }
        public int NumberOfBytes
        {
            get
            {
                string numberOfBits;
                int numberOfBytes;
                try
                {
                    numberOfBits = Regex.Match(VariableType, @"^(\d+)_bit").Groups[1].Value;
                    numberOfBytes = int.Parse(numberOfBits) / 8;
                }
                catch
                {
                    throw new Exception("Número de bytes inválido");
                }
                if (numberOfBytes == 0) return 1;
                else return numberOfBytes;
            }
        }
        private string _direction;
        public string Direction
        {
            set
            {
                if (Regex.IsMatch(value, @"^%[IQM]\d+\.[0-7]$"))
                {
                    _direction = value;
                }
            }
            get
            {
                return _direction;
            }
        }
        public char LetterDirection
        {
            get
            {
                string letter;
                try
                {
                    letter = Regex.Match(Direction, @"^%([IQM])(\d+)\.([0-7])$").Groups[1].Value;
                }
                catch
                {
                    throw new Exception("Dirección inválida");
                }
                return letter[0];
            }
        }
        public int ByteDirection
        {
            get
            {
                string bytedirection;
                try
                {
                    bytedirection = Regex.Match(Direction, @"^%([IQM])(\d+)\.([0-7])$").Groups[2].Value;
                }
                catch
                {
                    throw new Exception("Dirección inválida");
                }
                return int.Parse(bytedirection);
            }
        }
        public int BitDirection
        {
            get
            {
                string bitdirection;
                try
                {
                    bitdirection = Regex.Match(Direction, @"^%([IQM])(\d+)\.([0-7])$").Groups[3].Value;
                }
                catch
                {
                    throw new Exception("Dirección inválida");
                }
                return int.Parse(bitdirection);
            }
        }
        public string Group
        {
            set
            {
                // Inputs must have I or M letter directions to be writable
                // Outputs must have Q or M letter directions to be readable
                // Variables and States must have Q or M letter directions to be readable
                if (( value == InputGroup && this.IsInWritingDirection ) || 
                    ( value == OutputGroup && this.IsInReadingDirection ) ||
                    ( value == VariableGroup && this.IsInReadingDirection ) || 
                    ( value == StateGroup && this.IsInReadingDirection ) )
                {
                    _group = value;
                }
                else
                {
                    throw new Exception("Grupo inválido o incoherente con la dirección establecida");
                }
            }
            get
            {
                return _group;
            }
        }
        private string _group;
        private string _access;
        public string Access
        {
            set
            {
                if (value == ReadAccess || value == WriteAccess || value == ReadAndWriteAccess)
                {
                    _access = value;
                }
                else
                {
                    throw new Exception("Tipo de acceso inválido");
                }
            }
            get { return _access; }
        }
        public bool HasReadingAccess
        {
            get
            {
                if (Access == ReadAccess || Access == ReadAndWriteAccess) return true;
                else return false;
            }
        }
        public bool HasWritingAccess
        {
            get
            {
                if (Access == WriteAccess || Access == ReadAndWriteAccess) return true;
                else return false;
            }
        }
        public bool IsInput
        {
            get
            {
                if (Group == InputGroup) return true;
                else return false;
            }
        }
        public bool IsOutput
        {
            get
            {
                if (Group == OutputGroup) return true;
                else return false;
            }
        }
        public bool IsVariable
        {
            get
            {
                if (Group == VariableGroup) return true;
                else return false;
            }
        }
        public bool IsInWritingDirection
        {
            get
            {
                if (this.LetterDirection == 'I' || this.LetterDirection == 'M') return true;
                else return false;
            }
        }
        public bool IsInReadingDirection
        {
            get
            {
                if (this.LetterDirection == 'Q' || this.LetterDirection == 'M') return true;
                else return false;
            }
        }
        public bool IsState
        {
            get
            {
                if (Group == StateGroup) return true;
                else return false;
            }
        }
        public string? StateGroupName { get; set; }
        public virtual dynamic Value { get; set; }
        public Variable(string name, string description, string variableType,
            string direction, string group, string access, dynamic defaultValue, string? stateGroupName)
        {
            Name = name;
            Description = description;
            _variableType = variableType;
            _direction = direction;
            _group = group;
            _access = access;
            Value = defaultValue;
            StateGroupName = stateGroupName;
        }
    }
    public class BitVariable : Variable // 1-bit
    {
        public BitVariable(string name, string description, string variableType, string direction,
            string group, string access, bool defaultValue, string? stateGroupName)
            : base(name, description, variableType, direction, group, access, defaultValue, stateGroupName)
        {
        }
        public override dynamic Value
        {
            set
            {
                // First we check the variable type, because dynamic variables do not produce compiler errors
                if (value is bool && this.HasWritingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            S7.SetBitAt(ref PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart, BitDirection, value);
                            break;
                        case 'Q':
                            S7.SetBitAt(ref PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart, BitDirection, value);
                            break;
                        case 'M':
                            S7.SetBitAt(ref PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart, BitDirection, value);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            get
            {
                //if(this.HasReadingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            return S7.GetBitAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart, BitDirection);
                        case 'Q':
                            return S7.GetBitAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart, BitDirection);
                        case 'M':
                            return S7.GetBitAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart, BitDirection);
                        default:
                            throw new Exception();
                    }
                }
                //else
                //{
                //    throw new Exception();
                //}
            }
        }
    }
    public class ByteVariable : Variable // 1-bit
    {
        public ByteVariable(string name, string description, string variableType, string direction,
            string group, string access, bool defaultValue, string? stateGroupName)
            : base(name, description, variableType, direction, group, access, defaultValue, stateGroupName)
        {
        }
        public override dynamic Value
        {
            set
            {
                // First we check the variable type, because dynamic variables do not produce compiler errors
                if (value is bool && this.HasWritingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            S7.SetByteAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart, value);
                            break;
                        case 'Q':
                            S7.SetByteAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart, value);
                            break;
                        case 'M':
                            S7.SetByteAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart, value);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            get
            {
                if(this.HasReadingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            return S7.GetByteAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart);
                        case 'Q':
                            return S7.GetByteAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart);
                        case 'M':
                            return S7.GetByteAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart);
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
    public class IntVariable : Variable // 1-bit
    {
        public IntVariable(string name, string description, string variableType, string direction,
            string group, string access, bool defaultValue, string? stateGroupName)
            : base(name, description, variableType, direction, group, access, defaultValue, stateGroupName)
        {
        }
        public override dynamic Value
        {
            set
            {
                // First we check the variable type, because dynamic variables do not produce compiler errors
                if (value is bool && this.HasWritingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            S7.SetSIntAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart, value);
                            break;
                        case 'Q':
                            S7.SetSIntAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart, value);
                            break;
                        case 'M':
                            S7.SetSIntAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart, value);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            get
            {
                if(this.HasReadingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            return S7.GetSIntAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart);
                        case 'Q':
                            return S7.GetSIntAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart);
                        case 'M':
                            return S7.GetSIntAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart);
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
    public class FloatVariable : Variable // 1-bit
    {
        public FloatVariable(string name, string description, string variableType, string direction,
            string group, string access, bool defaultValue, string? stateGroupName)
            : base(name, description, variableType, direction, group, access, defaultValue, stateGroupName)
        {
        }
        public override dynamic Value
        {
            set
            {
                // First we check the variable type, because dynamic variables do not produce compiler errors
                if (value is bool && this.HasWritingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            S7.SetRealAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart, value);
                            break;
                        case 'Q':
                            S7.SetRealAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart, value);
                            break;
                        case 'M':
                            S7.SetRealAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart, value);
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            get
            {
                if(this.HasReadingAccess)
                {
                    switch (LetterDirection)
                    {
                        case 'I':
                            return S7.GetRealAt(PLCMemory.IDirectionBuffer, ByteDirection - PLCMemory.IStart);
                        case 'Q':
                            return S7.GetRealAt(PLCMemory.QDirectionBuffer, ByteDirection - PLCMemory.QStart);
                        case 'M':
                            return S7.GetRealAt(PLCMemory.MDirectionBuffer, ByteDirection - PLCMemory.MStart);
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }
    }
    public static class PLCMemory
    {
        // Private variable lists
        public static List<Variable> AllVariables = new List<Variable> { };
        private static List<Variable> MDirectionVariables
        {
            get
            {
                return AllVariables.Where(x => x.LetterDirection == 'M').ToList();
            }
        } 
        private static List<Variable> IDirectionVariables
        {
            get
            {
                return AllVariables.Where(x => x.LetterDirection == 'I').ToList();
            }
        }
        private static List<Variable> QDirectionVariables
        {
            get
            {
                return AllVariables.Where(x => x.LetterDirection == 'Q').ToList();
            }
        }

        // Public variable lists
        public static List<Variable> Inputs
        {
            get
            {
                return AllVariables.Where(x => x.IsInput).ToList();
            }
        }
        public static List<Variable> Outputs
        {
            get
            {
                return AllVariables.Where(x => x.IsOutput).ToList();
            }
        }
        public static List<Variable> InternalVariables
        {
            get
            {
                return AllVariables.Where(x => x.IsVariable).ToList();
            }
        }
        public static List<Variable> States
        {
            get
            {
                return AllVariables.Where(x => x.IsState).ToList();
            }
        }

        // Data buffer sizes
        public const int MStart = 0;
        public const int QStart = 0;
        public const int IStart = 0;

        public static int MLength
        {
            get
            {
                try
                {
                    int MEnd = MDirectionVariables.Max(x => x.ByteDirection);
                    return MEnd +1 - MStart;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static int QLength
        {
            get
            {
                try
                {
                    int QEnd = QDirectionVariables.Max(x => x.ByteDirection);
                    return QEnd + 1 - QStart;
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static int ILength
        {
            get
            {
                try
                {
                    int IEnd = IDirectionVariables.Max(x => x.ByteDirection);
                    return IEnd + 1 - IStart;
                }
                catch
                {
                    return 0;
                }
            }
        }

        // Data buffer declarations
        private const int MAX_BUFFER_SIZE = 255;
        public static byte[] MDirectionBuffer = new byte[MAX_BUFFER_SIZE];
        public static byte[] QDirectionBuffer = new byte[MAX_BUFFER_SIZE];
        public static byte[] IDirectionBuffer = new byte[MAX_BUFFER_SIZE];

        // Default TCP/IP Config con connection
        private const int defaultRACK = 0;
        private const int defaultSLOT = 1;
        private const string defaultIP = "10.26.11.131";

        public static int RACK = defaultRACK;
        public static int SLOT = defaultSLOT;
        public static string IP = defaultIP;

        public static bool ConnectionStatus = false;
        public static S7Client client = new S7Client();
        private static System.Timers.Timer? updateData;
        private static int Error
        {
            set
            {
                if(value != 0) Debug.WriteLine(client.ErrorText(value));
            }
        }
        public static void Connect()
        {
            if (ConnectionStatus) throw new Exception("Intento de conexión sin cerrar conexión anterior");
            Error = client.ConnectTo(IP, RACK, SLOT);
            if (!client.Connected) ConnectionStatus = false;
            else ConnectionStatus = true;
        }
        public static void Disconnect()
        {
            if (!ConnectionStatus) throw new Exception("No existe conexión que cerrar");
            Error = client.Disconnect();
            ConnectionStatus = false;
        }
        public static void StartAutoUpdating(TimeSpan updateTime)
        {
            updateData = new System.Timers.Timer(500);
            updateData.Elapsed += IWrite;
            updateData.Elapsed += QRead;
            updateData.Elapsed += MReadAndWrite;
            updateData.Start();
        }
        public static void StopAutoUpdating()
        {
            if (updateData == null)
            {
                throw new Exception("No existen actualizaciones que detener");
            }
            updateData.Stop();
        }
        //  PLC Inputs should exclusively be writen 
        public static void IWrite(Object source, ElapsedEventArgs e)
        {
            Error = client.EBWrite(IStart, ILength, IDirectionBuffer);
        }
        //  PLC Outputs should exclusively be read 
        public static void QRead(Object source, ElapsedEventArgs e)
        {
            Error = client.ABRead(QStart, QLength, QDirectionBuffer);
        }

        /*      PLCMarkers can behave as inputs, outputs, internal variables or states. Only 
            the inputs should be written, so we must avoid rewriting the other values 
            as the outputs.
                To do so, we have to read all the variables first, to update the buffer that we 
            are going to write, which will contain the input values and the updated outputs, 
            avoiding rewriting them. */
        public static void MReadAndWrite(Object source, ElapsedEventArgs e)
        {
            // To avoid rewriting unintentionally, a reading buffer is required
            byte[] readingBuffer = new byte[MLength];
            Error = client.MBRead(MStart, MLength, readingBuffer);
            
            // We only write the variables that are not PLC intputs.
            for (int i = 0; i < MDirectionVariables.Count; i++)
            {
                if (!MDirectionVariables[i].IsInput)
                {
                    int byteIndex = MDirectionVariables[i].ByteDirection - MStart;
                    if (MDirectionVariables[i] is BitVariable)
                    {
                        int bitDirection = MDirectionVariables[i].BitDirection;
                        bool boolValue = S7.GetBitAt(readingBuffer, byteIndex, bitDirection);
                        S7.SetBitAt(ref MDirectionBuffer, byteIndex, bitDirection, boolValue);
                    }
                    else if (MDirectionVariables[i] is ByteVariable)
                    {
                        byte byteValue = S7.GetByteAt(readingBuffer, byteIndex);
                        S7.SetByteAt(MDirectionBuffer, byteIndex, byteValue);
                    }
                    else if (MDirectionVariables[i] is IntVariable)
                    {
                        int intValue = S7.GetSIntAt(readingBuffer, byteIndex);
                        S7.SetSIntAt(MDirectionBuffer, byteIndex, intValue);
                    }
                    else if (MDirectionVariables[i] is FloatVariable)
                    {
                        float floatValue = S7.GetRealAt(readingBuffer, byteIndex);
                        S7.SetRealAt(MDirectionBuffer, byteIndex, floatValue);
                    }
                }
            }
            /* The outputs have already been read, only the inputs remain without change
               and can be written safely */
            Error =client.MBWrite(MStart, MLength, MDirectionBuffer);
        }
    }
}