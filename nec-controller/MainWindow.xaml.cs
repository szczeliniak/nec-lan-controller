using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Requests;
using Responses;
using System.Text.RegularExpressions;
using Utils;
using System.Windows.Controls.Primitives;

namespace NecController
{
    public partial class MainWindow : Window
    {

        const int MAX_VOLUME_STEP = 20;
        const int DEFAULT_PORT_NUMBER = 7142;
        const int DEFAULT_VOLUME_STEP = 1;
        const int PICTURE_ADJUST_STEP = 1;
        const string DEFAULT_IP_ADDRESS = "192.168.0.10";


        private List<InputTypeComboboxItem> inputSourceItems = new List<InputTypeComboboxItem>();

        private List<Control> functionalControls = new List<Control>();

        public MainWindow()
        {
            InitializeComponent();
            InitFunctionalButtonList();
            InitTextboxes();

            Utils.Logger.InitLogger(LogTxtblock);

            InitCheckboxes();

            if (!Utils.Utils.IsAddressValid(GetIpAddress()) || !Utils.Utils.IsPortVaild(GetPortNumber()))
            {
                EnableButtons(false);
            }
        }

        private void InitCheckboxes()
        {

            InputTypeComboboxItem item = null;

            foreach (Requests.InputType type in Enum.GetValues(typeof(Requests.InputType)))
            {
                if(type != Requests.InputType.ERROR)
                {
                    item = new InputTypeComboboxItem();
                    item.Type = type;
                    item.Text = type.ToString();
                    inputSourceItems.Add(item);
                    InputSourceListChckbox.Items.Add(item);
                }
            }
        }

        private void InitTextboxes()
        {
            portTextbox.Text = DEFAULT_PORT_NUMBER.ToString();
            ipAddressTextbox.Text = DEFAULT_IP_ADDRESS;
            VolumeStepTextbox.Text = DEFAULT_VOLUME_STEP.ToString();
        }

        private void InitFunctionalButtonList()
        {
            functionalControls.Add(PowerOnBtn);
            functionalControls.Add(PowerOffBtn);
            functionalControls.Add(SoundMuteOffBtn);
            functionalControls.Add(SoundMuteOnBtn);
            functionalControls.Add(VolumeAdjustUpBtn);
            functionalControls.Add(VolumeAdjustDownBtn);
            functionalControls.Add(ShutterOnBtn);
            functionalControls.Add(ShutterOffBtn);
            functionalControls.Add(PictureMuteOnBtn);
            functionalControls.Add(PictureMuteOffBtn);
            functionalControls.Add(InputSourceListChckbox);
            functionalControls.Add(BrightnessIncreaseBtn);
            functionalControls.Add(BrightnessDecreaseBtn);
            functionalControls.Add(ColorIncreaseBtn);
            functionalControls.Add(ColorDecreaseBtn);
            functionalControls.Add(SharpnessIncreaseBtn);
            functionalControls.Add(SharpnessDecreaseBtn);
            functionalControls.Add(HueIncreaseBtn);
            functionalControls.Add(HueDecreaseBtn);
            functionalControls.Add(ContrastIncreaseBtn);
            functionalControls.Add(ContrastDecreaseBtn);
            functionalControls.Add(GetInformationsBtn);
            functionalControls.Add(VolumeStepTextbox);
        }

        private void EnableButtons(Boolean enabled)
        {
            foreach (Control control in functionalControls)
            {
                control.IsEnabled = enabled;
            }
        }

        private bool PowerOn()
        {
            PowerOnRequest request = new PowerOnRequest(GetIpAddress(), GetPortNumber());
            PowerOnResponse response = SendRequest(request, "Power On") as PowerOnResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;

        }

        private bool PowerOff()
        {
            PowerOffRequest request = new PowerOffRequest(GetIpAddress(), GetPortNumber());
            PowerOffResponse response = SendRequest(request, "Power Off") as PowerOffResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;

        }

