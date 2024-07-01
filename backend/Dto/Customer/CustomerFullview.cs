﻿
using backend.Dto.Store;

namespace backend.Dto.Customer;

public class CustomerFullview
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public string? ZipCode { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public List<StoreItem> Stores { get; set; } = [];
}