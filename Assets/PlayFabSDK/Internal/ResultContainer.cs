/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections.Generic;
using System;
using System.Net;

namespace PlayFab.Internal
{
    public class PlayFabResultCommon
    {
        public object Request;
        public object CustomData;
    }

    internal class ResultContainer<TResultType> where TResultType : PlayFabResultCommon
    {
        public int code;
        public string status;
        public int? errorCode;
        public string errorMessage;
        public Dictionary<string, List<string>> errorDetails;
        public TResultType data;

        private static ResultContainer<TResultType> KillWarnings()
        {
            // Unity doesn't recognize decoding json as assigning variables, so we have to assign them here
            return new ResultContainer<TResultType>
            {
                code = (int)HttpStatusCode.OK,
                status = "",
                errorCode = (int)PlayFabErrorCode.Success,
                errorMessage = "",
                errorDetails = null,
                data = null
            };
        }

        public static TResultType HandleResults(CallRequestContainer callRequest, Delegate resultCallback, ErrorCallback errorCallback, Action<TResultType, CallRequestContainer> resultAction)
        {
            if (callRequest.Error == null) // Some other error earlier in the process, just report it below
            {
                try
                {
                    ResultContainer<TResultType> resultEnvelope = SimpleJson.DeserializeObject<ResultContainer<TResultType>>(callRequest.ResultStr, Util.ApiSerializerStrategy);
                    if (!resultEnvelope.errorCode.HasValue || resultEnvelope.errorCode.Value == (int)PlayFabErrorCode.Success)
                    {
                        resultEnvelope.data.Request = callRequest.Request;
                        resultEnvelope.data.CustomData = callRequest.CustomData;
                        if (resultAction != null)
                            resultAction(resultEnvelope.data, callRequest);
                        WrapCallback(resultCallback, resultEnvelope.data);
                        PlayFabSettings.InvokeResponse(callRequest.Url, callRequest.CallId, callRequest.Request, resultEnvelope.data, callRequest.Error, callRequest.CustomData); // Do the globalMessage callback
                        return resultEnvelope.data; // This is the expected output path for successful api call
                    }

                    // Successful HTTP interaction, but PlayFab server returned an error
                    callRequest.Error = new PlayFabError
                    {
                        HttpCode = resultEnvelope.code,
                        HttpStatus = resultEnvelope.status,
                        Error = (PlayFabErrorCode)resultEnvelope.errorCode.Value,
                        ErrorMessage = resultEnvelope.errorMessage,
                        ErrorDetails = resultEnvelope.errorDetails
                    };
                }
                catch (Exception e)
                {
                    // Failed to decode the result
                    callRequest.Error = new PlayFabError
                    {
                        HttpCode = (int)HttpStatusCode.OK, // Technically the server returned a result, the sdk just didn't parse it correctly
                        HttpStatus = "Client failed to parse response from server",
                        Error = PlayFabErrorCode.Unknown,
                        ErrorMessage = e.ToString(),
                        ErrorDetails = null
                    };
                }
            }

            WrapCallback(errorCallback, callRequest.Error);
            WrapCallback(PlayFabSettings.GlobalErrorHandler, callRequest.Error);
            return null;
        }
        private static readonly object[] _invokeParams = new object[1];
        private static void WrapCallback(Delegate callback, object singleParam)
        {
            if (callback == null)
                return;

            _invokeParams[0] = singleParam;
            try
            {
                callback.DynamicInvoke(_invokeParams);
            }
            catch (Exception e)
            {
                if (!PlayFabSettings.HideCallbackErrors)
                    UnityEngine.Debug.LogException(e);
            }
        }
    }
}
