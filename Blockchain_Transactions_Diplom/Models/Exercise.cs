﻿using System.ComponentModel.DataAnnotations;

namespace Blockchain_Transactions_Diplom.Models
{
    public class Exercise
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public byte[]? File { get; set; }
        public string? UserId { get; set; }
        public string? AnswerCreator { get; set; }
        public string? AnswerExecutor { get; set; }
        public byte[]? FileAnswer { get; set; }
        public string? FileNameAnswer { get; set; }
    }
}
