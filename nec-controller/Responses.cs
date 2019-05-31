using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Requests;

namespace Responses
{

    public enum ExecutionResult
    {
        SUCCESS = 0,
        FAILED,
        NO_EXECUTION_RESULT
    }

    public enum PowerStatus
    {
        STANDBY = 0,
        POWER_ON = 1,
        NOT_SUPPORTED = 255,
        NOT_KNOWN
    }

    public enum CoolingProcess
    {
        NOT_EXECUTED = 0,
        DURING_EXECUTION = 1,
        NOT_SUPPORTED = 255,
        NOT_KNOWN
    }

    public enum PowerOnOffProcess
    {
        NOT_EXECUTED = 0,
        DURING_EXECUTION = 1,
        NOT_SUPPORTED = 255,
        NOT_KNOWN
    }

    public enum OperationStatus
    {
        STANDBY_SLEEP = 0,
        POWER_ON = 4,
        COOLING = 5,
        STANDBY_ERROR = 6,
        STANDBY_POWER_SAVING = 15,
        NETWORK_STANDBY = 16,
        NOT_SUPPORTED = 255,
        NOT_KNOWN
    }

    public enum ErrorCode
    {
        NO_ERROR,
        COMMAND_NOT_RECOGNIZED,
        COMMAND_NOT_SUPPORTED,
        INVALID_VALUE,
        INVALID_INPUT_TERMINAL,
        INVALID_LANGUAGE,
        MEMORY_ALLOCATION_ERROR,
        MEMORY_IN_USE,
        VALUE_CANNOT_BE_SET,
        FORCED_SCREEN_MUTE_OFF,
        VIEWER_ERROR,
        NO_SIGNAL,
        TEST_PATTERN_DISPLAYED,
        NO_PC_CARD,
        MEMORY_OPERATION_ERROR,
        ENTRY_LIST_DISPLAYED,
        POWER_OFF,
        COMMAND_EXECUTION_FAILURE,
        NO_AUTHORITY,
        INVALID_GAIN_NUMBER,
        INVALID_GAIN,
        ADJUSMENT_FAILED
    }

    public enum Status
    {
        RESPONSE_OK = 34,
        RESPONSE_OK_2 = 35,
        RESPONSE_OK_3 = 32,
        RESPONSE_FAIL = 162
    }

    public enum SelectionSignalType
    {
        COMPUTER = 1,
        VIDEO = 2,
        S_VIDEO = 3,
        COMPONENT = 4,
        VIEWER = 7,
        DVI = 32,
        HDMI = 33,
        DISPLAY_PORT = 34,
        VIEWER_2 = 35,
        NOT_SOURCE_INPUT = 255
    }

    public enum DisplaySygnalType
    {
        NTSC_3_58 = 0,
        NTSC_4_43 = 1,
        PAL = 2,
        PAL_60 = 3,
        SECAM = 4,
        BW60 = 5,
        BW50 = 6,
        PALNM = 7,
        NTSC_3_58_LBX = 8,
        NTSC_3_58_SQZ = 9,
        COMPOMENT_60HZ = 10,
        COMPOMENT_50HZ = 11,
        UNKNOWN = 12,
        NTSC = 13,
        PAL_M = 14,
        PAL_L = 15,
        NOT_VIDEO_INPUT = 255
    }

    public abstract class AbstractResponse
    {
        public static int STATUS_BYTE = 0;
        public static int ID1_BYTE = 2;
        public static int ID2_BYTE = 3;
        public static int ERR1_BYTE = 5;
        public static int ERR2_BYTE = 6;

        protected byte[] bytes;

        public byte[] GetBytes(){
            return bytes;
        }

        public int GetControlId()
        {
            return bytes[ID1_BYTE];
        }

        public int GetModelCode()
        {
            return bytes[ID2_BYTE];
        }

        public byte[] GetRequestBytes()
        {
            return bytes;
        }

        public Status GetStatus()
        {
            if ((int)Status.RESPONSE_OK == (int)bytes[STATUS_BYTE] || (int)Status.RESPONSE_OK_2 == (int)bytes[STATUS_BYTE] || (int)Status.RESPONSE_OK_3 == (int)bytes[STATUS_BYTE]) return Status.RESPONSE_OK;
            return Status.RESPONSE_FAIL;
        }

        public abstract ExecutionResult GetExecutionResult();

