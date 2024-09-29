using AUTHENTICATION.DOMAIN;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace AUTHENTICATION.APPLICATION.Authentication.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly UserManager<User> _userManager;

    public CreateUserCommandValidator(UserManager<User> userManager)
    {
        _userManager = userManager;
        ValidatePassWordIsEmpty();
        ValidateUserName();
    }

    private void ValidateUserName()
    {
        RuleFor(command => command.Name)
            .MustAsync( async(userName,x) =>
            {
                var user = await _userManager.FindByNameAsync(userName);
                return user == null;
            })
            .WithMessage("Ja possui um usuario com esse nome");
    }

    private void ValidatePassWord()
    {
        RuleFor(command => command.Password)
            .Must(x =>
            {
                return ValidarSenha(x);
            })
            .WithMessage("Senha não esta seguindo os padrões");
    }

    private void ValidatePassWordIsEmpty()
    {
        RuleFor(command => command.Password)
           .Must(x => !string.IsNullOrEmpty(x))
           .DependentRules(ValidatePassWord)
           .WithMessage("Senha não pode esta vazia");
    }

    public bool ValidarSenha(string senha)
    {
        // Verifica se contém pelo menos uma letra 'A'
        bool contemLetraA = senha.Contains('A');

        // Verifica se contém pelo menos uma letra minúscula
        bool contemMinuscula = Regex.IsMatch(senha, @"[a-z]");

        // Verifica se contém pelo menos um caractere especial
        bool contemCaracterEspecial = Regex.IsMatch(senha, @"[\W_]");

        // Verifica se contém pelo menos um número
        bool contemNumero = Regex.IsMatch(senha, @"\d");

        // Retorna true se todos os critérios forem atendidos
        return contemLetraA && contemMinuscula && contemCaracterEspecial && contemNumero;
    }
}
