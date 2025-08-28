// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Play.AppUpdate.Samples.AppUpdateDemo.Scripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Google.Play.AppUpdate.Samples
{
  
    public class InAppUpdateManager : MonoBehaviour
    {
        public AppUpdateInfo appUpdateInfoResult;
        private AppUpdateManager _appUpdateManager;
        public AppUpdateType appUpdateType;

        public void Start()
        {
#if UNITY_ANDROID
            _appUpdateManager = new AppUpdateManager();
            StartCoroutine(GetUpdateInfoCoroutine());
#endif
        }
        private IEnumerator GetUpdateInfoCoroutine()
        {
            var appUpdateInfo = _appUpdateManager.GetAppUpdateInfo();
            yield return appUpdateInfo;

            Debug.Log("Get update info finished");
            if (appUpdateInfo.Error != AppUpdateErrorCode.NoError)
            {
                //var failedStatusMessage =
                //    String.Format("GetUpdateInfoCoroutine Failed: {0}", appUpdateInfo.Error.ToString());
                //SetStatus(failedStatusMessage);
                yield break;
            }

            appUpdateInfoResult = appUpdateInfo.GetResult();
            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                StartCoroutine(StartUpdateCoroutine());
            }
          

        }
        private IEnumerator StartUpdateCoroutine()
        {

            var appUpdateOptions = appUpdateType == AppUpdateType.Immediate
                ? AppUpdateOptions.ImmediateAppUpdateOptions()
                : AppUpdateOptions.FlexibleAppUpdateOptions();
            var appUpdateInfo = appUpdateInfoResult;
            var startUpdateRequest = _appUpdateManager.StartUpdate(appUpdateInfo, appUpdateOptions);
            yield return startUpdateRequest;
           
        }
      
    }
}