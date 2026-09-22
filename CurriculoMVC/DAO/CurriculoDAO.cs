using CurriculoMVC.Models;
using Microsoft.Data.SqlClient;

namespace CurriculoMVC.DAO;

/*
 * ============================================================================
 *  SCRIPT DE CRIAÇÃO DO BANCO E DA TABELA (SQL Server)
 *  Execute no SSMS / Azure Data Studio antes de rodar a aplicação.
 * ============================================================================
 *
 * CREATE DATABASE CurriculoDB;
 * GO
 * USE CurriculoDB;
 * GO
 *
 * CREATE TABLE Curriculo (
 *     Id INT IDENTITY(1,1) PRIMARY KEY,
 *     Nome NVARCHAR(100) NOT NULL,
 *     Cpf CHAR(11) NOT NULL UNIQUE,
 *     Endereco NVARCHAR(200) NOT NULL,
 *     Telefone NVARCHAR(20) NOT NULL,
 *     Email NVARCHAR(100) NOT NULL,
 *     PretensaoSalarial DECIMAL(10,2) NOT NULL,
 *     CargoPretendido NVARCHAR(100) NOT NULL,
 *     Objetivo NVARCHAR(500) NULL,
 *     Formacao1_Curso NVARCHAR(100) NULL,
 *     Formacao1_Instituicao NVARCHAR(100) NULL,
 *     Formacao1_Conclusao NVARCHAR(20) NULL,
 *     Formacao2_Curso NVARCHAR(100) NULL,
 *     Formacao2_Instituicao NVARCHAR(100) NULL,
 *     Formacao2_Conclusao NVARCHAR(20) NULL,
 *     Formacao3_Curso NVARCHAR(100) NULL,
 *     Formacao3_Instituicao NVARCHAR(100) NULL,
 *     Formacao3_Conclusao NVARCHAR(20) NULL,
 *     Formacao4_Curso NVARCHAR(100) NULL,
 *     Formacao4_Instituicao NVARCHAR(100) NULL,
 *     Formacao4_Conclusao NVARCHAR(20) NULL,
 *     Formacao5_Curso NVARCHAR(100) NULL,
 *     Formacao5_Instituicao NVARCHAR(100) NULL,
 *     Formacao5_Conclusao NVARCHAR(20) NULL,
 *     Experiencia1_Empresa NVARCHAR(100) NULL,
 *     Experiencia1_Cargo NVARCHAR(100) NULL,
 *     Experiencia1_Periodo NVARCHAR(50) NULL,
 *     Experiencia1_Descricao NVARCHAR(500) NULL,
 *     Experiencia2_Empresa NVARCHAR(100) NULL,
 *     Experiencia2_Cargo NVARCHAR(100) NULL,
 *     Experiencia2_Periodo NVARCHAR(50) NULL,
 *     Experiencia2_Descricao NVARCHAR(500) NULL,
 *     Experiencia3_Empresa NVARCHAR(100) NULL,
 *     Experiencia3_Cargo NVARCHAR(100) NULL,
 *     Experiencia3_Periodo NVARCHAR(50) NULL,
 *     Experiencia3_Descricao NVARCHAR(500) NULL,
 *     Idioma1_Nome NVARCHAR(50) NULL,
 *     Idioma1_Nivel NVARCHAR(30) NULL,
 *     Idioma2_Nome NVARCHAR(50) NULL,
 *     Idioma2_Nivel NVARCHAR(30) NULL,
 *     Idioma3_Nome NVARCHAR(50) NULL,
 *     Idioma3_Nivel NVARCHAR(30) NULL
 * );
 * GO
 *
 *  Observações:
 *  - Uma única tabela guarda o currículo inteiro.
 *  - Formação (até 5), Experiência (até 3) e Idiomas (até 3) ficam em colunas
 *    numeradas (Formacao1_..., Experiencia1_..., Idioma1_...).
 *  - Somente o 1º registro de formação é obrigatório; os demais podem ficar
 *    NULL e são ignorados na exibição do currículo.
 *  - A connection string está em appsettings.json (ConnectionStrings:DefaultConnection).
 */
public class CurriculoDAO
{
    private readonly string _connectionString;

    public CurriculoDAO(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // ------------------------------------------------------------------ Consultas

    /// <summary>Lista resumida (Id, CPF e Nome) para a tela inicial.</summary>
    public List<Curriculo> Listar()
    {
        var lista = new List<Curriculo>();

        using var cn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT Id, Cpf, Nome FROM Curriculo ORDER BY Nome", cn);
        cn.Open();
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            lista.Add(new Curriculo
            {
                Id = rd.GetInt32(0),
                Cpf = rd.GetString(1),
                Nome = rd.GetString(2)
            });
        }
        return lista;
    }

    /// <summary>Retorna o currículo completo ou null se não existir.</summary>
    public Curriculo BuscarPorId(int id)
    {
        using var cn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT * FROM Curriculo WHERE Id = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cn.Open();
        using var rd = cmd.ExecuteReader();
        return rd.Read() ? Mapear(rd) : null;
    }

    /// <summary>Verifica se o CPF já está cadastrado em outro currículo.</summary>
    public bool CpfExiste(string cpf, int idIgnorar)
    {
        using var cn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT COUNT(1) FROM Curriculo WHERE Cpf = @Cpf AND Id <> @Id", cn);
        cmd.Parameters.AddWithValue("@Cpf", cpf);
        cmd.Parameters.AddWithValue("@Id", idIgnorar);
        cn.Open();
        return (int)cmd.ExecuteScalar() > 0;
    }

