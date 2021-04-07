using System;
using System.ComponentModel.DataAnnotations;

namespace CheckDocto.Models
{
    public class KeyDocto
    {
        [Required]
        [StringLength(20, ErrorMessage ="Campo debe ser de {1} carácteres o menos.")]
        public string Empresa { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Campo debe ser de {1} carácteres o menos.")]
        public string Tipodocto { get; set; }
        [Required]
        [Range(1,99999999)]
        public decimal Correlativo { get; set; }
        
        public string Msg { get; set; }
    }
}