        private bool SoundMuteOn()
        {
            SoundMuteOnRequest request = new SoundMuteOnRequest(GetIpAddress(), GetPortNumber());
            SoundMuteOnResponse response = SendRequest(request, "Sound Mute On") as SoundMuteOnResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool SoundMuteOff()
        {
            SoundMuteOffRequest request = new SoundMuteOffRequest(GetIpAddress(), GetPortNumber());
            SoundMuteOffResponse response = SendRequest(request, "Sound Mute Off") as SoundMuteOffResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool AdjustVolume(bool increase)
        {
            int step = GetVolumeStep();

            if (step < 0)
            {
                Utils.Logger.log("Invalid step value. It has to be between 1 and " + MAX_VOLUME_STEP);
            }

            if (!increase)
            {
                step = -step;
            }

            VolumeAdjustRequest request = new VolumeAdjustRequest(GetIpAddress(), GetPortNumber(), AdjustmentType.RELATIVE, step);
            VolumeAjdustResponse response = SendRequest(request, "AdjustVolume") as VolumeAjdustResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;

        }

        private bool ShutterOn()
        {
            ShutterOnRequest request = new ShutterOnRequest(GetIpAddress(), GetPortNumber());
            ShutterOnResponse response = SendRequest(request, "Shutter On") as ShutterOnResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;

        }

        private bool ShutterOff()
        {
            ShutterOffRequest request = new ShutterOffRequest(GetIpAddress(), GetPortNumber());
            ShutterOffResponse response = SendRequest(request, "Shutter Off") as ShutterOffResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool PictureMuteOn()
        {
            PictureMuteOnRequest request = new PictureMuteOnRequest(GetIpAddress(), GetPortNumber());
            PictureMuteOnResponse response = SendRequest(request, "Picture Mute On") as PictureMuteOnResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool PictureMuteOff()
        {
            PictureMuteOffRequest request = new PictureMuteOffRequest(GetIpAddress(), GetPortNumber());
            PictureMuteOffResponse response =  SendRequest(request, "Picture Mute Off") as PictureMuteOffResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool InputSwitch(Requests.InputType inputType)
        {
            InputSwitchRequest request = new InputSwitchRequest(GetIpAddress(), GetPortNumber(), inputType);
            InputSwitchResponse response = SendRequest(request, "Input Switch") as InputSwitchResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool PictureAdjust(AdjustmentTarget target, bool increase)
        {
            int step = PICTURE_ADJUST_STEP;

            if (!increase)
            {
                step = -step;
            }

            PictureAdjustRequest request = new PictureAdjustRequest(GetIpAddress(), GetPortNumber(), target, AdjustmentType.RELATIVE, step);
            PictureAdjustResponse response = SendRequest(request, "Picture Adjust") as PictureAdjustResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            //handling specified for request

            return true;
        }

        private bool RefreshInformations()
        {
            BasicInformationRequest request = new BasicInformationRequest(GetIpAddress(), GetPortNumber());
            BasicInformationResponse response = SendRequest(request, "Refresh Informations") as BasicInformationResponse;

            if (response == null || response.GetStatus() == Status.RESPONSE_FAIL) return false;

            StatusLbl.Content = response.GetOperationStatus().ToString();

            //Requests.InputType inputType;

            //switch (response.GetSelectionSignalType())
            //{
            //    case SelectionSignalType.HDMI:
            //        inputType = Requests.InputType.HDMI;
            //        break;
            //    case SelectionSignalType.COMPUTER:
            //        inputType = Requests.InputType.COMPUTER;
            //        break;
            //    case SelectionSignalType.VIDEO:
            //        inputType = Requests.InputType.VIDEO;
            //        break;
            //    default:
            //        inputType = Requests.InputType.ERROR;
            //        break;
            //}

            //foreach(InputTypeComboboxItem item in inputSourceItems){
            //    if(item.Type == inputType)
            //    {
            //        InputSourceListChckbox.SelectedValue = item;
            //    }
            //}

            //handling specified for request
            //ProjectorNameLbl.Content = response.GetProjectorName();
            //LampUsageLbl.Content = response.GetLampUsageSeconds().ToString();
            //FilterUsageLbl.Content = response.GetFilterUsageSeconds().ToString();

            return true;

        }

        private void PowerOnReq_Click(object sender, RoutedEventArgs e)
        {
            PowerOn();
        }

        private void PowerOffReq_Click(object sender, RoutedEventArgs e)
        {
            PowerOff();
        }

        private void SoundMuteOn_Click(object sender, RoutedEventArgs e)
        {
            SoundMuteOn();
        }

        private void SoundMuteOff_Click(object sender, RoutedEventArgs e)
        {
            SoundMuteOff();
        }

        private void VolumeAdjustUp_Click(object sender, RoutedEventArgs e)
        {
            AdjustVolume(true);
        }

        private void VolumeAdjustDown_Click(object sender, RoutedEventArgs e)
        {
            AdjustVolume(false);
        }

        private void ShutterOnBtn_Click(object sender, RoutedEventArgs e)
        {
            ShutterOn();
        }

        private void ShutterOffBtn_Click(object sender, RoutedEventArgs e)
        {
            ShutterOff();
        }

        private void PictureMuteOnBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureMuteOn();
        }

        private void PictureMuteOffBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureMuteOff();
        }

        private void GetInformationsBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshInformations();
        }

        private void BrightnessIncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.BRIGHTNESS, true);
        }

        private void BrightnessDecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.BRIGHTNESS, false);
        }

