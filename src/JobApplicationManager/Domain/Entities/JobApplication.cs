// <copyright file="JobApplication.cs" company="Sascha Manns">
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

using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobApplicationManager.Domain.Entities;

[Table("JobApplication")]
public partial class JobApplication
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string UserGuid { get; set; } = null!;

    [StringLength(50)]
    public string Jobtitle { get; set; } = null!;

    [StringLength(100)]
    public string? Url { get; set; }

    [Column("CompanyID")]
    public int? CompanyId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EmailSent { get; set; }

    [StringLength(400)]
    [Unicode(false)]
    public string? InterviewQuestions { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FirstInterview { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SecondInterview { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ThirdInterview { get; set; }

    [StringLength(400)]
    [Unicode(false)]
    public string? InterviewNotes { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("CompanyId")]
    [InverseProperty("JobApplications")]
    public virtual Company? Company { get; set; }

    [ForeignKey("UserGuid")]
    [InverseProperty("JobApplications")]
    public virtual User User { get; set; } = null!;
}