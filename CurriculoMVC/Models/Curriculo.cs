using System.ComponentModel.DataAnnotations;

namespace CurriculoMVC.Models;

public class Formacao
{
    [Display(Name = "Curso")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Curso { get; set; }

    [Display(Name = "Instituição")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Instituicao { get; set; }

    [Display(Name = "Conclusão (ano ou \"cursando\")")]
    [StringLength(20, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Conclusao { get; set; }

    // Um registro só entra no currículo se algum campo foi preenchido
    public bool Preenchida =>
        !string.IsNullOrWhiteSpace(Curso) ||
        !string.IsNullOrWhiteSpace(Instituicao) ||
        !string.IsNullOrWhiteSpace(Conclusao);
}

public class Experiencia
{
    [Display(Name = "Empresa")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Empresa { get; set; }

    [Display(Name = "Cargo")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Cargo { get; set; }

    [Display(Name = "Período (ex.: 03/2020 a 08/2023)")]
    [StringLength(50, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Periodo { get; set; }

    [Display(Name = "Atividades desenvolvidas")]
    [StringLength(500, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Descricao { get; set; }

    public bool Preenchida =>
        !string.IsNullOrWhiteSpace(Empresa) ||
        !string.IsNullOrWhiteSpace(Cargo) ||
        !string.IsNullOrWhiteSpace(Periodo) ||
        !string.IsNullOrWhiteSpace(Descricao);
}

public class Idioma
{
    [Display(Name = "Idioma")]
    [StringLength(50, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Nome { get; set; }

    [Display(Name = "Nível")]
    [StringLength(30, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Nivel { get; set; }

    public bool Preenchida =>
        !string.IsNullOrWhiteSpace(Nome) ||
        !string.IsNullOrWhiteSpace(Nivel);
}

public class Curriculo
{
    public const int MaxFormacoes = 5;
    public const int MaxExperiencias = 3;
    public const int MaxIdiomas = 3;

    public int Id { get; set; }

    // ---------- Dados pessoais ----------
    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "Informe o nome completo.")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Nome { get; set; }

    [Display(Name = "CPF")]
    [Required(ErrorMessage = "Informe o CPF.")]
    [StringLength(14, ErrorMessage = "CPF inválido.")]
    public string Cpf { get; set; }

    [Display(Name = "Endereço")]
    [Required(ErrorMessage = "Informe o endereço.")]
    [StringLength(200, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Endereco { get; set; }

    [Display(Name = "Telefone")]
    [Required(ErrorMessage = "Informe o telefone.")]
    [StringLength(20, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Telefone { get; set; }

    [Display(Name = "E-mail")]
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Email { get; set; }

    [Display(Name = "Pretensão salarial (R$)")]
    [Required(ErrorMessage = "Informe a pretensão salarial.")]
    [Range(0.0, 99999999.99, ErrorMessage = "Valor fora do limite permitido.")]
    public decimal? PretensaoSalarial { get; set; }

    [Display(Name = "Cargo pretendido")]
    [Required(ErrorMessage = "Informe o cargo pretendido.")]
    [StringLength(100, ErrorMessage = "Máximo de {1} caracteres.")]
    public string CargoPretendido { get; set; }

    [Display(Name = "Objetivo profissional")]
    [StringLength(500, ErrorMessage = "Máximo de {1} caracteres.")]
    public string Objetivo { get; set; }

    // ---------- Listas com limite fixo ----------
    public List<Formacao> Formacoes { get; set; } = new();
    public List<Experiencia> Experiencias { get; set; } = new();
    public List<Idioma> Idiomas { get; set; } = new();

    // CPF é guardado só com dígitos; esta propriedade é usada para exibição
    public string CpfFormatado =>
        Cpf is { Length: 11 }
            ? $"{Cpf.Substring(0, 3)}.{Cpf.Substring(3, 3)}.{Cpf.Substring(6, 3)}-{Cpf.Substring(9, 2)}"
            : Cpf;

    /// <summary>
    /// Deixa o objeto pronto para uso: CPF só com dígitos e listas com
    /// exatamente 5 formações, 3 experiências e 3 idiomas (registros vazios completam).
    /// </summary>
    public void Normalizar()
    {
        Cpf = new string((Cpf ?? "").Where(char.IsDigit).ToArray());

        Formacoes ??= new();
        Experiencias ??= new();
        Idiomas ??= new();

        Ajustar(Formacoes, MaxFormacoes);
        Ajustar(Experiencias, MaxExperiencias);
        Ajustar(Idiomas, MaxIdiomas);
    }

    private static void Ajustar<T>(List<T> lista, int tamanho) where T : new()
    {
        if (lista.Count > tamanho)
            lista.RemoveRange(tamanho, lista.Count - tamanho);
        while (lista.Count < tamanho)
            lista.Add(new T());
    }

    /// <summary>Valida os dígitos verificadores. Chame depois de Normalizar().</summary>
    public bool CpfValido()
    {
        if (Cpf == null || Cpf.Length != 11 || Cpf.Distinct().Count() == 1)
            return false;

        for (int t = 9; t < 11; t++)
        {
            int soma = 0;
            for (int i = 0; i < t; i++)
                soma += (Cpf[i] - '0') * (t + 1 - i);

            int digito = soma * 10 % 11 % 10;
            if (Cpf[t] - '0' != digito)
                return false;
        }
        return true;
    }
}
