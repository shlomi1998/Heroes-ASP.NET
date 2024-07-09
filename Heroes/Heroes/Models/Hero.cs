using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Heroes.Models
{
    public class Hero
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Ability { get; set; }  // Attacker or Defender

        [Required]
        public DateTime StartTrainingDate { get; set; }

        [Required]
        public string SuitColors { get; set; }

        [Required]
        public decimal StartingPower { get; set; }

        public decimal CurrentPower { get; set; }

        public int? TrainerId { get; set; }

        [JsonIgnore]
        public Trainer? Trainer { get; set; }

        // Fields to track training
        public int TrainingCountToday { get; set; } = 0;
        public DateTime LastTrainingDate { get; set; } = DateTime.MinValue;
    }
}
