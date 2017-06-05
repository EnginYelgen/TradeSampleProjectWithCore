using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;

namespace TradeSampleProjectWithCore.BaseClasses
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool InUse { get; set; }
        [Required]
        public int UpdateUserId { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }

        [ForeignKey("UpdateUserId")]
        public User User { get; set; }
    }
}