        public ErrorCode GetError()
        {
            if (GetStatus() == Status.RESPONSE_FAIL)
            {
                switch((int)bytes[ERR1_BYTE]) {
                    case 0:
                        switch ((int)bytes[ERR2_BYTE])
                        {
                            case 0:
                                return ErrorCode.COMMAND_NOT_RECOGNIZED;
                            case 1:
                                return ErrorCode.COMMAND_NOT_SUPPORTED;
                        }
                        break;
                    case 1:
                        switch ((int)bytes[ERR2_BYTE])
                        {
                            case 0:
                                return ErrorCode.INVALID_VALUE;
                            case 1:
                                return ErrorCode.INVALID_INPUT_TERMINAL;
                            case 2:
                                return ErrorCode.INVALID_LANGUAGE;
                        }
                        break;
                    case 2:
                        switch ((int)bytes[ERR2_BYTE])
                        {
                            case 0:
                                return ErrorCode.MEMORY_ALLOCATION_ERROR;
                            case 2:
                                return ErrorCode.MEMORY_IN_USE;
                            case 3:
                                return ErrorCode.VALUE_CANNOT_BE_SET;
                            case 4:
                                return ErrorCode.FORCED_SCREEN_MUTE_OFF;
                            case 6:
                                return ErrorCode.VIEWER_ERROR;
                            case 7:
                                return ErrorCode.NO_SIGNAL;
                            case 8:
                                return ErrorCode.TEST_PATTERN_DISPLAYED;
                            case 9:
                                return ErrorCode.NO_PC_CARD;
                            case 10:
                                return ErrorCode.MEMORY_OPERATION_ERROR;
                            case 12:
                                return ErrorCode.ENTRY_LIST_DISPLAYED;
                            case 13:
                                return ErrorCode.POWER_OFF;
                            case 14:
                                return ErrorCode.COMMAND_EXECUTION_FAILURE;
                            case 15:
                                return ErrorCode.NO_AUTHORITY;
                        }
                        break;
                    case 3:
                        switch ((int)bytes[ERR2_BYTE])
                        {
                            case 0:
                                return ErrorCode.INVALID_GAIN_NUMBER;
                            case 1:
                                return ErrorCode.INVALID_GAIN;
                            case 2:
                                return ErrorCode.ADJUSMENT_FAILED;
                        }
                        break;
                }
            }
            return ErrorCode.NO_ERROR;
        }

        public AbstractResponse(byte[] responseBytes, int responseSize)
        {
            bytes = new byte[responseSize];
            for (int i = 0; i < responseSize; i++)
            {
                this.bytes[i] = responseBytes[i];
            }
        }

        public Boolean VerifyChecksum()
        {
            return Utils.Utils.CountChecksum(bytes) == bytes[bytes.Length - 1];
        }

    }

