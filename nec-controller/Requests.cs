using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Responses;

namespace Requests
{

    public enum AdjustmentType
    {
        ABSOLUTE = 0,
        RELATIVE = 1
    }

    public enum InputType
    {
        COMPUTER = 1,
        VIDEO = 6,
        HDMI = 26,
        HDMI2 = 42,
        ERROR = -1
    }

    public enum AdjustmentTarget
    {
        BRIGHTNESS = 0,
        CONTRAST = 1,
        COLOR = 2,
        HUE = 3,
        SHARPNESS = 4
    }

    public abstract class AbstractRequest
    {

        public const int MAX_VALUE = 127;
        public const int MIN_VALUE = -127;

        protected string ipAddr;
        protected int port;
        protected byte[] requestCode;
        protected byte[] responseCode;

        protected abstract byte[] PrepareRequest();

        public byte[] GetRequestBytes()
        {
            byte[] bytes = PrepareRequest();
            bytes[bytes.Length - 1] = Utils.Utils.CountChecksum(bytes);
            return bytes;
        }

        protected static int ParseValue(int value)
        {
            if (value <= MAX_VALUE && value >= MIN_VALUE) return value;

            if (value >= MAX_VALUE) return MAX_VALUE;

            return MIN_VALUE;

        }

        protected abstract AbstractResponse ParseResponse(byte[] responseBytes, int responseSize);

        public AbstractRequest(string ip, int port)
        {
            this.ipAddr = ip;
            this.port = port;
        }

        public AbstractResponse SendRequest()
        {

            TcpClient client = null;
            NetworkStream stream = null;
            bool isError = false;

            byte[] responseBytes = null;
            int bytesRead = 0;

            try
            {
                client = new TcpClient(this.ipAddr, this.port);
                stream = client.GetStream();

                byte[] requestCode = GetRequestBytes();
                stream.Write(requestCode, 0, requestCode.Length);

                responseBytes = new byte[client.ReceiveBufferSize];
                bytesRead = stream.Read(responseBytes, 0, client.ReceiveBufferSize);
            }
            catch (Exception e)
            {
                Utils.Logger.log(e.Message);
                isError = true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (client != null)
                {
                    client.Close();
                }
            }

            if (isError) return null;

            Utils.Logger.log("EUEUEUE");

            return ParseResponse(responseBytes, bytesRead);

        }
    }

    public class PowerOnRequest : AbstractRequest
    {
        public PowerOnRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 0, 0, 0, 0, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new PowerOnResponse(responseBytes, responseSize);
        }
    }

    public class PowerOffRequest : AbstractRequest
    {
        public PowerOffRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 1, 0, 0, 0, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new PowerOffResponse(responseBytes, responseSize);
        }
    }

    public class SoundMuteOnRequest : AbstractRequest
    {
        public SoundMuteOnRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 18, 0, 0, 0, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new SoundMuteOnResponse(responseBytes, responseSize);
        }
    }

    public class SoundMuteOffRequest : AbstractRequest
    {
        public SoundMuteOffRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 19, 0, 0, 0, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new SoundMuteOffResponse(responseBytes, responseSize);
        }
    }

    public class VolumeAdjustRequest : AbstractRequest
    {
        private AdjustmentType type;
        private int value;

        public VolumeAdjustRequest(string ip, int port, AdjustmentType type, int value) : base(ip, port) {

            this.type = type;
            this.value = ParseValue(value);
        }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 3, 16, 0, 0, 5, 5, 0, (byte)type, (byte)value, 0, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new VolumeAjdustResponse(responseBytes, responseSize);
        }
    }

    public class InputSwitchRequest : AbstractRequest
    {
        protected InputType inputType = 0;

        public InputSwitchRequest(string ip, int port, InputType type) : base(ip, port) {
            inputType = type;
        }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 3, 0, 0, 2, 1, (byte)inputType, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new InputSwitchResponse(responseBytes, responseSize);
        }
    }

    public class PictureMuteOnRequest : AbstractRequest
    {
        public PictureMuteOnRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 16, 0, 0, 0, 0 };
        }
        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new PictureMuteOnResponse(responseBytes, responseSize);
        }
    }

    public class PictureMuteOffRequest : AbstractRequest
    {
        public PictureMuteOffRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 17, 0, 0, 0, 0 };
        }
        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new PictureMuteOffResponse(responseBytes, responseSize);
        }
    }

    public class ShutterOnRequest : AbstractRequest
    {
        public ShutterOnRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 23, 0, 0, 0, 0 };
        }
        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new ShutterOnResponse(responseBytes, responseSize);
        }
    }

    public class ShutterOffRequest : AbstractRequest
    {
        public ShutterOffRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 2, 22, 0, 0, 0, 0 };
        }
        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new ShutterOffResponse(responseBytes, responseSize);
        }
    }

    public class PictureAdjustRequest : AbstractRequest
    {
        private AdjustmentTarget target;
        private AdjustmentType type;
        private int value;

        public PictureAdjustRequest(string ip, int port, AdjustmentTarget target, AdjustmentType type, int value) : base(ip, port) {
            this.target = target;
            this.type = type;
            this.value = ParseValue(value);
        }

        protected override byte[] PrepareRequest()
        {
            int val = value;
            int negation = 0;
            if(val < 0)
            {
                val += 256;
                negation = 255;
            }

            return new byte[] { 3, 16, 0, 0, 5, (byte)target, 255, (byte)type, (byte)value, (byte)negation, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new PictureAdjustResponse(responseBytes, responseSize);
        }
    }

    public class InformationRequest : AbstractRequest
    {
        public InformationRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 3, 149, 0, 0, 0, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new InformationResponse(responseBytes, responseSize);
        }
    }

    public class RunningStatusRequest : AbstractRequest
    {
        public RunningStatusRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 0, 133, 0, 0, 1, 1, 0 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new RunningStatusResponse(responseBytes, responseSize);
        }
    }

    public class BasicInformationRequest : AbstractRequest
    {
        public BasicInformationRequest(string ip, int port) : base(ip, port) { }

        protected override byte[] PrepareRequest()
        {
            return new byte[] { 0, 191, 0, 0, 1, 2, 194 };
        }

        protected override AbstractResponse ParseResponse(byte[] responseBytes, int responseSize)
        {
            return new BasicInformationResponse(responseBytes, responseSize);
        }
    }
}
