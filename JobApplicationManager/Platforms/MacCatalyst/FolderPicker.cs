#region copyright
// <copyright file="FolderPicker.cs" company="Sascha Manns">
// Copyright (C) 2023 Sascha Manns, Sascha.Manns@outlook.de
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation 
// files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, 
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE 
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// @author: Sascha Manns, Sascha.Manns@outlook.de
// @copyright: 2023, Sascha Manns, https://dev.azure.com/saigkill/JobApplicationManager
// </copyright>
#endregion
using Foundation;

using UIKit;

namespace JobApplicationManager.Platforms.MacCatalyst
{
    public class FolderPicker : IFolderPicker
    {
        class PickerDelegate : UIDocumentPickerDelegate
        {
            public Action<NSUrl[]> PickHandler { get; set; }

            public override void WasCancelled(UIDocumentPickerViewController controller)
                => PickHandler?.Invoke(null);

            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl[] urls)
                => PickHandler?.Invoke(urls);

            public override void DidPickDocument(UIDocumentPickerViewController controller, NSUrl url)
                => PickHandler?.Invoke(new NSUrl[] { url });
        }

        static void GetFileResults(NSUrl[] urls, TaskCompletionSource<string> tcs)
        {
            try
            {
                tcs.TrySetResult(urls?[0]?.ToString() ?? "");
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        public async Task<string> PickFolder()
        {
            var allowedTypes = new string[]
            {
              "public.folder"
            };

            var picker = new UIDocumentPickerViewController(allowedTypes, UIDocumentPickerMode.Open);
            var tcs = new TaskCompletionSource<string>();

            picker.Delegate = new PickerDelegate
            {
                PickHandler = urls => GetFileResults(urls, tcs)
            };

            if (picker.PresentationController != null)
            {
                picker.PresentationController.Delegate =
                    new UIPresentationControllerDelegate(() => GetFileResults(null, tcs));
            }

            var parentController = Platform.GetCurrentUIViewController();

            parentController.PresentViewController(picker, true, null);

            return await tcs.Task;
        }

        internal class UIPresentationControllerDelegate : UIAdaptivePresentationControllerDelegate
        {
            Action dismissHandler;

            internal UIPresentationControllerDelegate(Action dismissHandler)
                => this.dismissHandler = dismissHandler;

            public override void DidDismiss(UIPresentationController presentationController)
            {
                dismissHandler?.Invoke();
                dismissHandler = null;
            }

            protected override void Dispose(bool disposing)
            {
                dismissHandler?.Invoke();
                base.Dispose(disposing);
            }
        }
    }
}
