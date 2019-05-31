using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Requests;
using Responses;

namespace NecControllerTest
{
    [TestClass]
    public class ResponseSuite
    {
        private byte[] POWER_ON_SUCCESS_RESPONSE = { 34, 0, 55, 3, 0, 92 };
        private byte[] POWER_ON_ERROR_RESPONSE = { 162, 0, 0, 0, 2, 2, 7, 173 };
        private byte[] POWER_OFF_SUCCESS_RESPONSE = { 34, 1, 0, 0, 0, 35 };
        private byte[] INPUT_SWITCH_SUCCESS_RESPONSE = { 34, 3, 0, 0, 1, 0, 38 };
        private byte[] SOUND_MUTE_ON_SUCCESS_RESPONSE = { 34, 18, 0, 0, 0, 52 };
        private byte[] SOUND_MUTE_OFF_SUCCESS_RESPONSE = { 34, 19, 0, 0, 0, 53 };
        private byte[] PICTURE_MUTE_ON_SUCCESS_RESPONSE = { 34, 16, 0, 0, 0, 50 };
        private byte[] PICTURE_MUTE_OFF_SUCCESS_RESPONSE = { 34, 17, 0, 0, 0, 51 };
        private byte[] SHUTTER_ON_SUCCESS_RESPONSE = { 34, 23, 0, 0, 0, 57 };
        private byte[] SHUTTER_OFF_SUCCESS_RESPONSE = { 34, 22, 0, 0, 0, 56 };
        private byte[] VOLUME_ADJUST_SUCCESS_RESPONSE = { 34, 16, 0, 0, 2, 0, 0, 52 };
        private byte[] VOLUME_ADJUST_FAILED_RESPONSE = { 34, 16, 0, 0, 2, 0, 5, 57 };
        private byte[] INPUT_SWITCH_FAILED_RESPONSE = { 34, 3, 0, 0, 1, 255, 37 };
        private byte[] PICTURE_ADJUST_SUCCESS_RESPONSE = { 35, 16, 0, 0, 5, 0, 255, 1, 0, 10, 66 };
        private byte[] RUNNING_STATUS_RESPONSE = { 32, 133, 0, 0, 16, 0, 0, 1, 1, 1, 16, 200 };
        private byte[] BASIC_INFORMATION_RESPONSE = { 32, 191, 0, 0, 16, 2, 4, 0, 0, 33, 13, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 37 };

        private byte[] INFORMATIONS_RESPONSE =
        {
            35, 138, 0, 0, 98,
            68, 97, 119, 105, 100, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0,
            39, 167, 213, 68,
            3, 192, 183, 38,
            0, 0, 0, 0, 0, 0, 0, 0,
            127
        };

        private string PROJECTOR_NAME = "Dawid";
        private int LAMP_USAGE = 1154852647;
        private int FILTER_USAGE = 649576451;

        private int POWER_ON_MODEL_CODE = 3;
        private int POWER_ON_CONTROL_ID = 55;

