using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReservaSala.Models
{
    [Table("Salas")]
    public class Salas
    {
        [Key]
        [Display(Name = "Sala: ")]
        public int idSala { get; set; }

        [Required(ErrorMessage ="Para cadastrar uma sala é necessario informar um nome da sala")]
        [Display(Name = "Nome da sala: ")]
        public string nomeSala { get; set; }

        [Required(ErrorMessage = "Para cadastrar uma sala é necessario informar o andar da sala")]
        [Display(Name = "Andar da sala: ")]
        public string andarSala { get; set; }

        [Required(ErrorMessage = "Para cadastrar uma sala é necessario informar a quantidade de lugares da sala")]
        [Display(Name = "Lugares da sala: ")]
        public int lugarSala { get; set; }

        [Display(Name = "Observação sobre a sala: ")]
        public string observSala { get; set; }

        [Display(Name = "Situação da sala:")]
        public bool salaAtiva { get; set; }
    }
}