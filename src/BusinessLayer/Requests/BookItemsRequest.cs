﻿using BusinessLayer.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests;

public class BookItemsRequest
{
    [Required]
    [MaxLength(10)]
    [StringLength(10)]
    public string Barcode { get; set; } = string.Empty;

    public DateTime BorrowedDate { get; set; }

    public DateTime ReturnDate { get; set; }

    [Required]
    public BookItemStatusEnumeration BookStatus { get; set; }

    [Required]
    public Guid BookId { get; set; }
}