        [TestMethod]
        public void PowerOnSuccessResponseTest()
        {
            TestSuccessResponse(new PowerOnResponse(POWER_ON_SUCCESS_RESPONSE, POWER_ON_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void PowerOnErrorResponseTest()
        {
            TestFailedResponse(new PowerOnResponse(POWER_ON_ERROR_RESPONSE, POWER_ON_ERROR_RESPONSE.Length));
        }

        [TestMethod]
        public void PowerOffSuccessResponseTest()
        {
            TestSuccessResponse(new PowerOffResponse(POWER_OFF_SUCCESS_RESPONSE, POWER_OFF_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void SoundMuteOnSuccessResponseTest()
        {
            TestSuccessResponse(new SoundMuteOnResponse(SOUND_MUTE_ON_SUCCESS_RESPONSE, SOUND_MUTE_ON_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void SoundMuteOffSuccessResponseTest()
        {
            TestSuccessResponse(new SoundMuteOffResponse(SOUND_MUTE_OFF_SUCCESS_RESPONSE, SOUND_MUTE_OFF_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void PictureMuteOnSuccessResponseTest()
        {
            TestSuccessResponse(new PictureMuteOnResponse(PICTURE_MUTE_ON_SUCCESS_RESPONSE, PICTURE_MUTE_ON_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void PictureMuteOffSuccessResponseTest()
        {
            TestSuccessResponse(new PictureMuteOffResponse(PICTURE_MUTE_OFF_SUCCESS_RESPONSE, PICTURE_MUTE_OFF_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void ShutterOffSuccessResponseTest()
        {
            TestSuccessResponse(new ShutterOffResponse(SHUTTER_OFF_SUCCESS_RESPONSE, SHUTTER_OFF_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void ShutterOnSuccessResponseTest()
        {
            TestSuccessResponse(new ShutterOnResponse(SHUTTER_ON_SUCCESS_RESPONSE, SHUTTER_ON_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void VolumeAdjustOnSuccessResponseTest()
        {
            TestSuccessResponse(new VolumeAjdustResponse(VOLUME_ADJUST_SUCCESS_RESPONSE, VOLUME_ADJUST_SUCCESS_RESPONSE.Length));
        }

        [TestMethod]
        public void VerifyChecksumTest()
        {
            PowerOnResponse response = new PowerOnResponse(POWER_ON_ERROR_RESPONSE, POWER_ON_ERROR_RESPONSE.Length);
            Assert.IsTrue(response.VerifyChecksum());
        }

        [TestMethod]
        public void InputSwitchErrorResponse()
        {
            InputSwitchResponse response = new InputSwitchResponse(INPUT_SWITCH_FAILED_RESPONSE, INPUT_SWITCH_FAILED_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetExecutionResult(), ExecutionResult.FAILED);
        }

        [TestMethod]
        public void ErrorMessageFromResponseTest()
        {
            PowerOnResponse response = new PowerOnResponse(POWER_ON_ERROR_RESPONSE, POWER_ON_ERROR_RESPONSE.Length);
            TestFailedResponse(response);
            Assert.AreEqual(response.GetError(), ErrorCode.NO_SIGNAL);
        }

        [TestMethod]
        public void CheckExecutionResultFromResponseTest()
        {
            VolumeAjdustResponse response = new VolumeAjdustResponse(VOLUME_ADJUST_FAILED_RESPONSE, VOLUME_ADJUST_FAILED_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetExecutionResult(), ExecutionResult.FAILED);
        }

        [TestMethod]
        public void CheckNoExecutionResultFromResponseTest()
        {
            PowerOnResponse response = new PowerOnResponse(POWER_ON_SUCCESS_RESPONSE, POWER_ON_SUCCESS_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetExecutionResult(), ExecutionResult.NO_EXECUTION_RESULT);
        }

        [TestMethod]
        public void InputSwitchSuccessResponse()
        {
            InputSwitchResponse response = new InputSwitchResponse(INPUT_SWITCH_SUCCESS_RESPONSE, INPUT_SWITCH_SUCCESS_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetExecutionResult(), ExecutionResult.SUCCESS);
        }

        [TestMethod]
        public void PictureAdjustSuccessResponse()
        {
            InputSwitchResponse response = new InputSwitchResponse(PICTURE_ADJUST_SUCCESS_RESPONSE, PICTURE_ADJUST_SUCCESS_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetExecutionResult(), ExecutionResult.SUCCESS);
        }

        [TestMethod]
        public void InformationSuccessResponse()
        {
            InformationResponse response = new InformationResponse(INFORMATIONS_RESPONSE, INFORMATIONS_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetProjectorName(), PROJECTOR_NAME);
            Assert.AreEqual(response.GetLampUsageSeconds(), LAMP_USAGE);
            Assert.AreEqual(response.GetFilterUsageSeconds(), FILTER_USAGE);
        }

        [TestMethod]
        public void RunningStatusResponse()
        {
            RunningStatusResponse response = new RunningStatusResponse(RUNNING_STATUS_RESPONSE, RUNNING_STATUS_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetCoolingProcess(), CoolingProcess.DURING_EXECUTION);
            Assert.AreEqual(response.GetOperationStatus(), OperationStatus.NETWORK_STANDBY);
            Assert.AreEqual(response.GetPowerStatus(), PowerStatus.POWER_ON);
            Assert.AreEqual(response.GetPowerOnOffProcess(), PowerOnOffProcess.DURING_EXECUTION);
        }

        [TestMethod]
        public void BasicInformationResponse()
        {
            BasicInformationResponse response = new BasicInformationResponse(BASIC_INFORMATION_RESPONSE, BASIC_INFORMATION_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.IsTrue(response.IsVideoMuted());
            Assert.IsTrue(response.IsSoundMuted());
            Assert.IsFalse(response.IsScreenFreezed());
            Assert.IsFalse(response.IsOnscreenMuted());
            Assert.AreEqual(response.GetSelectionSignalType(), SelectionSignalType.HDMI);
            Assert.AreEqual(response.GetOperationStatus(), OperationStatus.POWER_ON);
            Assert.AreEqual(response.GetDisplaySygnalType(), DisplaySygnalType.NTSC);
        }
        
        [TestMethod]
        public void CheckModelCodeAndControlId()
        {
            PowerOnResponse response = new PowerOnResponse(POWER_ON_SUCCESS_RESPONSE, POWER_ON_SUCCESS_RESPONSE.Length);
            TestSuccessResponse(response);
            Assert.AreEqual(response.GetControlId(), POWER_ON_CONTROL_ID);
            Assert.AreEqual(response.GetModelCode(), POWER_ON_MODEL_CODE);
        }

        private void CheckChecksum(AbstractResponse response)
        {
            Assert.IsTrue(response.VerifyChecksum());
        }

        private void CheckStatus(AbstractResponse response)
        {
            Assert.AreEqual(response.GetStatus(), Status.RESPONSE_OK);
        }

        public void TestSuccessResponse(AbstractResponse response)
        {
            CheckStatus(response);
            CheckChecksum(response);
        }

        public void TestFailedResponse(AbstractResponse response)
        {
            Assert.AreEqual(response.GetStatus(), Status.RESPONSE_FAIL);
        }
    }
}
