using ArkanoDemoApp.Interfaces;
using ArkanoDemoApp.Utils;
using ArkanoDemoApp.Views;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Version.Plugin;
using static ArkanoDemoApp.Utils.CognitiveServices;

namespace ArkanoDemoApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes And Properties

        public ICommand LoginCommand { get; private set; }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                this.RaisePropertyChanged();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                this.RaisePropertyChanged();
            }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                this.RaisePropertyChanged();
            }
        }

        public LoginTypes LoginType { get; set; }

        #endregion Attributes And Properties

        #region Constructors

        public LoginViewModel()
        {
            this.LoginCommand = new Command(async () => await this.OnLoginCommand());
        }

        #endregion Constructors

        #region Operations

        public async Task RefreshViewModel()
        {
            this.SendWaitingMessage(true);
            try
            {
                //Load data if necessary.
                this.UserName = string.Empty;
                this.Password = string.Empty;

                this.Version = string.Format("v.{0}", CrossVersion.Current.Version);

                this.LoginType = LoginTypes.ActiveDirectory;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, this.DisplayAlert);
            }
            finally
            {
                this.SendWaitingMessage(false);
            }
        }

        public async Task DisposeViewModel()
        {
            try
            {
                this.UserName = null;
                this.Password = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Log(ex, this.DisplayAlert);
            }
        }

        public async Task OnLoginCommand()
        {
            this.SendWaitingMessage(true);
            try
            {
                if (this.LoginType == LoginTypes.ActiveDirectory)
                {
                    if (string.IsNullOrEmpty(this.UserName.Trim()) || string.IsNullOrEmpty(this.Password.Trim()))
                    {
                        await DisplayAlert("Required information", "You must enter username and password.", "OK");
                    }
                    else
                    {
                        this.SendWaitingMessage(true);
                        //TODO: hacer login y redirigir a la otra pagina.
                        App.NavigateTo(new Home(new HomeViewModel { UserName = this.UserName }));
                    }
                }
                else if (this.LoginType == LoginTypes.CognitiveServices)
                {
                    bool result = await CrossMedia.Current.Initialize();
                    result = CrossMedia.Current.IsCameraAvailable;
                    result = CrossMedia.Current.IsTakePhotoSupported;
                    StoreCameraMediaOptions cameraOptions = new StoreCameraMediaOptions { DefaultCamera = CameraDevice.Rear };
                    MediaFile mediaFileResult = await CrossMedia.Current.TakePhotoAsync(cameraOptions);
                    if (mediaFileResult != null)
                    {
                        List<FaceDetectResult> faceDetectResults = await CognitiveServices.FaceDetect(mediaFileResult);
                        if (faceDetectResults != null && faceDetectResults.Count > 0)
                        {
                            List<FaceIdentifyResult> faceIdentifyResults = await FaceIdentify(Constants.CognitiveServicesPersonGroupId, faceDetectResults[0].faceId);
                            if (faceIdentifyResults != null && faceIdentifyResults.Count > 0 && faceIdentifyResults[0].candidates.Count > 0)
                            {
                                PersonResult personResult = await CognitiveServices.GetPerson(Constants.CognitiveServicesPersonGroupId, faceIdentifyResults[0].candidates[0].personId, faceIdentifyResults[0].candidates[0].confidence);
                                if (personResult != null)
                                {
                                    string personText = string.Format("User: {0}.\nConfidence: {1}.\nData: {2}", personResult.name, personResult.confidence, personResult.userData);
                                    await DisplayAlert("Cognitive Services Result", personText, "OK");
                                }
                                else
                                {
                                    await DisplayAlert("Cognitive Services Result", "Not found the person.", "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Cognitive Services Result", "It has not been identified the user's face.", "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert("Cognitive Services Result", "Has not been detected user's face.", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Arkano Demo App", ex.Message, "OK");
            }
            finally
            {
                this.SendWaitingMessage(false);
            }
        }
        
        #endregion Operations
    }
}