    public class PowerOnResponse: AbstractResponse
    {
        public PowerOnResponse(byte[] responseBytes, int responseSize): base(responseBytes, responseSize) {}

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }

    }

    public class PowerOffResponse : AbstractResponse
    {
        public PowerOffResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) {}

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class PictureMuteOnResponse : AbstractResponse
    {
        public PictureMuteOnResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class PictureMuteOffResponse : AbstractResponse
    {
        public PictureMuteOffResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class SoundMuteOnResponse : AbstractResponse
    {
        public SoundMuteOnResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class SoundMuteOffResponse : AbstractResponse
    {
        public SoundMuteOffResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) {}

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class ShutterOnResponse : AbstractResponse
    {
        public ShutterOnResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class ShutterOffResponse : AbstractResponse
    {
        public ShutterOffResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }
    }

    public class VolumeAjdustResponse : AbstractResponse
    {
        private int RESULT1_BYTE = 5;
        private int RESULT2_BYTE = 6;

        public VolumeAjdustResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            if (GetStatus() == Status.RESPONSE_OK)
            {
                if ((int)bytes[RESULT1_BYTE] == (int)ExecutionResult.SUCCESS && (int)bytes[RESULT2_BYTE] == (int)ExecutionResult.SUCCESS) return ExecutionResult.SUCCESS;
            }
            return ExecutionResult.FAILED;
        }

    }

    public class InputSwitchResponse : AbstractResponse
    {
        public static int RESULT_BYTE = 5;

        public InputSwitchResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            if (GetStatus() == Status.RESPONSE_OK)
            {
                if ((int)bytes[RESULT_BYTE] == (int)ExecutionResult.SUCCESS) return ExecutionResult.SUCCESS;
            }
            return ExecutionResult.FAILED;
        }
    }

    public class PictureAdjustResponse : AbstractResponse
    {
        private int RESULT1_BYTE = 5;
        private int RESULT2_BYTE = 6;

        public PictureAdjustResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            if (GetStatus() == Status.RESPONSE_OK)
            {
                if ((int)bytes[RESULT1_BYTE] == (int)ExecutionResult.SUCCESS && (int)bytes[RESULT2_BYTE] == (int)ExecutionResult.SUCCESS) return ExecutionResult.SUCCESS;
            }
            return ExecutionResult.FAILED;
        }
    }

    public class InformationResponse : AbstractResponse
    {
        public static int DATA_BYTE_START = 5;

        public static int PROJECTOR_NAME_LENGTH = 49;
        public static int SYSTEM_RESERVED_LENGTH = 33;
        public static int LAMP_USAGE_LENGTH = 4;
        public static int FILTER_USAGE_LENGTH = 4;
        public static int SYSTEM_RESERVED_2_LENGTH = 8;

        private static int PROJECTOR_NAME_START_BYTE = DATA_BYTE_START;
        private static int SYSTEM_RESERVED_START_BYTE = PROJECTOR_NAME_START_BYTE + PROJECTOR_NAME_LENGTH;
        private static int LAMP_USAGE_START_BYTE = SYSTEM_RESERVED_START_BYTE + SYSTEM_RESERVED_LENGTH;
        private static int FILTER_USAGE_START_BYTE = LAMP_USAGE_START_BYTE + LAMP_USAGE_LENGTH;
        private static int SYSTEM_RESERVED_2_START_BYTE = FILTER_USAGE_START_BYTE + FILTER_USAGE_LENGTH;

        public InformationResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }

        public string GetProjectorName()
        {
            return System.Text.Encoding.UTF8.GetString(bytes, PROJECTOR_NAME_START_BYTE, PROJECTOR_NAME_LENGTH).TrimEnd('\0');
        }

        public int GetLampUsageSeconds()
        {
            return (int)BitConverter.ToUInt32(bytes, LAMP_USAGE_START_BYTE);
        }

        public int GetFilterUsageSeconds()
        {
            return (int)BitConverter.ToUInt32(bytes, FILTER_USAGE_START_BYTE);
        }
    }

    public class RunningStatusResponse : AbstractResponse
    {

        private static int POWER_STATUS_BYTE = 7;
        private static int COOLING_PROCESS_BYTE = 8;
        private static int POWER_ON_OFF_PROCESS_BYTE = 9;
        private static int OPERATION_STATUS_BYTE = 10;

        public RunningStatusResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }

        public PowerStatus GetPowerStatus()
        {
            return (PowerStatus)bytes[POWER_STATUS_BYTE];
        }

        public CoolingProcess GetCoolingProcess()
        {
            return (CoolingProcess)bytes[COOLING_PROCESS_BYTE];
        }

        public PowerOnOffProcess GetPowerOnOffProcess()
        {
            return (PowerOnOffProcess)bytes[POWER_ON_OFF_PROCESS_BYTE];
        }

        public OperationStatus GetOperationStatus()
        {
            return (OperationStatus)bytes[OPERATION_STATUS_BYTE];
        }

    }

    public class BasicInformationResponse : AbstractResponse
    {
        private const int OPERATION_STATUS_BYTE = 6;
        private const int SELECTION_SIGNAL_TYPE = 9;
        private const int DISPLAY_SIGNAL_TYPE_BYTE = 10;
        private const int VIDEO_MUTE = 11;
        private const int SOUND_MUTE = 12;
        private const int ONSCREEN_MUTE = 13;
        private const int FREEZE_STATUS = 14;

        public BasicInformationResponse(byte[] responseBytes, int responseSize) : base(responseBytes, responseSize) { }

        public override ExecutionResult GetExecutionResult()
        {
            return ExecutionResult.NO_EXECUTION_RESULT;
        }

        public OperationStatus GetOperationStatus()
        {
            return (OperationStatus)bytes[OPERATION_STATUS_BYTE];
        }

        public SelectionSignalType GetSelectionSignalType()
        {
            return (SelectionSignalType)bytes[SELECTION_SIGNAL_TYPE];
        }

        public DisplaySygnalType GetDisplaySygnalType()
        {
            return (DisplaySygnalType)bytes[DISPLAY_SIGNAL_TYPE_BYTE];
        }

        public bool IsVideoMuted()
        {
            return Convert.ToBoolean(bytes[VIDEO_MUTE]);
        }

        public bool IsSoundMuted()
        {
            return Convert.ToBoolean(bytes[SOUND_MUTE]);
        }

        public bool IsOnscreenMuted()
        {
            return Convert.ToBoolean(bytes[ONSCREEN_MUTE]);
        }

        public bool IsScreenFreezed()
        {
            return Convert.ToBoolean(bytes[FREEZE_STATUS]);
        }

    }
    

}
