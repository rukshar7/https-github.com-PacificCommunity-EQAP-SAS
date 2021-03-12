using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAS.Domain.Entities.Security
{
    public class MenuMaster
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int Id { get; set; }
        public string ActionId { get; set; }
        [Display(Name = "Action Display Name")]
        public string ActionDisplayName { get; set; }
        [Display(Name = "Action Name")]
        public string ActionName { get; set; }
        [Display(Name = "Controller Name")]
        public string ControllerName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
