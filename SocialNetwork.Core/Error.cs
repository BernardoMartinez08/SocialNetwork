﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core
{
    public class Error
    {
        [Required]
        public ErrorCode Code { get; set; }

        [Required]
        public string Message { get; set; }

        public string Target { get; set; }

        public IEnumerable<Error> Details { get; set; }

    }
}