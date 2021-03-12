using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SAS.Domain.Entities.Security
{
    public class MenuToRole
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int MenuToRoleId { get; set; }
        [ForeignKey("IdentityRole")]
        [Display(Name = "Identity Role")]
        public string UserRoleId { get; set; }
        public virtual IdentityRole IdentityRole { get; set; }
        [ForeignKey("MenuMaster")]
        [Display(Name = "Menu Master")]
        public int MenuMasterId { get; set; }
        public virtual MenuMaster MenuMaster { get; set; }

    }
}
