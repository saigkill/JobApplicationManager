// <copyright file="PickerEmailOptionsModel.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace JobApplicationManager.Infrastructure.Models;

/// <summary>
/// Class PickerEmailOptionsModel.
/// This class represents options for email picker settings in the application.
/// </summary>
public class PickerEmailOptionsModel
{
    public string Option { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public List<PickerEmailOptionsModel> GetOptions()
    {
        var options = new List<PickerEmailOptionsModel>
        {
            new PickerEmailOptionsModel() { Option = "None", Value = "None" },
            new PickerEmailOptionsModel() { Option = "Auto", Value = "Auto" },
            new PickerEmailOptionsModel() { Option = "SslOnConnect", Value = "SslOnConnect" },
            new PickerEmailOptionsModel() { Option = "StartTls", Value = "StartTls" },
            new PickerEmailOptionsModel() { Option = "StartTlsWhenAvailable", Value = "StartTlsWhenAvailable" }
        };
        return options;
    }
}