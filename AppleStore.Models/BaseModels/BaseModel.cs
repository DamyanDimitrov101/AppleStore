﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AppleStore.Models.BaseModels
{
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}