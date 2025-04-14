﻿namespace DogusProject.Application.DTOs;

public class RegisterRequestDto
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string UserName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string ConfirmPassword { get; set; } = null!;
	public string? Bio { get; set; }
	public string? Location { get; set; }
	public string? Website { get; set; }
}