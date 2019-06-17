using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ReservaSala.Models
{
    [Table("reservaSala")]
    public class ReservaSala
    {
        [Key]
        public int idResSala { get; set; }

        [Required(ErrorMessage = "Informe um titulo para esta reserva")]
        [Display(Name = "Informação")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "O titulo contem de 1 a 50 caracteres")]
        public string titulo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "É necessario informar a data que você deseja reservar")]
        [Display(Name = "Dia")]
        public DateTime reservaDia { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "É necessario informar o horario inicial da reserva")]
        [Display(Name = "Hora inicial da reserva")]
        public TimeSpan reservaHoraIni { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "É necessario informar o horario final da reserva")]
        [Display(Name = "Hora final da reserva")]
        public TimeSpan reservaHoraFim { get; set; }

        public DateTime dataReservaCriacao { get; set; }

        #region Salas

        [ForeignKey("idSala")]
        public virtual Salas Salas { get; set; }

        [Required(ErrorMessage = "É necessario informar a sala que você deseja reservar")]
        public int idSala { get; set; }

        #endregion

        #region UsuarioSala

        [ForeignKey("idUsuSala")]
        public virtual UsuarioSala UsuarioSala { get; set; }

        public int idUsuSala { get; set; }

        #endregion

    }
}