        private void ColorIncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.COLOR, true);
        }

        private void ColorDecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.COLOR, false);
        }

        private void ContrastIncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.CONTRAST, true);
        }

        private void ContrastDecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.CONTRAST, false);
        }

        private void HueIncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.HUE, true);
        }

        private void HueDecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.HUE, false);
        }

        private void SharpnessIncreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.SHARPNESS, true);
        }

        private void SharpnessDecreaseBtn_Click(object sender, RoutedEventArgs e)
        {
            PictureAdjust(AdjustmentTarget.SHARPNESS, false);
        }

        private void InputSourceListSelectionChangeHandle(object sender, SelectionChangedEventArgs e)
        {
            InputTypeComboboxItem item = InputSourceListChckbox.SelectedItem as InputTypeComboboxItem;
            InputSwitch(item.Type);
        }

        public AbstractResponse SendRequest(AbstractRequest request, string identifier)
        {

            //before every request
            EnableButtons(false);

            Utils.Logger.log(Utils.Utils.PrintByteArray(request.GetRequestBytes()));

            AbstractResponse response = request.SendRequest();

            //after evert request
            EnableButtons(true);

            if (response == null)
            {
                Utils.Logger.log(identifier + " - Connection couldn't get established.");
                return null;
            }

            Utils.Logger.log(Utils.Utils.PrintByteArray(response.GetRequestBytes()));

            if (response.GetStatus() != Status.RESPONSE_OK)
            {
                Utils.Logger.log(identifier + " - ERROR " + response.GetError().ToString());
            } else {
                Utils.Logger.log(identifier + " - SUCCESS");
            }

            return response;
        }

        private void CheckConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            string address = GetIpAddress();
            int port = GetPortNumber();

            if (!Utils.Utils.IsPortVaild(port) || !Utils.Utils.IsAddressValid(address))
            {
                Utils.Logger.log("IP Address or Port is not provided.");
                return;
            }

            Utils.Logger.log("Connection data looks valid. Checking connection...");

            if (RefreshInformations())
            {
                Utils.Logger.log("Connected!");
                EnableButtons(true);
            }
            else
            {
                Utils.Logger.log("Host seems to be unreachable :( Check address and port correctness.");
                EnableButtons(false);
            }
        }

        private int GetVolumeStep()
        {
            int value = 0;
            try
            {
                value = Convert.ToInt32(VolumeStepTextbox.Text);
            }
            catch (Exception)
            {
                value = -1;
            }

            if(value > MAX_VOLUME_STEP)
            {
                value = -1;
            }

            return value;
        }

        private string GetIpAddress()
        {
            return ipAddressTextbox.Text;
        }

        private int GetPortNumber()
        {
            int portNumber = 0;
            try
            {
                portNumber = Convert.ToInt32(portTextbox.Text);
            }
            catch (Exception)
            {
                portNumber = -1;
            }
            return portNumber;
        }

        private void IpAddressChangeHandle(object sender, TextChangedEventArgs args)
        {
            EnableButtons(false);
        }

        private void PortChangeEventHandle(object sender, TextChangedEventArgs args)
        {
            EnableButtons(false);
        }

    }

    public class InputTypeComboboxItem
    {
        public string Text { get; set; }
        public Requests.InputType Type { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
    
}
