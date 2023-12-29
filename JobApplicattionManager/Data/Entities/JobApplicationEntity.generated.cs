//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
//
//     Produced by Entity Framework Visual Editor v4.2.6.3
//     Source:                    https://github.com/msawczyn/EFDesigner
//     Visual Studio Marketplace: https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner
//     Documentation:             https://msawczyn.github.io/EFDesigner/
//     License (MIT):             https://github.com/msawczyn/EFDesigner/blob/master/LICENSE
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JobApplicattionManager.Entrities
{
   public partial class JobApplicationEntity
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected JobApplicationEntity()
      {
         Init();
      }

      /// <summary>
      /// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.
      /// </summary>
      public static JobApplicationEntity CreateJobApplicationEntityUnsafe()
      {
         return new JobApplicationEntity();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="_companyentity0"></param>
      public JobApplicationEntity(global::JobApplicattionManager.Entrities.CompanyEntity _companyentity0)
      {
         if (_companyentity0 == null) throw new ArgumentNullException(nameof(_companyentity0));
         _companyentity0.JobApplicationEntity.Add(this);

         Init();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="_companyentity0"></param>
      public static JobApplicationEntity Create(global::JobApplicattionManager.Entrities.CompanyEntity _companyentity0)
      {
         return new JobApplicationEntity(_companyentity0);
      }

      /*************************************************************************
       * Properties
       *************************************************************************/

      /// <summary>
      /// Identity, Indexed, Required
      /// Unique identifier
      /// </summary>
      [Key]
      [Required]
      [System.ComponentModel.Description("Unique identifier")]
      public long Id { get; set; }

      public string JobTitle { get; set; }

      public string Url { get; set; }

      /*************************************************************************
       * Navigation properties
       *************************************************************************/

   }
}