    // ------------------------------------------------------------------ Alterações

    public void Inserir(Curriculo c)
    {
        var valores = ObterValores(c);
        var colunas = string.Join(", ", valores.Keys);
        var parametros = string.Join(", ", valores.Keys.Select(k => "@" + k));

        using var cn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand($"INSERT INTO Curriculo ({colunas}) VALUES ({parametros})", cn);
        PreencherParametros(cmd, valores);
        cn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Atualizar(Curriculo c)
    {
        var valores = ObterValores(c);
        var sets = string.Join(", ", valores.Keys.Select(k => $"{k} = @{k}"));

        using var cn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand($"UPDATE Curriculo SET {sets} WHERE Id = @Id", cn);
        PreencherParametros(cmd, valores);
        cmd.Parameters.AddWithValue("@Id", c.Id);
        cn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var cn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("DELETE FROM Curriculo WHERE Id = @Id", cn);
        cmd.Parameters.AddWithValue("@Id", id);
        cn.Open();
        cmd.ExecuteNonQuery();
    }

    // ------------------------------------------------------------------ Auxiliares

    /// <summary>Converte o objeto em pares coluna/valor (usado por INSERT e UPDATE).</summary>
    private static Dictionary<string, object> ObterValores(Curriculo c)
    {
        var v = new Dictionary<string, object>
        {
            ["Nome"] = c.Nome,
            ["Cpf"] = c.Cpf,
            ["Endereco"] = c.Endereco,
            ["Telefone"] = c.Telefone,
            ["Email"] = c.Email,
            ["PretensaoSalarial"] = c.PretensaoSalarial,
            ["CargoPretendido"] = c.CargoPretendido,
            ["Objetivo"] = c.Objetivo
        };

        for (int i = 0; i < Curriculo.MaxFormacoes; i++)
        {
            var f = c.Formacoes[i];
            v[$"Formacao{i + 1}_Curso"] = f.Curso;
            v[$"Formacao{i + 1}_Instituicao"] = f.Instituicao;
            v[$"Formacao{i + 1}_Conclusao"] = f.Conclusao;
        }

        for (int i = 0; i < Curriculo.MaxExperiencias; i++)
        {
            var e = c.Experiencias[i];
            v[$"Experiencia{i + 1}_Empresa"] = e.Empresa;
            v[$"Experiencia{i + 1}_Cargo"] = e.Cargo;
            v[$"Experiencia{i + 1}_Periodo"] = e.Periodo;
            v[$"Experiencia{i + 1}_Descricao"] = e.Descricao;
        }

        for (int i = 0; i < Curriculo.MaxIdiomas; i++)
        {
            var d = c.Idiomas[i];
            v[$"Idioma{i + 1}_Nome"] = d.Nome;
            v[$"Idioma{i + 1}_Nivel"] = d.Nivel;
        }

        return v;
    }

    private static void PreencherParametros(SqlCommand cmd, Dictionary<string, object> valores)
    {
        foreach (var par in valores)
            cmd.Parameters.AddWithValue("@" + par.Key, par.Value ?? DBNull.Value);
    }

    /// <summary>Lê uma coluna de texto; DBNull vira null.</summary>
    private static string Texto(SqlDataReader rd, string coluna) => rd[coluna] as string;

    /// <summary>Monta o objeto Curriculo a partir da linha atual do reader.</summary>
    private static Curriculo Mapear(SqlDataReader rd)
    {
        var c = new Curriculo
        {
            Id = (int)rd["Id"],
            Nome = Texto(rd, "Nome"),
            Cpf = Texto(rd, "Cpf"),
            Endereco = Texto(rd, "Endereco"),
            Telefone = Texto(rd, "Telefone"),
            Email = Texto(rd, "Email"),
            PretensaoSalarial = rd["PretensaoSalarial"] as decimal?,
            CargoPretendido = Texto(rd, "CargoPretendido"),
            Objetivo = Texto(rd, "Objetivo")
        };

        for (int i = 1; i <= Curriculo.MaxFormacoes; i++)
        {
            c.Formacoes.Add(new Formacao
            {
                Curso = Texto(rd, $"Formacao{i}_Curso"),
                Instituicao = Texto(rd, $"Formacao{i}_Instituicao"),
                Conclusao = Texto(rd, $"Formacao{i}_Conclusao")
            });
        }

        for (int i = 1; i <= Curriculo.MaxExperiencias; i++)
        {
            c.Experiencias.Add(new Experiencia
            {
                Empresa = Texto(rd, $"Experiencia{i}_Empresa"),
                Cargo = Texto(rd, $"Experiencia{i}_Cargo"),
                Periodo = Texto(rd, $"Experiencia{i}_Periodo"),
                Descricao = Texto(rd, $"Experiencia{i}_Descricao")
            });
        }

        for (int i = 1; i <= Curriculo.MaxIdiomas; i++)
        {
            c.Idiomas.Add(new Idioma
            {
                Nome = Texto(rd, $"Idioma{i}_Nome"),
                Nivel = Texto(rd, $"Idioma{i}_Nivel")
            });
        }

        return c;
    }
}
