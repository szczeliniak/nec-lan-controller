using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Requests;

namespace NecControllerTest
{
    [TestClass]
    public class RequestsSuite
    {
        private byte[] POWER_ON_REQUEST_EXPECTED = { 2, 0, 0, 0, 0, 2 };
        private byte[] POWER_OFF_REQUEST_EXPECTED = { 2, 1, 0, 0, 0, 3 };
        private byte[] SOUND_MUTE_ON_REQUEST_EXPECTED = { 2, 18, 0, 0, 0, 20 };
        private byte[] SOUND_MUTE_OFF_REQUEST_EXPECTED = { 2, 19, 0, 0, 0, 21 };
        private byte[] PICTURE_MUTE_ON_REQUEST_EXPECTED = { 2, 16, 0, 0, 0, 18 };
        private byte[] PICTURE_MUTE_OFF_REQUEST_EXPECTED = { 2, 17, 0, 0, 0, 19 };
        private byte[] PICTURE_ADJUST_ABSOLUTE_BRIGHTNESS_REQUEST_EXPECTED = { 3, 16, 0, 0, 5, 0, 255, 0, 10, 0, 33 };
        private byte[] PICTURE_ADJUST_ABSOLUTE_BRIGHTNESS_NEGATIVE_REQUEST_EXPECTED = { 3, 16, 0, 0, 5, 0, 255, 0, 246, 255, 12 };
        private byte[] SHUTTER_ON_REQUEST_EXPECTED = { 2, 22, 0, 0, 0, 24 };
        private byte[] SHUTTER_OFF_REQUEST_EXPECTED = { 2, 23, 0, 0, 0, 25 };
        private byte[] INPUT_SWITCH_HDMI_REQUEST_EXPECTED = { 2, 3, 0, 0, 2, 1, 26, 34 };
        private byte[] VOLUME_ADJUST_RELATIVE_REQUEST_EXPECTED = { 3, 16, 0, 0, 5, 5, 0, 1, 5, 0, 35 };
        private byte[] VOLUME_ADJUST_ABSOLUTE_REQUEST_EXPECTED = { 3, 16, 0, 0, 5, 5, 0, 0, 5, 0, 34 };
        private byte[] INFORMATIONS_REQUEST_EXPECTED = { 3, 149, 0, 0, 0, 152 };
        private byte[] RUNNING_STATUS_REQUEST_EXPECTED = { 0, 133, 0, 0, 1, 1, 135 };
        private byte[] BASIC_INFORMATION_REQUEST_EXPECTED = { 0, 191, 0, 0, 1, 2, 194 };

        private string host = "127.0.0.1";
        private int port = 1111;

        [TestMethod]
        public void PowerOnRequest()
        {
            TestRequest(new PowerOnRequest(host, port), POWER_ON_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void PowerOffRequest()
        {
            TestRequest(new PowerOffRequest(host, port), POWER_OFF_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void SoundMuteOnRequest()
        {
            TestRequest(new SoundMuteOnRequest(host, port), SOUND_MUTE_ON_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void PictureMuteOnRequest()
        {
            TestRequest(new PictureMuteOnRequest(host, port), PICTURE_MUTE_ON_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void PictureMuteOffRequest()
        {
            TestRequest(new PictureMuteOffRequest(host, port), PICTURE_MUTE_OFF_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void ShutterOnRequest()
        {
            TestRequest(new ShutterOnRequest(host, port), SHUTTER_ON_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void ShutterOffRequest()
        {
            TestRequest(new ShutterOffRequest(host, port), SHUTTER_OFF_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void InputSwitchRequest()
        {
            TestRequest(new InputSwitchRequest(host, port, InputType.HDMI), INPUT_SWITCH_HDMI_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void VolumeAdjustRelativeRequest()
        {
            TestRequest(new VolumeAdjustRequest(host, port, AdjustmentType.RELATIVE, 5), VOLUME_ADJUST_RELATIVE_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void VolumeAdjustAbsoluteRequest()
        {
            TestRequest(new VolumeAdjustRequest(host, port, AdjustmentType.ABSOLUTE, 5), VOLUME_ADJUST_ABSOLUTE_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void PictureAdjustAbsolutePositiveRequest()
        {
            TestRequest(new PictureAdjustRequest(host, port, AdjustmentTarget.BRIGHTNESS, AdjustmentType.ABSOLUTE, 10), PICTURE_ADJUST_ABSOLUTE_BRIGHTNESS_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void PictureAdjustAbsoluteNegativeRequest()
        {
            TestRequest(new PictureAdjustRequest(host, port, AdjustmentTarget.BRIGHTNESS, AdjustmentType.ABSOLUTE, -10), PICTURE_ADJUST_ABSOLUTE_BRIGHTNESS_NEGATIVE_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void InformationRequest()
        {
            TestRequest(new InformationRequest(host, port), INFORMATIONS_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void RunningStatusRequest()
        {
            TestRequest(new RunningStatusRequest(host, port), RUNNING_STATUS_REQUEST_EXPECTED);
        }

        [TestMethod]
        public void BasicInformationRequest()
        {
            TestRequest(new BasicInformationRequest(host, port), BASIC_INFORMATION_REQUEST_EXPECTED);
        }

        public void AssertByteArrays(byte[] expected, byte[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++) Assert.AreEqual(expected[i], actual[i]);
        }

        public void TestRequest(AbstractRequest request, byte[] expectedBytes)
        {   
            AssertByteArrays(expectedBytes, request.GetRequestBytes());
        }

    }
}
