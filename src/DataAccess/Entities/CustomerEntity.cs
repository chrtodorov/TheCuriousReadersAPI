﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{

    public class CustomerEntity:AuditableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomerId { get; set; }

        [Required]
        public AddressEntity Address { get; set; } = null!;
        public Guid AddressId { get; set; }

        [Required]
        public virtual UserEntity User { get; set; } = null!;
    }
}
